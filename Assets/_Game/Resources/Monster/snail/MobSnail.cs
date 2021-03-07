using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using Random = UnityEngine.Random;

public class MobSnail : MonoBehaviour, IMob {
    public static float CHANGE_POSITION_RANGE = 7f;
    public static float CHANGE_POSITION_TIME = 7f;

    public CharacterStat stat;
    public Transform hpValue;
    public TextMeshPro textName;
    public int level = 1;

    Vector3 targetPos;
    float nextChangePos = 0;
    private BattleStat current;
    GameObject attackTarget;


    //private void Start() {
    //    LoadLevel(10);
    //}

    private void OnEnable() {
        LoadLevel(level);
    }

    void Update() {
        if (Time.time > nextChangePos) {
            nextChangePos = Time.time + Random.Range(CHANGE_POSITION_TIME * 0.5f, CHANGE_POSITION_TIME * 1.5f);
            targetPos = new Vector3(Random.Range(-CHANGE_POSITION_RANGE, CHANGE_POSITION_RANGE), Random.Range(-CHANGE_POSITION_RANGE, CHANGE_POSITION_RANGE)) + transform.position;
        }

        Vector3 velocity = GameSystem.GoToTargetVector(transform.position, targetPos, stat.speed);
        GetComponent<Rigidbody2D>().velocity = velocity;
    }

    public void GetHit(HitParam hit) {
        if (!hit.targetTags.Contains(gameObject.tag)) {
            return;
        }

        Debug.Log("Get hit from " + hit.owner.name);
        Debug.Log("THIS IS BOSS GET HIT");
        //getHit = true;
        //attackTarget = hit.owner;

        //MonsterSpawner spawner = movingPivot.GetComponent<MonsterSpawner>();
        //if (spawner != null) {
        //    Gameplay.Instance.AddAttackTargets(spawner);
        //}

        //if (hit.owner.tag == "Player" && Gameplay.Instance.pets.Count > 0) {
        //    MonsterSpawner spawner1 = movingPivot.GetComponent<MonsterSpawner>();
        //    if (spawner1 != null) {
        //        spawner1.AddAttackTargets(Gameplay.Instance.pets);
        //    }
        //}

        Gameplay.Instance.AddAttackTargets(new List<IMob> { this });

        float dameTake = hit.dame;
        GameSystem.TextFly(Convert.ToInt32(dameTake).ToString(), transform.position + new Vector3(0, 3f));

        current.hp -= dameTake;

        if (current.hp <= 0) {
            current.hp = 0;
            Died(hit);
            return;
        } else {
            //StartCoroutine(PauseMovement(0.1f));
        }

        textName.text = current.hp + "/" + current.maxhp;
        hpValue.localScale = new Vector3(current.hp / current.maxhp, 1);
    }

    public CharacterStat GetStatData() {
        return stat;
    }

    public void LoadLevel(int level) {
        textName.text = "lv" + level.ToString() + "." + stat.characterName;

        current = GameSystem.LoadLevel(stat, level);
        //current = new BattleStat();
        //current.speed = stat.speed;
        //current.baseExp = stat.baseExp;
        //current.currentExp = stat.baseExp * Mathf.Pow(1.1f, level);
        //current.nextLvlExp = stat.baseExp * Mathf.Pow(1.1f, level + 1);
        //current.expGainWhenKill = stat.expGainWhenKill * Mathf.Pow(1.1f, level);
        //current.hp = stat.hp * Mathf.Pow(1.1f, level);
        //current.maxhp = stat.hp * Mathf.Pow(1.1f, level);
        //current.dame = stat.dame * Mathf.Pow(1.1f, level);
        //current.attackRange = stat.attackRange;
        //current.visionRange = stat.visionRange;
        //current.attackCountDown = stat.attackCountDown;
        hpValue.localScale = new Vector3(current.hp / current.maxhp, 1);
    }

    public void SetStatData(CharacterStat stat) {
        this.stat = stat;
    }

    public void ApplyDame(GameObject target) {
        HitParam hit = new HitParam();
        hit.dame = current.dame;
        hit.owner = gameObject;
        hit.ownerTag = gameObject.tag;

        target.SendMessage("GetHit", hit, SendMessageOptions.DontRequireReceiver);
    }

    public void Died(HitParam hitParam) {
        gameObject.SetActive(false);
        if (gameObject.CompareTag("Monster")) {
            NetworkSystem.player.GetComponent<Player>().AddExp(current.expGainWhenKill);
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        DameOnContact dame = collision.GetComponentInChildren<DameOnContact>();
        if (dame != null && dame.hit.targetTags.Contains(gameObject.tag)) {
            GetHit(dame.hit);
        }
    }

    public GameObject GetGameObject() {
        return gameObject;
    }

    public void SetAttackTarget(GameObject attackTarget, bool forceChangeTarget = false) {
        if (this.attackTarget != null && !forceChangeTarget) {
            return;
        }

        if (attackTarget == null || attackTarget.activeSelf == false) {
            return;
        }

        this.attackTarget = attackTarget;
    }

    public void SetFollow(Transform pivot, float minDistance, float maxDistance) {
        //this.minMovePivotRange = minDistance;
        //this.maxMovePivotRange = maxDistance;
    }

}
