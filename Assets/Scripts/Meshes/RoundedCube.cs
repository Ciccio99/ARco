using System.Collections;
using UnityEngine;

[RequireComponent(typeof (MeshFilter), typeof (MeshRenderer))]
public class RoundedCube : MonoBehaviour {

    public int xSize, ySize, zSize;
    public int roundedness;

    private Mesh _mesh;
    private Vector3[] _vertices;
    private Vector3[] _normals;

    private void Awake()
    {
        _Generate ();
    }

    private void _Generate () {
        GetComponentInParent<MeshFilter>().mesh = _mesh = new Mesh ();
        _mesh.name = "Procedural Cube";

        _CreateVertices ();
        _CreateTriangles ();

    }

    private void _CreateVertices () {
        int cornerVerts = 8;
        int edgeVerts = (xSize + ySize + zSize - 3) * 4;
        int faceVerts = (
            (xSize - 1) * (ySize - 1) +
            (xSize - 1) * (zSize - 1) +
            (zSize - 1) * (ySize - 1)) * 2;

        _vertices = new Vector3[cornerVerts + edgeVerts + faceVerts];
        _normals = new Vector3[_vertices.Length];

        int v = 0;

        for (int y = 0; y <= ySize; y++) {
            for (int x = 0; x <= xSize; x++) {
                _SetVertex (v++, x, y, 0);
            }
            for (int z = 1; z <= zSize; z++) {
                _SetVertex (v++, xSize, y, z);
            }
            for (int x = xSize - 1; x >= 0; x--) {
                _SetVertex (v++, x, y, zSize);
            }
            for (int z = zSize - 1; z > 0; z--) {
                _SetVertex (v++, 0, y, z);
            }
        }

        for (int z = 1; z < zSize; z++) {
            for (int x = 1; x < xSize; x++) {
                _SetVertex (v++, x, ySize, z);
            }
        }
        for (int z = 1; z < zSize; z++) {
            for (int x = 1; x < xSize; x++) {
                _SetVertex (v++, x, 0, z);
            }
        }
        _mesh.vertices = _vertices;
        _mesh.normals = _normals;
    }

    private void _SetVertex (int i, int x, int y, int z) {
        Vector3 inner = _vertices[i] = new Vector3 (x, y, z);

        if (x < roundedness)
            inner.x = roundedness;
        else if (x > xSize - roundedness)
            inner.x = xSize - roundedness;

        if (y < roundedness) {
            inner.y = roundedness;
        } else if (y > ySize - roundedness)
            inner.y = ySize - roundedness;

        if (z < roundedness)
            inner.z = roundedness;
        else if (z > zSize - roundedness)
            inner.z = zSize - roundedness;

        _normals[i] = (_vertices[i] - inner).normalized;
        _vertices[i] = inner + _normals[i] * roundedness;
    }

    private void _CreateTriangles () {
        int quads = (xSize * ySize + xSize * zSize + ySize * zSize) * 2;
        int[] triangles = new int[quads * 6];

        int ring = (xSize + zSize) * 2;
        int t = 0, v = 0;

        for (int y = 0; y < ySize; y++, v++) {
            for (int q = 0; q < ring - 1; q++, v++) {
                t = _SetQuad (triangles, t, v, v + 1, v + ring, v + ring + 1);
            }
            t = _SetQuad (triangles, t, v, v - ring + 1, v + ring, v + 1);
        }

        t = _CreateTopFace (triangles, t, ring);
        t = _CreateBottomFace (triangles, t, ring);
        _mesh.triangles = triangles;
    }

    private int _CreateTopFace(int[] triangles, int t, int ring) {
        int v = ring * ySize;

        // First top row
        for (int x = 0; x < xSize - 1; x++, v++) {
            t = _SetQuad (triangles, t, v, v + 1, v + ring - 1, v + ring);
        }

        t = _SetQuad (triangles, t, v, v + 1, v + ring - 1, v + 2);


        int vMin = ring * (ySize + 1) - 1;
        int vMid = vMin + 1;
        int vMax = v + 2;


        for (int z = 1; z < zSize - 1; z++, vMin--, vMid++, vMax++) {
            t = _SetQuad (triangles, t, vMin, vMid, vMin - 1, vMid + xSize - 1);

            for (int x = 1; x < xSize - 1; x++, vMid++) {
                t = _SetQuad (
                    triangles, t,
                    vMid, vMid + 1, vMid + xSize - 1, vMid + xSize);
            }

            t = _SetQuad (triangles, t, vMid, vMax, vMid + xSize - 1, vMax + 1);
        }

        int vTop = vMin - 2;
        t = _SetQuad (triangles, t, vMin, vMid, vTop + 1, vTop);
        for (int x = 1; x < xSize - 1; x++, vTop--, vMid++) {
            t = _SetQuad (triangles, t, vMid, vMid + 1, vTop, vTop - 1);
        }
        t = _SetQuad (triangles, t, vMid, vTop - 2, vTop, vTop - 1);


        return t;
    }

    private int _CreateBottomFace (int[] triangles, int t, int ring) {
        int v = 1;
        int vMid = _vertices.Length - (xSize - 1) * (zSize - 1);
        t = _SetQuad (triangles, t, ring - 1, vMid, 0, 1);
        for (int x = 1; x < xSize - 1; x++, v++, vMid++) {
            t = _SetQuad (triangles, t, vMid, vMid + 1, v, v + 1);
        }
        t = _SetQuad (triangles, t, vMid, v + 2, v, v + 1);

        int vMin = ring - 2;
        vMid -= xSize - 2;
        int vMax = v + 2;

        for (int z = 1; z < zSize - 1; z++, vMin--, vMid++, vMax++) {
            t = _SetQuad (triangles, t, vMin, vMid + xSize - 1, vMin + 1, vMid);
            for (int x = 1; x < xSize - 1; x++, vMid++) {
                t = _SetQuad (
                    triangles, t,
                    vMid + xSize - 1, vMid + xSize, vMid, vMid + 1);
            }
            t = _SetQuad (triangles, t, vMid + xSize - 1, vMax + 1, vMid, vMax);
        }

        int vTop = vMin - 1;
        t = _SetQuad (triangles, t, vTop + 1, vTop, vTop + 2, vMid);
        for (int x = 1; x < xSize - 1; x++, vTop--, vMid++) {
            t = _SetQuad (triangles, t, vTop, vTop - 1, vMid, vMid + 1);
        }
        t = _SetQuad (triangles, t, vTop, vTop - 1, vMid, vTop - 2);

        return t;
    }

    private static int _SetQuad (int[] triangles, int i, int v00, int v10, int v01, int v11) {
        triangles[i] = v00;
        triangles[i + 1] = triangles[i + 4] = v01;
        triangles[i + 2] = triangles[i + 3] = v10;
        triangles[i + 5] = v11;
        return i + 6;
    }

    private void OnDrawGizmos()
    {
        if (_vertices == null) return;


        for (int i = 0; i < _vertices.Length; i++) {
            Gizmos.color = Color.black;
            Gizmos.DrawSphere (_vertices[i], 0.05f);
            Gizmos.color = Color.yellow;
            Gizmos.DrawRay (_vertices[i], _normals[i]);
        }
    }
}
