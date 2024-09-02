using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Security.Cryptography;
using System.Text;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class CreateABCompare
{
    [MenuItem("AB包工具/创建对比文件")]   
    public static void CreateABCompareFile()
    {
        //获取文件夹信息
        DirectoryInfo directory= Directory.CreateDirectory(Application.dataPath+ "/ArtRes/AB/PC/");
        //获取该目录下的所有文件信息
        FileInfo[] fileInfos =directory.GetFiles();

        // 用于存储信息的字符串
        string abCompareInfo = "";

        foreach (FileInfo info in fileInfos)
        {
            //没有后缀的才是AB包我们只想要AB包的信息
            if (info.Extension == "")
            {
                Debug.Log("文件名：" + info.Name);
                abCompareInfo += info.Name + " "+info.Length + " "+GetMd5(info.FullName);

                abCompareInfo += "\n";
            }
            //拼接一个AB包的信息
            // 用一个分隔符分开不同文件之间的信息   
        }
        abCompareInfo= abCompareInfo.Substring(0, abCompareInfo.Length - 1);


        // 写入文件
        File.WriteAllText(Application.dataPath + "/ArtRes/AB/PC/ABCompare.txt", abCompareInfo);

        AssetDatabase.Refresh();
        Debug.Log("AB包对比信息：" + abCompareInfo);

    }

    public static string GetMd5(string filePath)
    {
        // 将文件以流的形式打开
        using (FileStream file= new FileStream(filePath, FileMode.Open))
        {
            //声明一个MD5对象用于生成MD5码
            MD5 md5 =new MD5CryptoServiceProvider();
            // 利用API得到数据的MD5码16个字节数组
            byte[] md5Info= md5.ComputeHash(file);

            // 关闭文件流
            file.Close();

            // 把16个字节转换为16进制拼接成字符串为了减小md5码的长度
            StringBuilder sb= new StringBuilder();
            for (int i = 0; i < md5Info.Length; i++)
            {
                sb.Append(md5Info[i].ToString("x2"));
            }
            return sb.ToString();
        }
    }


}
