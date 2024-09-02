using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ABAssetMgr : MonoBehaviour
{
    private static ABAssetMgr instance;

    public static ABAssetMgr Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<ABAssetMgr>();
                if (instance == null)
                {
                    GameObject go = new GameObject("ABAssetMgr");
                    instance = go.AddComponent<ABAssetMgr>();
                }
            }
            return instance;
        }
    }

    public AssetBundle LoadAsset(string fileName, string assetName)
    {
        // //ƴ��AB��·�� 
        string absPath = fileName + "/"+assetName;
        // //����AB��
        AssetBundle assetBundle = AssetBundle.LoadFromFile(absPath);

        return assetBundle;
    }
}
