using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterStatRuntime
{
    public float speed = 0.7f;
    public float baseExp = 200;
    public float currentExp = 0;
    public float nextLvlExp = 0;
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

    public void ReadValue(CharacterStat stat) {
        this.speed = stat.speed;
        this.baseExp = stat.baseExp;
        this.currentExp = stat.currentExp;
        this.nextLvlExp = stat.nextLvlExp;
        this.hp = stat.hp;
        this.maxhp = stat.maxhp;
        this.dame = stat.dame;
        this.attackRange = stat.attackRange;
        this.visionRange = stat.visionRange;
        this.attackCountDown = stat.attackCountDown;

        this.jumpForce = stat.jumpForce;
        this.runSpeedMultipler = stat.runSpeedMultipler;
        this.fallingDelay = stat.fallingDelay;
        this.jumpLimit = stat.jumpLimit;

        this.attackObjectName = stat.attackObjectName;
        this.canFly = stat.canFly;
    }
}
