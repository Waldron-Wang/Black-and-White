using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : AbstractState<Player>
{
    public PlayerIdleState(Player player, StateMachine<Player> characterStateMachine) : base(player, characterStateMachine)
    {
    }
    
    public override void EnterState()
    {
        base.EnterState();
        Debug.Log("Enter Idle State");

        player.BeginVerticalMoveCheck();
    }

    public override void ExitState()
    {
        base.ExitState();

        player.EndVerticalMoveCheck();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

        // switch to run state
        if (player.HorizontalMoveInput > 0.1f || player.HorizontalMoveInput < -0.1f)
        {
            characterStateMachine.ChangeState(player.RunState);

            player.ChangeAnimationState(Player.AnimationRun);
        }

        // switch to jump state
        if (player.VerticalMoveInput  == true)
        {
            characterStateMachine.ChangeState(player.JumpState);

            player.ChangeAnimationState(Player.AnimationFirstJump);
        }
        
        // switch to Fall state
        if (player.IsFalling == true)
        {
            player.StateMachine.ChangeState(player.FallState);

            player.ChangeAnimationState(Player.AnimationFall);
        }
        
        // switch to Dodge state
        if (player.DodgeInput && player.CanDodge)
        {
            characterStateMachine.ChangeState(player.DodgeState);

            player.ChangeAnimationState(Player.AnimationDodge);
        }

        // switch to Attack state
        if (player.attack_first_input || GameManager.gameManager.IsAttackInputDetected())
        {
            characterStateMachine.ChangeState(player.AttackState);
        }
        if(player.isClimbing)
        {
            characterStateMachine.ChangeState(player.ClimbState);
        }
    }
}

