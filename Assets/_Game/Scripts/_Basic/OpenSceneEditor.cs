#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class OpenSceneEditor : MonoBehaviour
{

    [MenuItem("Project/Open Login", priority = 5000)]
    public static void OpenLoading()
    {
        OpenScene("Login");
    }

    [MenuItem("Project/Open Map1", priority = 5000)]
    public static void OpenMap1() {
        OpenScene("_Main/Map1");
    }

    [MenuItem("Project/Cheat/Turn On CanvasInput", priority = 5000)]
    public static void InputOn() {
        GameObject.Find("Canvas Input").SetActive(true);
        //OpenScene("Map1");
    }

    [MenuItem("Project/Cheat/Turn Off CanvasInput", priority = 5000)]
    public static void InputOff() {
        GameObject.Find("Canvas Input").SetActive(false);
        //OpenScene("Map1");
    }

    [MenuItem("Project/Open Scene Gameplay", false, 5000)]
    public static void OpenMain() {
        OpenScene("Gameplay");
    }

    [MenuItem("Project/Open Adventure", priority = 5000)]
    public static void OpenAdventure() {
        OpenScene("_Main/Adventure");
    }

    [MenuItem("Project/Open Home #a", priority = 5000)]
    public static void OpenHome() {
        OpenScene("_Main/Home");
    }

    [MenuItem("Project/Open WorldRandom #w", priority = 5000)]
    public static void OpenWorldRandom() {
        OpenScene("_Main/WorldRandom");
    }

    [MenuItem("Project/Open Friend", priority = 5000)]
    public static void OpenFriend() {
        OpenScene("_Main/Friend");
    }

    [MenuItem("Project/Open Inventory", priority = 5000)]
    public static void OpenInventory() {
        OpenScene("_Main/Inventory");
    }

    [MenuItem("Project/Open Market", priority = 5000)]
    public static void OpenMarket() {
        OpenScene("_Main/Market");
    }

    [MenuItem("Project/Open Pvp", priority = 5000)]
    public static void OpenPvp() {
        OpenScene("_Main/Pvp");
    }

    [MenuItem("Project/R1/R1_01", priority = 5000)]
    public static void R1_01() {
        OpenScene("Maps/Region1/R1_01");
    }

    [MenuItem("Project/R1/R1_02", priority = 5000)]
    public static void R1_02() {
        OpenScene("Maps/Region1/R1_02");
    }

    [MenuItem("Project/R1/R1_03", priority = 5000)]
    public static void R1_03() {
        OpenScene("Maps/Region1/R1_03");
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