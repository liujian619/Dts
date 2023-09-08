using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Dts
{
	internal sealed class Member
	{
		public Member(string declare, string comment, MemberType? type)
		{
			Declare = declare;
			Comment = comment;
			Type = type;
		}

		// 匹配每一行的开头的空白符
		private static readonly Regex _regex = new(@"^\s*", RegexOptions.Multiline | RegexOptions.Compiled);

		private static readonly Regex MemberRegex = new(@"^[\s\*]*@dts_[icmp][ \t]", RegexOptions.Compiled);

		internal static readonly Member Unset = new("", "", null);


		/// <summary>
		/// 声明。
		/// </summary>
		public string Declare { get; }

		/// <summary>
		/// 注释。
		/// </summary>
		public string Comment { get; }

		/// <summary>
		/// 成员类型。
		/// </summary>
		public MemberType? Type { get; init; }


		/// <summary>
		/// 子成员集合。
		/// </summary>
		public MemberCollection Children { get; } = new MemberCollection();


		internal int Level { get; set; }


		/// <summary>
		/// 渲染。
		/// </summary>
		/// <returns>返回渲染成的字符串。</returns>
		public string Render()
		{
			if (string.IsNullOrWhiteSpace(Declare) || !Type.HasValue)
			{
				return string.Empty;
			}

			string trimmedDeclare = Declare.Trim();
			string trimmedComment = Comment.Trim();	

			StringBuilder sb = new();
			sb.AppendLine(Format(trimmedComment, Level, true));

			string declare = Type == MemberType.Interface ? $"declare interface {trimmedDeclare} {{"
				: Type == MemberType.Class ? $"declare class {trimmedDeclare} {{"
				: trimmedDeclare;
			
			if (Type == MemberType.Method || Type == MemberType.Property)
			{
				if (Level <= 0)
				{
					var t = Type == MemberType.Method ? "function" : "let";
					declare = $"declare {t} {declare}";
				}
				if (!declare.EndsWith(";"))
				{
					declare += ";";
				}
			}
			
			sb.AppendLine(Format(declare, Level, false));

			for (int i = 0; i < Children.Count; i++)
			{
				sb.Append(Children[i].Render() + (i < Children.Count - 1 ? Environment.NewLine : ""));
			}

			if (Type == MemberType.Interface || Type == MemberType.Class)
			{
				sb.AppendLine("}");
			}

			return sb.ToString();
		}


		private static string Format(string content, int level, bool isComment)
		{
			string result = content;
			if (isComment)
			{
				result = string.Join(Environment.NewLine, Regex.Split(result, @"\r?\n").Where(s => !MemberRegex.IsMatch(s)));
			}

			int delta = isComment ? 1 : 0;
			string spaces = new(' ', level * 4 + delta); // 注释中除了第一行，其余行需要多缩进一个空格
			result = _regex.Replace(result, spaces)[delta..];

			return result;
		}
	}


	internal enum MemberType { Interface, Class, Method, Property }
}
