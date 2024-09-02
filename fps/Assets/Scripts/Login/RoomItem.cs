using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
public class RoomItem : MonoBehaviour
{
    public int owerId;

    public bool isReady=false;
    void Start()
    {
        if (owerId == PhotonNetwork.LocalPlayer.ActorNumber)
        {
            transform.Find("Button").GetComponent<Button>().onClick.AddListener(OnReadyBtn);
        }
        else
        {
            transform.Find("Button").GetComponent<Image>().color = Color.black;
        }
        ChangeReady(isReady);

    }

    public void OnReadyBtn()
    {
        isReady=!isReady;

        ExitGames.Client.Photon.Hashtable table = new ExitGames.Client.Photon.Hashtable();
        table.Add("isReady", isReady);
        PhotonNetwork.LocalPlayer.SetCustomProperties(table);//设置玩家自定义属性
        ChangeReady(isReady);
    }

    public void ChangeReady(bool ready)
    {
        transform.Find("Button/Text").GetComponent<Text>().text = ready==true ? "已准备" : "未准备";
    }
}
