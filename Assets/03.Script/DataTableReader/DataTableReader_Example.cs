using System.Data;
using System.IO;
using UnityEngine;
using ExcelDataReader;

public class DataTableReader_Example : MonoBehaviour
{
    private int rowCount;
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
        DataTable table = result.Tables[4];

        // 첫 번째 행부터 전체 출력
        for (int i = 0; i < table.Rows.Count; i++)
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
            Debug.Log(rowData);
        }
    }
}
