#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class OpenSceneEditor : MonoBehaviour
{

    [MenuItem("Project/Open Login #a", priority = 5000)]
    public static void OpenLoading()
    {
        OpenScene("Login");
        //EditorApplication.EnterPlaymode();
    }

    [MenuItem("Project/Open Scene - Level Editor Main %&w", false, 5000)]
    public static void OpenMain()
    {
        OpenScene("LevelEditorMain");
    }

    [MenuItem("Project/Open Scene - Level Editor %&w", false, 5000)]
    public static void OpenEditor()
    {
        OpenScene("LevelEditor");
    }

    [MenuItem("Project/Open Scene - Genre List %&w", false, 5000)]
    public static void OpenGerneList()
    {
        OpenScene("GenreList");
    }

    [MenuItem("Project/Open Scene - Level Recorder %&w", false, 5000)]
    public static void OpenRecorder()
    {
        OpenScene("LevelRecorder");
    }

    private static void OpenScene(string sceneName)
    {
        if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
        {
            EditorSceneManager.OpenScene("Assets/_Game/Scenes/" + sceneName + ".unity");
        }
    }

    private static void OpenNewScene(string sceneName)
    {
        if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
        {
            EditorSceneManager.OpenScene("Assets/Scenes/New/" + sceneName + ".unity");
        }
    }


}
#endif