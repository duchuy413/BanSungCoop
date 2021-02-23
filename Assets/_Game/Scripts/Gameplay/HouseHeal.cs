using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HouseHeal : MonoBehaviour
{
    public static float HEAL_RANGE = 3f;
    public static float HEAL_COOL_DOWN = 1f;

    float nextHeal = 0f;

    private void Update() {
        var player = NetworkSystem.player.GetComponent<Player>();
        if (player.current == null) return;

        if (Time.time > nextHeal) {
            nextHeal = Time.time + HEAL_COOL_DOWN;
            
            if (player.current.hp < player.current.maxhp) {
                float healAmount = 0.02f * player.current.maxhp;
                player.current.hp += healAmount;
                player.UpdateHPBar();

                GameSystem.TextFly("+" + Convert.ToInt32(healAmount + 1).ToString(), NetworkSystem.player.transform.position, "green");
            }
        }
    }


}
