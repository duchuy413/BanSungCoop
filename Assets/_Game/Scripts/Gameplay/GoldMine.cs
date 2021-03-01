﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GoldMine : MonoBehaviour
{
    public MonsterSpawner spawner;
    public TextMeshPro txtStatus;

    bool isCaptured = false;
    //Coroutine generatingGold;
    float nextSpawnGold = 0;

    private void Start() {
        txtStatus.text = "Protected";
    }

    private void Update() {
        if (!isCaptured) {
            bool monsterAlive = false;
            for (int i = 0; i < spawner.monsters.Count; i++) {
                if (spawner.monsters[i].gameObject.activeSelf == true) {
                    monsterAlive = true;
                }
            }
            if (!monsterAlive) {
                isCaptured = true;
                txtStatus.text = "Captured";
            }
            return;
        }

        if (Time.time > nextSpawnGold) {
            nextSpawnGold = Time.time + 1f;
            GameSystem.TextFly("+7", transform.position, "yellow");
            Gameplay.gold += 7;
        }
    }

}