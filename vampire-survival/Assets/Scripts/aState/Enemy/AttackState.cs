using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;
/// <summary>
/// F敌人攻击状态
/// </summary>
public class AttackState : IState
{
    public Enemy enemy;
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="enemy"></param>
    public AttackState(Enemy enemy)
    {
        this.enemy = enemy;
    }
    public void OnEnterState()
    {
        //enemy.animator.enabled = false;
        //Debug.Log("敌人进入攻击状态");
    }
    public void OnUpdateState()
    {
        enemy.StopChase();
        //受伤则进入受伤状态
        if (enemy.isHurt)
        {
            enemy.TransitionToState(EnemyState.Hurt);
        }

        if(enemy.cooldownTimer>0)
            enemy.TransitionToState(EnemyState.Chase);
        //Debug.Log("敌人进入攻击状态ing");

    }
    public void OnFixedUpdateState()
    {
        //Debug.Log("fixed敌人进入攻击状态ing");
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
