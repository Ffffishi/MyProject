using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
public class CreateRoomUI : MonoBehaviourPunCallbacks
{
    InputField roomNameInput;//�����������
    void Start()
    {
        transform.Find("bg/title/closeBtn").GetComponent<Button>().onClick.AddListener(onCloseBtn);
        transform.Find("bg/okBtn").GetComponent<Button>().onClick.AddListener(onCreateRoomBtn);
        roomNameInput = transform.Find("bg/InputField").GetComponent<InputField>();

        //���һ����������
        roomNameInput.text = "room_" + Random.Range(1000, 9999);
    }

    //��������
    public void onCreateRoomBtn()
    {
        Game.uiManager.ShowUI<MaskUI>("MaskUI").ShowMsg("������...");
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 6;
        PhotonNetwork.CreateRoom(roomNameInput.text, roomOptions);

    }

    public void onCloseBtn()
    {
        Game.uiManager.CloseUI(gameObject.name);
    }

    //��������ɹ�
    public override void OnCreatedRoom()
    {
        Debug.Log("��������ɹ�");
        Game.uiManager.CloseAllUI();

        //roomUI
        Game.uiManager.ShowUI<RoomUI>("RoomUI");
    }

    //��������ʧ��
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Game.uiManager.ShowUI<MaskUI>("MaskUI").ShowMsg("��������ʧ�ܣ�" + message);
        Debug.Log("��������ʧ��");
    }
}
