﻿using System.Collections.Generic;
using UnityEditor;

public class ScriptBatch 
{
    public static void BuildGame()
    {
        List<string> scenes = new List<string>();
        foreach(EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
        {
            if(scene.enabled) scenes.Add(scene.path);
        }
        
        BuildPipeline.BuildPlayer(scenes.ToArray(), "%temp%/Kaivos-Master-Build/Bin/Kaivos.exe", BuildTarget.StandaloneWindows64, BuildOptions.None);
    }
}
