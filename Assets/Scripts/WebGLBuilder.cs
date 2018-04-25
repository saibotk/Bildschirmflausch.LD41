using UnityEngine;

public class WebGLBuilder : MonoBehaviour
{
	public static void BuildWebGL()
	{
		string[] scenes = { "Assets/main.unity" };
		Debug.Log("BLUUUUUUUUUUUUUU");
		BuildSrcipt.BuildAll();
		//UnityEditor.BuildPlayerOptions options = new BuildPlayerOptions();
	}
}