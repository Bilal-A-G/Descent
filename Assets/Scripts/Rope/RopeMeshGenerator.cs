using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RopeMeshGenerator : MonoBehaviour
{
    public List<Transform> ropeSegments;

    public MeshFilter meshFilter;
    public MeshRenderer meshRenderer;

    public float thickness;
    [Range(0.0f, 180.0f)]
    public float cornerThreshold;
    [Range(0.5f, 1.5f)]
    public float cornerTightness;
    public int cornerResolution;

    private List<Vector3> optimizedControlPoints;
    private List <Vector3> vertices;
    private Mesh mesh;

    private void Start()
    {
        optimizedControlPoints = new List<Vector3>();
        vertices = new List<Vector3>();
        mesh = new Mesh();
        meshFilter.mesh = mesh;
    }

    private Vector3 BezierLerp(Vector3 start, Vector3 middle, Vector3 end, float lerp, float spacing)
    {
        return Vector3.Lerp(
            Vector3.Lerp(start * spacing, middle, lerp),
            Vector3.Lerp(middle, end * spacing, lerp),
        lerp) * spacing;
    }

    private void GenerateControlPoints()
    {
        for (int i = 0; i < ropeSegments.Count; i++)
        {
            Transform currentSegment = ropeSegments[i];
            Vector3 currentVertex = currentSegment.localPosition;

            if (i == 0 || i == ropeSegments.Count - 1)
            {
                optimizedControlPoints.Add(currentVertex);
                continue;
            }
            Transform nextSegment = ropeSegments[i + 1];
            Transform previousSegment = ropeSegments[i - 1];

            Vector3 toNextSegment = nextSegment.localPosition - currentSegment.localPosition;
            Vector3 toPreviousSegment = previousSegment.localPosition - currentSegment.localPosition;

            if (Vector3.Angle(toNextSegment, toPreviousSegment) > cornerThreshold)
            {
                optimizedControlPoints.Add(currentVertex);
                continue;
            }

            float inverseCornerTightness = 1 / cornerTightness;

            for (int j = 0; j < cornerResolution + 1; j++)
            {
                float lerp = (float)j / (float)cornerResolution;

                Vector3 offsetPosition = currentSegment.localPosition +
                    BezierLerp(toPreviousSegment, Vector3.zero, toNextSegment, lerp, inverseCornerTightness);

                Vector3 cornerSegment = offsetPosition;
                optimizedControlPoints.Add(cornerSegment);
            }
        }
    }

    private void GenerateVertices()
    {
        for (int i = 0; i < optimizedControlPoints.Count; i++)
        {
            Vector3 currentControlPoint = optimizedControlPoints[i];
            Vector3 nextControlPoint;
            if (i >= optimizedControlPoints.Count - 1)
            {
                nextControlPoint = optimizedControlPoints[i - 1];
            }
            else
            {
                nextControlPoint = optimizedControlPoints[i + 1];
            }

            Vector3 toNext = nextControlPoint - currentControlPoint;
            Vector3 right = (toNext.magnitude < 0.3f ? -1 : 1) * Vector3.Cross(toNext.normalized, Vector3.up);
            Vector3 localUp = (toNext.magnitude < 0.3f ? -1 : 1) * Vector3.Cross(toNext.normalized, right.normalized);

            vertices.AddRange(new List<Vector3>{
                    currentControlPoint + localUp * thickness,
                    i >= optimizedControlPoints.Count - 1 ? currentControlPoint - right * thickness  : currentControlPoint + right * thickness,
                    currentControlPoint - localUp * thickness,
                    i >= optimizedControlPoints.Count - 1 ? currentControlPoint + right * thickness  : currentControlPoint - right * thickness,
                }
            );

        }
    }

    private void GenerateMesh()
    {
        mesh.Clear();

        int numVertices = vertices.Count;
        List<int> triangles = new List<int>();

        for (int i = 0; i < vertices.Count - 4; i += 4)
        {
            //Top right square
            triangles.Add(i);
            triangles.Add(i + 4);
            triangles.Add(i + 1);

            triangles.Add(i + 1);
            triangles.Add(i + 4);
            triangles.Add(i + 5);

            //Top left square
            triangles.Add(i);
            triangles.Add(i + 7);
            triangles.Add(i + 4);

            triangles.Add(i + 3);
            triangles.Add(i + 7);
            triangles.Add(i);

            //Bottom left square
            triangles.Add(i + 3);
            triangles.Add(i + 2);
            triangles.Add(i + 7);

            triangles.Add(i + 2);
            triangles.Add(i + 6);
            triangles.Add(i + 7);

            //Bottom right square
            triangles.Add(i + 1);
            triangles.Add(i + 5);
            triangles.Add(i + 6);

            triangles.Add(i + 2);
            triangles.Add(i + 1);
            triangles.Add(i + 6);
        }

        if (vertices.Count != numVertices)
            return;

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();
    }

    void Update()
    {
        optimizedControlPoints.Clear();
        vertices.Clear();
        
        GenerateControlPoints();
        GenerateVertices();
        GenerateMesh();
    }
}
