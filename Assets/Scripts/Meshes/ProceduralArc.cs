/*
 * Author: Alberto Scicali
 * Procedural arc class, given a set of points will render an arc in space
 */
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Timers;
using System.Diagnostics;

[ExecuteInEditMode]
[RequireComponent (typeof (MeshFilter), typeof (MeshRenderer))]
public class ProceduralArc : MonoBehaviour {
    [Range (3, 100)]
    public int lineSteps = 90;
    [Range (3, 100)]
    public int smoothness = 40;

    public float radius = 0.08f;
    public Vector3[] bezierPointsPosition;
    public bool noisyArc;

    private Mesh _mesh;
    private Vector3[] _vertices;

    /// <summary>
    /// Sets the points.
    /// </summary>
    /// <param name="start">Start.</param>
    /// <param name="end">End.</param>
    public void SetPoints (Vector3 start, Vector3 end) {
        bezierPointsPosition = new Vector3[3];
        bezierPointsPosition[0] = start;
        bezierPointsPosition[2] = end;
        var endStart = (end - start);
        var mid = start + (endStart * 0.5f);
        var maxY = Mathf.Max (start.y, end.y);

        var finalMid = new Vector3 (mid.x, maxY + (mid.magnitude / 2f), mid.z);
        bezierPointsPosition[1] = finalMid;
    }   

    /// <summary>
    /// Generate the specified points and lineSteps.
    /// </summary>
    /// <returns>The generate.</returns>
    /// <param name="points">Points.</param>
    /// <param name="lineSteps">Line steps.</param>
    public void Generate (Vector3[] points, int lineSteps) {
        this.lineSteps = lineSteps;
        GetComponent<MeshFilter> ().mesh = _mesh = new Mesh ();
        _mesh.name = "Procedural Arc";

        UnityEngine.Random.InitState ((int) DateTime.Now.Ticks);

        var stopwatch = new Stopwatch ();
        stopwatch.Start ();

        if (noisyArc)
            _CreateVertices (points, true);
        else
            _CreateVertices (points);
        
        _CreateTriangles (points);

        _mesh.RecalculateNormals ();
        _mesh.RecalculateTangents ();

        stopwatch.Stop ();
        UnityEngine.Debug.Log ("Render Time for " + points.Length + " vertices: " + stopwatch.ElapsedMilliseconds);
    }

    /// <summary>
    /// Generate this instance.
    /// </summary>
    public void Generate () {
        GetComponent<MeshFilter> ().mesh = _mesh = new Mesh ();
        _mesh.name = "Procedural Arc";

        if (bezierPointsPosition == null)
            throw new NullReferenceException ("ProceduralArc: Bezier points is null...");
        
        var points = Bezier.GetBezierCurvePoints (lineSteps,
                                                  bezierPointsPosition[0],
                                                  bezierPointsPosition[1],
                                                  bezierPointsPosition[2]);
        
        UnityEngine.Random.InitState ((int)DateTime.Now.Ticks);

        if (noisyArc)
            _CreateVertices (points, true);
        else
            _CreateVertices (points);
        
        _CreateTriangles (points);

        _mesh.RecalculateNormals ();
        _mesh.RecalculateTangents ();
    }

