/*
 * Author: Alberto Scicali
 * Voronoi Shader
 */
Shader "Custom/Voronoi Diagram"
{
	Properties
	{
		_HeatTex("Texture", 2D) = "white" {}
		_Radius("Radius", Range(0,0.25)) = 0.03
		_P("P", Range(1,2)) = 2
		[Toggle] _D("Distance", Float) = 0
	}

	SubShader
	{
		Tags {"Queue" = "Transparent"}
		Blend SrcAlpha OneMinusSrcAlpha
		
		Pass
		{
			CGPROGRAM

			#pragma vertex vert             
			#pragma fragment frag
			uniform float _D;
			uniform half _P;
			uniform half _Radius;
			uniform int _Length = 0;
			uniform float4 _Points[5];
			uniform fixed3 _Colors[100];

			sampler2D _HeatTex;

			struct vertInput {
				float4 pos : POSITION;
			};

			struct vertOutput {
				float4 pos : POSITION;
				fixed3 worldPos : TEXCOORD1;
			};

			vertOutput vert(vertInput input) {
				vertOutput o;
				o.pos = UnityObjectToClipPos(input.pos);
				o.worldPos = mul(unity_ObjectToWorld, input.pos).xyz;
				return o;
			}

			half distance_manhattan(float2 a, float2 b)
			{
				return abs(a.x - b.x) + abs(a.y - b.y);
			}
			half distance_chebyshev(float2 a, float2 b)
			{
				return max(abs(a.x - b.x) , abs(a.y - b.y)	);
			}
			half distance_minkowski(float2 a, float2 b)
			{
				return pow(pow(abs(a.x - b.x),_P) + pow(abs(a.y - b.y),_P),1/_P);
			}
            float distance_minkowski_3(float3 a, float3 b)
            {
                return pow(pow(abs(a.x - b.x),_P) + pow(abs(a.y - b.y),_P) + pow(abs(a.z - b.z),_P), 1/_P);
            }

            
            /// Fragment shader
			fixed4 frag(vertOutput output) : COLOR {
                // Colors to represent each seed 
                fixed3 testColors[5] = {{0.5, 0.5, 1}, {0, 0, 0}, {1, 0, 1}, {0, 1, 1}, {1, 1, 0}};
                // Random points in space to reperesent seeds
                fixed3 testPoints[5] = {{-1, 2, 5}, {0.2056,-4.872363, 2}, {-0.667718,1.428572, 3}, {-0.606689, -3.112218, -1}, {4.361695, -0.1724522, 0.5}};
				float minDist = 10000;
				int minI = 0;	// Index of min
				for (int i = 0; i < 5; i++)
				{
					float dist = distance_minkowski_3(output.worldPos.xyz, testPoints[i].xyz);

					if (dist < minDist)
					{
						minDist = dist;
						minI = i;
					}
				}

                    return fixed4(testColors[minI], 1);

			}
		  ENDCG
		  }
	}
	Fallback "Diffuse"
}