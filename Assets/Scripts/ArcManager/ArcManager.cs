using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.iOS;

public class ArcManager : Singleton<ArcManager> {

    [SerializeField]
    private Material _arcMaterial;
    private List<GameObject> _arcList;
    private ArcFactory _arcFactory;
    private Vector3? _startPoint, _endPoint;

    private void OnEnable() {
        _arcFactory = GetComponent<ArcFactory> () ?? gameObject.AddComponent<ArcFactory> ();

        _arcList = new List<GameObject> ();

        InputManager.Instance.ARTouchBeganUpdateEvent += OnTouchBegan;
    }

    public void OnTouchBegan (Touch touch) {
#if UNITY_EDITOR
        var ray = Camera.main.ScreenPointToRay (touch.position);
        RaycastHit hit;
        if (Physics.Raycast (ray.origin, ray.direction, out hit)) {
            _CollectPoint (hit.point);   
        }
#else
        var screenPosition = Camera.main.ScreenToViewportPoint(touch.position);

        ARPoint point = new ARPoint
        {
            x = screenPosition.x,
            y = screenPosition.y
        };

        // use arkit hit test to find planes
        List<ARHitTestResult> hitResults = UnityARSessionNativeInterface.GetARSessionNativeInterface().HitTest(point,
                    ARHitTestResultType.ARHitTestResultTypeExistingPlaneUsingExtent);
        if (hitResults.Count > 0)
        {
            foreach (var hitResult in hitResults)
            {
                Vector3 position = UnityARMatrixOps.GetPosition(hitResult.worldTransform);
                _CollectPoint (position);
                break;
            }
        }
#endif
    }

    private void _InstantiateArc (Vector3 start, Vector3 end) {
        var go = _arcFactory.GetArc ();
        _arcList.Add (go);

        go.AddComponent<StealCamTextures> ();
        go.AddComponent<ShatterOnDistance> ();

        go.GetComponent<MeshRenderer> ().material = _arcMaterial;

        var arc = go.GetComponent<ProceduralArc> ();
        arc.noisyArc = true;
        arc.SetPoints (start, end);
        arc.Generate ();
    }

    private void _CollectPoint (Vector3 point) {
        if (_startPoint == null ) {
            _startPoint = point;
            return;
        }

        _endPoint = point;
        _InstantiateArc (_startPoint.Value, _endPoint.Value);
        _startPoint = _endPoint = null;
    }
}


