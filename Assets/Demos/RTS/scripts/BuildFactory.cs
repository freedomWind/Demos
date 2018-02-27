using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class BuildFactory
{ 
    public static BuildObj BuildObj(string buildIndex)
    {
        Buildings b = BuildSys.GettypeFromIndex(buildIndex);
        BuildObj obj = null;
        switch (b)
        {
            case Buildings.A:
                obj = new Abuilding(buildIndex);
                break;
            case Buildings.B:
                obj = new Bbuilding(buildIndex);
                break;
            case Buildings.C:
                obj = new Cbuilding(buildIndex);
                break;
            case Buildings.D:
                break;
            case Buildings.E:
                break;
            default:
                break;
        }
        return obj;
    }
}
/// <summary>
/// 建筑属性
/// </summary>
public class BuildAttr
{
    public string name;  //建筑名字
    public int level;    //建筑等级
    public int price    //建筑造价
    {
        get {
            return level * 100;
        }
    }
    public BuildAttr()
    {
        name = "";
        level = 1;
    }
}
public abstract class BuildObj:System.IDisposable
{
    string num;
    BuildAttr bAttr;
    protected GameObject _gameobject;
    public GameObject gameObject { get { return _gameobject; } }
    public string BuildNum { get { return num; } }
    public BuildObj(string name)
    {
        this.num = name;
        bAttr = new BuildAttr();
    }
    protected void InitTrans()
    {
        _gameobject.transform.localPosition = new Vector3(10000, 10000, 0);
        _gameobject.transform.localScale = Vector3.one;
        _gameobject.AddComponent<Selectable>().onSelect = OnSelect;
        _gameobject.GetComponent<Selectable>().onLongPress = OnLongPress;
        _gameobject.name = num;
    }
    protected void OnSelect()
    {
        Debug.Log(string.Format("建筑{0}被选中", num));
    }
    protected void OnLongPress()
    {
        Debug.Log(string.Format("建筑{0}被长按", num));
    }
    private void LevelUp()
    {
        bAttr.level++;
        OnLevelUpOver(bAttr.level);
    }
    protected void LevelDown()
    {
        bAttr.level--;
    }
    //升级
    public void DoLevelUp(float durtion,bool isRightNow = false)
    {
        if (isRightNow)
            LevelUp();
        else
        {
            OnLevelUpBegin(bAttr.level,durtion);
            Selectable.DelayAction(() => { LevelUp(); }, durtion);
        }
    }
    protected virtual void OnLevelUpBegin(int level,float duration)
    {
        Debug.Log(string.Format("建筑{0}开始升级，当前等级为{1}",num,bAttr.level));
        Debug.Log("播放升级动画");
    }
    protected virtual void OnLevelUpOver(int level)
    {
        Debug.Log(string.Format("建筑{0}升级完成，当前等级为{1}", num, bAttr.level));
    }
    public void Dispose()
    {
        if (_gameobject != null)
            GameObject.Destroy(_gameobject);
    }
}
class Abuilding : BuildObj
{
    public Abuilding(string nm) : base(nm)
    {
        _gameobject = Resources.Load("A_Building") as GameObject;
        _gameobject = GameObject.Instantiate(_gameobject);
        InitTrans();
    }
    protected override void OnLevelUpBegin(int level, float duration)
    {
        base.OnLevelUpBegin(level, duration);
    }
}
class Bbuilding : BuildObj
{
    public Bbuilding(string nm) : base(nm)
    {
        _gameobject = Resources.Load("B_Building") as GameObject;
        _gameobject = GameObject.Instantiate(_gameobject);
        InitTrans();
    }
}
class Cbuilding : BuildObj
{
    public Cbuilding(string nm) : base(nm)
    {
        _gameobject = Resources.Load("C_Building") as GameObject;
        _gameobject = GameObject.Instantiate(_gameobject);
        InitTrans();
    }
}
