using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

//��¼����
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
        //ע��pun2
        PhotonNetwork.AddCallbackTarget(this);
    }

    void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }
    public void onStartBtn()
    {
        //��ʼ��Ϸ
        Game.uiManager.ShowUI<MaskUI>("MaskUI").ShowMsg("���ӷ�������...");
        //���ӷ�����
        PhotonNetwork.ConnectUsingSettings();//�ɹ���ִ��
    }

    public void onExitBtn()
    {
        //�˳���Ϸ
        Application.Quit();
    }

    public void onFixBtn()
    {
        Game.uiManager.ShowUI<MaskUI>("MaskUI").ShowMsg("������...");
        ABUpdateMgr.Instance.CheckUpdate();
        //XLuaMgr.Instance.EnterGame();
        Game.isFixed = true;
        //Game.gtotal = 5;
        //Game.gscore = 5;
        Invoke("UI", 3f);
    }

    public void UI()
    {
        Game.uiManager.ShowUI<MaskUI>("MaskUI").ShowMsg("������ɣ�����");
        Invoke("Close", .6f);

    }
    public void Close()
    {
        
        Game.uiManager.CloseUI("MaskUI");
    }


    public void OnConnected()
    {
        
    }

    //���ӷ������ɹ���ִ�еĺ���
    public void OnConnectedToMaster()
    {
        Game.uiManager.CloseAllUI();
        Debug.Log("���ӷ������ɹ�");
        //������Ϸ lobby
        Game.uiManager.ShowUI<LobbyUI>("LobbyUI");
    }

    //�Ͽ���������ִ�еĺ���
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
