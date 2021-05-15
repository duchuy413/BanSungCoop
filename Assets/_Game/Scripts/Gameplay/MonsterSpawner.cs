using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    public string monsterName;
    public int amount;
    public float followRange;
    public List<IMob> monsters;
    //public bool attackMode = false;

    private void Start() {
        //monsters = new List<IMob>();
        //for (int i = 0; i < amount; i++) {
        //    Vector3 pos = transform.position + new Vector3(Random.Range(-followRange, followRange), Random.Range(-followRange, followRange));
        //    GameObject go = GameSystem.LoadPool("Monster/" + monsterName + "/" + monsterName, pos);
        //    go.GetComponent<IMob>().SetFollow(transform, followRange * 0.5f, followRange);
        //    monsters.Add(go.GetComponent<IMob>());
        //}
    }

    public void Spawn(string monsterName, int amount) {
        this.monsterName = monsterName;
        this.amount = amount;

        monsters = new List<IMob>();
        for (int i = 0; i < amount; i++) {
            Vector3 pos = transform.position + new Vector3(Random.Range(-followRange, followRange), Random.Range(-followRange, followRange));
            GameObject go = GameSystem.LoadPool("Monster/" + monsterName + "/" + monsterName, pos);
            go.GetComponent<IMob>().SetFollow(transform, followRange * 0.5f, followRange);
            monsters.Add(go.GetComponent<IMob>());
        }
    }

    public void AddAttackTargets(List<IMob> targets) {
        for (int i = 0; i < monsters.Count; i++) {
            int rand = Random.Range(0, targets.Count);
            monsters[i].SetAttackTarget(targets[rand].GetGameObject());
        }
    }
}
