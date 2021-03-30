using System.IO;
using System.Text.RegularExpressions;

namespace Gooball
{

	/// <summary>
	/// A Unity project.
	/// </summary>
	public class Project
	{
		public string ProductName
		{
			get => GetPlayerSetting("productName");
			set => SetPlayerSetting("productName", value);
		}
		public string CompanyNamy
		{
			get => GetPlayerSetting("companyName");
			set => SetPlayerSetting("companyName", value);
		}
		public string Version
		{
			get => GetPlayerSetting("bundleVersion");
			set => SetPlayerSetting("bundleVersion", value);
		}
		public string EditorVersion
		{
			get => GetProjectVersionSetting("m_EditorVersion");
			set => SetProjectVersionSetting("m_EditorVersion", value);
		}

		private string ProjectSettingsContent;
		private string ProjectVersionContent;

		public readonly string Path;

		private Project(string projectPath)
		{
			Path = projectPath;

		}

		public static Project Read(string path)
		{
			var projectSettingsFilePath = System.IO.Path.Join(path, "ProjectSettings", "ProjectSettings.asset");
			var projectVersionFilePath = System.IO.Path.Join(path, "ProjectSettings", "ProjectVersion.txt");

			var project = new Project(path)
			{
				ProjectSettingsContent = File.ReadAllText(projectSettingsFilePath),
				ProjectVersionContent = File.ReadAllText(projectVersionFilePath)
			};

			return project;
		}

		public static void Write(Project project)
		{
			var projectSettingsFilePath = System.IO.Path.Combine(project.Path, "ProjectSettings", "ProjectSettings.asset");
			var projectVersionFilePath = System.IO.Path.Join(project.Path, "ProjectSettings", "ProjectVersion.txt");

			File.WriteAllText(projectSettingsFilePath, project.ProjectSettingsContent);
			File.WriteAllText(projectVersionFilePath, project.ProjectVersionContent);
		}

		public string GetPlayerSetting(string key)
		{
			var regex = YamlRegex(key);
			var match = regex.Match(ProjectSettingsContent);

			return match.Success ? match.Groups[1].Value : null;
		}

		public void SetPlayerSetting(string key, string value)
		{
			var regex = YamlRegex(key);

			ProjectSettingsContent = regex.Replace(ProjectSettingsContent, YamlExpression(key, value));
		}

		public string GetProjectVersionSetting(string key)
		{
			var regex = YamlRegex(key);
			var match = regex.Match(ProjectVersionContent);

			return match.Success ? match.Groups[1].Value : null;
		}

		public void SetProjectVersionSetting(string key, string value)
		{
			var regex = YamlRegex(key);

			ProjectVersionContent = regex.Replace(ProjectVersionContent, YamlExpression(key, value));
		}

		private static Regex YamlRegex(string key, RegexOptions options = RegexOptions.Multiline) => new Regex($@"{key}:\s*([\w\s/._()]+)$", options);

		private static string YamlExpression(string key, string value) => $"{key}: {value}";
	}
}
