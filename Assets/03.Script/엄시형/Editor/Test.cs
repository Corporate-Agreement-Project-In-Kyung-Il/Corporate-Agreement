#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public class SOFinder : EditorWindow
{
    [MenuItem("Tools/Check Loaded ScriptableObjects")]
    static void CheckLoadedSO()
    {
        var loadedSOs = Resources.FindObjectsOfTypeAll<ScriptableObject>();
        foreach (var so in loadedSOs)
        {
            string path = AssetDatabase.GetAssetPath(so);
            if (!string.IsNullOrEmpty(path))
            {
                Debug.Log($"[로드됨] {so.name} at {path}");
            }
        }
    }
}
#endif