using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelMgr : MonoBehaviour
{
    public static LevelMgr instance;

    public int enemyCounter;

    public bool isKilledBoss;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    private bool isGameActive;
    public float timer;
    // Start is called before the first frame update
    void Start()
    {
        enemyCounter=0;
        isGameActive = true;
        isKilledBoss = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isGameActive == true)
        {
            timer += Time.deltaTime;
            UIController.instance.UpdateTimer(timer);
        }
        if (isKilledBoss==true)
        {
            UIController.instance.ShowDes();
            Invoke("ShowEndLevelPanel", 2f);
        }
    }

    public void ShowEndLevelPanel()
    {
        UIController.instance.levelEndPanel.SetActive(true);
        isGameActive = false;
        Time.timeScale = 0f;
    }
    public void EndLevel()
    {
        isGameActive = false;

        StartCoroutine(EndLevelCoroutine());
    }

    IEnumerator EndLevelCoroutine()
    {
        yield return new WaitForSeconds(1f);

        float minutes = Mathf.FloorToInt(timer / 60);
        float seconds = Mathf.FloorToInt(timer % 60);

        UIController.instance.endTimeText.text = minutes.ToString("00") + "m  " + seconds.ToString("00" + "s");
        UIController.instance.levelEndPanel.SetActive(true);
    }
}
