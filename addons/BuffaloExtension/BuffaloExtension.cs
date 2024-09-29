#if TOOLS
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Godot;

[Tool]
public partial class BuffaloExtension : EditorPlugin
{
	private (string, string, Script)[]		_scripts;
	private Texture2D						_texture;
	
	public BuffaloExtension() {
		_scripts = GetGlobalClassScripts();
	}
	
	public override void _EnterTree() {
		_texture = GD.Load<Texture2D>("res://addons/BuffaloExtension/node-icon.png");
		
		foreach (var tuple in _scripts) {
			AddCustomType(tuple.Item1, tuple.Item2, tuple.Item3, _texture);
		}
	}

	public override void _ExitTree() {
		foreach (var tuple in _scripts) {
			RemoveCustomType(tuple.Item1);
		}
	}

	private (string, string, Script)[] GetGlobalClassScripts() {
		var scriptFiles = GetScriptFiles("res://code/");
		
		var globalClassScripts = AppDomain.CurrentDomain
			.GetAssemblies()
			.SelectMany(a => a.GetTypes())
			.Where(t => t.IsDefined(typeof(GlobalClassAttribute), false))
			.ToArray();
		
		var scripts = new List<(string, string, Script)>();
		
		foreach (var type in globalClassScripts) {
			string typeName = type.Name;
			string baseType = type.BaseType?.Name ?? "Node";
			string scriptPath = scriptFiles.FirstOrDefault(path => path.EndsWith($"{typeName}.cs"));

			var script = GD.Load<Script>(scriptPath);
		
			if (script != null) {
				scripts.Add((typeName, baseType, script));
				GD.Print($"Added {scriptPath}");
			} else {
				GD.PrintErr($"Failed to load script for {typeName} at path {scriptPath}");
			}
		}
		
		return scripts.ToArray();
	}

	private List<string> GetScriptFiles(string directory) {
		var absolutePath = ProjectSettings.GlobalizePath(directory);
		var scriptFiles = new List<string>();
		
		if (Directory.Exists(absolutePath)) {
			string[] files = Directory.GetFiles(absolutePath, "*.cs", SearchOption.AllDirectories);

			foreach (string file in files) {
				string relativePath = ProjectSettings.LocalizePath(file);
				scriptFiles.Add(relativePath);
			}
		}

		return scriptFiles;
	}
}
#endif
