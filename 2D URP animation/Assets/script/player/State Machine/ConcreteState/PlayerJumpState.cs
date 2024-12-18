using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerJumpState : AbstractState<Player>
{
    bool HaveJumped = false;
    bool IsChangeDirection = false;
    float InitialHorizontalSpeed;
    float CurrentHorizontalSpeed;

    public PlayerJumpState(Player player, StateMachine<Player> characterStateMachine) : base(player, characterStateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();

        if (player.JumpCount == 0)
            Debug.Log("Enter First Jump state");
        else if (player.JumpCount == 1)
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
            characterStateMachine.ChangeState(player.DodgeState);

            player.ChangeAnimationState(Player.AnimationDodge);
        }
    }

    public override void PhysicsUpdate()
    {
        if (IsChangeDirection)
        {
            if (player.HorizontalMoveInput > 0.1f || player.HorizontalMoveInput < -0.1f)
                player.playerRigidbody.velocity = new Vector3(player.HorizontalMoveInput * player.RunSpeed * (1 - player.AirDrag), player.playerRigidbody.velocity.y, 0f);
            else
                player.playerRigidbody.velocity = new Vector3(0f, player.playerRigidbody.velocity.y, 0f);
        }
        else
        {
            if (player.HorizontalMoveInput > 0.1f || player.HorizontalMoveInput < -0.1f)
                player.playerRigidbody.velocity = new Vector3(player.HorizontalMoveInput * player.RunSpeed, player.playerRigidbody.velocity.y, 0f);
            else
                player.playerRigidbody.velocity = new Vector3(0f, player.playerRigidbody.velocity.y, 0f);
        }
    }

    void Jump()
    {
        switch (player.JumpCount)
        {
            case 1:
                player.playerRigidbody.AddForce(new Vector3(0f, player.FirstJumpForce, 0f), ForceMode2D.Impulse);
                player.VerticalMoveInput = false;
                break;
            case 2:
                player.playerRigidbody.velocity = new Vector3(player.playerRigidbody.velocity.x, 0f, 0f);
                player.playerRigidbody.AddForce(new Vector3(0f, player.SecondJumpForce, 0f), ForceMode2D.Impulse);
                player.VerticalMoveInput = false;
                break;
        }
    }
}
