using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.iOS;

public class PlaneAnchor {

	public ARPlaneAnchor anchor;
	public GameObject planeObject;

	public PlaneAnchor (ARPlaneAnchor anchor, GameObject planeObject) {
		this.anchor = anchor;
		this.planeObject = planeObject;
		UnityARUtility.UpdatePlaneWithAnchorTransform (planeObject, anchor);
	}

	public void UpdateAnchor (ARPlaneAnchor anchor) {
		this.anchor = anchor;
		UnityARUtility.UpdatePlaneWithAnchorTransform (planeObject, anchor);
	}
}
