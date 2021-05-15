using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//[CreateAssetMenu(fileName = "New DItem", menuName = "DItem")]
//public class DItem : ScriptableObject
//{
//    public string itemName;
//    public Image image;
//    public DPriceParam price;
//    public string detail;
//    public GameObject itemObj;
//}

//[CreateAssetMenu(fileName = "New DWeapon", menuName = "DWeapon")]
//public class DWeapon : DItem
//{
//    public string nickname;
//    public float baseDame;
//    public float cooldown;
//    public int level;
//    public float baseXP;
//    public float currentXP;
//    public int charm1;
//    public int charm2;
//}

//[System.Serializable]
//public class HitParam
//{
//    public float dame;
//    public GameObject owner;
//    public string ownerTag;
//    public List<string> targetTags;
//    public string type;
//    public string direction;
//}

//[System.Serializable]
//public class DPriceParam : MonoBehaviour
//{
//    public int gold;
//    public int diamond;
//}

//[System.Serializable]
//public class DSaveData
//{
//    public string playerName;
//    public int mapIndex;
//    public Vector3 position;
//    public string characterName;
//    public int level;
//    public int currentWeapon;
//    public List<DWeapon> weapons;
//    public int respawnMapIndex;
//    public Vector3 respawnMapPosition;
//}

[CreateAssetMenu(fileName = "New CharacterStat", menuName = "CharacterStat")]
public class CharacterStat : ScriptableObject
{
    public string characterName;
    public Sprite mugshot;

    public Sprite[] stand;
    public Sprite[] go;
    public Sprite[] run;
    public Sprite[] jump;
    public Sprite[] gethit;
    public Sprite[] attack;
    public Sprite[] die;
    public Sprite[] custom1;
    public Sprite[] custom2;
    public Sprite[] sleep;

    public float speed = 0.7f;
    public float baseExp = 200;
    public float currentExp = 0;
    public float nextLvlExp = 0;
    public float expGainWhenKill = 42;
    public float goldGainWhenKill = 20f;
    public float foodGainWhenKill = 84f;
    public float foodConsumePerSec = 2.4f;
    
    public float shopPrice = 400f;
    public float hp = 200;
    public float maxhp = 200;
    public float dame = 20;
    public float attackRange = 0.3f;
    public float visionRange = 0.7f;
    public float attackCountDown = 1f;
    public float jumpForce = 200f;
    public float runSpeedMultipler = 1.5f;
    public float fallingDelay = 0.5f;
    public int jumpLimit = 2;

    public string attackObjectName;
    public bool canFly = false;
    public int foodCount = 2;
}