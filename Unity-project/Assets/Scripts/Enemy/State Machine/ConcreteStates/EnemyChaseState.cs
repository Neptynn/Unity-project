using CustomEventBus.Signals;
using CustomEventBus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;


public class EnemyChaseState : EnemyState
{
    public EnemyChaseState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
    {
    }

    public override void AnimationTriggerEvent(Enemy.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);

        enemy.EnemyChaseBaseInstance.DoAnimationTriggerEventLogic(triggerType);
    }

    public override void EnterState() // Start
    {
        base.EnterState();

        enemy.EnemyChaseBaseInstance.DoEnterLogic();

    }

    public override void ExitState()
    {
        base.ExitState();

        enemy.EnemyChaseBaseInstance.DoExitLogic();
    }

    public override void FrameUpdate() // Update
    {
        base.FrameUpdate();

        enemy.EnemyChaseBaseInstance.DoFrameUpdateLogic();
    }

    public override void PhysicsUpdate() //Fixed Update
    {
        base.PhysicsUpdate();     

        enemy.EnemyChaseBaseInstance.DoPhysicsLogic(); 
    }

    
}
