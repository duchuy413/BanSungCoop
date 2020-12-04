using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallelBackground : MonoBehaviour
{
    public float depth = 1f;
    public Transform pivot;

    Vector3 curPos;

    private void Start() {
        curPos = pivot.position;
    }

    public void Update() {
        transform.position -= (pivot.position - curPos) * Time.deltaTime * depth;
        curPos = pivot.position;
    }
}
