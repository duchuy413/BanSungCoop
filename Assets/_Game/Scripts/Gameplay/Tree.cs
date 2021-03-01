using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour
{
    public Sprite[] treeShapes;

    void Start()
    {
        GetComponent<SpriteRenderer>().sprite = treeShapes[Random.Range(0, treeShapes.Length)];
    }
}
