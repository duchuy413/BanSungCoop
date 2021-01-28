using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeScene : MonoBehaviour
{
    public GameObject panelHostInfo;
    public GameObject hostButton;
    public GameObject jointButton;

    private void Start() {
        panelHostInfo.SetActive(false);
    }

    public void StartHost() {
        NetworkSystem.Instance.StartHost();
    }
    
    public void StartClient() {
        ShowHostInfoPanel();
        NetworkSystem.Instance.StartClient();
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
