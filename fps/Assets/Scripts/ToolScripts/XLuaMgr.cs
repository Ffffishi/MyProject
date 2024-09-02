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
        // //拼出AB包路径 
        string absPath = Application.persistentDataPath + "/lua";
        // //加载AB包
        AssetBundle assetBundle = AssetBundle.LoadFromFile(absPath);
        // //加载Lua文件 因为打成AB包的时候加了.txt后缀 所以加载类型是TextAsset 加上.lua后缀
        TextAsset luaTextAsset = assetBundle.LoadAsset<TextAsset>(fileName + ".lua");
        //把Lua文件转成byte数组并返回
        if (luaTextAsset != null)
            return luaTextAsset.bytes;
        else
            Debug.Log("MyCustomAssetBundleLoader重定向失败，文件名为：" + fileName);
        return null;
    }
    private void InitLuaEnv()
    {
        this.env = new LuaEnv();
        //添加我们自定义的lua代码装载器
        this.env.AddLoader(LuaScriptLoader);
    }
    public void EnterGame()
    {
        this.env.DoString("require(\"PlayerCtrl\")");
    }
}
