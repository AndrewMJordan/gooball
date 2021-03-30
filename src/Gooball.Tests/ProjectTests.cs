using NUnit.Framework;
using System;
using System.IO;

namespace Gooball.Tests
{

	public class ProjectTests : GooballTests
	{
		private const string PROJECT_NAME = "Project";
		private const string PROJECT_VERSION = "1.2.3";
		private const string EDITOR_VERSION = "2020.1.3f1";

		[Test]
		public void GetProjectVersion()
		{
			Action action = () => Interpreter.Instance.Run(new string[] { "project", "get-version", ExampleProjectRoot });

			AssertConsoleEquals(PROJECT_VERSION, action);
		}

		[Test]
		public void GetProjectInfoImplicit()
		{
			Action action = () =>
			{
				Directory.SetCurrentDirectory(ExampleProjectRoot);
				Interpreter.Instance.Run(new string[] { "project", "get-version" });
			};

			AssertConsoleEquals(PROJECT_VERSION, action);
		}

		[Test]
		public void SetName()
		{
			Interpreter.Instance.Run(new string[] { "project", ExampleProjectRoot, "set", "productName", "MyProject" });

			var	project = Project.Read(ExampleProjectRoot);
			Assert.AreEqual("MyProject", project.ProductName);
		}

		[Test]
		public void SetCompany()
		{
			Interpreter.Instance.Run(new string[] { "project", ExampleProjectRoot, "set", "company", "MyCompany" });

			var project = Project.Read(ExampleProjectRoot);
			Assert.AreEqual("MyCompany", project.ProductName);
		}

		[Test]
		public void GetEditorVersion()
		{
			var project = Project.Read(ExampleProjectRoot);

			Interpreter.Instance.Run(new string[] { "project", "get-editor-version", ExampleProjectRoot });

			Assert.AreEqual(EDITOR_VERSION, project.EditorVersion);
		}
	}
}
