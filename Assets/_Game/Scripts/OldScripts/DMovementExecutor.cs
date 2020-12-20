using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DMovement))]
public class DMovementExecutor : MonoBehaviour
{
    public List<string> movements;
    public float[] durations;

    private float count = 0f;
    [HideInInspector]
    public int moveindex = 0;

    public float timeCheck;

    private void Awake()
    {
        moveindex = -1;
        timeCheck = durations[0] * UnityEngine.Random.Range(0.5f, 1.5f);
    }

    public virtual void Update()
    {
        if (movements == null || durations == null) return;
        if (movements.Count == 0) return;
        count += Time.deltaTime;
        if (moveindex == -1 || count > timeCheck) { count = 0; NextMovement(); }
    }

    public virtual void NextMovement()
    {
        if (movements.Count == 0) return;
        moveindex++;
        if (moveindex >= movements.Count) EndOfMovement();

        if (moveindex != -1) {
            timeCheck = durations[moveindex] * UnityEngine.Random.Range(0.5f, 1.5f);
            GetComponent<DMovement>().state = movements[moveindex];
        }
    }

    public virtual void EndOfMovement()
    {
        moveindex = 0;
    }
}
