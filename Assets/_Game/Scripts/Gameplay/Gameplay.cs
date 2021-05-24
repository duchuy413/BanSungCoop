using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using Random = UnityEngine.Random;

public class Gameplay : MonoBehaviour {
    public static Gameplay Instance;
    public static Dictionary<GameObject, int> indexes;

    public static float gold;
    public static float food = 2000;
    public static float foodConsumePerSec = 3f;

    public List<Transform> mobPositions;
    public List<IMob> pets;
    public List<GameObject> worldObjs;

    public string sceneName;
    public Transform playerStartPosition;

    public GameObject gameplayCam;
    public GameObject btnSkip;
    public GameObject popupWeapon;
    public TextMeshProUGUI txtFood;
    public TextMeshProUGUI txtGold;

    List<Vector3> pivots = new List<Vector3>();
    List<Vector3> areaPivots = new List<Vector3>();
    List<MonsterSpawner> monsterSpawners = new List<MonsterSpawner>();

    int petCount = 0;

    private void Awake() {
        Instance = this;
        indexes = new Dictionary<GameObject, int>();
        pets = new List<IMob>();
        GenerateWorld();
    }

    void Start() {
        StartCoroutine(SpawnPlayerAtStartPosition());
        //StartCoroutine(GoldMineGeneratingGold());
        StartCoroutine(UpdatePetFormation());
        StartCoroutine(UpdateDisplayCorou());
        AudioSystem.Instance.PlaySound("Sound/background/Mystery of Love", 4);
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

    IEnumerator UpdateDisplayCorou() {
        while (true) {

            //food -= foodConsumePerSec;
            //txtFood.text = "Food:" + ((int)food).ToString() + "(-" + foodConsumePerSec + "/s)";
            //UpdateDisplay();

            Instance.txtGold.text = "Gold:" + ((int)gold).ToString();
            Instance.txtFood.text = "Food:" + ((int)food).ToString() + "/12";

            yield return new WaitForSeconds(0.2f);
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
        foodConsumePerSec = 3;

        int totalFood = 0;
        if (pets != null) {
            for (int i = 0; i < pets.Count; i++) {
                totalFood += pets[i].GetBaseStat().foodCount;
            }
        }

        food = totalFood;

        if (pets.Count == 0) {
            return;
        } 

        for (int i = pets.Count - 1; i >= 0; i--) {
            //Debug.Log("PETS i , i = " + i);
            //Debug.Log("PETS i , GetGameObject() = " + pets[i].GetGameObject());
            //Debug.Log("PETS i , pets[i].GetGameObject().activeSelf = " + pets[i].GetGameObject().activeSelf);

            if (pets[i].GetGameObject() == null || pets[i].GetGameObject().activeSelf == false) {
                pets.RemoveAt(i);
            } else {
                foodConsumePerSec += pets[i].GetCurrentStat().foodConsumePerSec;
            }
        }
        //foreach (IMob pet in pets) {
        //    if (pet.GetGameObject() == null || pet.GetGameObject().activeSelf == false) {
        //        pets.Remove(pet);
        //    } else {
        //        foodConsumePerSec += pet.GetCurrentStat().foodConsumePerSec;
        //    }
        //}
    }

    public Vector3 GetRandomMobSpawnPosition() {
        int rand = UnityEngine.Random.Range(0, mobPositions.Count);
        return mobPositions[rand].position;
    }

    public void GenerateWorld() {
        worldObjs = new List<GameObject>();

        for (int i = 0; i < 2000; i++) {
            pivots.Add(new Vector3(Random.Range(-GameManager.MAP_SIZE, GameManager.MAP_SIZE), Random.Range(-GameManager.MAP_SIZE, GameManager.MAP_SIZE)));
        }

        for (int i = 0; i < 350; i++) {
            areaPivots.Add(new Vector3(Random.Range(-GameManager.MAP_SIZE, GameManager.MAP_SIZE), Random.Range(-GameManager.MAP_SIZE, GameManager.MAP_SIZE)));
        }

        for (int i = 0; i < pivots.Count; i++) {
            int nearest = NearestAreaPivot(i);
            if (nearest < 50) {
                worldObjs.Add(GameSystem.LoadPool("world/tree", pivots[i]));
            } else if (nearest > 500 && nearest <= 100) {
                worldObjs.Add(GameSystem.LoadPool("world/grass", pivots[i]));
            } else if (nearest > 100 && nearest <= 150) {
                worldObjs.Add(GameSystem.LoadPool("world/stone", pivots[i]));
            } else if (nearest > 150 && nearest <= 200) {
                worldObjs.Add(GameSystem.LoadPool("world/sapling", pivots[i]));
            } else if (nearest > 200 && nearest <= 250) {
                worldObjs.Add(GameSystem.LoadPool("world/house", pivots[i]));
            } else if (nearest > 250 && nearest <= 300) {
                worldObjs.Add(GameSystem.LoadPool("world/spiderden", pivots[i]));
            } else if (nearest > 300 && nearest <= 350) {
                worldObjs.Add(GameSystem.LoadPool("world/grassyellow", pivots[i]));
            }
        }

        //for (int i = 0; i < 200; i++) {
        //    GameObject go = GameSystem.LoadPool("Monster/worf/worfspawner", new Vector3(Random.Range(-GameManager.MAP_SIZE, GameManager.MAP_SIZE), Random.Range(-GameManager.MAP_SIZE, GameManager.MAP_SIZE)));
        //    go.GetComponent<MonsterSpawner>().Spawn("monster5",5);
        //}

        //for (int i = 0; i < 20; i++) {
        //    GameObject go = GameSystem.LoadPool("Monster/worf/worfspawner", new Vector3(Random.Range(-GameManager.MAP_SIZE, GameManager.MAP_SIZE), Random.Range(-GameManager.MAP_SIZE, GameManager.MAP_SIZE)));
        //    go.GetComponent<MonsterSpawner>().Spawn("monster4", 5);
        //}

        //for (int i = 0; i < 20; i++) {
        //    GameObject go = GameSystem.LoadPool("Monster/worf/worfspawner", new Vector3(Random.Range(-GameManager.MAP_SIZE, GameManager.MAP_SIZE), Random.Range(-GameManager.MAP_SIZE, GameManager.MAP_SIZE)));
        //    go.GetComponent<MonsterSpawner>().Spawn("monster3", 5);
        //}

        //for (int i = 0; i < 20; i++) {
        //    GameObject go = GameSystem.LoadPool("Monster/worf/worfspawner", new Vector3(Random.Range(-GameManager.MAP_SIZE, GameManager.MAP_SIZE), Random.Range(-GameManager.MAP_SIZE, GameManager.MAP_SIZE)));
        //    go.GetComponent<MonsterSpawner>().Spawn("monster2", 5);
        //}

        //for (int i = 0; i < 25; i++) {
        //    GameObject go = GameSystem.LoadPool("Monster/worf/worfspawner", new Vector3(Random.Range(-GameManager.MAP_SIZE, GameManager.MAP_SIZE), Random.Range(-GameManager.MAP_SIZE, GameManager.MAP_SIZE)));
        //    go.GetComponent<MonsterSpawner>().Spawn("monster1", 1);
        //}

        //for (int i = 0; i < 20; i++) {
        //    GameObject go = GameSystem.LoadPool("Monster/worf/worfspawner", new Vector3(Random.Range(-GameManager.MAP_SIZE, GameManager.MAP_SIZE), Random.Range(-GameManager.MAP_SIZE, GameManager.MAP_SIZE)));
        //    go.GetComponent<MonsterSpawner>().Spawn("monster7", 5);
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

    public void ShowPopupWeapon() {
        if (!popupWeapon.activeSelf) {
            popupWeapon.SetActive(true);
        }
    }

    public void CreatePet(string monsterName) {
        //if (food >= 12 || gold < 200) return;

        petCount++;
        gold -= 200;
        GameObject go = GameSystem.LoadPool("Monster/" + monsterName + "/" + monsterName, NetworkSystem.player.transform.position);
        go.tag = "Pet";
        MobWorf pet = go.GetComponent<MobWorf>();
        pet.movingPivot = NetworkSystem.player.transform;
        pet.maxMovePivotRange = 10f + petCount*2;
        pet.minMovePivotRange = 5f + petCount;
        pet.level = NetworkSystem.player.GetComponent<Player>().level;
        pet.LoadLevel(1);
        //pet.textName.text = "--------------------";
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

    public static void AddGold(float amount) {
        Gameplay.gold += amount;
        LeanTween.delayedCall(Random.Range(0, 2f), () => {
            GameSystem.TextFly("+" + (int)amount + "gold", NetworkSystem.player.transform.position + new Vector3(0, 1f), "yellow");
        });        
    }

    public static void AddFood(float amount) {
        //Gameplay.food += amount;
        //LeanTween.delayedCall(Random.Range(0, 2f), () => {
        //    GameSystem.TextFly("+" + (int)amount + "food", NetworkSystem.player.transform.position + new Vector3(0, 1f), "red");
        //});
    }

    //public static void UpdateDisplay() {

    //}
}
