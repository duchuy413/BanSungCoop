using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseWeapon : MonoBehaviour
{
    private void OnMouseDown() {
        Gameplay.Instance.ShowPopupWeapon();
    }
}
