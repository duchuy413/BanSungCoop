using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallelBackground : MonoBehaviour
{
    public float moveSpeed = 1f;
    public Transform pivot;
    public float DEPTH = 0.001f;

    Vector3 myStartPos;
    Vector3 pivotStartPos;

    private void OnEnable() {
        pivotStartPos = pivot.position;
        myStartPos = transform.localPosition;
    }

    public void Update() {
        Vector3 delta = (pivot.position - pivotStartPos) * moveSpeed * DEPTH;
        Vector3 newPos = myStartPos - delta;
        transform.localPosition = new Vector3(newPos.x, newPos.y, transform.localPosition.z); // (pivot.position - curPos) * Time.deltaTime * depth;
    }
}
