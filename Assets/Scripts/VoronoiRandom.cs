﻿using System.Collections.Generic;
using UnityEngine;

public class VoronoiRandom : MonoBehaviour {

    public float minX = -1;
    public float maxX = +1;

    public float minY = -1;
    public float maxY = +1;

    public int length = 10;

    public Vector4[] points;
    public List<Vector4> pointsList;
    public Color[] colours;

    private Material material;

    // Use this for initialization
    void Start () {
        material = GetComponent<Renderer>().sharedMaterial;

        points = new Vector4[length];
        pointsList = new List<Vector4> (length);
        colours = new Color[length];
        for (int i = 0; i < length; i ++)
        {
            points[i] = new Vector4
            (
                transform.position.x + Random.Range (minX, maxX),
                transform.position.y + Random.Range (minY, maxY),
                transform.position.z + Random.Range (minY, maxY),
                    0f
            );
            pointsList.Add (points[i]);
            colours[i] = HSVToRGB( (1f / length) * i, 0.75f, 0.75f);

            // Shader


           // material.SetVector("_Colors" + i.ToString(), colours[i]);
        }
        material.SetVectorArray ("_Points", pointsList);
        material.SetInt("_Length", length);
    }


    [Range(0, 1)]
    public float amount = 0;
    void Update()
    {
        if (amount == 0)
            return;
        for (int i = 0; i < length; i++)
        {
            points[i].x += Random.Range(-0.1f, +0.1f) * amount;
            points[i].y += Random.Range(-0.1f, +0.1f) * amount;
            points[i].z += Random.Range (-0.1f, +0.1f) * amount;
            pointsList[i] = points[i];

            // Shader
            material.SetVectorArray ("_Points", pointsList);
            //material.SetVector("_Points" + i.ToString(), points[i]);
            //material.SetVector("_Colors" + i.ToString(), colours[i]);
        }
    }

    public static Color HSVToRGB(float H, float S, float V)
    {
        if (S == 0f)
            return new Color(V, V, V);
        else if (V == 0f)
            return Color.black;
        else
        {
            Color col = Color.black;
            float Hval = H * 6f;
            int sel = Mathf.FloorToInt(Hval);
            float mod = Hval - sel;
            float v1 = V * (1f - S);
            float v2 = V * (1f - S * mod);
            float v3 = V * (1f - S * (1f - mod));
            switch (sel + 1)
            {
                case 0:
                    col.r = V;
                    col.g = v1;
                    col.b = v2;
                    break;
                case 1:
                    col.r = V;
                    col.g = v3;
                    col.b = v1;
                    break;
                case 2:
                    col.r = v2;
                    col.g = V;
                    col.b = v1;
                    break;
                case 3:
                    col.r = v1;
                    col.g = V;
                    col.b = v3;
                    break;
                case 4:
                    col.r = v1;
                    col.g = v2;
                    col.b = V;
                    break;
                case 5:
                    col.r = v3;
                    col.g = v1;
                    col.b = V;
                    break;
                case 6:
                    col.r = V;
                    col.g = v1;
                    col.b = v2;
                    break;
                case 7:
                    col.r = V;
                    col.g = v3;
                    col.b = v1;
                    break;
            }
            col.r = Mathf.Clamp(col.r, 0f, 1f);
            col.g = Mathf.Clamp(col.g, 0f, 1f);
            col.b = Mathf.Clamp(col.b, 0f, 1f);
            return col;
        }
    }
}
