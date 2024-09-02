using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// µ–»ÀÀ¿Õˆ◊¥Ã¨
/// </summary>
public class DeathState : IState
{
    private Enemy enemy;

    public DeathState(Enemy enemy)
    {
        this.enemy = enemy;
    }
    public void OnEnterState()
    {
        if(enemy.animator!= null)
        {
            enemy.animator.Play("EDeath");
        }
        enemy.rb.velocity = Vector2.zero;
    }

    public void OnExitState()
    {
        
    }

    public void OnFixedUpdateState()
    {
        
    }

    public void OnUpdateState()
    {
        
    }

}
