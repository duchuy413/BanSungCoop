using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public static float GRAVITY = 2.5f;
    public static bool isPlaying = false;
    public static string sceneName = "R1_01";
    public static string weapon = "pike";
    public static Vector3 startPosition = new Vector3(0, 0);

    public GameObject mainUICamera;

    private void Start() {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void LoadScene(string sceneName, bool showUI = true) {
        mainUICamera.SetActive(showUI);
        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
    }
}
