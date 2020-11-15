using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(SpriteRenderer))]
public class FramesAnimator : MonoBehaviour
{
    // Prior Animation is animation that occur in priority.
    // After finish, prior animation will jump to normal animation.
    // So that say, prior animation will occur only one time, then jump to normal animation.
    public Sprite[] spritesheet;
    public float RATE = 0.2f;
    public float DELAY_RANGE = 0.2f;
    public bool looping = true;

    [HideInInspector]
    private SpriteRenderer spriteRenderer;

    [HideInInspector]
    private int frameshow = 0;

    private float count;

    public enum EndOfAnimation { DoNothing, SetActiveFalse, Destroy, SetActiveFalseParent, DestroyParent, DisableAnimation }
    public EndOfAnimation endOfAnimation = EndOfAnimation.DoNothing;

    EndOfAnimation currentEndOfAnim;

    void Start()
    {
        count = Random.Range(0f, DELAY_RANGE); //make animation pause for random time
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (spritesheet.Length == 0) return;

        if (spritesheet == null) return;

        count += Time.deltaTime;
        if (count > RATE) {
            count = 0;
            DrawNextFrame();
        }
    }

    private void OnEnable()
    {
        count = 0;
        frameshow = 0;
        currentEndOfAnim = endOfAnimation;
        looping = true;
    }

    public virtual void DrawNextFrame()
    {
        frameshow++;

        if (frameshow >= spritesheet.Length)
        {
            if (DoEndOfAnimation()) return;
            frameshow = 0;
        }

        spriteRenderer.sprite = spritesheet[frameshow];
    }

    public bool DoEndOfAnimation()
    {
        if (!looping)
            return true;

        if (currentEndOfAnim == EndOfAnimation.DoNothing) return false;
        if (currentEndOfAnim == EndOfAnimation.SetActiveFalse) gameObject.SetActive(false);
        if (currentEndOfAnim == EndOfAnimation.Destroy) Destroy(gameObject);
        if (currentEndOfAnim == EndOfAnimation.SetActiveFalseParent) transform.parent.gameObject.SetActive(false);
        if (currentEndOfAnim == EndOfAnimation.DestroyParent) Destroy(transform.parent.gameObject);
        if (currentEndOfAnim == EndOfAnimation.DisableAnimation) this.enabled = false;

        return true;
    }
}
