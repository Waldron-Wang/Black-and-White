using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDodgeState : PlayerState
{
    float original_gravity;
    float DodgeTimer;

    public PlayerDodgeState(Player player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        Debug.Log("Enter Dodge state");

        DodgeTimer = player.DodgeTime;

        player.CanDodge = false;
        player.DodgeInput = false;
        player.IsDodging = true;
        original_gravity = player.PlayerRigidbody.gravityScale;
        player.PlayerRigidbody.gravityScale = player.DodgeGravity;
    }

    public override void ExitState()
    {
        base.ExitState();

        player.IsDodging = false;
        player.PlayerRigidbody.velocity = new Vector3(0f, 0f, 0f);
        player.PlayerRigidbody.gravityScale = original_gravity;

        player.beginDodgeCoolTimer = true;
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

        if (DodgeTimer > 0)
        {
            DodgeTimer -= Time.deltaTime;
        }
        else
        {
            // switch to idle state
            if (player.IsGround && player.HorizontalMoveInput < 0.1f && player.HorizontalMoveInput > -0.1f)
            {
                playerStateMachine.ChangeState(player.IdleState);

                player.ChangeAnimationState(Player.AnimationIdle);
            }

            // switch to run state
            if (player.IsGround && (player.HorizontalMoveInput > 0.1f || player.HorizontalMoveInput < -0.1f))
            {
                playerStateMachine.ChangeState(player.RunState);

                player.ChangeAnimationState(Player.AnimationRun);
            }

            // switch to Fall state
            if (!player.IsGround)
            {
                player.StateMachine.ChangeState(player.FallState);

                player.ChangeAnimationState(Player.AnimationFall);
            }
        }

    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        if (DodgeTimer > 0)
            player.PlayerRigidbody.velocity = new Vector3(player.PlayerTransform.right.x * player.DodgeForce, 0f, 0f);
    }

}