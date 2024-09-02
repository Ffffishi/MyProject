using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MaskUI : MonoBehaviour
{


    void Start()
    {
        
    }

    public void ShowMsg(string msg)
    {
        transform.Find("msg/bg/Text").GetComponent<Text>().text = msg;
    }

}
