using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ClothParticle
{
    public Vector3 position;
    public Vector3 prevPosition;
    public bool isStatic = false;

    public ClothParticle(Vector3 pos, bool isStatic = false)
    {
        this.position = pos;
        this.prevPosition = pos;
        this.isStatic = isStatic;
    }
}

public class DistanceConstraint
{
    public int p1, p2;
    public float restLength;

    public DistanceConstraint(int p1, int p2, float restLength)
    {
        this.p1 = p1;
        this.p2 = p2;
        this.restLength = restLength;
    }
}

public class SimpleCloth : MonoBehaviour
{
    public Collider[] collisionColliders;

    public Transform cornerTL;
    public Transform cornerTR;
    public Transform cornerBL;
    public Transform cornerBR;

    public Rigidbody topCenterTarget;   // ← rigidbody bạn muốn cloth follow vào top-center

    public float stiffness = 1f;
    public int solveIterations = 5;

    Mesh mesh;
    Vector3[] originalVerts;
    Vector3[] verts;

    List<ClothParticle> particles = new List<ClothParticle>();
    List<DistanceConstraint> constraints = new List<DistanceConstraint>();

    int widthCount, heightCount;

    int topCenterIndex;
    public Rigidbody bottomCenterTarget;
    int bottomCenterIndex;

    public Rigidbody centerTarget;
    int centerIndex;

    void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;

        originalVerts = mesh.vertices;
        verts = new Vector3[originalVerts.Length];

        widthCount = Mathf.RoundToInt(Mathf.Sqrt(originalVerts.Length));
        heightCount = widthCount;

        // Tạo particle cho từng vertex
        for (int i = 0; i < originalVerts.Length; i++)
        {
            Vector3 worldPos = transform.TransformPoint(originalVerts[i]);
            particles.Add(new ClothParticle(worldPos));
        }
        bottomCenterIndex = (widthCount / 2);
        // Tính index top-center
        topCenterIndex = (heightCount - 1) * widthCount + (widthCount / 2);
        centerIndex = (heightCount / 2) * widthCount + (widthCount / 2);

