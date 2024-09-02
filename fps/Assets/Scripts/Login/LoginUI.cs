using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

//登录界面
public class LoginUI : MonoBehaviour,IConnectionCallbacks 
{

    void Start()
    {
        print(Application.persistentDataPath);
        transform.Find("startBtn").GetComponent<Button>().onClick.AddListener(onStartBtn);
        transform.Find("quitBtn").GetComponent<Button>().onClick.AddListener(onExitBtn);
        transform.Find("fixBtn").GetComponent<Button>().onClick.AddListener(onFixBtn);
    }

    void OnEnable()
    {
        //注册pun2
        PhotonNetwork.AddCallbackTarget(this);
    }

    void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }
    public void onStartBtn()
    {
        //开始游戏
        Game.uiManager.ShowUI<MaskUI>("MaskUI").ShowMsg("连接服务器中...");
        //连接服务器
        PhotonNetwork.ConnectUsingSettings();//成功后执行
    }

    public void onExitBtn()
    {
        //退出游戏
        Application.Quit();
    }

    public void onFixBtn()
    {
        Game.uiManager.ShowUI<MaskUI>("MaskUI").ShowMsg("更新中...");
        ABUpdateMgr.Instance.CheckUpdate();
        //XLuaMgr.Instance.EnterGame();
        Game.isFixed = true;
        //Game.gtotal = 5;
        //Game.gscore = 5;
        Invoke("UI", 3f);
    }

    public void UI()
    {
        Game.uiManager.ShowUI<MaskUI>("MaskUI").ShowMsg("更新完成！！！");
        Invoke("Close", .6f);

    }
    public void Close()
    {
        
        Game.uiManager.CloseUI("MaskUI");
    }


    public void OnConnected()
    {
        
    }

    //连接服务器成功后执行的函数
    public void OnConnectedToMaster()
    {
        Game.uiManager.CloseAllUI();
        Debug.Log("连接服务器成功");
        //进入游戏 lobby
        Game.uiManager.ShowUI<LobbyUI>("LobbyUI");
    }

    //断开服务器后执行的函数
    public void OnDisconnected(DisconnectCause cause)
    {
        Game.uiManager.CloseUI("MaskUI");
    }

    public void OnRegionListReceived(RegionHandler regionHandler)
    {

    }

    public void OnCustomAuthenticationResponse(Dictionary<string, object> data)
    {

    }

    public void OnCustomAuthenticationFailed(string debugMessage)
    {

    }
}
