using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    public AbstractState CurrentPlayerState { get; set; }

    public void InitializeState(AbstractState StartingState)
    {
        CurrentPlayerState = StartingState;
        CurrentPlayerState.EnterState();
    }

    public void ChangeState(AbstractState NewState)
    {
        CurrentPlayerState.ExitState();
        CurrentPlayerState = NewState;
        CurrentPlayerState.EnterState();
    }
}
