using System;
using System.Collections;

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.iOS;
public class ArcManager : Singleton<ArcManager> {

	List<GameObject> arcList;
	[SerializeField]
	private GameObject _arcPrefab;
	private ArcAnchors _currentArcAnchors;

	void Awake()
	{
		if (_arcPrefab == null)
			throw new NullReferenceException ("ArcManager: Missing Arc prefab reference!");
		arcList = new List<GameObject> ();

	}

	
}

public struct ArcAnchors {
	ARPlaneAnchor anchor1;
	ARPlaneAnchor anchor2;

	public ArcAnchors (ARPlaneAnchor anchor1, ARPlaneAnchor anchor2) {
		this.anchor1 = anchor1;
		this.anchor2 = anchor2;
	}
}
