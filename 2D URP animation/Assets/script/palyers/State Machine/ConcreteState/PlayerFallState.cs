using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallState : PlayerState
{
    public PlayerFallState(Player player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine)
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
        if (player.VerticalMoveInput && player.JumpCount == 1)
            player.StateMachine.ChangeState(player.JumpState);

        // ensure all the methods below can be called noly when player is not falling
        if (player.IsFalling)
            return;

        // switch to Idle or Run state
        if (player.PlayerRigidbody.velocity.x > 0.1f || player.PlayerRigidbody.velocity.x < -0.1f)
            player.StateMachine.ChangeState(player.RunState);
        else
            player.StateMachine.ChangeState(player.IdleState);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
