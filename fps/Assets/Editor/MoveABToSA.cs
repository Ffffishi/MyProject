using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class MoveABToSA 
{
    [MenuItem("AB包工具/移动选中资源到StreamingAssets目录")]
    private static void MoveABToStreamAssets()
    {
        Object[] objs = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
        if(objs.Length == 0)
        {
            return;
        }

        string abCompareInfo= "";
        foreach(Object asset in objs)
        {
            string path = AssetDatabase.GetAssetPath(asset);
            string fileName = path.Substring(path.LastIndexOf("/"));
            Debug.Log("path:" + fileName);

            if (fileName.IndexOf('.') != -1)
            {
                continue;
            }

            AssetDatabase.CopyAsset(path, "Assets/StreamingAssets" + fileName);

            FileInfo fileInfo = new FileInfo(Application.streamingAssetsPath + fileName);

            abCompareInfo += fileInfo.Name + " " + fileInfo.Length + " " + CreateABCompare.GetMd5(fileInfo.FullName);
            abCompareInfo+="\n";
        }

        abCompareInfo = abCompareInfo.Substring(0, abCompareInfo.Length - 1);

        string compareInfoPath = Application.streamingAssetsPath + "/ABCompare.txt";
        File.WriteAllText(compareInfoPath, abCompareInfo);

        AssetDatabase.Refresh();

    }
}
