using System;
using Dts.Properties;

namespace Dts
{
	internal class Program
	{
		static void Main(string[] args)
		{
#if DEBUG
			// args = new[] { "-h" };
			args = new[] { @"D:\code\Moty.Dev\Dao\wwwroot" };
#endif

			AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

			if (args.Length == 1)
			{
				if (args[0].ToUpper() == "-H")
				{
					WriteHelp();
				}
				else
				{
					Extractor.Run(args[0].Trim());
					Console.WriteLine("Done.");
				}
			}
			else if (args.Length == 2)
			{
				Extractor.Run(args[0].Trim(), args[1].Trim());
				Console.WriteLine("Done.");
			}
			else
			{
				Error("参数错误，请使用 -h 参数查看帮助。");
				return;
			}

#if DEBUG
			Console.Read();
#endif
		}
		
		private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			Error("程序错误。");
			Environment.Exit(0);
		}

		private static void WriteHelp()
		{
			Console.ForegroundColor = ConsoleColor.Cyan;
			Console.WriteLine("Dts: 用于从文件中提取特定格式的 JsDoc 文档注释以生成 .d.ts 文件。");
			Console.ResetColor();
			Console.WriteLine();

			Console.WriteLine(Resources.help);
			Console.WriteLine();
		}

		internal static void Error(string msg)
		{
			Console.ForegroundColor = ConsoleColor.Red;
			Console.Error.WriteLine(msg);
			Console.ResetColor();
		}
	}
}