using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    public string monsterName;
    public int amount;
    public float followRange;

    private void Start() {
        for (int i = 0; i < amount; i++) {
            Vector3 pos = transform.position + new Vector3(Random.Range(-followRange, followRange), Random.Range(-followRange, followRange));
            GameSystem.LoadPool("Monster/" + monsterName + "/" + monsterName, pos);
        }
    }
}
