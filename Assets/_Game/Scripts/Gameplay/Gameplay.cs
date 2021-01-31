using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

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

    MonsterWave currentWave;
    int waveIndex;
    float nextWaveTime = -1f;

    private void Awake() {
        Instance = this;

        waveIndex = -1;
        //StartCoroutine(RunGame());
        //StartCoroutine(UpdateCountDown());

        //test
        GameObject flyingtext = GameSystem.LoadPool("textdame", transform.position);
        flyingtext.GetComponent<TextMeshPro>().text = Convert.ToInt32(123123).ToString();

        //Debug.Log("FLYUTING TESTSSR" + flyingtext);
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
}
