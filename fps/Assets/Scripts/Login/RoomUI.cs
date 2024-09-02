using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class RoomUI : MonoBehaviour,IInRoomCallbacks
{
    Transform startTf;
    Transform contentTf;
    GameObject roomPrefab;
    public List<RoomItem> roomList;
    void Awake()
    {
        roomList = new List<RoomItem>();
        startTf = transform.Find("bg/startBtn");
        contentTf = transform.Find("bg/Content");
        roomPrefab = transform.Find("bg/roomItem").gameObject;
        transform.Find("bg/title/closeBtn").GetComponent<Button>().onClick.AddListener(onCloseBtn);
        startTf.GetComponent<Button>().onClick.AddListener(onStartBtn);
        
        PhotonNetwork.AutomaticallySyncScene = true;
    }
    void Start()
    {
        //
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            Player p = PhotonNetwork.PlayerList[i];
            CreateRoomItem(p);
        }
    }

    void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }
    public void onCloseBtn()
    {
        PhotonNetwork.Disconnect();
        Game.uiManager.CloseUI(gameObject.name);
        Game.uiManager.ShowUI<LoginUI>("LoginUI");
    }

    public void onStartBtn()
    {
        //������Ϸ
        PhotonNetwork.LoadLevel("game");
    }

    //�������
    public void CreateRoomItem(Player p)
    {
        GameObject go = Instantiate(roomPrefab, contentTf);
        go.SetActive(true);
        RoomItem ri = go.AddComponent<RoomItem>();
        ri.owerId= p.ActorNumber;//���id
        roomList.Add(ri);

        object val;
        if (p.CustomProperties.TryGetValue("isReady", out val))
        {
            ri.isReady = (bool)val;
        }

        //�����ж�ȫ������Ƿ�׼����
        if (PhotonNetwork.IsMasterClient)
        {
            bool allReady = true;
            for (int i = 0; i < roomList.Count; i++)
            {
                if (roomList[i].isReady == false)
                {
                    allReady = false;
                    break;
                }
            }
            startTf.gameObject.SetActive(allReady);
        }
    }

    //ɾ���뿪��������
    public void DeleteRoomItem(Player p)
    {
        RoomItem item = roomList.Find((RoomItem ri) => { return p.ActorNumber == ri.owerId; });
        if (item!= null)
        {
            Destroy(item.gameObject);
            roomList.Remove(item);
        }
    }

    //����Ҽ��뷿��
    public void OnPlayerEnteredRoom(Player newPlayer)
    {
        CreateRoomItem(newPlayer);
    }
    //����뿪����
    public void OnPlayerLeftRoom(Player otherPlayer)
    {
        DeleteRoomItem(otherPlayer);
    }
    //�������Ը���
    public void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        
    }
    //������Ը���
    public void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        RoomItem item = roomList.Find(( RoomItem ri) => { return ri.owerId==targetPlayer.ActorNumber; });
        if (item != null&& changedProps.ContainsKey("isReady"))
        {
            item.isReady = (bool)changedProps["isReady"];
            item.ChangeReady(item.isReady);
        }

        //�����ж�ȫ������Ƿ�׼����
        if (PhotonNetwork.IsMasterClient)
        {
            bool allReady = true;
            for (int i = 0; i < roomList.Count; i++)
            {
                if (roomList[i].isReady == false)
                {
                    allReady = false;
                    break;
                }
            }
            startTf.gameObject.SetActive(allReady);
        }
    }
    //������
    public void OnMasterClientSwitched(Player newMasterClient)
    {

    }
}
