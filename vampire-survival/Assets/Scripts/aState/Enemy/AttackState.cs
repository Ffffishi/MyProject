using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;
/// <summary>
/// F���˹���״̬
/// </summary>
public class AttackState : IState
{
    public Enemy enemy;
    /// <summary>
    /// ���캯��
    /// </summary>
    /// <param name="enemy"></param>
    public AttackState(Enemy enemy)
    {
        this.enemy = enemy;
    }
    public void OnEnterState()
    {
        //enemy.animator.enabled = false;
        //Debug.Log("���˽��빥��״̬");
    }
    public void OnUpdateState()
    {
        enemy.StopChase();
        //�������������״̬
        if (enemy.isHurt)
        {
            enemy.TransitionToState(EnemyState.Hurt);
        }

        if(enemy.cooldownTimer>0)
            enemy.TransitionToState(EnemyState.Chase);
        //Debug.Log("���˽��빥��״̬ing");

    }
    public void OnFixedUpdateState()
    {
        //Debug.Log("fixed���˽��빥��״̬ing");
        if (enemy.isHurt)
        {
            enemy.TransitionToState(EnemyState.Hurt);
        }
        enemy.Attack();
    }
        
    public void OnExitState()
    {
        //enemy.animator.enabled = true;
    }
}
