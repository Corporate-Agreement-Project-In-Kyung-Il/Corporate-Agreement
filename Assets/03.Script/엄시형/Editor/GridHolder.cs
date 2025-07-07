using System;
using _03.Script.엄시형.Data;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;

namespace _03.Script.엄시형.Editor
{
 
[CreateAssetMenu(fileName = "New Grid", menuName = "Test/Grid")]
public class GridHolder : ScriptableObject
{
    public int rows = 5;
    public int cols = 6;

    public Wrapper<MonsterType>[] grid;
    public Sprite[] textures;
    
    public void ResetGrid()
    {
        grid = new Wrapper<MonsterType>[rows];
        
        for (int i = 0; i < rows; i++)
        {
            grid[i] = new Wrapper<MonsterType>();
            grid[i].values = new MonsterType[cols];
        }
    }
}
 
#if UNITY_EDITOR
    [CustomEditor(typeof(GridHolder))]
    public class GridHolderEditor : UnityEditor.Editor
    {
        SerializedProperty rowsProp;
        SerializedProperty colsProp;
        SerializedProperty gridProp;
        SerializedProperty texturesProp;

        // TODO : 텍스쳐 뜨게 변경해야함
        private void OnEnable()
        {
            rowsProp = serializedObject.FindProperty("rows");
            colsProp = serializedObject.FindProperty("cols");
            gridProp = serializedObject.FindProperty("grid");
            texturesProp = serializedObject.FindProperty("textures");

            var holder = (GridHolder) target;
            
            if (holder.grid == null || holder.grid.Length != holder.rows || NeedColResize(holder))
            {
                holder.ResetGrid();
                EditorUtility.SetDirty(holder);
                serializedObject.Update();
            }
        }

        private bool NeedColResize(GridHolder holder)
        {
            foreach (var row in holder.grid)
            {
                if (row == null || row.values == null || row.values.Length != holder.cols)
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

            for (int i = 0; i < holder.rows; i++)
            {
                if (i >= gridProp.arraySize)
                {
                    continue;
                }

                SerializedProperty rowProp = gridProp.GetArrayElementAtIndex(i).FindPropertyRelative("values");

                EditorGUILayout.BeginHorizontal();
                
                for (int j = 0; j < holder.cols; j++)
                {
                    if (j >= rowProp.arraySize)
                        continue;

                    SerializedProperty cell = rowProp.GetArrayElementAtIndex(j);
                    int currentIndex = cell.enumValueIndex;
                    
                    Texture2D tex = null;
                    
                    if (
                        holder.textures != null 
                        && currentIndex < holder.textures.Length 
                        && holder.textures[currentIndex] != null
                        && (MonsterType) currentIndex != MonsterType.Unknown)
                    {
                        tex = holder.textures[currentIndex].texture;
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
#endif