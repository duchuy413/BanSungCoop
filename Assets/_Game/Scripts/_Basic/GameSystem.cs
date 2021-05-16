﻿using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameSystem : MonoBehaviour
{
    public static GameSystem Instance;

    public static List<GameObject> poolObjects;
    public static List<string> poolNames;

    public static string weapon = "pike";
    
    private void Awake() {
        Instance = this;
        DontDestroyOnLoad(gameObject);
        poolObjects = new List<GameObject>();
        poolNames = new List<string>();
    }

    public static GameObject LoadPool(string poolName, Vector3 position) {
        for (int i = 0; i < poolNames.Count; i++) {
            if (string.Compare(poolNames[i], poolName) == 0 && poolObjects[i].activeSelf == false) {
                poolObjects[i].SetActive(true);
                poolObjects[i].transform.position = position;
                return poolObjects[i];
            }
        }

        GameObject obj = Instantiate(Resources.Load<GameObject>(poolName) as GameObject, position, Quaternion.identity);
        poolNames.Add(poolName);
        poolObjects.Add(obj);
        return obj;
    }

    public static Component CopyComponent(Component original, GameObject destination) {
        System.Type type = original.GetType();
        Component copy = destination.AddComponent(type);
        System.Reflection.FieldInfo[] fields = type.GetFields();
        foreach (System.Reflection.FieldInfo field in fields) {
            field.SetValue(copy, field.GetValue(original));
        }
        return copy;
    }

    public static BattleStat LoadLevel(CharacterStat stat, int level) {
        BattleStat current = new BattleStat();

        current = new BattleStat();
        current.speed = stat.speed;
        current.baseExp = stat.baseExp;
        current.currentExp = stat.baseExp;
        current.nextLvlExp = stat.baseExp * Mathf.Pow(GameManager.INCREASE_RATE_EXP, level + 1);
        current.expGainWhenKill = stat.expGainWhenKill * Mathf.Pow(GameManager.INCREASE_RATE_STAT, level);
        current.goldGainWhenKill = stat.goldGainWhenKill * Mathf.Pow(GameManager.INCREASE_RATE_STAT, level);
        current.foodGainWhenKill = stat.foodGainWhenKill * Mathf.Pow(GameManager.INCREASE_RATE_STAT, level);
        current.foodConsumePerSec = stat.foodConsumePerSec * Mathf.Pow(GameManager.INCREASE_RATE_STAT, level);
        current.shopPrice = stat.foodGainWhenKill * Mathf.Pow(GameManager.INCREASE_RATE_STAT, level);
        current.hp = stat.hp * Mathf.Pow(GameManager.INCREASE_RATE_STAT, level);
        current.maxhp = stat.hp * Mathf.Pow(GameManager.INCREASE_RATE_STAT, level);
        current.dame = stat.dame * Mathf.Pow(GameManager.INCREASE_RATE_STAT, level);
        current.attackRange = stat.attackRange;
        current.visionRange = stat.visionRange;
        current.attackCountDown = stat.attackCountDown;

        return current;
    }

    //public static bool GetHit(IMob mob, HitParam hit) {
    //    //if (!hit.targetTags.Contains(gameObject.tag)) {
    //    //    return;
    //    //}

    //    //Gameplay.Instance.AddAttackTargets(new List<IMob> { this });

    //    float dameTake = hit.dame;
    //    GameSystem.TextFly(Convert.ToInt32(dameTake).ToString(), mob.GetGameObject().transform.position + new Vector3(0, 3f));

    //    BattleStat current = mob.GetCurrentStat();
    //    current.hp -= dameTake;

    //    if (current.hp <= 0) {
    //        current.hp = 0;
    //        mob.Died(hit);
    //        return true;
    //    } else {
    //        //StartCoroutine(PauseMovement(0.1f));
    //    }

    //    textName.text = current.hp + "/" + current.maxhp;
    //    hpValue.localScale = new Vector3(current.hp / current.maxhp, 1);
    //}

    public static double GetCurrentTimeStamp() {
        var epochStart = new System.DateTime(1970, 1, 1, 8, 0, 0, System.DateTimeKind.Utc);
        var timestamp = (System.DateTime.UtcNow - epochStart).TotalMilliseconds;
        return timestamp;
    }

    public static int GetSortingOrder(Transform tf) {
        return (int)(-tf.position.y * 100); ;
    }

    public static float GetPlayerDistance(Transform start) {
        Vector3 pos1 = new Vector3(start.position.x, start.position.y);
        Vector3 pos2 = new Vector3(NetworkSystem.player.transform.position.x, NetworkSystem.player.transform.position.y);
        float distance = Vector3.Distance(pos1,pos2);
        return Mathf.Abs(distance);
    }

    public static Vector3 GoToTargetVector(Vector3 current, Vector3 target, float speed, bool isFlying = false) {
        float distanceToTarget = Vector3.Distance(current, target);
        Vector3 vectorToTarget = target - current;

        vectorToTarget = vectorToTarget * speed / distanceToTarget;

        //if (!isFlying) {
        //    vectorToTarget.y = 0;
        //}

        return vectorToTarget;
    }

    public static void TextFly(string text, Vector3 pos, string color = "red") {
        GameObject flyingtext = GameSystem.LoadPool("textdame",pos + new Vector3(0f,4f));
        flyingtext.GetComponent<TextMeshPro>().text = text;
        if (color == "blue") {
            flyingtext.GetComponent<TextMeshPro>().color = new Color32(0, 0, 255, 255);
        } else if (color == "green") {
            flyingtext.GetComponent<TextMeshPro>().color = new Color32(0, 255, 0, 255);
        } else if (color == "yellow") {
            flyingtext.GetComponent<TextMeshPro>().color = new Color32(255, 255, 0, 255);
        } else {
            flyingtext.GetComponent<TextMeshPro>().color = new Color32(255, 0, 0, 255);
        }
    }
}
