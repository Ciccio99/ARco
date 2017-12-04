using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcFactory : MonoBehaviour {

    public GameObject GetArc () {
        return _InstantiateArc ();
    }

    private GameObject _InstantiateArc () {
        var arcGO = new GameObject ("Arc");
        arcGO.AddComponent<ProceduralArc> ();

        return arcGO;
    }
}
