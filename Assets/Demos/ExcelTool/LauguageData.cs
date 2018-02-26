using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System.Net;
using LitJson;

public class LauguageData
{
    public const string fileName = "languageConfig";            //配置文件名称
    private string fileFullName;
	private LauguageType _type;
	public LauguageType curLauguage
	{
		get { return _type; }
	}
	private Dictionary<string, string> _dic;
	public LauguageData(string filePath, LauguageType defaultLau = LauguageType.China)
	{
		this.fileFullName = filePath + "/"+fileName;
		_type = LauguageType.NULL;
		_dic = new Dictionary<string, string>();
		SetLauguageType(defaultLau);
	}
	public void SetLauguageType(LauguageType type,System.Action whenDone = null)
	{
		if (_type == type) return;
		_type = type;
        LoadConfig(whenDone);
        #region//在安卓路径下 这个有问题 采用www
        /*
        if (System.IO.File.Exists(filepath))  
		{
			using (System.IO.StreamReader sr = new System.IO.StreamReader(filepath))
			{             
				string str = sr.ReadToEnd();
				writeToDic(str);
			}
		}
		else
		{
			TextAsset ta = Resources.Load<TextAsset>("bb");
            if (ta != null)
            {
                Debug.Log("ta txt = "+ta.text);
                writeToDic(ta.text);
            }
            else
                Debug.LogError("not found");
		}
        */
#endregion
    }
    void LoadConfig(System.Action whenDone = null)
    {
        TextAsset ta = Resources.Load<TextAsset>(fileName);
        if (ta != null)
        {
         //   Debug.Log("ta txt = " + ta.text);
            writeToDic(ta.text, whenDone);
        }
        else
            Debug.LogError("not found");
        if (whenDone != null)
            whenDone();
        #region
        //LeboGameConfig.Instance.StartCoroutine(loadConfig(_ => {
        //    if (_ != "")
        //        writeToDic(_,whenDone);
        //    else
        //    {
        //        Debug.Log("commint");
        //        TextAsset ta = Resources.Load<TextAsset>(fileName);
        //        if (ta != null)
        //        {
        //            Debug.Log("ta txt = " + ta.text);
        //            writeToDic(ta.text,whenDone);
        //        }
        //        else
        //            Debug.LogError("not found");
        //    }
        //}));
        #endregion
    }
    IEnumerator loadConfig(System.Action<string> whenDone = null)
    {
        string text = "";
        WWW www = new WWW(fileFullName);
        Debug.Log("fullname = "+fileFullName);
        yield return www;
        if (www.error != null)
         Debug.LogError("errojjr = "+www.error); 
        else
            text = www.text;
        Debug.Log("text = " + text);
        if (whenDone != null)
        {
            whenDone(text);
        }
    }
    public string GetText(string key)
	{
		string value = key;
		bool flag = _dic.TryGetValue(key, out value);
        if (flag) return value;
		return key;
	}
	void writeToDic(string str,System.Action whenDone = null)
	{
      //  TextData data = readDataFromXml(str);
		TextData data = readDataFromJson<TextData>(str);
		if (data == null || data.data.Count == 0 || data.data [0].values.Count < (int)_type + 1) {
			Debug.LogError ("fileConfig is not correct");
			return;
		}
		_dic.Clear ();
		data.data.ForEach ((x) => {
			_dic.Add (x.key, x.values [(int)_type]);
		});
        if (whenDone != null)
            whenDone();
	}
	public void Serialize(LauguageMgr.SourceType sType, string filePath)
	{
        if (sType == LauguageMgr.SourceType.jsonType)
            serializeToJson(this, filePath);
        else if (sType == LauguageMgr.SourceType.xmlType)
            serializeToXml(this, filePath);
	}
	static void serializeToXml(object obj,string path)
	{
		//XmlHelper.XmlSerializeToFile(obj, path, System.Text.Encoding.UTF8);
	}
	static void serializeToJson(object obj,string path)
	{
		try {
            
            string str =  JsonMapper.ToJson(obj);
			Debug.Log("str = "+str);
			System.IO.FileStream fs = new System.IO.FileStream (path, System.IO.FileMode.CreateNew);
			System.IO.StreamWriter sw = new System.IO.StreamWriter (fs);
			sw.WriteLine (str);
			sw.Close ();
			fs.Close ();
		} finally {
		

		}
	}
	public static T readDataFromJson<T>(string str)
	{
	//try
	//	{
            return JsonMapper.ToObject<T>(str);
	//	}
	//	catch(System.Exception ex) {
	//		Debug.LogError ("json deserialize error = "+ex.Data);
	//		return  default(T);
	//	}
	}
	static TextData readDataFromXml(string str)
	{
		try
		{
            return null;
			//return XmlHelper.XmlDeserialize<TextData> (str, System.Text.Encoding.UTF8);//.GetFromStr<TextData>(str);
		}
		catch(System.Exception ex) {
			Debug.LogError ("json deserialize error = "+ex.Data);
			return null;
		}
	}
	public static TextData ReadDataFromXmlPath(string filepath)
	{
		if (System.IO.File.Exists(filepath))
		{
			using (System.IO.StreamReader sr = new System.IO.StreamReader(filepath))
			{             
				string str = sr.ReadToEnd();
				return readDataFromXml(str);
			}
		}
		return null;
	}
    public static TextData ReadDataFromJsonPath(string jsonPath)
    {
        if (System.IO.File.Exists(jsonPath))
        {
            using (System.IO.StreamReader sr = new System.IO.StreamReader(jsonPath))
            {
                  string str = sr.ReadToEnd();
               
                return readDataFromJson<TextData>(str);
            }
        }
        return null;
    }
    public static T ReadDataFromJsonPath<T>(string jsonPath)
    {
        if (System.IO.File.Exists(jsonPath))
        {
            using (System.IO.StreamReader sr = new System.IO.StreamReader(jsonPath))
            {
                string str = sr.ReadToEnd();
                Debug.Log("str ==== " + str);
                return readDataFromJson<T>(str);
            }
            
        }
        return default(T);
    }
	public static void TurnXmlToJson(string xmlPath,string jsonPath)
	{
		TextData data = ReadDataFromXmlPath (xmlPath);
		if (data == null) {
			Debug.LogError ("xmlpath error");
			return;
		}
		Debug.Log (data.data [0].key);
		serializeToJson (data, jsonPath);
	}
	public static void TurnJsonToXml(string jsonPath,string xmlPath)
	{
	}
    public static void SerializeToJson(object obj, string path)
    {
        serializeToJson(obj, path);
    }
}
[XmlRoot("TextData")]
public class TextData
{
	public List<TextItem> data;

