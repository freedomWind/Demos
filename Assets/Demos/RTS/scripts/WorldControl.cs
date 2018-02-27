using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WorldControl : MonoBehaviour, IDragHandler
{
    static WorldControl _ins;
    public Transform map;
    
    public static WorldControl Ins { get { return _ins; } }
    void Awake()
    {
        _ins = this;
    }
    private void Update()
    {
        if (Input.GetMouseButtonUp(1))
        {
            int i = Random.Range(0, 3);
            Buildings type = Buildings.A;
            if (i == 0)
                type = Buildings.A;
            else if (i == 1)
                type = Buildings.B;
            else if (i == 2)
                type = Buildings.C;
            Build(type, Input.mousePosition);
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            BuildSys.GetIns().GetBuildObjFromIndex("A_0").DoLevelUp(5);
            //Remove("A_0");
        }
    }
    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        Vector3 t = map.localPosition;
        t += new Vector3(eventData.delta.x, eventData.delta.y);// eventData.delta;
        float x = Mathf.Clamp(t.x, -394, 394);
        float y = Mathf.Clamp(t.y, -255, 255);
        map.transform.localPosition = new Vector3(x, y);
    }
    public void Build(Buildings e,Vector3 pos)
    {
        try
        {
            GameObject oo = BuildSys.GetIns().Build(e).gameObject;
            oo.transform.SetParent(map);
            oo.transform.position = pos;
        }
        catch (System.Exception ex)
        {
            Debug.LogError("null exception"+ex);
        }
    }
    public void Remove(string idx)
    {
        if (BuildSys.GetIns().Remove(idx))
        {
            Debug.Log(string.Format("移除建筑{0}成功：",idx));
        }
        else
        {
            Debug.Log(string.Format("建筑{0}不存在",idx));
        }
    }
}
