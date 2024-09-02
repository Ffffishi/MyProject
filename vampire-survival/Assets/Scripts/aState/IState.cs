using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState
{
    //进入状态
    void OnEnterState();

    //退出状态
    void OnExitState();

    //状态更新
    void OnUpdateState();

    //固定更新状态
    void OnFixedUpdateState();
}