    /// <summary>
    /// Creates the vertices.
    /// </summary>
    /// <param name="bezierPoints">Bezier points.</param>
    /// <param name="noise">If set to <c>true</c> noise.</param>
    private void _CreateVertices (Vector3[] bezierPoints, bool noise = false) {
        
        int vertexCount = bezierPoints.Length * smoothness;

        _vertices = new Vector3[vertexCount];
        var uv = new Vector2[vertexCount];

        // Rotations allows by each vertex around center
        var vertRot = -360f / smoothness;

        int v = 0;

        var currPoint = bezierPoints[0];
        var nextPoint = bezierPoints[1];
        var currToNextVect = (nextPoint - currPoint).normalized;

        // Create the first ring vertices
        var vertVect = Vector3.Cross (currToNextVect, Vector3.up);
        for (int r = 0; r < smoothness; r++) {

            Quaternion rotation;

            if (noise) {
                float randRot = UnityEngine.Random.Range (vertRot - (vertRot * 0.25f), vertRot + (vertRot * 0.25f));
                rotation = Quaternion.AngleAxis (randRot, Vector3.up);
            } else
                rotation = Quaternion.AngleAxis (vertRot, Vector3.up);
            
            // Rotation vector
            vertVect = rotation * vertVect;

            // Determine radius
            float size;
            if (noise)
                size = UnityEngine.Random.Range (radius / 2f, radius);
            else
                size = radius;
            
            var finalVert = vertVect * size + bezierPoints[0];

            _vertices[v++] = finalVert;
        }

        // create ring of vertices up until the last one
        for (int i = 1; i < bezierPoints.Length - 1; i++) {
            currPoint = bezierPoints[i];
            nextPoint = bezierPoints[i + 1];
            currToNextVect = (nextPoint - currPoint).normalized;
            vertVect = Vector3.Cross (currToNextVect, Vector3.up).normalized;

            for (int r = 0; r < smoothness; r++) {
                Quaternion rotation;

                if (noise) {
                    float randRot = UnityEngine.Random.Range (vertRot - (vertRot * 0.25f), vertRot + (vertRot * 0.25f));
                    rotation = Quaternion.AngleAxis (randRot, currToNextVect);
                } else
                    rotation = Quaternion.AngleAxis (vertRot, currToNextVect);
                
                // Rotation Vect
                vertVect = (rotation * vertVect);

                // Determine radius
                float size;
                if (noise)
                    size = UnityEngine.Random.Range (radius / 2f, radius);
                else
                    size = radius;
                
                var finalVect = vertVect * size + bezierPoints[i];

                _vertices[v++] = finalVect;
            } 
        }

        currPoint = bezierPoints[bezierPoints.Length - 1];
        var prevPoint = bezierPoints[bezierPoints.Length - 2];
        var prevToCurrVect = (currPoint - prevPoint).normalized;

        // Create vertices for last point
        vertVect = Vector3.Cross (prevToCurrVect, Vector3.up);
        for (int r = 0; r < smoothness; r++) {

            Quaternion rotation;

            if (noise) {
                float randRot = UnityEngine.Random.Range (vertRot - (vertRot * 0.25f), vertRot + (vertRot * 0.25f));
                rotation = Quaternion.AngleAxis (randRot, Vector3.down);
            } else
                rotation = Quaternion.AngleAxis (vertRot, Vector3.down);
            
            // Rotation vector
            vertVect = rotation * vertVect;

            // Determine radius
            float size;
            if (noise)
                size = UnityEngine.Random.Range (radius / 2f, radius);
            else
                size = radius;
            
            var finalVert = vertVect * size + bezierPoints[bezierPoints.Length - 1];

            _vertices[v++] = finalVert;
        }

        _mesh.vertices = _vertices;
    }

    /// <summary>
    /// Creates the triangles.
    /// </summary>
    /// <param name="bezierPoints">Bezier points.</param>
    private void _CreateTriangles (Vector3[] bezierPoints) {
        // smoothnes == # of faces per ring

        int quads = (bezierPoints.Length - 1) * smoothness * 2;
        int[] triangles = new int[quads * 6];

        int t = 0, v = 0;

        for (int y = 0; y < lineSteps; y++, v++) {
            for (int q = 0; q < smoothness - 1; q++, v++) {
                t = _SetQuad (triangles, t, v, v + smoothness, v + 1, v + smoothness + 1);
            }
            t = _SetQuad (triangles, t, v, v + smoothness, v - smoothness + 1, v + 1);
        }
        _mesh.triangles = triangles;
    }

    /// <summary>
    /// Sets the quad.
    /// </summary>
    /// <returns>The quad.</returns>
    /// <param name="triangles">Triangles.</param>
    /// <param name="i">The index.</param>
    /// <param name="v00">V00.</param>
    /// <param name="v01">V01.</param>
    /// <param name="v10">V10.</param>
    /// <param name="v11">V11.</param>
    private static int _SetQuad (int[] triangles, int i, int v00, int v01, int v10, int v11) {
        triangles[i] = v00;
        triangles[i + 1] = triangles[i + 4] = v01;
        triangles[i + 2] = triangles[i + 3] = v10;
        triangles[i + 5] = v11;

        return i + 6;
    }

    //private void OnDrawGizmos()
    //{
    //    if (_vertices == null) return;


    //    for (int i = 0; i < _vertices.Length; i++) {
    //        Gizmos.color = Color.black;
    //        Gizmos.DrawSphere (_vertices[i] + transform.position, 0.01f);
    //    }
    //}

    /// <summary>
    /// On the validate.
    /// </summary>
    private void OnValidate()
    {
        if (smoothness < 3) smoothness = 3;


        if (bezierPointsPosition.Length == 3)
            Generate ();
    }
}
