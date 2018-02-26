using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderLockEx : SliderLock
{
    public Sprite successSprite;
    public Sprite failSprite;
    private ILockLogic lockLogic;
    public ILockLogic LockLogic { get { return lockLogic; } }
    public Color32 successLineColor = Color.green;
    public Color32 failLineColor = Color.red;
    public Text tip;
    //  private PwdSafeController controller;
    //public System.Action<string> DoneDel;
    public void SetLockLogic(ILockLogic logic)
    {
        this.lockLogic = logic;
        this.lockLogic.slock = this;
    }
    protected override void Start()
    {
        base.Start();
        successLineColor = Color.green;
        failLineColor = Color.red;
    }
    public void InitUIText(string str)
    {
        ShowTip(str);
    }
    public void ShowTip(string tip)
    {
        this.tip.text = tip;
    }
    public void ShowSuccessDraw()
    {
        base.SetNowDrawing(successSprite, successLineColor);
    }
    public void ShowFailDraw()
    {
        Debug.LogError("pppppp");
        base.SetNowDrawing(failSprite, failLineColor);
    }
    protected override void OnDrawEnd(int[] value)
    {
        string str = "";
        for (int i = 0; i < value.Length; i++)
        {
            str = str + value[i].ToString();
        }
        //if (DoneDel != null)
        //    DoneDel(str);
        lockLogic?.OnTryOver(str);
        base.OnDrawEnd(value);
    }
    protected override void OnDrawIllegal(int count)
    {
        base.OnDrawIllegal(count);
        if (count < minPoint || count >maxPoint)
        {
            lockLogic?.OnPwdIllegal();
           // ShowTip(LauguageMgr.GetIns().GetText("PointCountNoPass"));
        }

    }
}
public abstract class ILockLogic
{
    public SliderLockEx slock;
    public Text Tip;
    private string _rightPwd;
    protected string rightPwd { get { return _rightPwd; } }
    protected int tempTry = 0;
    public int? tryTimes = null;  //null表示无次数限制

    public void SetRightPwd(string pwd)
    {
        this._rightPwd = pwd;
    }
    public virtual void OnTryOver(string str)
    {
        tempTry++;
        
        int value = string.Compare(str, _rightPwd);
        if (value == 0)
        {
            tempTry = 0;
            OnSuccessed();
        }
        else
        {
            if (tryTimes == null || tempTry < tryTimes)
                OnPwdError();
            else
                OnFailed();
        }
    }
    /// <summary>
    /// 解锁失败
    /// </summary>
    public virtual void OnFailed()
    {
        if (Tip != null)
            Tip.color = Color.red;
        slock?.ShowFailDraw();
    }
    /// <summary>
    /// 解锁成功
    /// </summary>
    public virtual void OnSuccessed()
    {
        if (Tip != null)
            Tip.color = Color.green;
        slock?.ShowSuccessDraw();
    }
    /// <summary>
    /// 密码错误
    /// </summary>
    public virtual void OnPwdError()
    {
        if (Tip != null)
            Tip.color = Color.red;
        slock?.ShowFailDraw();
    }
    /// <summary>
    /// 密码不合法
    /// </summary>
    public virtual void OnPwdIllegal()
    {
        if (Tip != null)
            Tip.color = Color.red;
    }
    public void ShowTip(string str)
    {
        if (Tip != null)
            Tip.text = str;

    }
}
