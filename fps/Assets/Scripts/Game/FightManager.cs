using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class FightManager : MonoBehaviour
{

private void Awake()
{
    // �������
    Cursor.lockState = CursorLockMode.Locked;
    Cursor.visible = false;

    // �ر�����UI
    Game.uiManager.CloseAllUI();
    // ����ս��UI
    Game.uiManager.ShowUI<FightUI>("FightUI");
    

        // ���ҳ����㲢�������
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
        // //ƴ��AB��·�� 
        string absPath = Application.persistentDataPath + "/model";
        // //����AB��
        AssetBundle assetBundle = AssetBundle.LoadFromFile(absPath);
        // //�����ļ�
        //TextAsset luaTextAsset = assetBundle.LoadAsset<TextAsset>(fileName + ".lua");
        var loadAssetRequest = assetBundle.LoadAsset<GameObject>("FixPanel");

        GameObject fixPanelInstance = Instantiate(loadAssetRequest);

        // ��ʵ�����Ķ�������� Canvas ������߳����е��ض�λ��
        fixPanelInstance.transform.SetParent(GameObject.Find("Canvas").transform, false);
    }
    public void ShowFixPanel()
    {
        //LoadABUI();
        // ����Ԥ����
        GameObject fixPanelPrefab = Resources.Load<GameObject>("FixPanel");

        if (fixPanelPrefab != null)
        {
            // ʵ����Ԥ����
            GameObject fixPanelInstance = Instantiate(fixPanelPrefab);

            // ��ʵ�����Ķ�������� Canvas ������߳����е��ض�λ��
            fixPanelInstance.transform.SetParent(GameObject.Find("Canvas").transform, false);
        }
        else
        {
            Debug.LogError("FixPanel Ԥ����δ�ҵ�����ȷ����λ�� Resources �ļ����С�");
        }
    }
}
