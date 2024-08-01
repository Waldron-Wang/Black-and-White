using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerState
{
    public PlayerIdleState(Player player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine)
    {
    }
    
    public override void EnterState()
    {
        base.EnterState();
        Debug.Log("Enter Idle State");
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

        // switch to run state
        if (player.HorizontalMoveInput > 0.1f || player.HorizontalMoveInput < -0.1f)
            playerStateMachine.ChangeState(player.RunState);
        
        // switch to jump state
        if (player.VerticalMoveInput  == true)
            playerStateMachine.ChangeState(player.JumpState);
    }
}

