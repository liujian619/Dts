using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Dts
{
	internal static class Joiner
	{
		private const string ReferenceComment = "/// <reference";
		private const string Placeholder = "/*dts.join.placeholder*/";

		public static void Join(string settingFile) 
		{
			FileUtil.ExistsFile(settingFile);

			string[]? rules = FileUtil.ReadLines(settingFile);
			if (rules is null) { Environment.Exit(0); }

			JoinParser parser = new();
			parser.ParseRules(Path.GetDirectoryName(settingFile), rules);

			if (parser.Disable)
			{
				return;
			}

			var (begin, end) = SplitMain(parser.MainFile);
			FileUtil.AppendLines(parser.OutputFile, begin, true);

			AppendNewlines(parser.OutputFile);
			foreach (var file in parser.Files)
			{
				string[] lines = FileUtil.ReadLines(file);
				lines = RemoveReferenceComment(lines);
				lines = AddSpaces(lines, parser.Spaces);
				FileUtil.AppendLines(parser.OutputFile, lines);
				AppendNewlines(parser.OutputFile);
			}

			FileUtil.AppendLines(parser.OutputFile, end);
		}

		private static (string[], string[]) SplitMain(string mainFile) 
		{
			string[] lines = FileUtil.ReadLines(mainFile);
			lines = RemoveReferenceComment(lines);

			int i = lines.ToList().FindIndex(o => string.Equals(o.Trim(), Placeholder, StringComparison.OrdinalIgnoreCase));
			if (i >= 0)
			{
				return (lines[..i], lines[(i + 1)..]);
			}
			else 
			{
				return (Array.Empty<string>(), Array.Empty<string>());
			}
		}

		private static string[] RemoveReferenceComment(IEnumerable<string> lines) 
		{
			return lines.Where(o => !o.Trim().StartsWith(ReferenceComment, StringComparison.OrdinalIgnoreCase)).ToArray();
		}

		private static void AppendNewlines(string file)
		{
			FileUtil.AppendLines(file, new[] { Environment.NewLine });
		}

		private static string[] AddSpaces(string[] lines, int spaces)
		{
			if (spaces > 0)
			{
				return lines.Select(o => new string(' ', spaces) + o).ToArray();
			}
			else
			{
				return lines;
			}
		}
	}
}
