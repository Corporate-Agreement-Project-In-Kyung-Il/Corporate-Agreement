using System.Collections;
using System.Collections.Generic;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class SoundExecel : MonoBehaviour
{

#if UNITY_EDITOR
    [MenuItem("Tools/Create SFXData (From Path)")]
    public static void CreateAllSFXData()
    {
        string audioFolderPath = "Assets/00.Resources/Sound/";
        string savePath = audioFolderPath + "Data/";

        if (!Directory.Exists(savePath))
        {
            Directory.CreateDirectory(savePath);
        }

        // .mp3, .wav, .ogg 모두 지원
        string[] soundFiles = Directory.GetFiles(audioFolderPath, "SE_*.mp3", SearchOption.TopDirectoryOnly);

        foreach (string filePath in soundFiles)
        {
            string fileName = Path.GetFileNameWithoutExtension(filePath);

            // AudioClip 불러오기
            AudioClip clip = AssetDatabase.LoadAssetAtPath<AudioClip>(filePath);
            if (clip == null)
            {
                Debug.LogWarning($"AudioClip not found at {filePath}");
                continue;
            }

            // SFXData 생성
            SFXData sfxData = ScriptableObject.CreateInstance<SFXData>();
            sfxData.clip = clip;
            sfxData.volume = 1f;

            // 저장 경로
            string assetPath = savePath + fileName + ".asset";
            AssetDatabase.CreateAsset(sfxData, assetPath);         
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
#endif
}
