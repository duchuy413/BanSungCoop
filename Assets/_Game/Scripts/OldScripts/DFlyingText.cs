using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DFlyingText : MonoBehaviour
{
    public float ANIMATION_SPEED = 3f;
    public float COUNT_DOWN_TIME = 0.5f;
    private float countdown = 0;

    private void OnEnable()
    {
        countdown = COUNT_DOWN_TIME;
    }

    void Update()
    {
        countdown -= Time.deltaTime;
        if (countdown < 0)
            gameObject.SetActive(false);

        Vector3 position = transform.position;
        position += new Vector3(0f, ANIMATION_SPEED);
        transform.position = Vector3.Lerp(transform.position, position, Time.deltaTime * 0.3f);
    }
}
