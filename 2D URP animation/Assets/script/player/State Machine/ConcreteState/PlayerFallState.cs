using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallState : AbstractState<Player>
{
    public PlayerFallState(Player player, StateMachine<Player> characterStateMachine) : base(player, characterStateMachine)
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
            characterStateMachine.ChangeState(player.DodgeState);

            player.ChangeAnimationState(Player.AnimationDodge);
        }

        // ensure all the methods below can be called noly when player is not falling
        if (player.IsFalling)
            return;

        // switch to Idle or Run state
        if (player.playerRigidbody.velocity.x > 0.1f || player.playerRigidbody.velocity.x < -0.1f)
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
            player.playerRigidbody.velocity = new Vector3(player.HorizontalMoveInput * player.RunSpeed * (1 - player.AirDrag), player.playerRigidbody.velocity.y, 0f);
        else
            player.playerRigidbody.velocity = new Vector3(0f, player.playerRigidbody.velocity.y, 0f);
    }
}
