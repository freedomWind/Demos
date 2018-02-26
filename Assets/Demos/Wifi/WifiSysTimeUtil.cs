using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using System.Text;

public class WifiSysTimeUtil : MonoBehaviour {

    Animator _anim;
    public Text sysTime;
    static AndroidJavaObject bridge = null;

    private float _lastFrameTime = 0;
    private StringBuilder _timerBuilder;
    // Use this for initialization
    void Start () {
        _anim = GetComponent<Animator>();
        _timerBuilder = new StringBuilder();
#if !UNITY_EDITOR && UNITY_ANDROID
        bridge = new AndroidJavaObject("com.example.dong.androidnative.GetWIFIRssi");
#endif
    }

    private void OnEnable()
    {
        // 开启协程 并保持更新  
        InvokeRepeating("UpdateUI", 1, 10f);
    }

    void UpdateUI()
    {
        print("UpdateUI");
        _timerBuilder.Clear();
        _timerBuilder.AppendFormat("{0}", DateTime.Now.AddHours(8f).ToString("HH:mm"));
        sysTime.text = _timerBuilder.ToString();
#if !UNITY_EDITOR && UNITY_ANDROID || UNITY_IOS
        //当用户使用WiFi时    
        if (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)
        {
            GetWiFISingalLevel(GetWIFISignalStrength());
        }
        //当用户使用移动网络时    
        else if (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork)
        {
            GetSingalLevel(GetTeleSignalStrength());
        }
#endif
    }
    

    private void OnDisable()
    {
        CancelInvoke();
    }

    private void GetWiFISingalLevel(int value)
    {
        switch (value)
        {
            case 5:
            case 4:
                _anim.CrossFade("wifi_high", 0.2f);
                break;
            case 3:
                _anim.CrossFade("wifi_common", 0.2f);
                break;
            case 2:
                _anim.CrossFade("wifi_mid", 0.2f);
                break;
            case 1:
                _anim.CrossFade("wifi_low", 0.2f);
                break;
        }

    }

    /// <summary>  
    /// 返回应该显示的格子数（共5个）  
    /// </summary>  
    /// <param name="value"></param>  
    /// <returns></returns>  
    private int GetSingalLevel(int value)
    {
        //1、当信号大于等于 - 85dBm时候，信号显示满格  
        //2、当信号大于等于 - 95dBm时候，而小于 - 85dBm时，信号显示4格  
        //3、当信号大于等于 - 105dBm时候，而小于 - 95dBm时，信号显示3格，不好捕捉到。  
        //4、当信号大于等于 - 115dBm时候，而小于 - 105dBm时，信号显示2格，不好捕捉到。  
        //5、当信号大于等于 - 140dBm时候，而小于 - 115dBm时，信号显示1格，不好捕捉到。  
        print("GetSingalLevel" + value);
        if (value > -85)
        {
            _anim.CrossFade("wifi_high", 0.2f);
            return 5;
        }
        if (value < -85 && value > -95)
        {
            _anim.CrossFade("wifi_high", 0.2f);
            return 4;
        }
        else if (value < -95 && value > -105)
        {
            _anim.CrossFade("wifi_common", 0.2f);
            return 3;
        }
        else if (value < -105 && value > -115)
        {
            _anim.CrossFade("wifi_mid", 0.2f);
            return 2;
        }
        else if (value < -115 && value > -140)
        {
            _anim.CrossFade("wifi_low", 0.2f);
            return 1;
        }

        return -1;
    }

    public static int CallStatic(string className, string methodName, params object[] args)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        try  
        {   
            int value = bridge.CallStatic<int>(methodName, args);  
            return value;  
        }   
        catch (System.Exception ex)  
        {  
            Debug.LogWarning(ex.Message);  
        }  
#endif
        return -1;
    }

    // 返回WIFI      返回的是负值  越靠近0 越强  
    public static int GetWIFISignalStrength()
    {
        return CallStatic("GetWIFIRssi", "GetWIFISignalStrength");
    }

    // 返回Telephone  
    public static int GetTeleSignalStrength()
    {
        return CallStatic("GetWIFIRssi", "GetTeleSignalStrength");
    }
}
