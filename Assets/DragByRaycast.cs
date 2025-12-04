using UnityEngine;

public class DragByRaycast : MonoBehaviour
{
    [Header("Pick Settings")]
    public LayerMask pickableLayer;

    [Header("Limit Settings")]
    public Transform centerPoint;    // điểm center để giới hạn
    public float maxDistance = 3f;   // khoảng cách tối đa cho phép

    private Camera cam;
    private Transform selectedObj;
    private float dragDistance;
    private Vector3 offset;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        // Bắt object khi click
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 1000f, pickableLayer))
            {
                selectedObj = hit.transform;
                dragDistance = Vector3.Distance(cam.transform.position, hit.point);
                offset = selectedObj.position - hit.point;
            }
        }

        // Move object theo chuột
        if (Input.GetMouseButton(0) && selectedObj != null)
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            Vector3 targetPos = ray.GetPoint(dragDistance) + offset;

            // --- Clamp khoảng cách ---
            if (centerPoint != null)
            {
                Vector3 dir = targetPos - centerPoint.position;
                float dist = dir.magnitude;

                if (dist > maxDistance)
                {
                    // clamp lại vị trí
                    targetPos = centerPoint.position + dir.normalized * maxDistance;
                }
            }

            selectedObj.position = targetPos;
        }

        // Thả chuột
        if (Input.GetMouseButtonUp(0))
        {
            selectedObj = null;
        }
    }
}
