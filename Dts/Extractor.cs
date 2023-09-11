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
		private const string Other = $@"/\*\*{SpacesOrEmpty}@dts_{{0}}{Space}(?<{DECLARE}>.*){SpacesOrEmpty}\*/";
		private const string End = $@"/\*\*{SpacesOrEmpty}@dts_end{SpacesOrEmpty}\*/";

		private static readonly Regex CommentRegex = new($@"/\*\*{Any}\*/", RegexOptions.Compiled);

		private static readonly Regex InterfaceRegex = new(string.Format(TypeMember, "i"), RegexOptions.Compiled);
		private static readonly Regex ClassRegex = new(string.Format(TypeMember, "c"), RegexOptions.Compiled);
		private static readonly Regex MethodRegex = new(string.Format(TypeMember, "m"), RegexOptions.Compiled);
		private static readonly Regex PropertyRegex = new(string.Format(TypeMember, "p"), RegexOptions.Compiled);

		private static readonly Regex TypeRegex = new(string.Format(Other, "type"), RegexOptions.Compiled);
		private static readonly Regex RawRegex = new(string.Format(Other, "raw"), RegexOptions.Compiled);
		private static readonly Regex EndRegex = new(End, RegexOptions.Compiled);

		#endregion


		private static readonly string NL = Environment.NewLine;
		private static readonly Regex SepRegex = new(@"\\+", RegexOptions.Compiled);


		public static void Extract(string[] files, string? outputFileForMerging = null)
		{
			OptionParser parser = OptionParser.Create(files, outputFileForMerging);

			if (parser.Disable)
			{
				return;
			}

			string mergedOutputFile = Path.GetFullPath(parser.OutputFile);
			if (parser.MergeOutput && File.Exists(mergedOutputFile))
			{
				File.Delete(mergedOutputFile);
			}

			foreach (var file in parser.Files)
			{
				string fileContent = FileUtil.ReadAllText(file).Trim();

				string outputFile = Path.ChangeExtension(file, ".d.ts");
				if (!parser.MergeOutput && File.Exists(outputFile))
				{
					File.Delete(outputFile);
				}

				string finalOutputFile = parser.MergeOutput ? mergedOutputFile : outputFile;
				string fileRelativePath = Path.GetRelativePath(Path.GetDirectoryName(finalOutputFile) ?? finalOutputFile, file);
				fileRelativePath = SepRegex.Replace(fileRelativePath, "/");

				List<Member> members = Parse(fileContent);
				string content = new MemberCollection(members).Render().Trim();
				string output = (parser.MergeOutput ? $"/*! {fileRelativePath} */{NL}" : "") + $"{content}{NL}{NL}{NL}";
				FileUtil.WriteAllText(finalOutputFile, output, parser.MergeOutput);
			}
		}

		public static void Extract(string path, string?config = null)
		{
			config ??= ".dtssetting";

			string[]? rules = ReadRules(path, config);
			if (rules is null) { Environment.Exit(0); }

			OptionParser parser = new();
			parser.ParseRules(path, rules);

			if (parser.Disable)
			{
				return;
			}

			string mergedInputFile = Path.GetFullPath(parser.InputFile, path);
			if (parser.MergeInput && File.Exists(mergedInputFile))
			{
				File.Delete(mergedInputFile);
			}

			string mergedOutputFile = Path.GetFullPath(parser.OutputFile, path);
			if (parser.MergeOutput && File.Exists(mergedOutputFile))
			{
				File.Delete(mergedOutputFile);
			}

			foreach (var file in parser.Files)
			{
				string fileContent = FileUtil.ReadAllText(file).Trim();

				string outputFile = Path.ChangeExtension(file, ".d.ts");
				if (!parser.MergeOutput && File.Exists(outputFile))
				{
					File.Delete(outputFile);
				}

				string finalOutputFile = parser.MergeOutput ? mergedOutputFile : outputFile;
				string fileRelativePath = Path.GetRelativePath(Path.GetDirectoryName(finalOutputFile) ?? finalOutputFile, file);
				fileRelativePath = SepRegex.Replace(fileRelativePath, "/");
				
				if (parser.MergeInput)
				{
					string input = $"/*! {fileRelativePath} */{NL}{fileContent}{NL}{NL}{NL}";
					FileUtil.WriteAllText(mergedInputFile, input, true);
				}

				List<Member> members = Parse(fileContent);
				string content = new MemberCollection(members).Render().Trim();
				string output = (parser.MergeOutput ? $"/*! {fileRelativePath} */{NL}" : "") + $"{content}{NL}{NL}{NL}";
				FileUtil.WriteAllText(finalOutputFile, output, parser.MergeOutput);
			}
		}


		private static string[]? ReadRules(string path, string config)
		{
			FileUtil.ExistsDir(path);

			string configFile = Path.GetFullPath(config, path);
			FileUtil.ExistsFile(configFile);
			return FileUtil.ReadLines(configFile);
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
			string commentReversed = string.Join(NL, Regex.Split(comment, "\r?\n").Reverse());	

			Match match;
			if (EndRegex.Match(comment).Success)
			{
				return Member.Unset;
			}
			else if ((match = TypeRegex.Match(commentReversed)).Success)
			{
				return new Member(match.Groups[DECLARE].Value.Trim(), "", MemberType.Type); // 类型定义
			}
			else if ((match = RawRegex.Match(commentReversed)).Success)
			{
				return new Member(match.Groups[DECLARE].Value.Trim(), "", MemberType.Raw); // 原始内容
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
