/*
 * Author: Alberto Scicali
 * Factory for generating arcs
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcFactory : MonoBehaviour {

    /// <summary>
    /// Gets the arc.
    /// </summary>
    /// <returns>The arc.</returns>
    public GameObject GetArc () {
        return _InstantiateArc ();
    }

    /// <summary>
    /// Instantiates the arc.
    /// </summary>
    /// <returns>The arc gameobject</returns>
    private GameObject _InstantiateArc () {
        var arcGO = new GameObject ("Arc");
        arcGO.AddComponent<ProceduralArc> ();

        return arcGO;
    }
}
