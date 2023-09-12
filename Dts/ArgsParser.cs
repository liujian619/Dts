using System;
using System.Linq;

namespace Dts
{
	internal class ArgsParser
	{
		private static readonly string[] Help = new[] { "-h", "--help" };
		private static readonly string[] ExtendedHelp = new[] { "-H", "--extended-help" };
		private static readonly string[] Version = new[] { "-v" };

		public static bool IsHelp(string[] args) 
		{
			return args.Length == 1 && Help.Contains(args[0]);
		}
		
		public static bool IsExtendedHelp(string[] args) 
		{
			return args.Length == 1 && ExtendedHelp.Contains(args[0]);
		}

		public static bool IsVersion(string[] args)
		{
			return args.Length == 1 && Version.Contains(args[0]);
		}

		public static bool IsFile(string[] args) 
		{
			return args.Length >= 2 && args[0] == "-f";
		}

		public static bool IsDir(string[] args)
		{
			return (args.Length == 2 || args.Length == 3) && args[0] == "-d";
		}


		public static (string[], string?) ParseForIsFile(string[] args)
		{
			string[] arr = args[1..];

			string[]? files = null;
			string? outputFile = null;

			int i = Array.IndexOf(arr, "-m");
			if (i >= 0)
			{
				if (i == 0)
				{
					Program.WriteErrorMessage("未指定输入文件，请使用“-h”参数查看帮助。");
				}
				else if (i == arr.Length - 1)
				{
					Program.WriteErrorMessage("未指定合并后的输出文件，请使用“-h”参数查看帮助。");
				}
				else if (i < arr.Length - 2)
				{
					Program.WriteErrorMessage("多次指定合并后的输出文件，请使用“-h”参数查看帮助。");
				}

				files = arr[..i];
				outputFile = arr[^1];
			}
			else
			{
				files = arr;
			}

			foreach (var f in files!)
			{
				FileUtil.ExistsFile(f);
			}

			return (files, outputFile);
		}

		public static (string, string?) ParseForIsDir(string[] args)
		{
			var (dir, settingFile) = (args[1], args.Length == 3 ? args[2] : null);
			FileUtil.ExistsDir(dir);
			if (settingFile is not null)
			{
				FileUtil.ExistsFile(settingFile);
			}
			return (dir, settingFile);
		}
	}
}
