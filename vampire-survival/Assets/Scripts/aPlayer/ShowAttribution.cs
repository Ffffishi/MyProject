using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowAttribution : MonoBehaviour
{
    public Text textHealth;

    public Text textSpeed;

    public Text textAttack;

    // Start is called before the first frame update
    void Start()
    {
        textHealth.text= "�������ֵ:"+ PlayerHealthMgr.Instance.maxHealth.ToString();
        textSpeed.text= "�ٶ�:"+ PlayeManager.instance.speed.ToString();
        textAttack.text= "������:"+ PlayeManager.instance.perDamage.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        textHealth.text = "�������ֵ:" + PlayerHealthMgr.Instance.maxHealth.ToString();
        textSpeed.text = "�ٶ�:" + PlayeManager.instance.speed.ToString();
        textAttack.text = "������:" + PlayeManager.instance.perDamage.ToString();
    }
}
