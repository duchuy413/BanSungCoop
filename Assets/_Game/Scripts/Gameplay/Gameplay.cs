using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using Random = UnityEngine.Random;

public class Gameplay : MonoBehaviour {
    //[System.Serializable]
    //public class MonsterWave {
    //    public List<int> numbers;
    //    public List<string> mobs;
    //    public float time;
    //}

    public static Gameplay Instance;
    public static float gold;
    public static float hungry;

    [Header("SceneSetting \n")]
    public string sceneName;
    public Transform playerStartPosition;
    //public List<MonsterWave> waves;
    public List<Transform> mobPositions;
    public GameObject gameplayCam;

    [Header("Do not change these var's preferences \n")]
    public TextMeshProUGUI txtHungry;
    public TextMeshProUGUI txtGold;
    public GameObject btnSkip;
    public GameObject popupWeapon;

    List<Vector3> pivots = new List<Vector3>();
    List<Vector3> areaPivots = new List<Vector3>();

    //MonsterWave currentWave;
    //int waveIndex;
    //float nextWaveTime = -1f;
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
            //if (NetworkSystem.player != null) {
            //    NetworkSystem.player.transform.position = playerStartPosition.position;
            //    break;
            //}
            yield return new WaitForSeconds(1f);
        }
    }

    private void Update() {

    }

    //IEnumerator UpdateCountDown() {
    //    while (GameManager.isPlaying) {
    //        TimeSpan remain = TimeSpan.FromSeconds((double)(nextWaveTime - Time.time));
    //        //txtCountDown.text = remain.ToString(@"mm\:ss");
    //        yield return new WaitForSeconds(1f);
    //    }
    //}

    public Vector3 GetRandomMobSpawnPosition() {
        int rand = UnityEngine.Random.Range(0, mobPositions.Count);
        return mobPositions[rand].position;
    }

    //IEnumerator SpawnInRandomTime(string mobName, Vector3 pos) {
    //    yield return new WaitForSeconds(UnityEngine.Random.Range(0, currentWave.time / 2));
    //    GameSystem.LoadPool(mobName, pos);
    //}

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
