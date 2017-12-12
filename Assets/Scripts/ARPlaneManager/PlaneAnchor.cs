/*
 * Author: Alberto Scicali
 * Plane anchor object to contain information about a given AR anchor
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.iOS;

public class PlaneAnchor {

	public ARPlaneAnchor anchor;
	public GameObject planeObject;

    /// <summary>
    /// Initializes a new instance of the <see cref="T:PlaneAnchor"/> class.
    /// </summary>
    /// <param name="anchor">Anchor.</param>
    /// <param name="planeObject">Plane object.</param>
	public PlaneAnchor (ARPlaneAnchor anchor, GameObject planeObject) {
		this.anchor = anchor;
		this.planeObject = planeObject;
		UnityARUtility.UpdatePlaneWithAnchorTransform (planeObject, anchor);
	}

    /// <summary>
    /// Updates the anchor.
    /// </summary>
    /// <param name="anchor">Anchor.</param>
	public void UpdateAnchor (ARPlaneAnchor anchor) {
		this.anchor = anchor;
		UnityARUtility.UpdatePlaneWithAnchorTransform (planeObject, anchor);
	}
}
