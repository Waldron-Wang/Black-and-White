using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackState : AbstractState<Player>
{
    private bool isStateEnd;

    public PlayerAttackState(Player player, StateMachine<Player> characterStateMachine) : base(player, characterStateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();

        isStateEnd = false;
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

            default:
                player.attack_count = 1;
                player.BeginCheckAttackInput();
                player.ChangeAnimationState(Player.AnimationAttack1);
                Debug.Log("Reset to first attack state");
                break;
        }
    }

    public override void ExitState()
    {
        base.ExitState();

        player.is_attacking_end = true;
        player.is_attacking = false;
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

        switch (player.attack_count)
        {
            case 1:
                if (!player.IsAnimationPlaying(Player.AnimationAttack1))
                {
                    isStateEnd = true;
                }
                break;
            case 2:
                if (!player.IsAnimationPlaying(Player.AnimationAttack2))
                {
                    isStateEnd = true;
                }
                break;
            case 3:
                if (!player.IsAnimationPlaying(Player.AnimationAttack3))
                {
                    player.attack_input=false;
                    isStateEnd = true;
                }
                break;
        }

        if (isStateEnd)
        {
            if (player.attack_input)
            {
                player.attack_input = false;
                characterStateMachine.ChangeState(player.AttackState);
            }
            else
            {
                player.attack_count = 0;
                if (player.HorizontalMoveInput < 0.1f && player.HorizontalMoveInput > -0.1f)
                {
                    characterStateMachine.ChangeState(player.IdleState);
                    player.ChangeAnimationState(Player.AnimationIdle);
                }
                else
                {
                    characterStateMachine.ChangeState(player.RunState);
                    player.ChangeAnimationState(Player.AnimationRun);
                }
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
