using CommandLine;
using System;

namespace Gooball
{

	[Verb("project", HelpText = "Commands for working with Unity projects.")]
	internal class ProjectOptions
	{
		[Value(0, Required = false, Default = "./", HelpText = "The path to the Unity project.")]
		public string ProjectPath { get; set; }
		[Value(1, Required = true, MetaName = "command", HelpText = "The project action to perform.")]
		public string Command { get; set; }

		// Run, Test options
		[Option("editor", HelpText = "A specific version of the Unity editor to use")]
		public string EditorPath { get; set; }

		// Get/Set options
		[Value(2, MetaName = "key", HelpText = "The key to use in get/set.")]
		public string Key { get; set; }

		[Value(3, MetaName = "value", HelpText = "The value to use in set.")]
		public string Value { get; set; }
	}

	internal static class ProjectOperation
	{

		[Operation(typeof(ProjectOptions))]
		public static void OnParse(ProjectOptions options)
		{
			var projectPath = options.ProjectPath;
			var project = Project.Read(projectPath);

			switch (options.Command)
			{
				case "open":
					Open();
					break;
				case "build":
					Build();
					break;
				case "test":
					Test();
					break;
				case "get-version":
					PrintProjectVersion();
					break;
				case "get-editor-version":
					PrintProjectEditorVersion();
					break;
				case "get":
					Print();
					break;
				case "set":
					Set();
					break;
			}

			void Open()
			{
				var unityArgs = new UnityArgs(Interpreter.Instance.PassthroughArgs);
				var exitCode = new UnityRunner(unityArgs).Open(project);

				Environment.Exit(exitCode);
			}

			void Build()
			{
				var unityArgs = new UnityArgs(Interpreter.Instance.PassthroughArgs);
				var exitCode = new UnityRunner(unityArgs).Build(project);

				Environment.Exit(exitCode);
			}

			void Test()
			{
				var unityArgs = new UnityArgs(Interpreter.Instance.PassthroughArgs);
				var exitCode = new UnityRunner(unityArgs).Test(project);

				Environment.Exit(exitCode);
			}

			void Print()
			{
				string value;
				switch (options.Key)
				{
					case "m_EditorVersion":
					case "m_EditorVersionWithRevision":
						value = project.GetProjectVersionSetting(options.Key);
						break;
					default:
						value = project.GetPlayerSetting(options.Key);
						break;
				}
				Console.WriteLine(value);
			}

			void Set()
			{
				switch (options.Key)
				{
					case "m_EditorVersion":
					case "m_EditorVersionWithRevision":
						project.SetProjectVersionSetting(options.Key, options.Value);
						break;
					default:
						project.SetPlayerSetting(options.Key, options.Value);
						break;
				}

				Project.Write(project);
			}

			void PrintProjectVersion()
			{
				Console.WriteLine(project.Version);
			}

			void PrintProjectEditorVersion()
			{
				Console.WriteLine(project.EditorVersion);
			}
		}
	}
}
