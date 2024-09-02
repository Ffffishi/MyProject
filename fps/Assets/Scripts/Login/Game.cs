using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public static UIManager uiManager;

    public static bool isLoaded = false;
    //public static int gtotal=10;
    //public static int gscore=11;
    void Awake()
    {
        if (isLoaded == true)
        {
            Destroy(gameObject);
        }
        else
        {
            isLoaded = true;
            DontDestroyOnLoad(gameObject); //��ת������ǰ��Ϸ���岻ɾ��
            uiManager =new UIManager();
            uiManager.Init();
        }
    }
    void Start()
    {
        //��ʾ��¼����
        uiManager.ShowUI<LoginUI>("LoginUI");
    }


    public static bool isFixed = false;

}
