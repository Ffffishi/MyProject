using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FightUI : MonoBehaviour
{
    private Image bloodImg;
    //private string right;
    // Start is called before the first frame update
    void Start()
    {
        bloodImg = transform.Find("blood").GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //更新子弹个数显示
    public void UpdateBulletCount(int count,int total)
    {
        transform.Find("bullet/Text").GetComponent<Text>().text = count.ToString()+"/"+total.ToString();

    }

    //更新血量
    public void UpdateHp(float cur, float max)
    {
        transform.Find("hp/fill").GetComponent<Image>().fillAmount = cur / max;
        transform.Find("hp/Text").GetComponent<Text>().text = cur + "/" + max;
    }

    public void UpdateBlood()
    {
        StopAllCoroutines();
        StartCoroutine(UpdateBloodCo());
    }

    public IEnumerator UpdateBloodCo()
    {
        bloodImg.color = Color.white;
        Color color = bloodImg.color;
        float t = 0.35f;
        while (t >= 0)
        {
            t -= Time.deltaTime;
            color.a = Mathf.Abs(Mathf.Sin(Time.realtimeSinceStartup));
            bloodImg.color = color;

            yield return null;
        }

        color.a = 0;
        bloodImg.color = color;
    }


}
