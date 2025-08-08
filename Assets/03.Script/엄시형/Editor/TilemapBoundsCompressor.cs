using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapBoundsCompressor : EditorWindow
{
    [MenuItem("Tools/Tilemap/타일맵 압축툴")]
    private static void ShowWindow()
    {
        GetWindow<TilemapBoundsCompressor>("타일맵 압축툴");
    }

    private void OnGUI()
    {
        GUILayout.Label("타일맵 프리팹을 선택하고 압축 버튼을 눌러주세요", EditorStyles.boldLabel);

        if (GUILayout.Button("타일맵 압축"))
        {
            CompressSelectedTilemaps();
        }
    }

    private void CompressSelectedTilemaps()
    {
        var selected = Selection.gameObjects;

        if (selected.Length == 0)
        {
            Debug.LogWarning("선택된 GameObject가 없습니다.");
            return;
        }

        int compressedCount = 0;

        foreach (var go in selected)
        {
            var tilemap = go.GetComponent<Tilemap>();
            if (tilemap == null)
                continue;

            Undo.RecordObject(tilemap, "Compress Tilemap Bounds");

            tilemap.CompressBounds();
            // EditorUtility.SetDirty(tilemap.gameObject);
            compressedCount++;
        }

        if (compressedCount > 0)
        {
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
            AssetDatabase.SaveAssets();

            Debug.Log($"{compressedCount}개의 Tilemap bounds를 압축하고 저장했습니다.");
        }
        else
        {
            Debug.LogWarning("선택된 오브젝트에 Tilemap 컴포넌트가 없습니다.");
        }
    }
}
