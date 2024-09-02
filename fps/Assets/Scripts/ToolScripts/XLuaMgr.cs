using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using XLua;

public class XLuaMgr : UnitySingleton<XLuaMgr>
{
    //private static readonly string luaScriptsFolder = "LuaTxt";
    private LuaEnv env = null;
    public override void Awake()
    {
        base.Awake();
        this.InitLuaEnv();
    }
    public byte[] LuaScriptLoader(ref string fileName)
    {
        // //ƴ��AB��·�� 
        string absPath = Application.persistentDataPath + "/lua";
        // //����AB��
        AssetBundle assetBundle = AssetBundle.LoadFromFile(absPath);
        // //����Lua�ļ� ��Ϊ���AB����ʱ�����.txt��׺ ���Լ���������TextAsset ����.lua��׺
        TextAsset luaTextAsset = assetBundle.LoadAsset<TextAsset>(fileName + ".lua");
        //��Lua�ļ�ת��byte���鲢����
        if (luaTextAsset != null)
            return luaTextAsset.bytes;
        else
            Debug.Log("MyCustomAssetBundleLoader�ض���ʧ�ܣ��ļ���Ϊ��" + fileName);
        return null;
    }
    private void InitLuaEnv()
    {
        this.env = new LuaEnv();
        //��������Զ����lua����װ����
        this.env.AddLoader(LuaScriptLoader);
    }
    public void EnterGame()
    {
        this.env.DoString("require(\"PlayerCtrl\")");
    }
}
