using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InputSystem : MonoBehaviour
{
    public static InputSystem Instance;
    public Player player;

    public static List<MyNetworkPuppet> listPuppet;
    public TMP_InputField baseSpeed;
    public TMP_InputField fastSpeed1;
    public TMP_InputField fastSpeed2;

    private void Awake() {
        Instance = this;
        listPuppet = new List<MyNetworkPuppet>();
    }

    public void Jump() {
        NetworkSystem.player.GetComponent<Player>().Jump();
    }

    public void ShootStart() {
        NetworkSystem.player.GetComponent<Player>().ShootStart();
    }

    public void ShootEnd() {
        NetworkSystem.player.GetComponent<Player>().ShootEnd();
    }

    public void RunStart() {
        NetworkSystem.player.GetComponent<Player>().RunStart();
    }

    public void RunEnd() {
        NetworkSystem.player.GetComponent<Player>().RunEnd();
    }
    public void UseSkill() {
        NetworkSystem.player.GetComponent<Player>().UseSkill();
    }

    public void UpdateSpeed() {
        float speed = float.Parse(baseSpeed.text);
        float fast1 = float.Parse(fastSpeed1.text);
        float fast2 = float.Parse(fastSpeed2.text);

        for (int i = 0; i < listPuppet.Count; i++) {
            listPuppet[i].speed = speed;
            listPuppet[i].FAST_SPEED_1 = fast1;
            listPuppet[i].FAST_SPEED_2 = fast2;
        }
    }

    //public void SendChat() {
    //    NetworkSystem.player.GetComponent<ChatBehavior>().Send();
    //}
}
