using System;
using System.Reflection;
using Dts.Properties;

namespace Dts
{
	internal class Program
	{
		static void Main(string[] args)
		{
			AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
			
			if (ArgsParser.IsHelp(args))
			{
				Console.WriteLine(Resources.help);
			}
			else if (ArgsParser.IsExtendedHelp(args))
			{
				string nl = Environment.NewLine;
				Console.WriteLine($"{Resources.help}{nl}{nl}{nl}{Resources.detail}");
			}
			else if (ArgsParser.IsVersion(args))
			{
				string v = typeof(Program).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()!.InformationalVersion;
				Console.WriteLine(v);
			}
			else if (ArgsParser.IsFile(args))
			{
				var (files, outputFile) = ArgsParser.ParseForIsFile(args);
				Extractor.Extract(files, outputFile);
				Console.WriteLine("执行成功。");
			}
			else if (ArgsParser.IsDir(args))
			{
				var (dir, settingFile) = ArgsParser.ParseForIsDir(args);
				Extractor.Extract(dir, settingFile);
				Console.WriteLine("执行成功。");
			}
			else
			{
				WriteErrorMessage("未知的命令，请使用“-h”参数查看帮助。");
			}
		}
		
		private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			WriteErrorMessage("程序错误。");
		}

		internal static void WriteErrorMessage(string msg)
		{
			Console.ForegroundColor = ConsoleColor.Red;
			Console.Error.WriteLine(msg);
			Console.ResetColor();

			Environment.Exit(0);
		}
	}
}