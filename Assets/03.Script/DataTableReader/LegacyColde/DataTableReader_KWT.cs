using System;
using System.Data;
using System.IO;
using UnityEngine;
using ExcelDataReader;
using UnityEditor;

public class DataTableReader_KWT : MonoBehaviour
{
    //기획자 분들 쓰실 수도 있어서 0번 시작이아니라 1번 시작입니다.
    [SerializeField]
    private int m_TableSheetNumber;
    
    public string optionChoiceClassName = "OptionChoice_EquipDic"; // 스크립터블 오브젝트 클래스 이름
    void Start()
    {
        // 인코딩 등록 (한글 깨짐 방지)
        System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

        // 엑셀 파일 경로
        string filePath = Application.dataPath + "/05.DataTable/성장 데이터 종합 v1.1.xlsx";

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
                Debug.Log(columnNames.Length);
            }
        }
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
            string[] values = rowData.Split(new string[] { " | " }, System.StringSplitOptions.None);
#if UNITY_EDITOR
            Type optionType = Type.GetType(optionChoiceClassName);
            if (optionType == null)
            {
                Debug.LogError($"{optionChoiceClassName} 타입을 찾을 수 없습니다.");
                return;
            }
            var option = ScriptableObject.CreateInstance(optionType);
            option.name = $"OptionChoice_{i - 1}";
            for (int x = 0; x < columnNames.Length; x++)
            {
                var field = optionType.GetField($"{columnNames[x]}", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                if (field != null)
                {
                    if (field.FieldType.IsEnum)
                    {
                        var enumValue = Enum.Parse(field.FieldType, values[x]);
                        field.SetValue(option, enumValue);
                    }
                    else
                    {
                        field.SetValue(option, values[x]);
                    }
                }
            }
            AssetDatabase.CreateAsset(option, $"Assets/00.Resources/OptionChoice/OptionChoice_{i - 1}.asset");
            AssetDatabase.SaveAssets();
#endif
        }
    }
}
