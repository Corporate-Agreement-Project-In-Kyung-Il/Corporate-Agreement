using UnityEngine;

public class CameraConfinerBox : MonoBehaviour
{
    public Camera mainCamera;
    public BoxCollider2D confineBox;

    private void LateUpdate()
    {
        if (!mainCamera.orthographic)
        {
            Debug.LogWarning("Orthographic 카메라만 지원합니다.");
            return;
        }

        Vector3 pos = mainCamera.transform.position;

        float vertExtent = mainCamera.orthographicSize;
        float horzExtent = vertExtent * Screen.width / Screen.height;

        Bounds bounds = confineBox.bounds;

        // 카메라 중심 위치 클램핑
        float minX = bounds.min.x + horzExtent;
        float maxX = bounds.max.x - horzExtent;
        float minY = bounds.min.y + vertExtent;
        float maxY = bounds.max.y - vertExtent;

        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);

        mainCamera.transform.position = pos;
    }
}