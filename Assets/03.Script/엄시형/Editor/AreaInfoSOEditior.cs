using System;
using System.Collections;
using System.Collections.Generic;
using _03.Script.엄시형.Data.V2;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AreaInfoSO))]
public class AreaInfoSOEditior : Editor
{
    private bool mHasOpenedModal = false;
    
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        
        GUILayout.Space(10);
        Rect lastRect = GUILayoutUtility.GetLastRect();
        
        // 스폰 위치 설정 버튼
        if (GUILayout.Button("스폰 위치 설정"))
        {
            SetSpawnWindow.Open();
        }
    }
}

// 모달 창 클래스
public class SetSpawnWindow : EditorWindow
{
    public static void Open()
    {
        SetSpawnWindow window = CreateInstance<SetSpawnWindow>();
        window.titleContent = new GUIContent("스폰위치 설정 창");
        // Vector2 screenPos = GUIUtility.GUIToScreenPoint(new Vector2(lastRect.x, lastRect.y + lastRect.height));
        window.position = new Rect();
        window.ShowModalUtility();
        
    }

    private void OnGUI()
    {
        GUILayout.Label("스폰 위치를 설정하세요.", EditorStyles.boldLabel);
    }
}

