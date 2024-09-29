using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : AbstractState<Enemy>
{
    public EnemyIdleState(Enemy enemy, StateMachine<Enemy> characterStateMachine) : base(enemy, characterStateMachine)
    {
    }

}
