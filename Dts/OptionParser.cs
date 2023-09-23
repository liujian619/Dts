using System;
using FileSystemSearcher;

namespace Dts
{
	internal sealed class OptionParser : RuleParser
	{
		internal static OptionParser Create(string[] files, string? outputFile = null)
		{
			var parser = new OptionParser();

			parser.AddFiles(files);
			if (outputFile is not null)
			{
				parser.MergeOutput = true;
				parser.OutputFile = outputFile;
			}

			return parser;
		}

		/// <summary>
		/// 是否禁用。
		/// </summary>
		public bool Disable { get; private set; } = false;

		/// <summary>
		/// 是否合并输出。
		/// </summary>
		public bool MergeOutput { get; private set; } = false;

		/// <summary>
		/// 一个文件名，合并所有输出到该文件。
		/// </summary>
		public string OutputFile { get; private set; } = "lib.d.ts";

		/// <summary>
		/// 是否合并输入。
		/// </summary>
		public bool MergeInput { get; private set; } = false;

		/// <summary>
		/// 一个文件名，合并所有输入到该文件。
		/// </summary>
		public string InputFile { get; private set; } = "lib.js";


		protected override void ParseCustomRule(string path, string rule)
		{
			if (string.Equals(rule, "--mergeInput", StringComparison.OrdinalIgnoreCase))
			{
				MergeInput = true;
			}
			else if (rule.StartsWith("<<"))
			{
				string f = rule[2..].Trim();
				if (f.Length > 0)
				{
					InputFile = f;
				}
			}
			else if (string.Equals(rule, "--mergeOutput", StringComparison.OrdinalIgnoreCase))
			{
				MergeOutput = true;
			}
			else if (rule.StartsWith(">>"))
			{
				string f = rule[2..].Trim();
				if (f.Length > 0)
				{
					OutputFile = f;
				}
			}
			else if (string.Equals(rule, "--disable", StringComparison.OrdinalIgnoreCase))
			{
				Disable = true;
			}
		}
	}
}
