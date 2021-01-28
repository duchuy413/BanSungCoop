using NativeWebSocket;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// Link github Native websocket: https://github.com/endel/NativeWebSocket
public class WebsocketServer : MonoBehaviour {
    public static WebsocketServer Instance;

    public static string SERVER_URL = "ws://chatgolang.herokuapp.com/ws/main/1";

    WebSocket websocket;

    private void Awake() {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    async void Start() {
        websocket = new WebSocket(SERVER_URL);

        websocket.OnOpen += () => {
            MyDebug.Log("Connection open!");
        };

        websocket.OnError += (e) => {
            MyDebug.Log("Error! " + e);
        };

        websocket.OnClose += (e) => {
            MyDebug.Log("Connection closed!");
        };

        websocket.OnMessage += (bytes) => {
            var message = System.Text.Encoding.UTF8.GetString(bytes);

            MyDebug.Log("Receive Message: " + message);
        };

        InvokeRepeating("SendWebSocketMessage", 0.0f, 0.3f);

        await websocket.Connect();
    }

    void Update() {
#if !UNITY_WEBGL || UNITY_EDITOR
        websocket.DispatchMessageQueue();
#endif
    }

    async void SendWebSocketMessage() {
        if (websocket.State == WebSocketState.Open) {
            string message = "THIS IS MESSAGE";
            await websocket.SendText(message);
        }
    }

    private async void OnApplicationQuit() {
        await websocket.Close();
    }
}