using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveNearbyWorldObjects : MonoBehaviour
{
    public float range;

    void Start()
    {
        LeanTween.delayedCall(5f, () => {
            var objs = Gameplay.Instance.worldObjs;

            for (int i = objs.Count - 1; i >= 0; i--) {
                if (Vector3.Distance(transform.position, objs[i].transform.position) < range) {
                    Destroy(objs[i]);
                    objs.RemoveAt(i);
                }
            }
        });

    }
}
