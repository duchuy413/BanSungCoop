using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    //const - setting
    public static float GRAVITY = 0f;
    public static float MAP_SIZE = 100f;
    public static float INCREASE_RATE_STAT = 1.5f;
    public static float INCREASE_RATE_EXP = 1.7f;

    //public field
    public static bool isPlaying = false;
    public static string sceneName = "WorldRandom";
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
