using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ����׷��״̬
/// </summary>
public class ChaseState : IState
{

    private Enemy enemy;
    /// <summary>
    /// ���캯��
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
        //������׷��Ŀ������Ϊ���
        if(enemy.destinationSetter.target !=enemy.targetPoint)
            enemy.destinationSetter.target = enemy.targetPoint;
        //���˿�ʼ׷��
        if (enemy.destinationSetter.target != null)
        {
            enemy.aiPath.canMove = true;
        }
        //Debug.Log("׷��״̬");

    }
    public void OnUpdateState()
    {
        if (enemy.isHurt)
        {
            enemy.TransitionToState(EnemyState.Hurt);
        }

        //�������Ƿ��ڹ�����Χ��
        enemy.GetPlayerDistance();
        //�������ڹ�����Χ�ڣ����л�������״̬
        if (enemy.isBirth == true)
        {
            enemy.isBirth = false;
            return;
        }

        if (enemy.distanceToPlayer < enemy.attackRange)
        {
            //enemy.StopChase();
            //Debug.Log("��������Ҿ��룺" + enemy.distanceToPlayer);
            //Debug.Log("���˹�����Χ��" + enemy.attackRange);
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
