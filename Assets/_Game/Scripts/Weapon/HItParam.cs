using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HitParam
{
    public float dame;
    public Vector3 startPos;
    public GameObject owner;
    public string ownerTag;
    public List<string> targetTags;
    public string type;
    public string direction;
}

