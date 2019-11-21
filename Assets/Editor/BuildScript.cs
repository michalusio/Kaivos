using System.Collections.Generic;
using UnityEditor;

public class ScriptBatch 
{
    private static string[] GetAllScenes()
    {
        List<string> scenes = new List<string>();
        foreach(EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
        {
            if (scene.enabled) scenes.Add(scene.path);
        }
        return scenes.ToArray();
    }

    public static void BuildGameDebug()
    {
        BuildPipeline.BuildPlayer(GetAllScenes(), "../Kaivos-Master-Build/Debug/Kaivos.exe", BuildTarget.StandaloneWindows64, BuildOptions.Development);
    }

    public static void BuildGameRelease()
    {
        BuildPipeline.BuildPlayer(GetAllScenes(), "../Kaivos-Master-Build/Release/Kaivos.exe", BuildTarget.StandaloneWindows64, BuildOptions.None);
    }
}
