using UnityEngine;

public class CharacterSelectUI : MonoBehaviour
{
    public GameObject slotPrefab;       // 캐릭터 슬롯 Prefab
    public Transform gridParent;        // CharacterSelectPanel

    public int characterCount = 9;

    void Start()
    {
        for (int i = 0; i < characterCount; i++)
        {
            GameObject slot = Instantiate(slotPrefab, gridParent);
            // 슬롯마다 캐릭터 이미지나 이름 설정 가능
        }
    }
}
