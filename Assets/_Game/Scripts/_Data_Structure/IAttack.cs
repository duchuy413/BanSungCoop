using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttack 
{
    void AttackButtonDown();
    void AttackButtonUp();
    void Attack(HitParam hit);
}
