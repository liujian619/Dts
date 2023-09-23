using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Dts
{
	internal static class FileUtil
	{
		public static string[] ReadLines(string file)
		{
			ExistsFile(file);

			List<string> lines = new();
			using var sr = new StreamReader(File.Open(file, FileMode.Open, FileAccess.Read, FileShare.Read), Encoding.UTF8, true);
			while (!sr.EndOfStream)
			{
				lines.Add(sr.ReadLine() ?? string.Empty);
			}

			return lines.ToArray();
		}

		public static void AppendLines(string file, string[] lines, bool overwrite = false)
		{
			if (overwrite)
				File.WriteAllLines(file, lines);
			else
				File.AppendAllLines(file, lines);
		}

		public static string ReadAllText(string file)
		{
			ExistsFile(file);

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

		internal static void ExistsFile(string file)
		{
			if (!File.Exists(file))
			{
				Program.WriteErrorMessage($"文件“{file}”不存在或已被删除。");
			}
		}

		internal static void ExistsDir(string dir)
		{
			if (!Directory.Exists(dir))
			{
				Program.WriteErrorMessage($"目录“{dir}”不存在或已被删除。");
			}
		}
	}
}
