using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SideMenu : MonoBehaviour
{
    //public static SideMenu Instance;

    //private void Awake() {
    //    if (!Instance) {
    //        Instance = this;
    //        DontDestroyOnLoad(gameObject);
    //    } else {
    //        Destroy(gameObject);
    //    }
    //}

    public void LoadScene(string sceneName) {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }
}
