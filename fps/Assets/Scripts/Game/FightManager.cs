using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class FightManager : MonoBehaviour
{

private void Awake()
{
    // 隐藏鼠标
    Cursor.lockState = CursorLockMode.Locked;
    Cursor.visible = false;

    // 关闭所有UI
    Game.uiManager.CloseAllUI();
    // 加载战斗UI
    Game.uiManager.ShowUI<FightUI>("FightUI");
    

        // 查找出生点并生成玩家
        GameObject pointGO = GameObject.Find("Point");
    if (pointGO != null)
    {
        Transform pointTF = pointGO.transform;
        if (pointTF != null && pointTF.childCount > 0)
        {
            Vector3 pos = pointTF.GetChild(Random.Range(0, pointTF.childCount)).position;
            PhotonNetwork.Instantiate("Player", pos, Quaternion.identity);
        }
    }
}
    private void LoadABUI()
    {
        // //拼出AB包路径 
        string absPath = Application.persistentDataPath + "/model";
        // //加载AB包
        AssetBundle assetBundle = AssetBundle.LoadFromFile(absPath);
        // //加载文件
        //TextAsset luaTextAsset = assetBundle.LoadAsset<TextAsset>(fileName + ".lua");
        var loadAssetRequest = assetBundle.LoadAsset<GameObject>("FixPanel");

        GameObject fixPanelInstance = Instantiate(loadAssetRequest);

        // 将实例化的对象放置在 Canvas 下面或者场景中的特定位置
        fixPanelInstance.transform.SetParent(GameObject.Find("Canvas").transform, false);
    }
    public void ShowFixPanel()
    {
        //LoadABUI();
        // 加载预制体
        GameObject fixPanelPrefab = Resources.Load<GameObject>("FixPanel");

        if (fixPanelPrefab != null)
        {
            // 实例化预制体
            GameObject fixPanelInstance = Instantiate(fixPanelPrefab);

            // 将实例化的对象放置在 Canvas 下面或者场景中的特定位置
            fixPanelInstance.transform.SetParent(GameObject.Find("Canvas").transform, false);
        }
        else
        {
            Debug.LogError("FixPanel 预制体未找到，请确保其位于 Resources 文件夹中。");
        }
    }
}
