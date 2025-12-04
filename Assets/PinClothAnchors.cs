using UnityEngine;

[ExecuteInEditMode]
public class PinClothAnchors : MonoBehaviour
{
    public Cloth cloth;                 // reference tới Cloth component
    public Transform[] anchors = new Transform[4]; // 4 anchor transforms
    public int verticesPerAnchor = 3;   // số vertex gần nhất cho mỗi anchor
    public float maxDistanceForPin = 0f; // giá trị maxDistance để "dán" (0 = dán cứng)

    // Gọi từ context menu trong inspector
    [ContextMenu("Auto Pin Nearest Vertices")]
    public void AutoPin()
    {
        if (cloth == null)
        {
            Debug.LogError("Assign a Cloth component first.");
            return;
        }

        // Lấy mesh vertices
        Mesh mesh = null;
        SkinnedMeshRenderer smr = GetComponent<SkinnedMeshRenderer>();
        if (smr != null)
            mesh = smr.sharedMesh;
        else
        {
            MeshFilter mf = GetComponent<MeshFilter>();
            if (mf != null) mesh = mf.sharedMesh;
        }

        if (mesh == null)
        {
            Debug.LogError("No mesh found on object (SkinnedMeshRenderer or MeshFilter).");
            return;
        }

        Vector3[] verts = mesh.vertices;
        int vCount = verts.Length;

        // Tính vị trí world của từng vertex
        Vector3[] worldVerts = new Vector3[vCount];
        for (int i = 0; i < vCount; i++)
            worldVerts[i] = transform.TransformPoint(verts[i]);

        // Lấy hoặc khởi tạo coefficients
        ClothSkinningCoefficient[] coeffs = cloth.coefficients;
        if (coeffs == null || coeffs.Length != vCount)
            coeffs = new ClothSkinningCoefficient[vCount];

        // Mặc định giữ lại các giá trị cũ, chỉ chỉnh những vertex được pin
        for (int i = 0; i < anchors.Length; i++)
        {
            if (anchors[i] == null) continue;

            // tìm N vertex gần nhất với anchor i
            System.Collections.Generic.List<System.Tuple<int, float>> list =
                new System.Collections.Generic.List<System.Tuple<int, float>>(vCount);

            for (int v = 0; v < vCount; v++)
            {
                float d = Vector3.Distance(worldVerts[v], anchors[i].position);
                list.Add(System.Tuple.Create(v, d));
            }

            list.Sort((a, b) => a.Item2.CompareTo(b.Item2));

            for (int k = 0; k < Mathf.Min(verticesPerAnchor, list.Count); k++)
            {
                int vid = list[k].Item1;
                // set maxDistance = maxDistanceForPin để "dán" (0 = cố định)
                if (coeffs[vid].maxDistance != maxDistanceForPin)
                {
                    coeffs[vid].maxDistance = maxDistanceForPin;
                    // bạn có thể set coeffs[vid].collisonSphereDistance nếu muốn
                }
            }
        }

        // Gán trở lại cho cloth
        cloth.coefficients = coeffs;
        Debug.Log("AutoPin complete. Anchors used: " + anchors.Length);
    }
}
