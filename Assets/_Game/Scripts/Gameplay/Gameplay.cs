using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gameplay : MonoBehaviour
{
    public static Gameplay Instance;
    public Transform startPos;
    public GameObject gameplayCam;

    private void Awake() {
        Instance = this;
        //gameplayCam.SetActive(true);
    }

    void Start()
    {
        NetworkSystem.player.transform.position = Instance.startPos.position;
        AudioSystem.Instance.PlaySound("Sound/background/gunnytheme",4);
        AudioSystem.Instance.SetLooping(true, 4);
    }

}
