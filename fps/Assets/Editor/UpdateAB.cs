using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

public class UpdateAB 
{
    [MenuItem("AB包工具/上传AB包和对比文件")]
    public static void UpdateABFile()
    {
        //获取文件夹信息
        DirectoryInfo directory = Directory.CreateDirectory(Application.dataPath + "/ArtRes/AB/PC/");
        //获取该目录下的所有文件信息
        FileInfo[] fileInfos = directory.GetFiles();

        foreach (FileInfo info in fileInfos)
        {
            //没有后缀的才是AB包我们只想要AB包的信息||.txt文件
            if (info.Extension == ""||
                info.Extension==".txt")
            {
                FtpUploadFile(info.FullName, info.Name);
            } 
        }
    }

    private async static void FtpUploadFile(string filePath,string fileName)
    {
        await Task.Run(() =>
        {
            try
            {
                FtpWebRequest req = FtpWebRequest.Create(new Uri("ftp://192.168.56.1/AB/PC/" + fileName)) as FtpWebRequest;

                NetworkCredential n = new NetworkCredential();
                req.Credentials = n;
                req.Proxy = null;
                req.KeepAlive = false;
                req.Method = WebRequestMethods.Ftp.UploadFile;
                req.UseBinary = true;

                Stream upLoadStream = req.GetRequestStream();

                using (FileStream file = File.OpenRead(filePath))
                {
                    byte[] bytes = new byte[2048];

                    int cLength = file.Read(bytes, 0, bytes.Length);

                    while (cLength != 0)
                    {
                        upLoadStream.Write(bytes, 0, cLength);

                        cLength = file.Read(bytes, 0, bytes.Length);
                    }

                    file.Close();
                    upLoadStream.Close();
                }

                Debug.Log(fileName + "succely");
            }
            catch (Exception ex)
            {
                Debug.Log(ex.Message);
            }
        });
    


    }
}
