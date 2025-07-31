using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class TSVLoaderAndroid : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(LoadTSV());
    }

    IEnumerator LoadTSV()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "PlayerSaveFile.tsv");

#if UNITY_ANDROID
        UnityWebRequest www = UnityWebRequest.Get(path);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            string text = www.downloadHandler.text;
            string[] lines = text.Split('\n');
            string[] firstRow = lines[0].Split('\t');
            Debug.Log("첫 행 첫 열: " + firstRow[0]);
        }
        else
        {
            Debug.LogError("TSV 로딩 실패: " + www.error);
        }
#else
        string text = System.IO.File.ReadAllText(path);
        string[] lines = text.Split('\n');
        string[] firstRow = lines[0].Split('\t');
        Debug.Log("첫 행 첫 열: " + firstRow[0]);
        yield break;
#endif
    }
}
