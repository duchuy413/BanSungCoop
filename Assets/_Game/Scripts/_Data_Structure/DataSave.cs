using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DataSave 
{
    public string playerName;
    public int mapIndex;
    public Vector3 position;
    public string characterName;
    public int level;
    public int currentWeapon;
    public List<WeaponStat> weapons;
    public int respawnMapIndex;
    public Vector3 respawnMapPosition;
}
