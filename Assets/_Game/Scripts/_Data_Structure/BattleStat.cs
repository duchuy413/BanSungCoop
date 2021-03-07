using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleStat 
{
    public float speed = 4f;
    public float baseExp = 200;
    public float currentExp = 0;
    public float nextLvlExp = 0;

    public float expGainWhenKill = 42f;
    public float goldGainWhenKill = 20f;
    public float foodGainWhenKill = 84f;
    public float foodConsumePerSec = 6f;
    public float shopPrice = 400f;

    public float hp = 200;
    public float maxhp = 200;
    public float dame = 20;
    public float attackRange = 2f;
    public float visionRange = 4f;
    public float attackCountDown = 1f;
}
