using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState : AbstractState<Enemy>
{
    public EnemyAttackState(Enemy character, StateMachine<Enemy> characterStateMachine) : base(character, characterStateMachine)
    {
    }
}
