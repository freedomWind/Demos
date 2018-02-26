using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILauguage
{
    /// <summary>
    /// UI界面text设置
    /// </summary>
    void InitUIText();
}

public enum LauguageType
{
    China,
    Hongkong,
    English,
    Korea,
    Jopan,
    NULL,
}

/// <summary>
/// 多语言管理类
/// 支持动态切换语言
/// </summary>
public class LauguageMgr
{
#region  //内部变量
    //多语言文件路径 （放在streamingAsset目录,方便客户端动态更新）
    public string lauguageConfigFilePath = Application.streamingAssetsPath;
    private List<ILauguage> lauList;
    private LauguageData data;
    private static LauguageMgr _ins;
    public static LauguageMgr GetIns() { if (_ins == null) _ins = new LauguageMgr(); return _ins; }
    private LauguageMgr()
    {
        lauList = new List<ILauguage>();
        data = new LauguageData(lauguageConfigFilePath);
    }
    public LauguageType curLauguage
    {
        get { return data.curLauguage; }
    }
    #endregion
#region //外部接口
    public void SetLauguage(LauguageType type,System.Action whenDone = null)
    {
        data.SetLauguageType(type,whenDone);
    }
    public string GetText(string key)
    {
     //   return key;
        return data.GetText(key);
    }
    public void ChangeLauguage(LauguageType type,System.Action del = null)
    {
        if (type == curLauguage) return;
        SetLauguage(type,()=> {
            lauList.ForEach((x) => {
                x.InitUIText();
            });
            if (del != null) del();
        });
        Debug.Log("change lauguage == "+type);

    }
    /// <summary>
    /// 将对象序列化到本地
    /// </summary>
    /// <param name="sType"></param>
    /// <param name="filePath"></param>
    public void SaveToFile(SourceType sType, string filePath)
    {
		data.Serialize(sType,filePath);
    }
    public void AddILauguage(ILauguage lau)
    {
        lauList.Add(lau);
    }
    public void RemoveILauguage(ILauguage lau)
    {
        lauList.Remove(lau);
    }
	public enum SourceType
	{
		xmlType,
		jsonType,
	}
#endregion
}
