using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Dts
{
	internal static class FileUtil
	{
		public static string[] ReadLines(string file)
		{
			Check(file);

			List<string> lines = new();
			using var sr = new StreamReader(File.Open(file, FileMode.Open, FileAccess.Read, FileShare.Read), Encoding.UTF8, true);
			while (!sr.EndOfStream)
			{
				lines.Add(sr.ReadLine() ?? string.Empty);
			}

			return lines.ToArray();
		}

		public static string ReadAllText(string file)
		{
			Check(file);

			using var sr = new StreamReader(File.OpenRead(file), Encoding.UTF8, true);
			return sr.ReadToEnd();
		}


		public static void WriteAllText(string file, string content, bool append)
		{
			FileMode mode = append ? FileMode.Append : FileMode.OpenOrCreate;
			using var sw = new StreamWriter(File.Open(file, mode, FileAccess.Write, FileShare.None), Encoding.UTF8);
			sw.Write(content);
			sw.Flush();
		}

		private static void Check(string file)
		{
			if (!File.Exists(file))
			{
				Program.Error($"文件“{file}”不存在或已被删除。");
			}
		}
	}
}
