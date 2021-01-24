using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttack 
{
    void Init(Player player);
    WeaponStat GetWeaponStat();
    void AttackButtonDown();
    void AttackButtonUp();
    void Attack();
}
