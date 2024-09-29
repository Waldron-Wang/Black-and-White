using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine<T> where T : MonoBehaviour
{
    public AbstractState<T> CurrentPlayerState { get; set; }

    public void InitializeState(AbstractState<T> StartingState)
    {
        CurrentPlayerState = StartingState;
        CurrentPlayerState.EnterState();
    }

    public void ChangeState(AbstractState<T> NewState)
    {
        CurrentPlayerState.ExitState();
        CurrentPlayerState = NewState;
        CurrentPlayerState.EnterState();
    }
}
