using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

/// <summary>
/// 坐标
/// </summary>
[System.Serializable]
public class Loc
{
    public int x;
    public int y;

    public static float Distance(Loc a, Loc b)
    {
        int x = Mathf.Abs(a.x - b.x);
        int y = Mathf.Abs(a.y - b.y);
        return Mathf.Sqrt(x * x + y * y);
    }
}
public class LockItem : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler
{
    public Loc loc;
    public System.Action<LockItem> OnPDownDel;
    public System.Action<LockItem> OnPUpDel;
    public System.Action<LockItem> OnPEnterDel;
    public int index
    {
        get { return loc.x + loc.y * 3; }
    }
    public int getValue()
    {
        return loc.x + loc.y * 3;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        //Debug.Log("opoint down");
        if (OnPDownDel != null)
            OnPDownDel(this);
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        // EventSystem.current.currentSelectedGameObject
        // Debug.Log("opoint up");
        if (OnPUpDel != null)
            OnPUpDel(this);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log("opoint enter");
        if (OnPEnterDel != null)
            OnPEnterDel(this);
    }
}

