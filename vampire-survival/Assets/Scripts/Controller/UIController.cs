using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    //public Text des;
    public Text killDes;

    //exp
    public Slider expSlider;
    public TMP_Text levelText;

    public LevelGradeBtn[] levelUpBtns;
    public GameObject levelUpPanel;

    public TMP_Text coinText;

    public Text timeText;
    
    public GameObject levelEndPanel;

    public TMP_Text endTimeText;

    public string mainMenu;

    public GameObject pauseMenu;

    public PlayerStatUpDisplay moveSpeedDisplay,healthDisplay,pickupDisplay,maxWeaponsDisplay;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseUnpause();
        }
    }

    public void ShowDes()
    {
        killDes.text = "你一共击杀"+LevelMgr.instance.enemyCounter+"只怪物";
    }

    public void UpdateExp(int currenrExp, int levelExp, int currentLevel)
    {
        expSlider.maxValue = levelExp;
        expSlider.value = currenrExp;

        levelText.text = "Level: " + currentLevel;
    }

    public void SkipLevelUp()
    {
        levelUpPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void UpdateCoins()
    {
        coinText.text = "Coins: " + CoinController.instance.currentCoin;
    }

    public void PurchaseMoveSpeed()
    {
        PlayerStatesController.instance.PurchaseMoveSpeed();
        SkipLevelUp();
    }

    public void PurchaseHealth()
    {
        PlayerStatesController.instance.PurchaseHealth();
        SkipLevelUp();

    }

    public void PurchaseMaxWeapons()
    {
        PlayerStatesController.instance.PurchaseMaxWeapons();
        SkipLevelUp();
    
    }

    public void PurchasePickupRange()
    {
        PlayerStatesController.instance.PurchasePickupRange();
        SkipLevelUp();
    }

    public void UpdateTimer(float time)
    {
        float minutes = Mathf.FloorToInt(time / 60);
        float seconds = Mathf.FloorToInt(time % 60);

        timeText.text = "存活时间: " + minutes.ToString("00") + ":" + seconds.ToString("00");
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene(mainMenu);
        Time.timeScale = 1f;
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1f;
    }

    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Exiting game");
    }

    public void PauseUnpause()
    {
        if (pauseMenu.activeSelf==true)
        {
            pauseMenu.SetActive(false);
            if (levelUpPanel.activeSelf == false)
            {
                Time.timeScale = 1f;
            }
        }
        else
        {
            pauseMenu.SetActive(true);
            Time.timeScale = 0f;
        }
    }
}
