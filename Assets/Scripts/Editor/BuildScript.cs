using UnityEditor;
using UnityEngine;

public class BuildSrcipt
{
    static string[] scenes = { "Assets/Scenes/CodeDEV.unity", "Assets/Scenes/TitleScene.unity" };
	static string name = "DungeonDrifter";

    [MenuItem("Build/Build WebGL")]
    static void BuildWebGL()
    {
		BuildPipeline.BuildPlayer(scenes, "./" + name + "_Web/" + name, BuildTarget.WebGL, BuildOptions.None);
    }

    [MenuItem("Build/Build Windows")]
    static void BuildWindows()
    {
		BuildPipeline.BuildPlayer(scenes, "./" + name + "_Windows/" + name, BuildTarget.StandaloneWindows64, BuildOptions.None);
    }

    [MenuItem("Build/Build Linux")]
    static void BuildLinux()
    {
		BuildPipeline.BuildPlayer(scenes, "./" + name + "_Linux/" + name, BuildTarget.StandaloneLinux64, BuildOptions.None);
	}

    [MenuItem("Build/Build All")]
    public static void BuildAll()
    {
		BuildLinux();
		BuildWindows();
		BuildWebGL();
    }
}
