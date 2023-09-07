using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Dts
{
	internal static class Extractor
	{
		#region Regex Definitions

		private const string DECLARE = "declare";

		private const string Space = @"[ \t]"; // 一个空格或一个制表符
		private const string SpacesOrEmpty = @"\s*"; // 0 个或多个空白符
		private const string Any = @"[\s\S]+?"; // 任意字符串

		// \n 表示成员必须在新的一行； .* 由于不是单行模式，.*会匹配到本行结束
		private const string TypeMember = $@"\n[\s\*]*@dts_{{0}}{Space}(?<{DECLARE}>.*){SpacesOrEmpty}"; // 成员
		private const string End = $@"/\*\*{SpacesOrEmpty}@dts_end{SpacesOrEmpty}\*/";

		// 需要优先匹配 /** @dts_end */
		private static readonly Regex CommentRegex = new($@"/\*\*{Any}\*/", RegexOptions.Compiled);

		private static readonly Regex InterfaceRegex = new(string.Format(TypeMember, "i"), RegexOptions.Compiled);
		private static readonly Regex ClassRegex = new(string.Format(TypeMember, "c"), RegexOptions.Compiled);
		private static readonly Regex MethodRegex = new(string.Format(TypeMember, "m"), RegexOptions.Compiled);
		private static readonly Regex PropertyRegex = new(string.Format(TypeMember, "p"), RegexOptions.Compiled);
		private static readonly Regex EndRegex = new(End, RegexOptions.Compiled);

		#endregion


		public static void Run(string path, string config = "dtsconfig")
		{
			string[]? rules = ReadRules(path, config);

			OptionParser parser = new();
			parser.ParseRules(path, rules!);

			if (parser.Disable)
			{
				return;
			}

			string mergedFile = Path.GetFullPath(parser.Output, path);
			if (File.Exists(mergedFile))
			{
				File.Delete(mergedFile);
			}

			foreach (var file in parser.Files)
			{
				List<Member> members = Parse(FileUtil.ReadAllText(file));
				string content = new MemberCollection(members).Render();

				string fp = Path.ChangeExtension(file, OptionParser.EXT);
				if (File.Exists(fp))
				{
					File.Delete(fp);
				}

				string output = parser.Merge ? mergedFile : fp;

				FileUtil.WriteAllText(output, content, parser.Merge);
			}
		}


		private static string[]? ReadRules(string path, string config)
		{
			if (!Directory.Exists(path))
			{
				Program.Error($"目录“{path}”不存在或已被删除。");
				return null;
			}

			try
			{
				string configFile = Path.GetFullPath(config, path);
				if (!File.Exists(configFile))
				{
					throw new FileNotFoundException();
				}

				return FileUtil.ReadLines(configFile);
			}
			catch
			{
				Program.Error("配置文件不存在或已被删除。");
				return null;
			}
		}

		private static List<Member> Parse(string content)
		{
			List<Member> members = new();

			string input = content.Trim();
			if (string.IsNullOrEmpty(input))
			{
				return members;
			}

			while (true)
			{
				Match match = CommentRegex.Match(input);
				if (!match.Success)
				{
					break;
				}

				Member? member = Match(match.Value);
				if (member is not null)
				{
					members.Add(member);
				}

				input = input[(match.Index + match.Length)..];
			}

			return members;
		}

		private static Member? Match(string comment)
		{
			// 将行反转，为了匹配出重复设置的最后一项
			string commentReversed = string.Join(Environment.NewLine, Regex.Split(comment, "\r?\n").Reverse());	

			Match match;
			if (EndRegex.Match(comment).Success)
			{
				return Member.Unset;
			}
			else if ((match = InterfaceRegex.Match(commentReversed)).Success)
			{
				return new Member(match.Groups[DECLARE].Value.Trim(), comment, MemberType.Interface);
			}
			else if ((match = ClassRegex.Match(commentReversed)).Success)
			{
				return new Member(match.Groups[DECLARE].Value.Trim(), comment, MemberType.Class);
			}
			else if ((match = MethodRegex.Match(commentReversed)).Success)
			{
				return new Member(match.Groups[DECLARE].Value.Trim(), comment, MemberType.Method);
			}
			else if ((match = PropertyRegex.Match(commentReversed)).Success)
			{
				return new Member(match.Groups[DECLARE].Value.Trim(), comment, MemberType.Property);
			}
			else
			{
				return null;
			}
		}
	}
}
