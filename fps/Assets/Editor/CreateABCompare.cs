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
    [MenuItem("AB������/�����Ա��ļ�")]   
    public static void CreateABCompareFile()
    {
        //��ȡ�ļ�����Ϣ
        DirectoryInfo directory= Directory.CreateDirectory(Application.dataPath+ "/ArtRes/AB/PC/");
        //��ȡ��Ŀ¼�µ������ļ���Ϣ
        FileInfo[] fileInfos =directory.GetFiles();

        // ���ڴ洢��Ϣ���ַ���
        string abCompareInfo = "";

        foreach (FileInfo info in fileInfos)
        {
            //û�к�׺�Ĳ���AB������ֻ��ҪAB������Ϣ
            if (info.Extension == "")
            {
                Debug.Log("�ļ�����" + info.Name);
                abCompareInfo += info.Name + " "+info.Length + " "+GetMd5(info.FullName);

                abCompareInfo += "\n";
            }
            //ƴ��һ��AB������Ϣ
            // ��һ���ָ����ֿ���ͬ�ļ�֮�����Ϣ   
        }
        abCompareInfo= abCompareInfo.Substring(0, abCompareInfo.Length - 1);


        // д���ļ�
        File.WriteAllText(Application.dataPath + "/ArtRes/AB/PC/ABCompare.txt", abCompareInfo);

        AssetDatabase.Refresh();
        Debug.Log("AB���Ա���Ϣ��" + abCompareInfo);

    }

    public static string GetMd5(string filePath)
    {
        // ���ļ���������ʽ��
        using (FileStream file= new FileStream(filePath, FileMode.Open))
        {
            //����һ��MD5������������MD5��
            MD5 md5 =new MD5CryptoServiceProvider();
            // ����API�õ����ݵ�MD5��16���ֽ�����
            byte[] md5Info= md5.ComputeHash(file);

            // �ر��ļ���
            file.Close();

            // ��16���ֽ�ת��Ϊ16����ƴ�ӳ��ַ���Ϊ�˼�Сmd5��ĳ���
            StringBuilder sb= new StringBuilder();
            for (int i = 0; i < md5Info.Length; i++)
            {
                sb.Append(md5Info[i].ToString("x2"));
            }
            return sb.ToString();
        }
    }


}