	public TextData()
	{
		data = new List<TextItem>();
	}
	public void AddTextItem(TextItem item)
	{
		if (data.Exists((x) => x.key == item.key))
		{
			Debug.LogError(string.Format("textItem key = {0} is repeated", item.key));
		}
		else
			data.Add(item);
	}
    public Dictionary<string, string[]> ToDic()
    {
        Dictionary<string, string[]> dic = new Dictionary<string, string[]>();
        foreach (var v in data)
        {
            string[] temp = new string[v.values.Count];
            for (int i = 0; i < v.values.Count; i++)
                temp[i] = v.values[i];
            dic.Add(v.key, temp);
        }
        return dic;
    }
    public static TextData FromDic(Dictionary<string,string[]> dic)
    {
        TextData td = new TextData();
        foreach (KeyValuePair<string, string[]> kp in dic)
        {
            TextItem item = new TextItem();
            item.key = kp.Key;
            for (int i = 0; i < kp.Value.Length; i++)
                item.values.Add(kp.Value[i]);
            td.AddTextItem(item);
        }
        return td;
    }
}
public class TextItem
{
	[XmlElement("Key")]
	public string key;

	public List<string> values;

	public TextItem()
	{
		key = "";
		values = new List<string>();
	}
	public TextItem(string key, string[] value)
	{
		this.key = key;
		this.values = new List<string>();
		for (int i = 0; i < value.Length; i++)
		{
			values.Add(value[i]);
		}
	}
}

