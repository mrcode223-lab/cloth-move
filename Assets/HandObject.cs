using UnityEngine;

public class HandObject : MonoBehaviour
{
    public LayerMask wallLayer;      // Layer Wall
    public float checkDistance = 0.2f; // Khoảng raycast kiểm tra

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    /// <summary>
    /// Hàm này được gọi từ script DragByRaycast khi thả chuột
    /// </summary>
    public void CheckAttachToWall()
    {
        // Hướng raycast — thường là hướng vào tường hoặc hướng -forward
        Vector3 dir = transform.forward;
        var hits = Physics.OverlapSphere(transform.position, 0.2f, wallLayer);
        if (hits.Length > 0)
        {
            // Có Wall ngay trước mặt
            rb.isKinematic = true;
        }
        else
        {
            // Không có Wall
            rb.isKinematic = false;
        }
    }
    private void LateUpdate()
    {
        //transform.position = new Vector3(transform.position.x, transform.position.y, 0);
    }
    private void OnDrawGizmosSelected()
    {
        // Gizmo để xem hướng raycast
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * checkDistance);
    }
}
