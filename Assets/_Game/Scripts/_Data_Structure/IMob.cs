using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMob 
{
    CharacterStat GetStatData();
    void SetStatData(CharacterStat stat);
    void GetHit(HitParam hit);
    void LoadLevel(int level);
    void ApplyDame(GameObject target);
    void Died(HitParam hitParam);
}
