using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;
using UnityEngine;
using UnityEngine.Networking;

public static class TSVLoader
{
    private static readonly CsvConfiguration TsvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
    {
        Delimiter = "\t",
        Mode = CsvMode.NoEscape,
        HasHeaderRecord = true,
        MissingFieldFound = null,
        HeaderValidated = null,
    };

    private static Task<UnityWebRequest> SendWebRequestAsync(UnityWebRequest request)
    {
        var tcs = new TaskCompletionSource<UnityWebRequest>();
        var asyncOp = request.SendWebRequest();
        asyncOp.completed += _ =>
        {
            tcs.SetResult(request);
        };
        return tcs.Task;
    }
    
    /// <summary>
    /// Application.persistentDataPath/Table 폴더에서 주어진 테이블 이름의 TSV 파일을 읽어 List<T>로 반환합니다.
    /// </summary>
    /// <typeparam name="T"> 매핑할 클래스 타입 (public getter/setter 필수)</typeparam>
    /// <param name="tableName"> 파일 이름 (확장자 제외)</param>
    /// <returns> 파싱된 데이터 리스트</returns>
    public static async Task<List<T>> LoadTableAsync<T>(string tableName, bool isStreamingAssetPath = false)
    {
        string basePath = Application.persistentDataPath;
        string folderPath = Path.Combine(basePath, "Table");
        Directory.CreateDirectory(folderPath);
        string filePath = Path.Combine(folderPath, tableName + ".tsv");


        if (File.Exists(filePath) == false)
        {
            string streamingPath = Path.Combine(Application.streamingAssetsPath, tableName + ".tsv");
            Debug.LogError($"[TableLoader] 파일이 존재하지 않습니다: {filePath}");
            
            try
            {
#if UNITY_ANDROID && !UNITY_EDITOR
            // Android는 UnityWebRequest로 읽어야 함
using (UnityWebRequest request = UnityWebRequest.Get(streamingPath))
                {
                    await SendWebRequestAsync(request);

                    if (request.result != UnityWebRequest.Result.Success)
                    {
                        Debug.LogError($"[TableLoader] StreamingAssets에서 파일 복사 실패: {request.error}");
                        return null;
                    }

                    Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                    File.WriteAllBytes(filePath, request.downloadHandler.data);
                    Debug.Log($"[TableLoader] StreamingAssets에서 {tableName}.tsv 복사 완료");
                }
#else
                if (File.Exists(streamingPath))
                {
                    File.Copy(streamingPath, filePath);
                    Debug.Log($"[TableLoader] StreamingAssets에서 {tableName}.tsv 복사 완료");
                }
                else
                {
                    Debug.LogError($"[TableLoader] 파일이 존재하지 않습니다 (StreamingAssets도 없음): {streamingPath}");
                    return null;
                }
#endif
            }
            catch (Exception e)
            {
                Debug.LogError($"[TableLoader] 복사 중 예외 발생: {e.Message}");
                return null;
            }
        }
        

        try
        {
            using var reader = new StreamReader(filePath);
            using var csv = new CsvReader(reader, TsvConfig);

            var records = new List<T>();
            await foreach (var record in csv.GetRecordsAsync<T>())
            {
                records.Add(record);
            }

            return records;
        }
        catch (Exception ex)
        {
            Debug.LogError($"[TableLoader] {tableName}.tsv 로딩 실패: {ex.Message}");
            return null;
        }
    }
    
     public static async Task SaveTableAsync<T>(string tableName, List<T> dataList, bool isStreamingAssetPath = false)
     {
         string basePath = Application.persistentDataPath;
         string folderPath = Path.Combine(basePath, "Table");
         Directory.CreateDirectory(folderPath);
         string filePath = Path.Combine(folderPath, tableName + ".tsv");


         using (StreamWriter writer = new StreamWriter(filePath, false))
         {
             var props = typeof(T).GetProperties();

             // 헤더 작성
             await writer.WriteLineAsync(string.Join("\t", props.Select(p => p.Name)));

             // 데이터 작성
             foreach (var item in dataList)
             {
                 var values = props.Select(p => p.GetValue(item, null)?.ToString() ?? "");
                 await writer.WriteLineAsync(string.Join("\t", values));
             }
             Debug.Log("저장되었습니다.");
         }
     }
    
}