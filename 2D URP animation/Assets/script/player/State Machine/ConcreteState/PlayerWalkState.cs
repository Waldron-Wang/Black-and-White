using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWalkState : AbstractState<Player>
{
    public PlayerWalkState(Player player, StateMachine<Player> characterStateMachine) : base(player, characterStateMachine)
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
    }
}
