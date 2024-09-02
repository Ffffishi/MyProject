using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

//��������
public class LobbyUI : MonoBehaviourPunCallbacks
{
    TypedLobby lobby;

    private Transform contentTf;
    private GameObject roomPrefab;
    void Start()
    {
        transform.Find("content/createBtn").GetComponent<Button>().onClick.AddListener(onCreatRoomBtn);
        transform.Find("content/title/closeBtn").GetComponent<Button>().onClick.AddListener(onCloseBtn);
        transform.Find("content/updateBtn").GetComponent<Button>().onClick.AddListener(onUpdateRoomBtn);

        contentTf = transform.Find("content/Scroll View/Viewport/Content");
        roomPrefab = transform.Find("content/Scroll View/Viewport/item").gameObject;

        lobby = new TypedLobby("fpsLobby", LobbyType.SqlLobby);
        //join lobby
        PhotonNetwork.JoinLobby(lobby);

    }

    //
    public override void OnJoinedLobby()
    {
        Debug.Log("�������...");
        //create room
        //PhotonNetwork.CreateRoom("fpsRoom", new RoomOptions() { MaxPlayers = 2 });
    }

    public void onCloseBtn()
    {
        //disconnect
        PhotonNetwork.Disconnect();
        Game.uiManager.CloseUI(gameObject.name);
        //show login ui
        Game.uiManager.ShowUI<LoginUI>("LoginUI");

    }

    public void onCreatRoomBtn()
    {
        Game.uiManager.ShowUI<CreateRoomUI>("CreateRoomUI");
    }

    //update room
    public void onUpdateRoomBtn()
    {
        Game.uiManager .ShowUI<MaskUI>("MaskUI").ShowMsg("���ڸ��·���...");
        PhotonNetwork.GetCustomRoomList(lobby, "1");
    }

    //clear room
    private void ClearRoomList()
    {
        while(contentTf.childCount != 0)
        {
            Destroy(contentTf.GetChild(0).gameObject);
        }
    }

    //after update room
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Game.uiManager.CloseUI("MaskUI");

        Debug.Log("�����б����...");
        ClearRoomList();

        for (int i = 0; i < roomList.Count; i++)
        {
            GameObject obj = Instantiate(roomPrefab, contentTf);
            obj.SetActive(true);
            string roomName = roomList[i].Name;  //��������
            obj.transform.Find("roomName").GetComponent<Text>().text = roomName;
            obj.transform.Find("joinBtn").GetComponent<Button>().onClick.AddListener(delegate ()
            {
                Debug.Log(roomName);
                //���뷿��
                Game.uiManager.ShowUI<MaskUI>("MaskUI").ShowMsg("������...");

                PhotonNetwork.JoinRoom(roomName); //���뷿��
            });
        }
    }

    public override void OnJoinedRoom()
    {
        Game.uiManager.CloseAllUI();
        Game.uiManager.ShowUI<RoomUI>("RoomUI");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Game.uiManager.CloseUI("MaskUI");
    }
}
