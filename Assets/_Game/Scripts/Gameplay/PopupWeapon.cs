using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupWeapon : MonoBehaviour
{
    public void LoadWeapon(string weaponName) {
        NetworkSystem.player.GetComponent<Player>().LoadWeapon(weaponName);
    }

    public void Close() {
        gameObject.SetActive(false);
    }
}
