using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System;

delegate void DTestBuild();
delegate void DOnTouch();

public class Jenkins
{
    static string[] SCENES = FindEnabledEditorScenes();
    static string APP_NAME = "MetaBox";

    [UnityEditor.MenuItem("TestBuild/BuildStart", false, 1)]
    static void PerformBuild()
    {

        string target_filename = "/Build/" + APP_NAME + ".apk";
        SCENES = FindEnabledEditorScenes();

        GenericBuild(SCENES, target_filename, BuildTarget.Android, BuildOptions.None);
    }
    private static string[] FindEnabledEditorScenes()
    {
        List<string> EditorScenes = new List<string>();
        foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
        {
            if (!scene.enabled) continue;
            EditorScenes.Add(scene.path);
        }

        return EditorScenes.ToArray();
    }

    static void GenericBuild(string[] scenes, string target_filename, BuildTarget build_target, BuildOptions build_options)
    {
        if (EditorUserBuildSettings.activeBuildTarget != BuildTarget.Android)
            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, BuildTarget.Android);

        BuildPipeline.BuildPlayer(scenes, Directory.GetCurrentDirectory() + target_filename, build_target, build_options);
    }
}
