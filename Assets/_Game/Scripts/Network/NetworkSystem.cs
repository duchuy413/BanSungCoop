using Mirror;
using Mirror.Discovery;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[DisallowMultipleComponent]
[HelpURL("https://mirror-networking.com/docs/Components/NetworkDiscovery.html")]
[RequireComponent(typeof(NetworkDiscovery))]
public class NetworkSystem : MonoBehaviour {
    public static Dictionary<long, ServerResponse> discoveredServers = new Dictionary<long, ServerResponse>();

    public static NetworkSystem Instance;
    public static GameObject player;
    public static bool isPlaying = false;
    public NetworkDiscovery networkDiscovery;

    Vector2 scrollViewPos = Vector2.zero;

    public Vector3 SpawnPosition {
        get {
            if (SceneManager.GetActiveScene().name == "Gameplay") {
                return new Vector3(0, 0);
            } else {
                return new Vector3(-9999f, -9999f);
            }
        }
    }

    private void Awake() {
        if (!Instance) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }

#if UNITY_EDITOR
    void OnValidate() {
        if (networkDiscovery == null) {
            networkDiscovery = GetComponent<NetworkDiscovery>();
            UnityEditor.Events.UnityEventTools.AddPersistentListener(networkDiscovery.OnServerFound, OnDiscoveredServer);
            UnityEditor.Undo.RecordObjects(new Object[] { this, networkDiscovery }, "Set NetworkDiscovery");
        }
    }
#endif

    public void StartHost() {
        discoveredServers.Clear();
        NetworkManager.singleton.StartHost();
        networkDiscovery.AdvertiseServer();
        isPlaying = true;
        GameManager.Instance.LoadScene(GameManager.sceneName, false);
    }

    public void StartClient(){
        discoveredServers.Clear();
        networkDiscovery.StartDiscovery();
    }

    void Connect(ServerResponse info) {
        NetworkManager.singleton.StartClient(info.uri);
    }

    public void OnDiscoveredServer(ServerResponse info) {
        discoveredServers[info.serverId] = info;
    }
}
