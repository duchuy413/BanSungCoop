using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using Random = UnityEngine.Random;

public class Gameplay : MonoBehaviour {
    public static Gameplay Instance;
    public static float gold;
    public static float hungry;

    public List<Transform> mobPositions;

    public string sceneName;
    public Transform playerStartPosition;

    public GameObject gameplayCam;
    public GameObject btnSkip;
    public GameObject popupWeapon;
    public TextMeshProUGUI txtHungry;
    public TextMeshProUGUI txtGold;

    List<Vector3> pivots = new List<Vector3>();
    List<Vector3> areaPivots = new List<Vector3>();

    int petCount = 0;

    private void Awake() {
        Instance = this;
        //waveIndex = -1;
        GenerateWorld();
    }

    void Start() {
        StartCoroutine(SpawnPlayerAtStartPosition());
        StartCoroutine(GoldMineGeneratingGold());
        AudioSystem.Instance.PlaySound("Sound/background/gunnytheme", 4);
        AudioSystem.Instance.SetLooping(true, 4);
    }

    IEnumerator SpawnPlayerAtStartPosition() {
        while (true) {
            if (NetworkSystem.player != null) {
                NetworkSystem.player.transform.position = playerStartPosition.position;
                break;
            }
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator GoldMineGeneratingGold() {
        while (true) {
            gold += 7f;
            txtGold.text = gold.ToString();
            yield return new WaitForSeconds(1f);
        }
    }

    private void Update() {

    }

    public Vector3 GetRandomMobSpawnPosition() {
        int rand = UnityEngine.Random.Range(0, mobPositions.Count);
        return mobPositions[rand].position;
    }

    public void GenerateWorld() {
        for (int i = 0; i < 100; i++) {
            pivots.Add(new Vector3(Random.Range(-GameManager.MAP_SIZE, GameManager.MAP_SIZE), Random.Range(-GameManager.MAP_SIZE, GameManager.MAP_SIZE)));
        }

        for (int i = 0; i < 10; i++) {
            areaPivots.Add(new Vector3(Random.Range(-GameManager.MAP_SIZE, GameManager.MAP_SIZE), Random.Range(-GameManager.MAP_SIZE, GameManager.MAP_SIZE)));
        }

        for (int i = 0; i < pivots.Count; i++) {
            int nearest = NearestAreaPivot(i);
            if (nearest < 5) {
                GameSystem.LoadPool("tree", pivots[i]);
            } else {
                GameSystem.LoadPool("grass", pivots[i]);
            }
        }

        for (int i = 0; i < 10; i++) {
            GameSystem.LoadPool("Monster/worf/worfspawner", new Vector3(Random.Range(-GameManager.MAP_SIZE, GameManager.MAP_SIZE), Random.Range(-GameManager.MAP_SIZE, GameManager.MAP_SIZE)));
        }
    }

    int NearestAreaPivot(int pivotIndex) {
        int nearest = 0;
        for (int i = 0; i < areaPivots.Count; i++) {
            if (Vector3.Distance(areaPivots[i], pivots[pivotIndex]) < Vector3.Distance(areaPivots[nearest], pivots[pivotIndex])) {
                nearest = i;
            }
        }
        return nearest;
    }

    public void ShowPopupWeapon() {
        if (!popupWeapon.activeSelf) {
            popupWeapon.SetActive(true);
        }
    }

    public void CreatePet() {
        petCount++;
        gold -= 200;
        GameObject go = GameSystem.LoadPool("Monster/worf/worf", NetworkSystem.player.transform.position);
        go.tag = "Pet";
        go.GetComponent<MobWorf>().movingPivot = NetworkSystem.player.transform;
        go.GetComponent<MobWorf>().maxMovePivotRange = 10f + petCount*2;
        go.GetComponent<MobWorf>().minMovePivotRange = 5f + petCount;
    }
}
