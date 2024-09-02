using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LossUI : MonoBehaviour
{

    public System.Action onClickCallBack;
    // Start is called before the first frame update
    void Start()
    {
        transform.Find("resetBtn").GetComponent<Button>().onClick.AddListener(OnClickBtn);
        transform.Find("exitBtn").GetComponent<Button>().onClick.AddListener(onExit);
    }

    public void OnClickBtn()
    {
        if (onClickCallBack != null)
        {
            onClickCallBack();
        }

        Game.uiManager.CloseUI(gameObject.name);
    }

    public void onExit( )
    {
        SceneManager.LoadScene("Login");
        Game.uiManager.CloseAllUI();
        Game.uiManager.ShowUI<LoginUI>("LoginUI");
        Destroy(this.gameObject);
    }
}
