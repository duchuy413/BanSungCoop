using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

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
