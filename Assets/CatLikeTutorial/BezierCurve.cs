using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(LineRenderer))]
public class BezierCurve : MonoBehaviour {

	public Vector3[] points;
	public int lineSteps = 15;
	[HideInInspector]
	public LineRenderer curveRenderer;
	public Vector3[] curvePoints {get; private set;}

	void Awake()
	{
		curveRenderer = GetComponent<LineRenderer> () ?? gameObject.AddComponent<LineRenderer> ();
	}

	void Start()
	{
		SetLineRendererCurve ();
	}

	public void Reset () {
		points = new Vector3[] {
			new Vector3 (1f, 0f, 0f),
			new Vector3 (2f, 0f, 0f),
			new Vector3 (3f, 0f, 0f)
		};
		curveRenderer = GetComponent<LineRenderer> () ?? gameObject.AddComponent<LineRenderer> ();
	}

	public Vector3 GetPoint (float t) {
		return transform.TransformPoint (Bezier.GetPoint (points[0], points[1], points[2], t));
	}

	public void SetLineRendererCurve () {
		Vector3[] linePositions = new Vector3[lineSteps + 1];
		curveRenderer.positionCount = linePositions.Length;

		linePositions[0] = GetPoint (0f);
		for (int i = 1; i<= lineSteps; i++) {
			Vector3 lineEnd = GetPoint (i / (float) lineSteps);
			linePositions[i] = lineEnd;
		}
		curveRenderer.SetPositions (linePositions);
		curvePoints = linePositions;
	}

	public void SetMainPoints (Vector3 start, Vector3 mid, Vector3 end) {
		points[0] = start;
		points[1] = mid;
		points[2] = end;
	}
}
