using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using Random = UnityEngine.Random;

public class Gameplay : MonoBehaviour {
    [System.Serializable]
    public class MonsterWave {
        public List<int> numbers;
        public List<string> mobs;
        public float time;
    }

    public static Gameplay Instance;

    [Header("SceneSetting \n")]
    public string sceneName;
    public Transform playerStartPosition;
    public List<MonsterWave> waves;
    public List<Transform> mobPositions;
    public GameObject gameplayCam;

    [Header("Do not change these var's preferences \n")]
    public TextMeshProUGUI txtWaveName;
    public TextMeshProUGUI txtCountDown;
    public GameObject btnSkip;

    List<Vector3> pivots = new List<Vector3>();
    List<Vector3> areaPivots = new List<Vector3>();

    MonsterWave currentWave;
    int waveIndex;
    float nextWaveTime = -1f;

    private void Awake() {
        Instance = this;
        waveIndex = -1;
        GenerateWorld();
    }

    void Start() {
        StartCoroutine(SpawnPlayerAtStartPosition());
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

    IEnumerator RunGame() {
        GameManager.isPlaying = true;

        while (waveIndex < waves.Count) {
            waveIndex++;
            currentWave = waves[waveIndex];
            nextWaveTime = Time.time + currentWave.time;

            for (int i = 0; i < currentWave.mobs.Count; i++) {
                for (int j = 0; j < currentWave.numbers[i]; j++) {
                    string mobName = currentWave.mobs[i];
                    Vector3 spawnPos = GetRandomMobSpawnPosition();
                    StartCoroutine(SpawnInRandomTime(mobName, spawnPos));
                }
            }
            yield return new WaitForSeconds(currentWave.time);
        }
    }

    private void Update() {

    }

    IEnumerator UpdateCountDown() {
        while (GameManager.isPlaying) {
            TimeSpan remain = TimeSpan.FromSeconds((double)(nextWaveTime - Time.time));
            txtCountDown.text = remain.ToString(@"mm\:ss");
            yield return new WaitForSeconds(1f);
        }
    }

    public Vector3 GetRandomMobSpawnPosition() {
        int rand = UnityEngine.Random.Range(0, mobPositions.Count);
        return mobPositions[rand].position;
    }

    IEnumerator SpawnInRandomTime(string mobName, Vector3 pos) {
        yield return new WaitForSeconds(UnityEngine.Random.Range(0, currentWave.time / 2));
        GameSystem.LoadPool(mobName, pos);
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

        //for (int i = 0; i < 100; i++) {
        //    GameSystem.LoadPool("tree", new Vector3(Random.Range(-GameManager.MAP_SIZE, GameManager.MAP_SIZE), Random.Range(-GameManager.MAP_SIZE, GameManager.MAP_SIZE)));
        //}

        //for (int i = 0; i < 20; i++) {
        //    GameSystem.LoadPool("carrot", new Vector3(Random.Range(-GameManager.MAP_SIZE, GameManager.MAP_SIZE), Random.Range(-GameManager.MAP_SIZE, GameManager.MAP_SIZE)));
        //}

        //for (int i = 0; i < 500; i++) {
        //    GameSystem.LoadPool("grass", new Vector3(Random.Range(-GameManager.MAP_SIZE, GameManager.MAP_SIZE), Random.Range(-GameManager.MAP_SIZE, GameManager.MAP_SIZE)));
        //}

        //for (int i = 0; i < 20; i++) {
        //    GameSystem.LoadPool("grassbig", new Vector3(Random.Range(-GameManager.MAP_SIZE, GameManager.MAP_SIZE), Random.Range(-GameManager.MAP_SIZE, GameManager.MAP_SIZE)));
        //}

        //for (int i = 0; i < 100; i++) {
        //    GameSystem.LoadPool("Monster/worf/worf", new Vector3(Random.Range(-GameManager.MAP_SIZE, GameManager.MAP_SIZE), Random.Range(-GameManager.MAP_SIZE, GameManager.MAP_SIZE)));
        //}
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
}
