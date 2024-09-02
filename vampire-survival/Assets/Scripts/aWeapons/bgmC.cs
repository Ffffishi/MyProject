using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bgmC : MonoBehaviour
{
    public GameObject bgm;
    public GameObject bgm2;
    // Start is called before the first frame update
    void Start()
    {
        bgm.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if(LevelMgr.instance.timer >= 180f)
        {
            bgm.SetActive(false);
            bgm2.SetActive(true);
        }
    }
}
