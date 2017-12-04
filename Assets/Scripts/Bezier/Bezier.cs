﻿using UnityEngine;

public static class Bezier {

	public static Vector3 GetPoint (Vector3 p0, Vector3 p1, Vector3 p2, float t) {
		t = Mathf.Clamp01 (t);
		float oneMinusT = 1f - t;
		// B(t) = (1-t)^2 * p0 + 2 * (1 - t) * t * p1 + t^2 * p2
		return oneMinusT * oneMinusT * p0 +
				2f * oneMinusT * t * p1 +
				t * t * p2;
		// lerp way of doing it
		// return Vector3.Lerp (Vector3.Lerp (p0, p1, t), Vector3.Lerp (p1, p2, t), t);
	}

    public static Vector3[] GetBezierCurvePoints (int steps, Vector3 p0, Vector3 p1, Vector3 p2) {
        var bezierVertices = new Vector3[steps + 1];

        bezierVertices[0] = GetPoint (p0, p1, p2, 0f);
        for (int i = 1; i <= steps; i++) {
            Vector3 lineEnd = GetPoint (p0, p1, p2, (i / (float)steps));
            bezierVertices[i] = lineEnd;
        }

        return bezierVertices;
    }
}
