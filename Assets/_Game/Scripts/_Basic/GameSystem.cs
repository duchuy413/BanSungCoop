﻿using System.Collections;
using System.Collections.Generic;
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
        // Copied fields can be restricted with BindingFlags
        System.Reflection.FieldInfo[] fields = type.GetFields();
        foreach (System.Reflection.FieldInfo field in fields) {
            field.SetValue(copy, field.GetValue(original));
        }
        return copy;
    }

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

        if (!isFlying) {
            vectorToTarget.y = 0;
        }

        return vectorToTarget;
    }

    public void FindEnemy() {

    }
}
