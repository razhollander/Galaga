using System;
using UnityEditor;
using UnityEngine;

public class BuildScript : MonoBehaviour
{
    private const string BuildPathName = "-buildPath";
    private const string BuildScriptName = "BuildScript.PerformBuild";
    private const string BuildTargetName = "-buildTarget";
    private const string IsDevelopBuildName = "-isDevelopBuild";
    private const string ScriptingDefineSymbolsName = "-scriptingDefineSymbols";
    private const string BrunchName = "-brunchName";
    private const string AddressableURLName = "-addressableURL";
    
    private const string IOSBuildBuildTarget = "iOS";
    private const string AndroidBuildTarget = "Android";
    private const string StandaloneMacBuildTarget = "OSXUniversal";

    private const string AddressableURL = "https://amazonaws.com/";
    
    private static readonly string[] Scenes =
    {
        "Assets/Scenes/game.unity"
    };
    
    [MenuItem("Tools/Build/Android")]
    public static void BuildAndroid()
    {
        PerformBuildWithParams(AndroidBuildTarget, "1", "../Builds", true,"DEBUG_ENABLED", string.Empty, string.Empty);
    }

    [MenuItem("Tools/Build/iOS")]
    public static void BuildiOS()
    {
        PerformBuildWithParams(IOSBuildBuildTarget, "1", "../Builds", true,"DEBUG_ENABLED",string.Empty,string.Empty);
    }
    
    [MenuItem("Tools/Build/MacOS")]
    public static void BuildMacOS()
    {
        PerformBuildWithParams(StandaloneMacBuildTarget, "1", "../Builds", true,"DEBUG_ENABLED", string.Empty,string.Empty);
    }

    public static void PerformBuild()
    {
        string buildTarget = GetParam(BuildTargetName);
        string buildVersion = GetParam(BuildScriptName);
        string buildPath = GetParam(BuildPathName);
        string scriptingDefineSymbols = GetParam(ScriptingDefineSymbolsName);
        bool isDevelopmentBuild = false;
        string addressableUrl = GetParam(AddressableURLName);
        string brunchName = GetParam(BrunchName);
        
        try
        {
            isDevelopmentBuild = Convert.ToBoolean(GetParam(IsDevelopBuildName));
        }
        catch
        {
            Debug.Log($"{IsDevelopBuildName} param missed");
        }

        if (buildTarget == string.Empty || buildVersion == string.Empty || buildPath == string.Empty)
        {
            throw new Exception($"wrong params, buildTarget: {buildTarget}, buildVersion: {buildVersion}, buildPath: {buildPath}");
        }

        PerformBuildWithParams(buildTarget, buildVersion, buildPath, isDevelopmentBuild, scriptingDefineSymbols, addressableUrl, brunchName);
    }

    private static void PerformBuildWithParams(string buildTarget, string buildVersion, string buildPath, bool isDevelopmentBuild, string scriptingDefineSymbols, string addressableUrl, string brunchName)
    {
        Debug.Log($"PerformBuild, version: {buildVersion}, platform: {buildTarget}, path: {buildPath}, isDevelopmentBuild: {isDevelopmentBuild}, scriptingDefineSymbols: {scriptingDefineSymbols}, addressableUrl: {addressableUrl}, brunchName: {brunchName} ");

        switch (buildTarget)
        {
            case AndroidBuildTarget:
                PerformAndroidBuild(buildVersion, buildPath, isDevelopmentBuild, scriptingDefineSymbols);

                break;
            case IOSBuildBuildTarget:
                PerformIOSBuild(buildVersion, buildPath, isDevelopmentBuild, scriptingDefineSymbols);

                break;
            case StandaloneMacBuildTarget:
                PerformMacStandaloneBuild(buildVersion, buildPath, isDevelopmentBuild, scriptingDefineSymbols);

                break;
            default:
                Debug.LogError("unknown platform");

                break;
        }
    }

    private static void PerformAndroidBuild(string buildVersion, string buildPath, bool isDevelopmentBuild, string newScriptingDefineSymbols)
    {
        string rawVersion = "0000" + Application.version.Replace(".", "");
        rawVersion += (int.Parse(buildVersion) % 1000).ToString("000");

        PlayerSettings.bundleVersion = rawVersion;
        
        PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.IL2CPP);
        
        var buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.target = BuildTarget.Android;
        buildPlayerOptions.targetGroup = BuildTargetGroup.Android;
        buildPlayerOptions.locationPathName = $"{buildPath}{buildVersion}.apk";
        if (newScriptingDefineSymbols != String.Empty)
        {
            buildPlayerOptions.extraScriptingDefines = newScriptingDefineSymbols.Split(';');
        }
        if (isDevelopmentBuild)
        {
            buildPlayerOptions.options = BuildOptions.Development;
        }

        buildPlayerOptions.scenes = Scenes;

        BuildPipeline.BuildPlayer(buildPlayerOptions);
    }

    private static void PerformIOSBuild(string buildVersion, string buildPath, bool isDevelopmentBuild, string newScriptingDefineSymbols)
    {
        PlayerSettings.bundleVersion = Application.version;
        PlayerSettings.iOS.buildNumber = buildVersion;
        
        var buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.target = BuildTarget.iOS;
        buildPlayerOptions.targetGroup = BuildTargetGroup.iOS;
        buildPlayerOptions.locationPathName = $"{buildPath}";
        if (newScriptingDefineSymbols != String.Empty)
        {
            buildPlayerOptions.extraScriptingDefines = newScriptingDefineSymbols.Split(';');
        }

        if (isDevelopmentBuild)
        {
            buildPlayerOptions.options = BuildOptions.Development;
        }

        buildPlayerOptions.scenes = Scenes;

        BuildPipeline.BuildPlayer(buildPlayerOptions);
    }

    private static void PerformMacStandaloneBuild(string buildVersion, string buildPath, bool isDevelopmentBuild, string scriptingDefineSymbols )
    {
        PlayerSettings.bundleVersion = buildVersion;

        var buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.target = BuildTarget.StandaloneOSX;
        buildPlayerOptions.targetGroup = BuildTargetGroup.Standalone;
        buildPlayerOptions.locationPathName = $"{buildPath}{buildVersion}.app";
        if (isDevelopmentBuild)
        {
            buildPlayerOptions.options = BuildOptions.Development;
        }
        if (scriptingDefineSymbols != String.Empty)
        {
            buildPlayerOptions.extraScriptingDefines = scriptingDefineSymbols.Split(';');
        }
        buildPlayerOptions.scenes = Scenes;

        BuildPipeline.BuildPlayer(buildPlayerOptions);
    }

    private static string GetParam(string paramName)
    {
        string[] commandLineArgs = Environment.GetCommandLineArgs();
        string returnValue = string.Empty;

        for (int i = 0; i < commandLineArgs.Length; i++)
        {
            if (commandLineArgs[i] == paramName)
            {
                i++;

                if (commandLineArgs.Length < i)
                {
                    break;
                }

                returnValue = commandLineArgs[i];

                break;
            }
        }

        return returnValue;
    }
}