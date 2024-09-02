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
        // //拼出AB包路径 
        string absPath = fileName + "/"+assetName;
        // //加载AB包
        AssetBundle assetBundle = AssetBundle.LoadFromFile(absPath);

        return assetBundle;
    }
}
