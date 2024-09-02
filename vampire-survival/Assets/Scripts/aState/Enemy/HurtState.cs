using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;
/// <summary>
/// ��������״̬
/// </summary>
public class HurtState : IState
{
    //���˷���
    private Vector2 direction;
    
    private float timer;

    private Enemy enemy;
    /// <summary>
    /// ���캯��
    /// </summary>
    /// <param name="enemy"></param>
    public HurtState(Enemy enemy)
    {
        this.enemy = enemy;
    }
    public void OnEnterState()
    {
        //�������˶���
        if (enemy.animator != null)
            enemy.animator.Play("EHurt");
        //Debug.Log("����״̬����");

    }
    public void OnUpdateState()
    {
        //���Ŀ����Ի��ˣ����ȡ���˷���
        if (enemy.canKnockback == true)
        {
            direction=(enemy.transform.position - enemy.player.position).normalized;
        }
    }
    public void OnFixedUpdateState()
    {
        //�����ʱ��С�ڻ���ʱ�䣬������˷��������
        if (timer < enemy.knockbackTime)
        {
            enemy.rb.AddForce(direction * enemy.knockbackForce*0.5f, ForceMode2D.Impulse);
            //enemy.transform.position += (Vector3)direction  * Time.deltaTime;

            timer += Time.fixedDeltaTime;
        }
        else
        {
            //����ص�׷��״̬
            timer = 0;
            enemy.isHurt = false;
            enemy.TransitionToState(EnemyState.Chase);
        }
    }
    public void OnExitState()
    {
        //�������״̬
        enemy.isHurt = false;
    }
}
