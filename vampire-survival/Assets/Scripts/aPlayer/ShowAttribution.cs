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
        textHealth.text= "最大生命值:"+ PlayerHealthMgr.Instance.maxHealth.ToString();
        textSpeed.text= "速度:"+ PlayeManager.instance.speed.ToString();
        textAttack.text= "攻击力:"+ PlayeManager.instance.perDamage.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        textHealth.text = "最大生命值:" + PlayerHealthMgr.Instance.maxHealth.ToString();
        textSpeed.text = "速度:" + PlayeManager.instance.speed.ToString();
        textAttack.text = "攻击力:" + PlayeManager.instance.perDamage.ToString();
    }
}
