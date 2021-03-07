﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using Random = UnityEngine.Random;

public class Gameplay : MonoBehaviour {
    public static Gameplay Instance;
    public static Dictionary<GameObject, int> indexes;

    public static float gold;
    public static float hungry;

    public List<Transform> mobPositions;
    public List<IMob> pets;
    public List<GameObject> worldObjs;

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
        indexes = new Dictionary<GameObject, int>();
        pets = new List<IMob>();
        GenerateWorld();
    }

    void Start() {
        StartCoroutine(SpawnPlayerAtStartPosition());
        StartCoroutine(GoldMineGeneratingGold());
        StartCoroutine(UpdatePetFormation());
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

    IEnumerator UpdatePetFormation() {
        while (true) {
            for (int i = 0; i < pets.Count; i++) {
                pets[i].SetFollow(NetworkSystem.player.transform, 3f + i, 7f + i * 2);
                //pets[i].maxMovePivotRange = 
                //pets[i].minMovePivotRange = 
            }
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
        worldObjs = new List<GameObject>();

        for (int i = 0; i < 1000; i++) {
            pivots.Add(new Vector3(Random.Range(-GameManager.MAP_SIZE, GameManager.MAP_SIZE), Random.Range(-GameManager.MAP_SIZE, GameManager.MAP_SIZE)));
        }

        for (int i = 0; i < 10; i++) {
            areaPivots.Add(new Vector3(Random.Range(-GameManager.MAP_SIZE, GameManager.MAP_SIZE), Random.Range(-GameManager.MAP_SIZE, GameManager.MAP_SIZE)));
        }

        for (int i = 0; i < pivots.Count; i++) {
            int nearest = NearestAreaPivot(i);
            if (nearest < 5) {
                worldObjs.Add(GameSystem.LoadPool("tree", pivots[i]));
            } else {
                worldObjs.Add(GameSystem.LoadPool("grass", pivots[i]));
            }
        }

        for (int i = 0; i < 10; i++) {
            GameSystem.LoadPool("Monster/worf/worfspawner", new Vector3(Random.Range(-GameManager.MAP_SIZE, GameManager.MAP_SIZE), Random.Range(-GameManager.MAP_SIZE, GameManager.MAP_SIZE)));
        }

        for (int i = 0; i < 100; i++) {
            GameSystem.LoadPool("Monster/snail/snail", new Vector3(Random.Range(-GameManager.MAP_SIZE, GameManager.MAP_SIZE), Random.Range(-GameManager.MAP_SIZE, GameManager.MAP_SIZE)));
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
        MobWorf pet = go.GetComponent<MobWorf>();
        pet.movingPivot = NetworkSystem.player.transform;
        pet.maxMovePivotRange = 10f + petCount*2;
        pet.minMovePivotRange = 5f + petCount;
        pet.level = 1;
        pet.LoadLevel(1);
        pet.textName.text = "--------------------";
        pets.Add(pet);
    }

    public void AddAttackTargets(List<IMob> targets) {
        for (int i = 0; i < pets.Count; i++) {
            int rand = Random.Range(0, targets.Count);
            //pets[i].SetAttackTarget()
            pets[i].SetAttackTarget(targets[rand].GetGameObject());

            //if (pets[i].attackTarget == null || pets[i].attackTarget.activeSelf == false) {
            //    pets[i].attackTarget = spawner.monsters[rand].gameObject;
            //}
        }
    }

    public int TranslateToIndex(GameObject obj) {
        if (!indexes.ContainsKey(obj)) {
            indexes.Add(obj, indexes.Count);
        }
        return indexes[obj];
    }
}
