using System;
using FileSystemSearcher;

namespace Dts
{
	internal sealed class OptionParser : RuleParser
	{
		internal const string EXT = ".d.ts";

		/// <summary>
		/// 是否禁用。
		/// </summary>
		public bool Disable { get; private set; } = false;

		/// <summary>
		/// 是否合并。
		/// </summary>
		public bool Merge { get; private set; } = false;

		/// <summary>
		/// 输出文件。
		/// </summary>
		public string Output { get; private set; } = "reference.d.ts";


		protected override void ParseCustomRule(string path, string rule)
		{
			if (string.Equals(rule, ">>"))
			{
				string name = rule[2..].Trim();
				if (!name.EndsWith(EXT, StringComparison.OrdinalIgnoreCase))
				{
					name += EXT;
				}
				Output = name;
			}
			else if (string.Equals(rule, "--disable", StringComparison.OrdinalIgnoreCase))
			{
				Disable = true;
			}
			else if (string.Equals(rule, "--merge", StringComparison.OrdinalIgnoreCase))
			{
				Merge = true;
			}
		}
	}
}
