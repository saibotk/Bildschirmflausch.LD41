using UnityEditor;
using UnityEngine;
using System;
using System.IO;
using System.IO.Compression;
using System.Collections;
using System.Text;


public class BuildSrcipt
{
    static string[] scenes = { "Assets/Scenes/TitleScene.unity", "Assets/Scenes/CodeDEV.unity" };
	static string name = "DungeonDrifter";

    [MenuItem("Build/Build WebGL")]
    static void BuildWebGL()
    {
		BuildPlatform("Web", BuildTarget.WebGL);
    }

    [MenuItem("Build/Build Windows")]
    static void BuildWindows()
    {
		BuildPlatform("Windows", BuildTarget.StandaloneWindows64);
    }

    [MenuItem("Build/Build Linux")]
    static void BuildLinux()
    {
		//System.Diagnostics.Process.Start("mkdir /home/piegames/Documents/GitHub/Bildschirmflausch.LD41/teghsfdt");
		//Debug.Log("fdsghjk");
		BuildPlatform("Linux", BuildTarget.StandaloneLinux64);
	}

	[MenuItem("Build/Build All")]
	public static void BuildAll()
	{
		BuildLinux();
		BuildWindows();
		BuildWebGL();
	}

	public static void BuildPlatform(String platformName, BuildTarget target) {
		if (!Directory.Exists("./Build"))
			Directory.CreateDirectory("./Build");
		if (Directory.Exists("./" + name + "_" + platformName))
		    Directory.Delete("./" + name + "_" + platformName, true);
		if (File.Exists("./Build/DungeonDrifter_" + platformName + ".zip"))
    		File.Delete("./Build/DungeonDrifter_" + platformName + ".zip");
		BuildPipeline.BuildPlayer(scenes, "./" + name + "_" + platformName + "/" + name, target, BuildOptions.None);
		//System.Diagnostics.Process.Start("zip -r Build/DungeonDrifter_" + platformName + ".zip " + platformName + "_" + platformName);
	}
}
