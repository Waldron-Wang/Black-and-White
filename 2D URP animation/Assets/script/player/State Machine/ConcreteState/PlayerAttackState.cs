using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackState : PlayerState
{
    bool IsStateEnd;

    public PlayerAttackState(Player player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();

        IsStateEnd = false;
        player.is_attacking = true;
        player.attack_input = false;
        player.attack_first_input = false;
        player.attack_count++;

        switch (player.attack_count)
        {
            case 1:
                player.BeginCheckAttackInput();
                player.ChangeAnimationState(Player.AnimationAttack1);
                Debug.Log("Enter first attack state");
                break;

            case 2:
                player.BeginCheckAttackInput();
                player.ChangeAnimationState(Player.AnimationAttack2);
                Debug.Log("Enter second attack state");
                break;

            case 3:
                player.BeginCheckAttackInput();
                player.ChangeAnimationState(Player.AnimationAttack3);
                Debug.Log("Enter third attack state");
                break;
        }
    }

    public override void ExitState()
    {
        base.ExitState();

        player.is_attacking_end = true;
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        Debug.Log("attack count: " + player.attack_count);
        Debug.Log("attack input: " + player.attack_input);
        Debug.Log("is checking attack input: " + player.is_checking_attack_input);

        switch (player.attack_count)
        {
            case 1:
                if (!player.IsAnimationPlaying(Player.AnimationAttack1))
                {
                    player.is_attacking = false;
                    IsStateEnd = true;
                    Debug.Log("Is state end 1: " + IsStateEnd);
                }
                break;
            case 2:
                if (!player.IsAnimationPlaying(Player.AnimationAttack2))
                {
                    player.is_attacking = false;
                    IsStateEnd = true;
                    Debug.Log("Is state end 2: " + IsStateEnd);
                }
                break;
            case 3:
                if (!player.IsAnimationPlaying(Player.AnimationAttack3))
                {
                    player.is_attacking = false;
                    IsStateEnd = true;
                    Debug.Log("Is state end 3: " + IsStateEnd);
                }
                break;
        }

        if (IsStateEnd)
        {
            // switch to Idle state
            if (player.HorizontalMoveInput < 0.1f && player.HorizontalMoveInput > -0.1f)
            {
                playerStateMachine.ChangeState(player.IdleState);

                player.ChangeAnimationState(Player.AnimationIdle);
            }

            // switch to run state
            if (player.HorizontalMoveInput > 0.1f || player.HorizontalMoveInput < -0.1f)
            {
                playerStateMachine.ChangeState(player.RunState);

                player.ChangeAnimationState(Player.AnimationRun);
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
