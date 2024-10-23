using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public abstract class AbstractState<T> where T : MonoBehaviour
{
    protected T character;
    protected Player player
    {
        get { return character as Player; }
    }
    protected Enemy enemy
    {
        get { return character as Enemy; }
    }
    protected StateMachine<T> characterStateMachine;

    public AbstractState(T character, StateMachine<T> characterStateMachine)
    {
        this.character = character;
        this.characterStateMachine = characterStateMachine;
    }

    public virtual void EnterState() { }
    public virtual void ExitState() { }
    public virtual void FrameUpdate() { }
    public virtual void PhysicsUpdate() { }
    public virtual void OnTriggerEnter2D(Collider2D other) { }
    public virtual void OnTriggerStay2D(Collider2D other) { }
    public virtual void OnTriggerExit2D(Collider2D other) { }
    public virtual void AnimationTriggerEvent(Player.AnimationTriggerType triggerType) { }
}
