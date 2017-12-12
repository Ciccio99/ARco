using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.iOS;

[RequireComponent (typeof (MeshRenderer))]
public class GetCameraTextures : MonoBehaviour {

  public UnityARVideo video;
  private Material _mat;

	void Start () {
		_mat = GetComponent<MeshRenderer>().material;
        video = FindObjectOfType<UnityARVideo> ();
	}
	
	// Update is called once per frame
	void Update () {
        _mat.SetTexture( "_vidTexY" , video.m_ClearMaterial.GetTexture( "_textureY" ) );
        _mat.SetTexture( "_vidTexCBCR" , video.m_ClearMaterial.GetTexture( "_textureCbCr" ));
	}
}
