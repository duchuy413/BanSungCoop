using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputSystem : MonoBehaviour
{
    public static InputSystem Instance;
    public Player player;

    private void Awake() {
        Instance = this;
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

    public void UseSkill() {
        NetworkSystem.player.GetComponent<Player>().UseSkill();
    }
}
