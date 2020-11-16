using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

[RequireComponent(typeof(Player))]
[RequireComponent(typeof(FramesAnimator))]
public class PlayerAnimationUpdate : NetworkBehaviour{
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

    public void Update() {
        if (isLocalPlayer) {
            UpdateAnim(player.state);
        }
    }

    public void UpdateAnim(string state) {
        if (state == "stand") {
            animator.spritesheet = stand;
        } else if (state == "go") {
            animator.spritesheet = go;
        } else if (state == "jump") {
            animator.spritesheet = jump;
        } else if (state == "fall") {
            animator.spritesheet = fall;
        } else if (state == "run") {
            animator.spritesheet = run;
        }
    }
}
