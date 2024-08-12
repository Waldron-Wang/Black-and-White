using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerJumpState : PlayerState
{
    bool HaveJumped = false;
    bool IsChangeDirection = false;
    float InitialHorizontalSpeed;
    float CurrentHorizontalSpeed;

    public PlayerJumpState(Player player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();

        if (player.JumpCount == 1)
            Debug.Log("Enter First Jump state");
        else
            Debug.Log("Enter Second Jump state");

        player.BeginVerticalMoveCheck();

        player.JumpCount++;

        InitialHorizontalSpeed = player.HorizontalMoveInput;

        Jump();
        HaveJumped = true;
    }

    public override void ExitState()
    {
        base.ExitState();

        HaveJumped = false;

        if (player.JumpCount == 2)
            player.EndVerticalMoveCheck();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

        CurrentHorizontalSpeed = player.HorizontalMoveInput;

        // check if player change its direction
        if (!IsChangeDirection && (CurrentHorizontalSpeed != InitialHorizontalSpeed))
            IsChangeDirection = true;

        // ensure all the method below can be called only after the Jump method are called
        if (!HaveJumped)
            return;

        // switch to Fall state
        if (player.IsFalling == true)
        {
            player.StateMachine.ChangeState(player.FallState);

            player.ChangeAnimationState(Player.AnimationFall);
        }

        // switch to second jump state
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
    }

    public override void PhysicsUpdate()
    {
        if (IsChangeDirection)
        {
            if (player.HorizontalMoveInput > 0.1f || player.HorizontalMoveInput < -0.1f)
                player.PlayerRigidbody.velocity = new Vector3(player.HorizontalMoveInput * player.RunSpeed * (1 - player.AirDrag), player.PlayerRigidbody.velocity.y, 0f);
            else
                player.PlayerRigidbody.velocity = new Vector3(0f, player.PlayerRigidbody.velocity.y, 0f);
        }
        else
        {
            if (player.HorizontalMoveInput > 0.1f || player.HorizontalMoveInput < -0.1f)
                player.PlayerRigidbody.velocity = new Vector3(player.HorizontalMoveInput * player.RunSpeed, player.PlayerRigidbody.velocity.y, 0f);
            else
                player.PlayerRigidbody.velocity = new Vector3(0f, player.PlayerRigidbody.velocity.y, 0f);
        }
    }

    void Jump()
    {
        switch (player.JumpCount)
        {
            case 1:
                player.PlayerRigidbody.AddForce(new Vector3(0f, player.FirstJumpForce, 0f), ForceMode2D.Impulse);
                player.VerticalMoveInput = false;
                break;
            case 2:
                player.PlayerRigidbody.velocity = new Vector3(player.PlayerRigidbody.velocity.x, 0f, 0f);
                player.PlayerRigidbody.AddForce(new Vector3(0f, player.SecondJumoForce, 0f), ForceMode2D.Impulse);
                player.VerticalMoveInput = false;
                break;
        }
    }
}
