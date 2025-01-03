using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animation movements;

    void Awake()
    {
        movements = GetComponent<Animation>();
    }

    // Animations controllers
    public void DidJump()
    {
        movements.Play(Tags.ANIMATION_JUMP);
        movements.PlayQueued(Tags.ANIMATION_JUMP_FALL);
        movements.PlayQueued(Tags.ANIMATION_JUMP_LAND);
    }

    public void DidLand()
    {
        movements.Stop(Tags.ANIMATION_JUMP_FALL);
        movements.Stop(Tags.ANIMATION_JUMP_LAND);
        movements.Blend(Tags.ANIMATION_JUMP_LAND, 0);
        movements.CrossFade(Tags.ANIMATION_RUN);
    }

    public void PlayerRun()
    {
        movements.Play(Tags.ANIMATION_RUN);
    }
}
