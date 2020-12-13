using Mirror.Discovery;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HostInfoPanel : MonoBehaviour
{
    public Transform content;
    public GameObject hostInfoButton;
    
    private List<ServerResponse> displayHosts;
    private List<GameObject> buttons;

    private void Awake() {
        buttons = new List<GameObject>();
        displayHosts = new List<ServerResponse>();
    }

    private void Update() {
        List<ServerResponse> discovered = new List<ServerResponse>();
        discovered.AddRange(NetworkSystem.discoveredServers.Values);

        for (int i = displayHosts.Count-1; i >= 0; i--) {
            if (!discovered.Contains(displayHosts[i])) {
                displayHosts.RemoveAt(i);
                Destroy(buttons[i]);
                buttons.RemoveAt(i);
            }
        }

        for (int i = 0; i < discovered.Count; i++) {
            if (!displayHosts.Contains(discovered[i])) {
                displayHosts.Add(discovered[i]);
                GameObject go = Instantiate(hostInfoButton, content);
                go.GetComponent<HostInfoButton>().SetData(discovered[i]);
                buttons.Add(go);
            }
        }
    }
}
