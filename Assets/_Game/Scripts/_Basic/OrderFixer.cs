using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class OrderFixer : MonoBehaviour
{
    public enum Type { Static, Dynamic }
    public Type type = Type.Dynamic;

    public SpriteRenderer[] layer1;
    public SpriteRenderer[] layer2;
    public SpriteRenderer[] layer3;

    private float UPDATE_RATE = 0.1f;
    private int DEPTH = 10;
    private int PLAYER_FIX = 3;

    float count;
    SpriteRenderer render;

    private void Start()
    {
        render = GetComponent<SpriteRenderer>();

        if (type == Type.Static)
        {
            render.sortingOrder = (int)(-transform.position.y * DEPTH);
            Destroy(this);
        }
    }
    private void Update()
    {
        count -= Time.deltaTime;
        if (count < 0)
        {
            count = UPDATE_RATE;
            int order = (int)(-transform.position.y * DEPTH);
            //if (gameObject.tag == "Player")
            //    render.sortingOrder = order + PLAYER_FIX;
            //else
            //    render.sortingOrder = order;
            render.sortingOrder = order;
            for (int i = 0; i < layer1.Length; i++) {
                layer1[i].sortingOrder = order + 1;
            }

            for (int i = 0; i < layer2.Length; i++) {
                layer2[i].sortingOrder = order + 2;
            }

            for (int i = 0; i < layer3.Length; i++) {
                layer3[i].sortingOrder = order + 3;
            }

            //foreach (Transform child in transform) {
            //    SpriteRenderer renderer = child.GetComponent<SpriteRenderer>();
            //    if (renderer != null)
            //        renderer.sortingOrder = order + 1;
            //}
        }
    }

    public void UpdateOrder()
    {
        render.sortingOrder = (int)(-transform.position.y * DEPTH);
    }

}
