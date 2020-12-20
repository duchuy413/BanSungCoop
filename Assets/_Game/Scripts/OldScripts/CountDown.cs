using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DCountDown : MonoBehaviour
{
    public float countdownTime;
    public enum Action { DoNothing, SetActiveFalse, Destroy, SetActiveFalseParent, DestroyParent }
    public Action action = Action.SetActiveFalse;

    private void OnEnable()
    {
        Invoke("Trigger", countdownTime);
    }

    void Trigger()
    {
        if (action == Action.DoNothing) return;
        if (action == Action.SetActiveFalse) gameObject.SetActive(false);
        if (action == Action.Destroy) Destroy(gameObject);
        if (action == Action.SetActiveFalseParent) transform.parent.gameObject.SetActive(false);
        if (action == Action.DestroyParent) Destroy(transform.parent.gameObject);
    }
}
