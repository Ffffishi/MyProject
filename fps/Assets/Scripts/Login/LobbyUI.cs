using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

//大厅界面
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
        Debug.Log("加入大厅...");
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
        Game.uiManager .ShowUI<MaskUI>("MaskUI").ShowMsg("正在更新房间...");
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

        Debug.Log("房间列表更新...");
        ClearRoomList();

        for (int i = 0; i < roomList.Count; i++)
        {
            GameObject obj = Instantiate(roomPrefab, contentTf);
            obj.SetActive(true);
            string roomName = roomList[i].Name;  //房间名称
            obj.transform.Find("roomName").GetComponent<Text>().text = roomName;
            obj.transform.Find("joinBtn").GetComponent<Button>().onClick.AddListener(delegate ()
            {
                Debug.Log(roomName);
                //加入房间
                Game.uiManager.ShowUI<MaskUI>("MaskUI").ShowMsg("加入中...");

                PhotonNetwork.JoinRoom(roomName); //加入房间
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
