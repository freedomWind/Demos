using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 组件，提供长按，选中，和延时函数
/// </summary>
public class Selectable : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{
    static Selectable _ins;
    float duration = 1.2f;
    float curTime;
    bool isLongPress;
    bool isPointDown;
    public System.Action onSelect;   //选中
    public System.Action onLongPress;               //长按  

    private void Awake()
    {
        if (_ins == null)
            _ins = this;
    }
    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        if (!isLongPress)
            onSelect?.Invoke();
    }
    private void Update()
    {
        if (isPointDown && !isLongPress)
        {
            if (Time.time - curTime > duration)
            {
                isLongPress = true;
                onLongPress?.Invoke();
            }
        }
    }
    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        curTime = Time.time;
        isPointDown = true;
        isLongPress = false;
    }
    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
        isPointDown = false;
    }
    /// <summary>
    /// 延时函数
    /// </summary>
    /// <param name="action"></param>
    /// <param name="t"></param>
    public static void DelayAction(System.Action action, float t)
    {
        _ins.StartCoroutine(delayA(action, t));
    }
    static IEnumerator delayA(System.Action a, float t)
    {
        yield return new WaitForSeconds(t);
        a?.Invoke();
    }
}
