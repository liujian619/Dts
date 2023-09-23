using System;
using System.IO;
using FileSystemSearcher;

namespace Dts
{
	internal class JoinParser : RuleParser
	{
		/// <summary>
		/// 是否禁用。
		/// </summary>
		public bool Disable { get; private set; } = false;

		/// <summary>
		/// 一个文件名，合并所有输出到该文件。
		/// </summary>
		public string OutputFile { get; private set; } = "output.js";

		/// <summary>
		/// 一个文件名，合并所使用的主文件。
		/// </summary>
		public string MainFile { get; private set; } = "main.js";

		/// <summary>
		/// 行首空格数。
		/// </summary>
		public int Spaces { get; private set; } = 0;

		protected override void ParseCustomRule(string path, string rule)
		{
			if (rule.StartsWith(">>"))
			{
				string f = rule[2..].Trim();
				if (f.Length > 0)
				{
					OutputFile = Path.GetFullPath(f, path);
				}
			}
			else if (rule.StartsWith("@main:"))
			{
				string f = rule[6..].Trim();
				if (f.Length > 0)
				{
					MainFile = Path.GetFullPath(f, path);
				} 
			}
			else if (rule.StartsWith("@spaces:"))
			{
				string f = rule[8..].Trim();
				if (f.Length > 0 && int.TryParse(f, out int r))
				{
					Spaces = r;
				}
			}
			else if (string.Equals(rule, "--disable", StringComparison.OrdinalIgnoreCase))
			{
				Disable = true;
			}
		}
	}
}
