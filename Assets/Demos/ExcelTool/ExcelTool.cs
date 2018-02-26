using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using Excel;
using System.Data;
using OfficeOpenXml;

public static class ExcelTool
{   
    /// <summary>
    /// ��excel���뵽dic
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static Dictionary<string, string[]> ToArrayListFromExcel(string path)
    {
        
        if (!System.IO.File.Exists(path))
            return null;
        FileStream stream = File.Open(path, FileMode.Open, FileAccess.Read);
        IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
        DataSet result = excelReader.AsDataSet();

        int columns = result.Tables[0].Columns.Count;
        int rows = result.Tables[0].Rows.Count;
        //result.Tables[��mySheet��].Rows.Count
        Dictionary<string, string[]> arrDic = new Dictionary<string, string[]>();
        for (int i = 0; i < rows; i++)
        {
            string[] temp = new string[columns];
            string key = "";
            for (int j = 0; j < columns; j++)
            {
                if (j == 0)
                {
                    key = result.Tables[0].Rows[i][j].ToString();
                    continue;
                }
                temp[j-1] = result.Tables[0].Rows[i][j].ToString();
            }
            if (!arrDic.ContainsKey(key))
                arrDic.Add(key, temp);
            else
                Debug.LogError("key reapeat in excel file��the reapeat key is at line��"+i+" key:"+key);
        }
        return arrDic;
    }
    /// <summary>
    /// ��dicд�뵽excel
    /// </summary>
    /// <param name="eTitles"></param>
    /// <param name="arrDic"></param>
    /// <param name="path"></param>
    public static void ToExcelFromArrList(string[] eTitles, Dictionary<string,string[]> arrDic, string path)
    {
        if (File.Exists(path))
        {
            Debug.LogError("excel�ļ��Ѵ��ڣ�Ϊ���⸲�ǣ�������");
            return;
        }
        FileInfo newFile = new FileInfo(path);
        using (ExcelPackage package = new ExcelPackage(newFile))
        {
            ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Sheet1");
            for (int i = 0; i < eTitles.Length; i++)  //set titles
            {
                worksheet.Cells[1, i + 1].Value = eTitles[i];
            }
            int j = 2, k = 1;
            foreach (KeyValuePair<string, string[]> kp in arrDic)
            {
                k = 1;
                worksheet.Cells[j, k].Value = kp.Key;
                for (int m = 0; m < kp.Value.Length; m++)
                {
                    k++;
                    worksheet.Cells[j, k].Value = kp.Value[m];
                }
                j++;
            }
            package.Save();
        }
        
    }
}
