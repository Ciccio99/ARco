/*
 * Author: Alberto Scicali
 * Used to calculate the distance between the arc object and the camera and set the value to a shader to determine when it should "shatter"
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShatterOnDistance : MonoBehaviour {

    public float distUntilShatterBegin = 0.2f;
    Material _mat;
    Transform _CamTransform;
	// Use this for initialization
	void Start () {
        _mat = GetComponent<MeshRenderer> ().material;
        _CamTransform = Camera.main.transform;
	}

    private void Update()
    {
        var dist = (transform.position - _CamTransform.position).magnitude;
      
        var val = 1f - Mathf.Clamp01 (dist / distUntilShatterBegin);
        
        _mat.SetFloat ("_ShatterDistance", val);
    }

}
