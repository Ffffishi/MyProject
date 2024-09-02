using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;
/// <summary>
/// 敌人受伤状态
/// </summary>
public class HurtState : IState
{
    //击退方向
    private Vector2 direction;
    
    private float timer;

    private Enemy enemy;
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="enemy"></param>
    public HurtState(Enemy enemy)
    {
        this.enemy = enemy;
    }
    public void OnEnterState()
    {
        //播放受伤动画
        if (enemy.animator != null)
            enemy.animator.Play("EHurt");
        //Debug.Log("受伤状态进入");

    }
    public void OnUpdateState()
    {
        //如果目标可以击退，则获取击退方向
        if (enemy.canKnockback == true)
        {
            direction=(enemy.transform.position - enemy.player.position).normalized;
        }
    }
    public void OnFixedUpdateState()
    {
        //如果计时器小于击退时间，则向击退方向添加力
        if (timer < enemy.knockbackTime)
        {
            enemy.rb.AddForce(direction * enemy.knockbackForce*0.5f, ForceMode2D.Impulse);
            //enemy.transform.position += (Vector3)direction  * Time.deltaTime;

            timer += Time.fixedDeltaTime;
        }
        else
        {
            //否则回到追逐状态
            timer = 0;
            enemy.isHurt = false;
            enemy.TransitionToState(EnemyState.Chase);
        }
    }
    public void OnExitState()
    {
        //清除受伤状态
        enemy.isHurt = false;
    }
}
