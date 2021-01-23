using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttack 
{
    void Init(Player player);
    void AttackButtonDown();
    void AttackButtonUp();
    void Attack();
}
