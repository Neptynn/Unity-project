using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : EnemyState
{
    public EnemyIdleState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
    {
    }

    public override void AnimationTriggerEvent(Enemy.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);

        enemy.EnemyIdleBaseInstance.DoAnimationTriggerEventLogic(triggerType);
    }

    public override void EnterState() //Start
    {
        base.EnterState();

        enemy.EnemyIdleBaseInstance.DoEnterLogic();
    }

    public override void ExitState()
    {
        base.ExitState();

        enemy.EnemyIdleBaseInstance.DoExitLogic();
    }

    public override void FrameUpdate() //Update
    {
        base.FrameUpdate();

        enemy.EnemyIdleBaseInstance.DoFrameUpdateLogic();
    }
    public override void PhysicsUpdate() //Fixed Update
    {
        base.PhysicsUpdate();

        enemy.EnemyIdleBaseInstance.DoPhysicsLogic();
    }


}
