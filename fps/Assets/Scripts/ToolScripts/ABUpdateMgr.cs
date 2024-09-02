using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class ABUpdateMgr : MonoBehaviour
{
    private Dictionary<string, ABInfo> remoteABInfos = new Dictionary<string, ABInfo>();

    private Dictionary<string, ABInfo> localABInfos = new Dictionary<string, ABInfo>();
    private List<string> downloadList = new List<string>();
    private static ABUpdateMgr instance;

    public static ABUpdateMgr Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<ABUpdateMgr>();
                if (instance == null)
                {
                    GameObject go = new GameObject("ABUpdateMgr");
                    instance = go.AddComponent<ABUpdateMgr>();
                }
            }
            return instance;
        }
    }


    public void CheckUpdate()
    {
        //Game.isFixed = true;

        remoteABInfos.Clear();
        localABInfos.Clear();
        downloadList.Clear();
        //��ȡԶ��ab���Ա���Ϣ
        DownLoadABCompareFile();
        print("��ȡԶ��ab���Ա���Ϣ");
        //��ȡ����ab���Ա���Ϣ
        GetLocalABCampareFileInfo();
        print("��ȡ����ab���Ա���Ϣ");
        //�Ա�������Ϣ�����ش����ص�ab��
        CompareRemoteLocal();

    }

    public void CompareRemoteLocal()
    {
        foreach (string abName in remoteABInfos.Keys)
        {
            if (!localABInfos.ContainsKey(abName))
            {
                downloadList.Add(abName);
            }
            else
            {
                if (remoteABInfos[abName].md5 != localABInfos[abName].md5)
                {
                    downloadList.Add(abName);
                }
                localABInfos.Remove(abName);
            }
        }
        foreach (string abName in localABInfos.Keys)
        {
            if (File.Exists(Application.persistentDataPath + "/" + abName))
            {
                File.Delete(Application.persistentDataPath + "/" + abName);
            }
        }
        DownLoadABFile();
    }

    /// <summary>
    /// ���ش����ص�ab��
    /// </summary>
    public async void DownLoadABFile()
    {
        //foreach(string name in remoteABInfos.Keys)
        //{
        //    downloadList.Add(name);
        //}
        string localPath = Application.persistentDataPath + "/";
        for (int i = 0; i < downloadList.Count; i++)
        {
            await Task.Run(() =>
            {
                DownloadFile(downloadList[i], localPath + downloadList[i]);
            });
            //print("!!DownloadFile Success:" + downloadList[i]);
        }
        File.WriteAllText(Application.persistentDataPath + "/ABCompare.txt", File.ReadAllText(Application.persistentDataPath + "/ABCompare_TMP.txt"));
    }
    /// <summary>
    /// ����ab���Ա��ļ�����ȡab����Ϣ
    /// </summary>
    public void DownLoadABCompareFile()
    {
        print(Application.persistentDataPath);
        DownloadFile("ABCompare.txt", Application.persistentDataPath + "/ABCompare_TMP.txt");

        string info = File.ReadAllText(Application.persistentDataPath + "/ABCompare_TMP.txt");

        string[] str = info.Split('\n');
        string[] infos = null;

        for (int i = 0; i < str.Length; i++)
        {
            infos = str[i].Split(' ');
            remoteABInfos.Add(infos[0], new ABInfo(infos[0], infos[1], infos[2]));

        }
    }

    /// <summary>
    /// ��ȡ����ab���Ա��ļ���Ϣ
    /// </summary>
    public void GetLocalABCampareFileInfo()
    {

        //�ɶ���д�ļ�
        if (File.Exists(Application.persistentDataPath + "/ABCompare.txt"))
        {
            StartCoroutine(GetLocalABCampareFileInfo(Application.persistentDataPath + "/ABCompare.txt"));
        }
        else if (File.Exists(Application.streamingAssetsPath + "/ABCompare.txt"))
        {
            //�ɶ��ļ�����һ�ν�����Ϸ��
            StartCoroutine(GetLocalABCampareFileInfo(Application.streamingAssetsPath + "/ABCompare.txt"));
        }
    }

    private IEnumerator GetLocalABCampareFileInfo(string localPath)
    {
        UnityWebRequest req = UnityWebRequest.Get(localPath);
        yield return req.SendWebRequest();

        //��ȡ�ļ��ɹ�
        //Debug.Log(req.downloadHandler.text);

        GetLocalABInfo(req.downloadHandler.text, localABInfos);
    }
    /// <summary>
    /// ��������ab����Ϣ
    /// </summary>
    public void GetLocalABInfo(string info, Dictionary<string, ABInfo> ABInfos)
    {
        string[] str = info.Split('\n');
        string[] infos = null;

        for (int i = 0; i < str.Length; i++)
        {
            infos = str[i].Split(' ');
            ABInfos.Add(infos[0], new ABInfo(infos[0], infos[1], infos[2]));
        }
        //print("GetLocalABInfo Success");
    }

    /// <summary>
    /// �����ļ�����
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="localPath"></param>
    private void DownloadFile(string fileName, string localPath)
    {
        try
        {
            FtpWebRequest req = FtpWebRequest.Create(new Uri("ftp://192.168.56.1/AB/PC/" + fileName)) as FtpWebRequest;

            NetworkCredential n = new NetworkCredential();
            req.Credentials = n;
            req.Proxy = null;
            req.KeepAlive = false;
            req.Method = WebRequestMethods.Ftp.DownloadFile;
            req.UseBinary = true;

            //Stream upLoadStream = req.GetRequestStream();
            FtpWebResponse response = req.GetResponse() as FtpWebResponse;
            Stream ftpStream = response.GetResponseStream();
            //StreamReader reader = new StreamReader(ftpStream);

            using (FileStream file = File.Create(localPath))
            {
                byte[] bytes = new byte[2048];

                int cLength = ftpStream.Read(bytes, 0, bytes.Length);

                while (cLength != 0)
                {
                    file.Write(bytes, 0, cLength);

                    cLength = ftpStream.Read(bytes, 0, bytes.Length);
                }

                file.Close();
                ftpStream.Close();
                //Debug.Log("DownloadFile Success:"+fileName);

            }
        }
        catch (Exception e)
        {
            Debug.LogError("DownloadFile Error:" + e.Message);
        }


    }
    void OnDestroy()
    {
        instance = null;
    }


    public class ABInfo
    {
        public string name;
        public string md5;
        public long size;

        public ABInfo(string name, string size, string md5)
        {
            this.name = name;
            this.md5 = md5;
            this.size = long.Parse(size);
        }
    }
}
