using UnityEngine;
using UnityEditor;
using System;
using System.Threading;
using UnityEditor.SceneManagement;
using UnityEditor.VersionControl;

public class AutoSave : EditorWindow
{
    private bool autoSaveScene = true;
    private bool showMessage = true;
    private bool isLaunchProcess = true;
    private bool isStarted = false;
    private int intervalScene;
    private DateTime lastSaveTimeScene = DateTime.Now;

    private string projectPath = Application.dataPath;
    private string scenePath;

    [MenuItem("Window/AutoSave")]
    static void Init()
    {
        AutoSave saveWindow = (AutoSave)EditorWindow.GetWindow(typeof(AutoSave));
        saveWindow.Show();
        Debug.Log("启动自动存储");
        //Thread thread = new Thread(new ParameterizedThreadStart((object obj) => 
        //{
        //    AutoSave autoSave = obj as AutoSave;
        //    Debug.Log("启动自动存储线程");

        //    autoSave.Auto();
        //    Thread.Sleep(1000);
        //}));
        //thread.Start(saveWindow);
    }

    void OnGUI()
    {
        GUILayout.Label("Info:", EditorStyles.boldLabel);
        EditorGUILayout.LabelField("Saving to:", "" + projectPath);
        EditorGUILayout.LabelField("Saving scene:", "" + scenePath);
        GUILayout.Label("Options:", EditorStyles.boldLabel);
        autoSaveScene = EditorGUILayout.BeginToggleGroup("Auto save", autoSaveScene);
        intervalScene = EditorGUILayout.IntSlider("Interval (minutes)", intervalScene, 1, 10);
        if (isStarted)
        {
            EditorGUILayout.LabelField("Last save:", "" + lastSaveTimeScene);
        }
        EditorGUILayout.EndToggleGroup();
        showMessage = EditorGUILayout.BeginToggleGroup("Show Message", showMessage);
        EditorGUILayout.EndToggleGroup();
        //isLaunchProcess = EditorGUILayout.BeginToggleGroup("Is Launch Process", isLaunchProcess);
        //EditorGUILayout.EndToggleGroup();
    }

    void Auto()
    {
        scenePath = EditorApplication.currentScene;
        //Debug.Log(scenePath);
        //scenePath = EditorSceneManager.GetActiveScene().ToString();
        if (autoSaveScene)
        {
            if (DateTime.Now.Minute >= (lastSaveTimeScene.Minute + intervalScene) || DateTime.Now.Minute == 59 && DateTime.Now.Second == 59)
            {
                saveScene();
            }
        }
        else
        {
            isStarted = false;
        }
    }
    void Update()
    {
        Auto();
    }

    void saveScene()
    {
        EditorApplication.SaveScene(scenePath);
        lastSaveTimeScene = DateTime.Now;
        isStarted = true;
        if (showMessage)
        {
            Debug.Log("AutoSave saved: " + scenePath + " on " + lastSaveTimeScene);
        }
        AutoSave repaintSaveWindow = (AutoSave)EditorWindow.GetWindow(typeof(AutoSave));
        repaintSaveWindow.Repaint();
    }
}