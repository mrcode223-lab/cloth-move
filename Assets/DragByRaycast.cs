using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class DragByRaycast : MonoBehaviour
{
    [Header("Pick Settings")]
    public LayerMask pickableLayer;

    [Header("Limit Settings")]
    public Transform centerPoint;
    public float maxDistance = 3f;

    [Header("Hands (drag objects)")]
    public List<Transform> handObjects = new List<Transform>();

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
        HandlePickDrag();
          // 👈 NEW — Luôn giới hạn tất cả hand trong frame
    }
    private void FixedUpdate()
    {
        //ClampAllHands();
    }
    void HandlePickDrag()
    {
        // Pick
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 1000f, pickableLayer))
            {
                if (handObjects.Contains(hit.transform))
                {
                    selectedObj = hit.transform;
                    dragDistance = Vector3.Distance(cam.transform.position, hit.point);
                    offset = selectedObj.position - hit.point;
                }
            }
        }

        // Drag
        if (Input.GetMouseButton(0) && selectedObj != null)
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            Vector3 targetPos = ray.GetPoint(dragDistance) + offset;

            selectedObj.position = new Vector3(targetPos.x, targetPos.y, selectedObj.position.z);
            selectedObj.GetComponent<Rigidbody>().isKinematic = true;
            selectedObj.GetComponent<HandObject>().isDrag = true;
            var dist = Vector2.Distance(selectedObj.position, centerPoint.position);
            if (dist >= maxDistance)
            {
                selectedObj.GetComponent<Rigidbody>().isKinematic = false;
                selectedObj = null;
                return;
            }
        }

        // Release
        if (Input.GetMouseButtonUp(0))
        {
            if (selectedObj != null)
            {
                var dist = Vector2.Distance(selectedObj.position, centerPoint.position);
                if (dist >= maxDistance)
                {
                    selectedObj.GetComponent<Rigidbody>().isKinematic = false;
                    return;
                } 
                    
                var hand = selectedObj.GetComponent<HandObject>();
                if (hand != null)
                    hand.CheckAttachToWall();
            }
            selectedObj = null;
        }
    }

    /// <summary>
    /// Luôn giữ hand trong phạm vi maxDistance, kể cả khi không drag
    /// </summary>
    void ClampAllHands()
    {
        foreach (Transform hand in handObjects)
        {
            if (hand == null) continue;

            Vector3 pos = hand.position;

            // Lock Z
            pos.z = 0;

            // Vector từ center đến hand
            Vector3 dir = pos - centerPoint.position;
            float dist = dir.magnitude;

            // Nếu vượt quá phạm vi → kéo lại
            if (dist > maxDistance)
            {
                pos = centerPoint.position + dir.normalized * maxDistance;
            }

            hand.position = Vector3.Slerp(hand.position, pos, Time.fixedDeltaTime * 10);
        }
    }
}
