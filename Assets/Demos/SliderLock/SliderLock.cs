using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SliderLock : MonoBehaviour
{
    public Sprite selectSprite;
    public Sprite origSprite;
    public bool isDelayDispare = true;
    public float delayTime = 0.4f;
    public int minPoint = 4;
    public int maxPoint = 9;
    public GameObject line;
    private Transform lineTrans;
    //  public 

    private LockItem[] btns;
    private bool _isEnd;
    private bool isEnd
    {
        set
        {
            _isEnd = value;
        }
    }
    private bool isRight = false;
    private bool isBegin = false;
    private LockItem prePoint;
    private List<Transform> recordList;

    // Use this for initialization
    protected virtual void Start()
    {
        recordList = new List<Transform>();
        btns = GetComponentsInChildren<LockItem>();
        for (int i = 0; i < btns.Length; i++)
        {
            btns[i].OnPDownDel = OnItemPressed;
            btns[i].OnPEnterDel = OnItemEnter;
            btns[i].OnPUpDel = OnItemUp;
        }
        if (lineTrans == null)
            lineTrans = new GameObject("lineTrans").transform;
        lineTrans.SetParent(transform);
        lineTrans.SetAsFirstSibling();
        lineTrans.localPosition = Vector3.zero;
        lineTrans.localScale = Vector3.one;
        line.transform.localPosition = new Vector3(100000, 0, 0);


    }
    private void OnEnable()
    {
        DisDrawSelection();
    }

    void OnItemEnter(LockItem trans)
    {
        if (!isBegin || _isEnd) return;
        if (recordList.Contains(trans.transform)) return;
        if (Loc.Distance(prePoint.loc, trans.loc) >= 2) return;
        DrawSelection(trans);
        prePoint = trans;
    }
    void OnItemPressed(LockItem trans)
    {
        if (!isBegin)
        {
            isBegin = true;
            prePoint = trans;
            DrawSelection(trans);
            Debug.Log("begin draw");
        }
    }
    void OnItemUp(LockItem trans)
    {
        if (!isBegin) return;
        if (recordList.Count <= maxPoint && recordList.Count >= minPoint)  //点个数有效
            isEnd = true;
        if (!_isEnd)
        {
            OnDrawIllegal(recordList.Count);
            DisDrawSelection();
        }
        else
        {
            List<int> vl = new List<int>();
            recordList.ForEach((x) => {
                vl.Add(x.GetComponent<LockItem>().getValue());
            });
            OnDrawEnd(vl.ToArray());
        }
    }
    protected virtual void OnDrawEnd(int[] values)
    {
        DisDrawSelection();
    }
    /// <summary>
    /// 绘制点个数不满足要求回调
    /// </summary>
    /// <param name="count"></param>
    protected virtual void OnDrawIllegal(int count)
    {
        DisDrawSelection();
        Debug.Log("draw illegel count = " + count);
    }
    void DrawSelection(LockItem trans)
    {
        ConnectTwoPoint(prePoint, trans);
        recordList.Add(trans.transform);
        trans.GetComponent<Image>().sprite = selectSprite;
        prePoint = trans;
    }
    public void DisDrawSelection()
    {
        if (isDelayDispare)
            StartCoroutine(delayDispare(delayTime));
        else
            Dispare();
    }
    IEnumerator delayDispare(float time)
    {
        yield return new WaitForSeconds(time);
        if (recordList == null) yield break;
        Dispare();
    }
    void Dispare()
    {
        recordList?.ForEach((Transform trans) =>
        {
            trans.GetComponent<Image>().sprite = origSprite;
        });
        for (int i = 0; i < lineTrans.childCount; i++)
            GameObject.Destroy(lineTrans.GetChild(i).gameObject);
        recordList?.Clear();
        isEnd = false;
        isBegin = false;
        isRight = false;
    }
    /// <summary>
    /// 设置当前绘制的颜色样式
    /// </summary>
    /// <param name="sp"></param>
    /// <param name="lineC"></param>
    public void SetNowDrawing(Sprite sp, Color lineC)
    {
        recordList?.ForEach((Transform trans) =>
        {
            trans.GetComponent<Image>().sprite = sp;
        });
        for (int i = 0; i < lineTrans.childCount; i++)
            lineTrans.GetChild(i).GetComponent<Image>().color = lineC;
    }
    void ConnectTwoPoint(LockItem pre, LockItem now)
    {
        if (pre == now) return;
        Vector3 pos = (pre.transform.position + now.transform.position) / 2;
        Vector3 engel = Vector3.zero;
        float dis = Loc.Distance(pre.loc, now.loc);
        if (dis != 1)
        {
            if (pre.loc.y < now.loc.y && pre.loc.x < now.loc.x || pre.loc.y > now.loc.y && pre.loc.x > now.loc.x)
                engel = new Vector3(0, 0, -45);
            else
                engel = new Vector3(0, 0, 45);
        }
        else
        {
            if (pre.loc.y != now.loc.y)
                engel = new Vector3(0, 0, 90);
        }
        Image mag = GameObject.Instantiate<GameObject>(line).GetComponent<Image>();
        mag.transform.SetParent(lineTrans);
        mag.transform.localScale = Vector3.one;
        mag.transform.position = pos;
        mag.transform.localEulerAngles = engel;
    }
}
