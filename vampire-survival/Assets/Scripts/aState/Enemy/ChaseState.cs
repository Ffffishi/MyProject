using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 敌人追击状态
/// </summary>
public class ChaseState : IState
{

    private Enemy enemy;
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="enemy"></param>
    public ChaseState(Enemy enemy)
    {
        this.enemy = enemy;
    }
    public void OnEnterState()
    {
        if (enemy.animator != null)
            enemy.animator.Play("ERun");
        //将敌人追击目标设置为玩家
        if(enemy.destinationSetter.target !=enemy.targetPoint)
            enemy.destinationSetter.target = enemy.targetPoint;
        //敌人开始追击
        if (enemy.destinationSetter.target != null)
        {
            enemy.aiPath.canMove = true;
        }
        //Debug.Log("追击状态");

    }
    public void OnUpdateState()
    {
        if (enemy.isHurt)
        {
            enemy.TransitionToState(EnemyState.Hurt);
        }

        //检测玩家是否在攻击范围内
        enemy.GetPlayerDistance();
        //如果玩家在攻击范围内，则切换到攻击状态
        if (enemy.isBirth == true)
        {
            enemy.isBirth = false;
            return;
        }

        if (enemy.distanceToPlayer < enemy.attackRange)
        {
            //enemy.StopChase();
            //Debug.Log("敌人与玩家距离：" + enemy.distanceToPlayer);
            //Debug.Log("敌人攻击范围：" + enemy.attackRange);
            enemy.TransitionToState(EnemyState.Attack);
        }
    }
    public void OnFixedUpdateState()
    {
       
    }
    public void OnExitState()
    {
        enemy.aiPath.canMove = false;
    }
}
