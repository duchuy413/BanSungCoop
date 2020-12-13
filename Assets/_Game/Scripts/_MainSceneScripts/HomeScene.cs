﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeScene : MonoBehaviour
{
    public GameObject panelHostInfo;
    public GameObject hostButton;
    public GameObject jointButton;

    public void StartHost() {
        NetworkSystem.Instance.StartHost();
    }

    public void ShowHostInfoPanel() {
        panelHostInfo.SetActive(true);
        hostButton.SetActive(false);
        jointButton.SetActive(false);
    }

    public void HideHostInfoPanel() {
        panelHostInfo.SetActive(false);
        hostButton.SetActive(true);
        jointButton.SetActive(true);
    }
}
