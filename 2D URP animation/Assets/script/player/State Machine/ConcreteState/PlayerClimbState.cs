using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClimbState : AbstractState<Player>
{
    public PlayerClimbState(Player player, StateMachine<Player> characterStateMachine) : base(player, characterStateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        Debug.Log("Enter Climb state");

        player.CanClimb = false;
        player.isClimbing = true;
    }

    public override void ExitState()
    {
        base.ExitState();

        player.isClimbing = false;
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

        if (player.isClimbing)
        {
            player.VerticalMoveInput = Input.GetAxis("Vertical") > 0 ? true : false;

            if (player.VerticalMoveInput == true)
            {
                player.transform.position += Vector3.up * player.ClimbSpeed * Time.deltaTime;
            }
            else if (player.VerticalMoveInput == false)
            {
                player.transform.position += Vector3.down * player.ClimbSpeed * Time.deltaTime;
            }
        }
    }
}
