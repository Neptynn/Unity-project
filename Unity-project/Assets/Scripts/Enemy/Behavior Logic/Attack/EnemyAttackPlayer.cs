using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Enemy;

[CreateAssetMenu(fileName = "Attack-Player", menuName = "Enemy Logic/Attack Logic/Direct Chase")]
public class EnemyAttackPlayer : EnemyAttackSOBase
{
    
    public override void DoAnimationTriggerEventLogic(Enemy.AnimationTriggerType triggerType)
    {
        base.DoAnimationTriggerEventLogic(triggerType);

        switch (triggerType)
        {
            case AnimationTriggerType.EnableMove:
                enableMove = true;
                break;
            case AnimationTriggerType.DisableMove:                             
                enableMove = false;
                break;
            default:
                break;
        }
    }

    public override void DoEnterLogic()
    {
        base.DoEnterLogic();

        _timer = _timerBetweenAttack * 0.8f;


    }

    public override void DoExitLogic()
    {
        base.DoExitLogic();


    }

    public override void DoFrameUpdateLogic()
    {
        base.DoFrameUpdateLogic();

        enemy.Anim.SetBool("Move", false);
        enemy.MoveEnemy(new Vector2(0,0));

        if (_timer > _timerBetweenAttack)
        {
            _timer = 0f;

            Vector2 dir = new Vector2(playerTransform.position.x - enemy.transform.position.x, 0f).normalized;
            enemy.CheckForLeftOrRightFacing(dir);
            if (enemy.Anim != null)
            {
                enemy.Anim.SetTrigger("Attack");
            }
        }
        if(enableMove)
        {
            _timer += Time.deltaTime;
        }
       
    }

    public override void DoPhysicsLogic()
    {
        base.DoPhysicsLogic();
    }

    public override void Initialize(GameObject gameObject, Enemy enemy)
    {
        base.Initialize(gameObject, enemy);
    }

    public override void ResetValues()
    {
        base.ResetValues();

    }
}
