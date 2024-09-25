using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallState : AbstractState
{
    public PlayerFallState(Player player, StateMachine playerStateMachine) : base(player, playerStateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        Debug.Log("Enter Fall state");

        if (player.JumpCount == 0)
            player.JumpCount = 1;
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

        // switch to Second Jump state
        if (player.VerticalMoveInput == true && player.JumpCount == 1)
        {
            player.StateMachine.ChangeState(player.JumpState);

            player.ChangeAnimationState(Player.AnimationSecondJump);
        }

        // switch to Dodge state
        if (player.DodgeInput && player.CanDodge)
        {
            playerStateMachine.ChangeState(player.DodgeState);

            player.ChangeAnimationState(Player.AnimationDodge);
        }

        // ensure all the methods below can be called noly when player is not falling
        if (player.IsFalling)
            return;

        // switch to Idle or Run state
        if (player.PlayerRigidbody.velocity.x > 0.1f || player.PlayerRigidbody.velocity.x < -0.1f)
        {
            player.StateMachine.ChangeState(player.RunState);

            player.ChangeAnimationState(Player.AnimationRun);
        }
        else
        {
            player.StateMachine.ChangeState(player.IdleState);

            player.ChangeAnimationState(Player.AnimationIdle);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        if (player.HorizontalMoveInput > 0.1f || player.HorizontalMoveInput < -0.1f)
            player.PlayerRigidbody.velocity = new Vector3(player.HorizontalMoveInput * player.RunSpeed * (1 - player.AirDrag), player.PlayerRigidbody.velocity.y, 0f);
        else
            player.PlayerRigidbody.velocity = new Vector3(0f, player.PlayerRigidbody.velocity.y, 0f);
    }
}
