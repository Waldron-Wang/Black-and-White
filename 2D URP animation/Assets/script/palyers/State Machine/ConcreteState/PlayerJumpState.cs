using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerJumpState : PlayerState
{
    bool HaveJumped = false;

    public PlayerJumpState(Player player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();

        player.BeginVerticalMoveCheck();

        player.JumpCount++;

        if (player.JumpCount == 1)
            Debug.Log("Enter First Jump state");
        else
            Debug.Log("Enter Second Jump state");

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

        // ensure all the method below can be called only after the Jump method are called
        if (!HaveJumped)
            return;

        // switch to Fall state
        if (player.IsFalling == true)
            player.StateMachine.ChangeState(player.FallState);

        // switch to second jump state
        if (player.VerticalMoveInput == true && player.JumpCount == 1)
            player.StateMachine.ChangeState(player.JumpState);
    }

    public override void PhysicsUpdate()
    {
        MoveInAir();
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

    void MoveInAir()
    {
        if (player.HorizontalMoveInput > 0.1f || player.HorizontalMoveInput < -0.1f)
            player.PlayerRigidbody.velocity = new Vector3(player.HorizontalMoveInput * player.RunSpeed * (1 - player.AirDrag), player.PlayerRigidbody.velocity.y, 0f);
        else
            player.PlayerRigidbody.velocity = new Vector3(0f, player.PlayerRigidbody.velocity.y, 0f);
    }
}
