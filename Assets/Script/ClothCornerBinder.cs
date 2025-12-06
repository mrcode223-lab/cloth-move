using UnityEngine;

[RequireComponent(typeof(Cloth))]
public class ClothCornersFollow : MonoBehaviour
{
    public Transform[] cornerTargets = new Transform[4]; // 4 Transform mà 4 góc Cloth sẽ theo

    private Cloth cloth;
    private int[] cornerVertexIndices = new int[4];

    void Start()
    {
        cloth = GetComponent<Cloth>();

        // 1. Tìm 4 vertex gần từng Transform nhất
        Vector3[] verts = cloth.vertices;

        for (int i = 0; i < 4; i++)
        {
            float minDist = float.MaxValue;
            int closest = 0;
            for (int v = 0; v < verts.Length; v++)
            {
                float d = (cloth.transform.TransformPoint(verts[v]) - cornerTargets[i].position).sqrMagnitude;
                if (d < minDist)
                {
                    minDist = d;
                    closest = v;
                }
            }
            cornerVertexIndices[i] = closest;
        }

        // 2. Pin các vertex góc bằng maxDistance = 0
        var coeffs = cloth.coefficients;
        for (int i = 0; i < 4; i++)
        {
            var c = coeffs[cornerVertexIndices[i]];
            c.maxDistance = 0f; // khóa vertex cứng
            coeffs[cornerVertexIndices[i]] = c;
        }
        cloth.coefficients = coeffs;
    }

    void LateUpdate()
    {
        // 3. Ép các vertex góc theo Transform bằng lực externalAcceleration
        Vector3 totalForce = Vector3.zero;

        for (int i = 0; i < 4; i++)
        {
            Vector3 targetPos = cornerTargets[i].position;
            Vector3 vertexWorld = cloth.transform.TransformPoint(cloth.vertices[cornerVertexIndices[i]]);
            Vector3 offset = targetPos - vertexWorld;

            totalForce += offset; // cộng lực từ 4 góc
        }

        cloth.externalAcceleration = totalForce * 1000f; // 1000 = sức mạnh, chỉnh nếu cần
    }
}
