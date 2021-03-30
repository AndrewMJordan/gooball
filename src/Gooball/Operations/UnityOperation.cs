using CommandLine;
using System;
using System.IO;
using System.Text.RegularExpressions;

namespace Gooball
{

	[Verb("unity", HelpText = "Project-independent Unity commands.")]
	internal class UnityOptions
	{
		[Value(0, Required = true, MetaName = "command", HelpText = "The editor action to perform.")]
		public string Command { get; set; }
		[Value(1, Required = false, Default = "./", MetaName = "project-path", HelpText = "The path to the project.")]
		public string ProjectPath { get; set; }

		// List-Installs options
		[Option("install-path", HelpText = "The editor installation location.")]
		public string Path { get; set; }
	}

	internal static class UnityOperation
	{

		[Operation(typeof(UnityOptions))]
		public static void OnParse(UnityOptions options)
		{
			switch (options.Command)
			{
				case "run":
					Run();
					break;
				case "list-installs":
					ListEditorInstalls();
					break;
			}

			void Run()
			{
				var unityArgs = new UnityArgs(Interpreter.Instance.PassthroughArgs);
				var exitCode = new UnityRunner(unityArgs).Run();

				Environment.Exit(exitCode);
			}

			void ListEditorInstalls()
			{
				UnityInstallationHelper helper;
				if (options.Path is null)
				{
					helper = new UnityInstallationHelper();
				}
				else
				{
					helper = new UnityInstallationHelper(options.Path);
				}
				var editors = helper.GetInstalledEditors();
				Console.WriteLine(string.Join(Environment.NewLine, editors));
			}
		}
	}
}
