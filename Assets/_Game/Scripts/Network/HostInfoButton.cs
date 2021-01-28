using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Mirror.Discovery;
using TMPro;

public class HostInfoButton : MonoBehaviour
{
    public ServerResponse info;

    public void SetData(ServerResponse info) {
        this.info = info;
        GetComponentInChildren<TextMeshProUGUI>().text = info.EndPoint.Address.ToString();
    }

    public void OnClick() {
        NetworkManager.singleton.StartClient(info.uri);
        GameManager.Instance.LoadScene(GameManager.sceneName, false);
    }

}
