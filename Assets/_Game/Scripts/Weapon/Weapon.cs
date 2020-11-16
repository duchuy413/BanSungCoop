using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Weapon : NetworkBehaviour{
    public Transform barrel;
    public WeaponStat stat;

    public void Init() {
        GetComponent<SpriteRenderer>().sprite = stat.sprite;
        transform.localPosition = stat.localPosision;
    }
}
