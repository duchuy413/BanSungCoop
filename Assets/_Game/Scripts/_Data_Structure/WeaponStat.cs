using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName ="new WeaponStat",menuName = "WeaponStat")]
public class WeaponStat : ScriptableObject {
    public Vector3 localPosision;
    public Sprite sprite;
    public string bulletName = "Bullet";
    public float forceBack = 50f;
    public float dame = 20f;
    public float attackCountDown = 0.2f;
}
