using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunState : PlayerState
{
    public PlayerRunState(Player player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();        
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        player.HorizontalMoveInput = Input.GetAxisRaw("Horizontal");

        if (player.HorizontalMoveInput > 0.1f || player.HorizontalMoveInput < -0.1f)
        {
            player.RigidBody.velocity = new Vector3(player.HorizontalMoveInput * player.RunSpeed, player.RigidBody.velocity.y, 0f);
        }
        else
        {
            player.RigidBody.velocity = new Vector3(0f, player.RigidBody.velocity.y, 0f);
        }
    }

}
