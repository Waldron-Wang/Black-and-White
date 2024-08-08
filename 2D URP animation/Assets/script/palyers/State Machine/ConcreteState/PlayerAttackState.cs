using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackState : PlayerState
{
    public PlayerAttackState(Player player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();

        player.is_attacking = true;
        player.attack_input = false;

        switch (player.attack_count)
        {
            case 1:
                player.BeginCheckAttackInput();
                player.attack_count++;
                Debug.Log("Enter first attack state");
                break;

            case 2:
                player.BeginCheckAttackInput();
                player.attack_count++;
                Debug.Log("Enter second attack state");
                break;

            case 3:
                player.BeginCheckAttackInput();
                Debug.Log("Enter third attack state");
                break;
        }
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

        switch (player.attack_count)
        {
            case 1:
                if (!player.IsAnimationPlaying(Player.AnimationAttack1))
                {
                    // StartCoroutine(player.EndCheckAttackInput());
                    player.is_attacking = false;
                    //Debug.Log("end 1");
                }
                break;
            case 2:
                if (!player.IsAnimationPlaying(Player.AnimationAttack2))
                {
                    // StartCoroutine(player.EndCheckAttackInput());
                    player.is_attacking = false;
                    //Debug.Log("end 2");
                }
                break;
            case 3:
                if (!player.IsAnimationPlaying(Player.AnimationAttack3))
                {
                    // StartCoroutine(player.EndCheckAttackInput());
                    player.is_attacking = false;
                    //Debug.Log("end 3");
                }
                break;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
