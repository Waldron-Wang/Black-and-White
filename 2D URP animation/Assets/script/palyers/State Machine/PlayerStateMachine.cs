using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine
{
    public PlayerState CurrentPlayerState { get; set; }

    public void InitializeState(PlayerState StartingState)
    {
        CurrentPlayerState = StartingState;
        CurrentPlayerState.EnterState();
    }

    public void ChangeState(PlayerState NewState)
    {
        CurrentPlayerState.ExitState();
        CurrentPlayerState = NewState;
        CurrentPlayerState.EnterState();
    }
}