        // Tạo constraints theo grid
        for (int y = 0; y < heightCount; y++)
        {
            for (int x = 0; x < widthCount; x++)
            {
                int index = y * widthCount + x;

                if (x < widthCount - 1)
                    CreateConstraint(index, index + 1);

                if (y < heightCount - 1)
                    CreateConstraint(index, index + widthCount);
            }
        }
    }

    void CreateConstraint(int i1, int i2)
    {
        float length = Vector3.Distance(particles[i1].position, particles[i2].position);
        constraints.Add(new DistanceConstraint(i1, i2, length));
    }
    private void Update()
    {
        ApplyBottomCenterAttachment();
    }

    void FixedUpdate()
    {
        // 1. Verlet integration
        foreach (var p in particles)
        {
            if (p.isStatic) continue;

            Vector3 temp = p.position;
            p.position += (p.position - p.prevPosition);
            p.prevPosition = temp;
        }

        // 2. Pin 4 góc theo transform
        ApplyCornerControl();

        // 3. Pin top-center bằng Rigidbody (soft pin + rigidbody follow)
        

        // 4. Solve constraints
        for (int i = 0; i < solveIterations; i++)
        {
            foreach (var c in constraints)
            {
                var p1 = particles[c.p1];
                var p2 = particles[c.p2];

                Vector3 delta = p2.position - p1.position;
                float dist = delta.magnitude;
                float diff = (dist - c.restLength) / dist;

                if (!p1.isStatic)
                    p1.position += delta * 0.5f * diff * stiffness;

                if (!p2.isStatic)
                    p2.position -= delta * 0.5f * diff * stiffness;
            }
        }

        // 5. Update mesh
        for (int i = 0; i < verts.Length; i++)
        {
            verts[i] = transform.InverseTransformPoint(particles[i].position);
        }

        mesh.vertices = verts;
        mesh.RecalculateNormals();

        // 6. Collision
        ResolveCollisions();
        ApplyTopCenterAttachment();
        
        ApplyCenterAttachment();
    }
    private void LateUpdate()
    {
        transform.localScale = Vector3.one * 10;
    }
    void ApplyTopCenterAttachment()
    {
        if (topCenterTarget == null) return;

        var p = particles[topCenterIndex];

        // Nếu bạn muốn cho phép rigidbody kéo cloth thì để isStatic = false
        p.isStatic = false;

        // 1. Particle kéo về rigidbody (soft pin)
        float particleStrength = 0.25f;
        Vector3 toRB = topCenterTarget.position - p.position;
        p.position += toRB * particleStrength;

        // 2. Rigidbody kéo về particle (giống joint)
        float rbStrength = 10f;
        Vector3 rbPos = Vector3.Lerp(
            topCenterTarget.position,
            p.position,
            Time.deltaTime * rbStrength
        );

        topCenterTarget.MovePosition(rbPos);
    }

    void ResolveCollisions()
    {
        foreach (var p in particles)
        {
            if (p.isStatic) continue;

            foreach (var col in collisionColliders)
            {
                if (col == null) continue;

                if (col is SphereCollider sphere)
                {
                    Vector3 center = sphere.transform.TransformPoint(sphere.center);
                    float radius = sphere.radius * sphere.transform.lossyScale.x;

                    Vector3 dir = p.position - center;
                    float dist = dir.magnitude;

                    if (dist < radius)
                    {
                        p.position = center + dir.normalized * radius;
                    }
                }
                else if (col is BoxCollider box)
                {
                    Vector3 localPos = box.transform.InverseTransformPoint(p.position);
                    Vector3 half = box.size * 0.5f;

                    bool inside =
                        Mathf.Abs(localPos.x) < half.x &&
                        Mathf.Abs(localPos.y) < half.y &&
                        Mathf.Abs(localPos.z) < half.z;

                    if (inside)
                    {
                        Vector3 closest = new Vector3(
                            Mathf.Clamp(localPos.x, -half.x, half.x),
                            Mathf.Clamp(localPos.y, -half.y, half.y),
                            Mathf.Clamp(localPos.z, -half.z, half.z)
                        );

                        Vector3 worldClosest = box.transform.TransformPoint(closest);
                        Vector3 pushDir = (p.position - worldClosest).normalized;
                        p.position = worldClosest + pushDir * 0.001f;
                    }
                }
                else if (col is CapsuleCollider capsule)
                {
                    Vector3 center = capsule.transform.TransformPoint(capsule.center);
                    float height = capsule.height * capsule.transform.lossyScale.y;
                    float radius = capsule.radius * capsule.transform.lossyScale.x;

                    Vector3 axis = capsule.transform.TransformDirection(Vector3.up);
                    float half = (height * 0.5f) - radius;

                    Vector3 p1 = center + axis * half;
                    Vector3 p2 = center - axis * half;

                    Vector3 closest = ClosestPointOnLineSegment(p.position, p1, p2);
                    Vector3 dir = p.position - closest;
                    float dist = dir.magnitude;

                    if (dist < radius)
                    {
                        p.position = closest + dir.normalized * radius;
                    }
                }
            }
        }
    }
    void ApplyBottomCenterAttachment()
    {
        if (bottomCenterTarget == null) return;

        var p = particles[bottomCenterIndex];
        p.isStatic = false;

        float particleStrength = 0.25f;
        Vector3 toRB = bottomCenterTarget.position - p.position;
        p.position += toRB * particleStrength;

        float rbStrength = 10f;
        Vector3 rbPos = Vector3.Lerp(
            bottomCenterTarget.position,
            p.position,
            Time.fixedDeltaTime * rbStrength
        );

        bottomCenterTarget.MovePosition(p.position);
    }

    Vector3 ClosestPointOnLineSegment(Vector3 p, Vector3 a, Vector3 b)
    {
        Vector3 ap = p - a;
        Vector3 ab = b - a;
        float t = Vector3.Dot(ap, ab) / ab.sqrMagnitude;
        t = Mathf.Clamp01(t);
        return a + ab * t;
    }

    void ApplyCornerControl()
    {
        // 4 góc
        particles[0].position = cornerBL.position;
        particles[0].prevPosition = cornerBL.position;
        particles[0].isStatic = true;

        particles[widthCount - 1].position = cornerBR.position;
        particles[widthCount - 1].prevPosition = cornerBR.position;
        particles[widthCount - 1].isStatic = true;

        particles[(heightCount - 1) * widthCount].position = cornerTL.position;
        particles[(heightCount - 1) * widthCount].prevPosition = cornerTL.position;
        particles[(heightCount - 1) * widthCount].isStatic = true;

        particles[heightCount * widthCount - 1].position = cornerTR.position;
        particles[heightCount * widthCount - 1].prevPosition = cornerTR.position;
        particles[heightCount * widthCount - 1].isStatic = true;
    }
    void ApplyCenterAttachment()
    {
        if (centerTarget == null) return;

        var p = particles[centerIndex];

        p.isStatic = false; // để hạt còn lại tự do, nhưng point center vẫn bị kéo

        // Particle kéo về Rigidbody
        float particleStrength = 0.25f;
        Vector3 toRB = centerTarget.position - p.position;
        p.position += toRB * particleStrength;

        // Rigidbody kéo về particle
        float rbStrength = 10f;
        Vector3 rbPos = Vector3.Lerp(
            centerTarget.position,
            p.position,
            Time.fixedDeltaTime * rbStrength
        );

        centerTarget.MovePosition(rbPos);
    }

}
