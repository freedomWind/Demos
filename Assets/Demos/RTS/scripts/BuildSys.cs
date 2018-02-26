using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Buildings
{
    A,
    B,
    C,
    D,
    E
}
/// <summary>
/// 建造系统
/// </summary>
public sealed class BuildSys
{
    static BuildSys _ins;
    int bIndex = 0;
    Dictionary<string, BuildObj> buildObjs;
    Dictionary<string, List<string>> bDic;
    public static BuildSys GetIns()
    {
        if (_ins == null)
            _ins = new BuildSys();
        return _ins;
    }
    BuildSys()
    {
        bDic = new Dictionary<string, List<string>>();
        buildObjs = new Dictionary<string, BuildObj>();
    }
    /// <summary>
    /// get build type form buildNum
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public static Buildings GettypeFromIndex(string index)
    {
        string[] result = index.Split(new char[] { '_' });
        if (result.Length != 2)
            return default(Buildings);
        return (Buildings)System.Enum.Parse(typeof(Buildings),result[0]);
    }
    public int GetCount(Buildings buildingType)
    {
        string e = buildingType.ToString();
        if (!bDic.ContainsKey(e))
            return 0;
        return bDic[e].Count;
    }
    /// <summary>
    /// get obj from buildnum
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public BuildObj GetBuildObjFromIndex(string index)
    {
        BuildObj oo = null;
        buildObjs.TryGetValue(index, out oo);
        return oo;
    }
    public BuildObj Build(Buildings buildingType)
    {
        string e = buildingType.ToString();
        string ix = e + "_" + bIndex;
        if (!bDic.ContainsKey(e))
        {
            bDic.Add(e, new List<string>());
        }
        bDic[e].Add(ix);
        BuildObj obj = BuildFactory.BuildObj(ix);
        buildObjs.Add(ix,obj);
        bIndex++;
        
        return obj;
    }
    public bool Remove(string bInx)
    {
        Buildings b = GettypeFromIndex(bInx);
        if (!bDic.ContainsKey(b.ToString())||!buildObjs.ContainsKey(bInx))
            return false;
        bDic[b.ToString()].Remove(bInx);
        buildObjs[bInx].Dispose();
        buildObjs.Remove(bInx);
        return true;
    }
}
