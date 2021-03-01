using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    public string monsterName;
    public int amount;
    public float followRange;
    public List<MobWorf> monsters;
    //public bool attackMode = false;

    private void Start() {
        for (int i = 0; i < amount; i++) {
            Vector3 pos = transform.position + new Vector3(Random.Range(-followRange, followRange), Random.Range(-followRange, followRange));
            GameObject go = GameSystem.LoadPool("Monster/" + monsterName + "/" + monsterName, pos);
            go.GetComponent<MobWorf>().movingPivot = transform;
            monsters.Add(go.GetComponent<MobWorf>());
        }
    }

    public void AddAttackTargets(List<MobWorf> targets) {
        for (int i = 0; i < monsters.Count; i++) {
            int rand = Random.Range(0, targets.Count);
            if (monsters[i].attackTarget == null || monsters[i].attackTarget.activeSelf == false) {
                monsters[i].attackTarget = targets[rand].gameObject;
            }
        }
    }
}
