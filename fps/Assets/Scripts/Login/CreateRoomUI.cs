using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
public class CreateRoomUI : MonoBehaviourPunCallbacks
{
    InputField roomNameInput;//房间名输入框
    void Start()
    {
        transform.Find("bg/title/closeBtn").GetComponent<Button>().onClick.AddListener(onCloseBtn);
        transform.Find("bg/okBtn").GetComponent<Button>().onClick.AddListener(onCreateRoomBtn);
        roomNameInput = transform.Find("bg/InputField").GetComponent<InputField>();

        //随机一个房价名称
        roomNameInput.text = "room_" + Random.Range(1000, 9999);
    }

    //创建房间
    public void onCreateRoomBtn()
    {
        Game.uiManager.ShowUI<MaskUI>("MaskUI").ShowMsg("创建中...");
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 6;
        PhotonNetwork.CreateRoom(roomNameInput.text, roomOptions);

    }

    public void onCloseBtn()
    {
        Game.uiManager.CloseUI(gameObject.name);
    }

    //创建房间成功
    public override void OnCreatedRoom()
    {
        Debug.Log("创建房间成功");
        Game.uiManager.CloseAllUI();

        //roomUI
        Game.uiManager.ShowUI<RoomUI>("RoomUI");
    }

    //创建房间失败
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Game.uiManager.ShowUI<MaskUI>("MaskUI").ShowMsg("创建房间失败：" + message);
        Debug.Log("创建房间失败");
    }
}
