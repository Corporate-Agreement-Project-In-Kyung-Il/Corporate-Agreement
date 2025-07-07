using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using UnityEngine;
using ExcelDataReader;
using UnityEditor;

public class DataTableReader_KeyValue : MonoBehaviour
{
    //기획자 분들 쓰실 수도 있어서 0번 시작이아니라 1번 시작입니다.
    [SerializeField] private int m_TableSheetNumber;

    void Start()
    {
        // 인코딩 등록 (한글 깨짐 방지)
        System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

        // 엑셀 파일 경로
        string filePath = Application.dataPath + "/05.DataTable/성장 데이터 종합.xlsx";

        // 엑셀 열기
        using var stream = File.Open(filePath, FileMode.Open, FileAccess.Read);
        using var reader = ExcelReaderFactory.CreateReader(stream);
        var result = reader.AsDataSet();

        // N 번째 시트 가져오기
        DataTable table = result.Tables[m_TableSheetNumber - 1];

        string[] columnNames = new string[table.Columns.Count];
        for (int j = 0; j < table.Columns.Count; j++)
        {
            var cell = table.Rows[1][j]?.ToString();
            if (!string.IsNullOrEmpty(cell))
            {
                columnNames[j] = cell;
                Debug.Log(table.Rows.Count);
            }
        }

        string[] rowDatas = new string[table.Rows.Count - 2];
        // 첫 번째 행부터 전체 출력
        for (int i = 2; i < table.Rows.Count; i++)
        {
            string rowData = "";
            for (int j = 0; j < table.Columns.Count; j++)
            {
                var cell = table.Rows[i][j]?.ToString();
                if (!string.IsNullOrEmpty(cell))
                {
                    if (!string.IsNullOrEmpty(rowData))
                        rowData += " | ";
                    rowData += cell;
                }
            }
            rowDatas[i - 2] = rowData;
        }
#if UNITY_EDITOR
        var option = ScriptableObject.CreateInstance<OptionChoiceBase>();

        for (int i = 0; i < rowDatas.Length; i++)
        {
            var strarr = rowDatas[i].Split('|');
            // 공백 제거
            for (int k = 0; k < strarr.Length; k++)
                strarr[k] = strarr[k].Trim();
            
            int key = int.Parse(strarr[0]);
            int equipmentTypeId = int.Parse(strarr[1]);
            MyEnum grade = (MyEnum)Enum.Parse(typeof(MyEnum), strarr[2]);
            string description = strarr[3];
            float attackLvUpEffect = float.Parse(strarr[4]);
            float hpLvUpEffect = float.Parse(strarr[5]);
            int equipmentLvUp = int.Parse(strarr[6]);
            
            var value = new Equip
            {
                Equipment_Type_ID = equipmentTypeId,
                Selection_Level = grade,
                Description = description,
                Attack_LV_UP_Effect = attackLvUpEffect,
                HP_LV_UP_Effect = hpLvUpEffect,
                Equipment_LvUP = equipmentLvUp
            };
            
            var pair = new IDValuePair<Equip>
            {
                Selection_ID = key,
                val = value
            };
            
            option.data.Add(pair);
        }
        AssetDatabase.CreateAsset(option, $"Assets/00.Resources/OptionChoice/Base.asset");
        AssetDatabase.SaveAssets();
#endif
    }
}
