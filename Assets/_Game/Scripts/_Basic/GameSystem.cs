using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSystem : MonoBehaviour
{
    public static GameSystem Instance;

    public static List<GameObject> poolObjects;
    public static List<string> poolNames;
    
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
}
