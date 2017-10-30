using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.iOS;
using System;
public class ARPlaneManager : Singleton<ARPlaneManager> {
	[SerializeField]
	private GameObject planePrefab;
	private Dictionary<string, PlaneAnchor> _storedPlaneAnchors;

	void Awake () {
		_storedPlaneAnchors = new Dictionary<string, PlaneAnchor> ();
		UnityARSessionNativeInterface.ARAnchorAddedEvent += OnPlaneAdded;
		UnityARSessionNativeInterface.ARAnchorUpdatedEvent += OnPlaneUpdated;
		UnityARSessionNativeInterface.ARAnchorRemovedEvent += OnPlaneRemoved;
	}

	public void OnPlaneAdded (ARPlaneAnchor anchor) {
		if (!_storedPlaneAnchors.ContainsKey (anchor.identifier)) {
			var planeAnchor = new PlaneAnchor (anchor, GameObject.Instantiate (planePrefab)); 
			_storedPlaneAnchors.Add (anchor.identifier, planeAnchor);
		}
	}

	public void OnPlaneRemoved (ARPlaneAnchor anchor) {
		Destroy (_storedPlaneAnchors[anchor.identifier].planeObject);
		_storedPlaneAnchors.Remove (anchor.identifier);
	}

	public void OnPlaneUpdated (ARPlaneAnchor anchor) {
		if (_storedPlaneAnchors.ContainsKey (anchor.identifier)) {
			_storedPlaneAnchors[anchor.identifier].UpdateAnchor (anchor);
		}
	}
}
