using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ExcelMeau
{
    [MenuItem("ExcelMeau/ToJsonFromSelectedFile")]
    public static void toJson()
    {
        var select = Selection.activeObject;
        var path = AssetDatabase.GetAssetPath(select);
        Dictionary<string,string[]> dic = ExcelTool.ToArrayListFromExcel(path,1);
        string newPath =path.Replace(path.Substring(path.LastIndexOf(".")),".json");
        if (System.IO.File.Exists(newPath))
        {
            System.IO.File.Delete(newPath);
        }
        using (System.IO.FileStream fs = System.IO.File.Create(newPath))// file = new System.IO.File())
        {
            System.IO.StreamWriter sw = new System.IO.StreamWriter(fs);
            sw.Write(ExcelTool.ToJsonFromDic(dic));
            sw.Close();
        }
    }
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
