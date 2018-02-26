using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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
public class Selectable : MonoBehaviour,IPointerClickHandler
{
    public System.Action<BaseEventData> onSelect;
    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        onSelect?.Invoke(eventData);
    }
}
public abstract class BuildObj:System.IDisposable
{
    string name;
    protected GameObject _gameobject;
    public GameObject gameObject { get { return _gameobject; } }
    public string BuildNum { get { return name; } }
    public BuildObj(string name)
    {
        this.name = name;
    }
    protected void InitTrans()
    {
        _gameobject.transform.localPosition = new Vector3(10000, 10000, 0);
        _gameobject.transform.localScale = Vector3.one;
        _gameobject.AddComponent<Selectable>().onSelect = OnSelect;
        _gameobject.name = name;
    }
    protected void OnSelect(BaseEventData eventData)
    {
        Debug.Log(string.Format("建筑{0}被选中",name));
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
