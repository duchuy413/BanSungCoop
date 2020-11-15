using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
[RequireComponent(typeof(FramesAnimator))]
public class PlayerAnimationUpdate : MonoBehaviour {
    public Sprite[] go;
    public Sprite[] stand;
    public Sprite[] jump;
    public Sprite[] fall;
    public Sprite[] run;

    Player player;
    FramesAnimator animator;

    void Start() {
        player = GetComponent<Player>();
        animator = GetComponent<FramesAnimator>();
    }

    void Update() {
        if (player.state == "stand") {
            animator.spritesheet = stand;
        } else if (player.state == "go") {
            animator.spritesheet = go;
        } else if (player.state == "jump") {
            animator.spritesheet = jump;
        } else if (player.state == "fall") {
            animator.spritesheet = fall;
        } else if (player.state == "run") {
            animator.spritesheet = run;
        }

        if (player.direction == "left") {
            GetComponent<SpriteRenderer>().flipX = false;
        } else {
            GetComponent<SpriteRenderer>().flipX = true;
        }

    }
}
