using UnityEngine;

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
}
