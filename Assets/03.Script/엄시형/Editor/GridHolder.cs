using System;
using _03.Script.엄시형.Data;
using _03.Script.엄시형.Monster;
using _03.Script.엄시형.Util;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Serialization;

namespace _03.Script.엄시형.Editor
{
    [CreateAssetMenu(fileName = "New Grid", menuName = "Test/Grid")]
    public sealed class GridHolder : ScriptableObject
    {
        public int Rows = 5;
        public int Cols = 6;

        public Wrapper<MonsterType>[] Grid;
        public Sprite[] Textures;
        
        public void ResetGrid()
        {
            Grid = new Wrapper<MonsterType>[Rows];
            
            for (int i = 0; i < Rows; i++)
            {
                Grid[i] = new Wrapper<MonsterType>();
                Grid[i].Values = new MonsterType[Cols];
            }
        }
    }
 

    [CustomEditor(typeof(GridHolder))]
    public sealed class GridHolderEditor : UnityEditor.Editor
    {
        SerializedProperty rowsProp;
        SerializedProperty colsProp;
        SerializedProperty gridProp;
        SerializedProperty texturesProp;

        // TODO : 텍스쳐 뜨게 변경해야함
        private void OnEnable()
        {
            rowsProp = serializedObject.FindProperty("Rows");
            colsProp = serializedObject.FindProperty("Cols");
            gridProp = serializedObject.FindProperty("Grid");
            texturesProp = serializedObject.FindProperty("Textures");

            var holder = (GridHolder) target;
            
            if (holder.Grid == null || holder.Grid.Length != holder.Rows || NeedColResize(holder))
            {
                holder.ResetGrid();
                EditorUtility.SetDirty(holder);
                serializedObject.Update();
            }
        }

        private bool NeedColResize(GridHolder holder)
        {
            foreach (var row in holder.Grid)
            {
                if (row == null || row.Values == null || row.Values.Length != holder.Cols)
                    return true;
            }
            
            return false;
        }
        
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(rowsProp);
            EditorGUILayout.PropertyField(colsProp);
            EditorGUILayout.PropertyField(texturesProp);

            if (GUILayout.Button("Reset Grid"))
            {
                GridHolder holder = (GridHolder) target;
                holder.ResetGrid();
                EditorUtility.SetDirty(holder);
                serializedObject.Update();
                return;
            }

            DrawGrid();
            serializedObject.ApplyModifiedProperties();

        }

        void DrawGrid()
        {
            var holder = (GridHolder) target;

            for (int i = 0; i < holder.Rows; i++)
            {
                if (i >= gridProp.arraySize)
                {
                    continue;
                }

                SerializedProperty rowProp = gridProp.GetArrayElementAtIndex(i).FindPropertyRelative("Values");

                EditorGUILayout.BeginHorizontal();
                
                for (int j = 0; j < holder.Cols; j++)
                {
                    if (j >= rowProp.arraySize)
                        continue;

                    SerializedProperty cell = rowProp.GetArrayElementAtIndex(j);
                    int currentIndex = cell.enumValueIndex;
                    
                    Texture2D tex = null;
                    
                    if (
                        holder.Textures != null 
                        && currentIndex < holder.Textures.Length 
                        && holder.Textures[currentIndex] != null
                        && (MonsterType) currentIndex != MonsterType.Unknown)
                    {
                        tex = holder.Textures[currentIndex].texture;
                    }
                    
                    
                    GUIContent content = new GUIContent(tex);
                    
                    Rect buttonRect = GUILayoutUtility.GetRect(40, 40);
                    
                    if (GUI.Button(buttonRect, content))
                    {
                        GenericMenu menu = new GenericMenu();

                        foreach (MonsterType type in Enum.GetValues(typeof(MonsterType)))
                        {
                            menu.AddItem(
                                new GUIContent(type.ToString()),
                                type == (MonsterType) currentIndex,
                                () => 
                                {
                                    cell.enumValueIndex = (int) type;
                                    serializedObject.ApplyModifiedProperties();
                                    GUI.changed = true;
                                }
                            );
                        }

                        menu.ShowAsContext();
                    }
                    
                    GUI.backgroundColor = Color.white;
                }
                
                EditorGUILayout.EndHorizontal();
            }
        }
        
    }
}
