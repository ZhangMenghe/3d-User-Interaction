// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

// Original Cg/HLSL code stub copyright (c) 2010-2012 SharpDX - Alexandre Mutel
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
// 
// Adapted for COMP30019 by Jeremy Nicholson, 10 Sep 2012
// Adapted further by Chris Ewin, 23 Sep 2013
// Adapted further (again) by Alex Zable (port to Unity), 19 Aug 2016

Shader "Unlit/PhongShader"
{
	SubShader
	{
		Tags{ "Queue" = "Geometry" "RenderType" = "Opaque" }
		Pass
		{
			Tags{ "LightMode" = "ForwardBase" }
			CGPROGRAM
			#pragma target 3.0
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fwdbase 

			#define MAX_LIGHTS 10

			#include "UnityCG.cginc"
			#include "AutoLight.cginc"

			uniform float _AmbientCoeff;
			uniform float _DiffuseCoeff;
			uniform float _SpecularCoeff;
			uniform float _SpecularPower;

			uniform int _NumPointLights;
			uniform float3 _PointLightColors[MAX_LIGHTS];
			uniform float3 _PointLightPositions[MAX_LIGHTS];

			struct vertIn
			{
				float4 vertex : POSITION;
				float4 normal : NORMAL;
				float4 color : COLOR;
			};

			struct vertOut
			{
				float4 vertex : SV_POSITION;
				float4 color : COLOR;
				float4 worldVertex : TEXCOORD0;
				float3 worldNormal : TEXCOORD1;
				LIGHTING_COORDS(3, 4)
			};

			// Implementation of the vertex shader
			vertOut vert(vertIn v)
			{
				vertOut o;

				// Convert Vertex position and corresponding normal into world coords
				// Note that we have to multiply the normal by the transposed inverse of the world 
				// transformation matrix (for cases where we have non-uniform scaling; we also don't
				// care about the "fourth" dimension, because translations don't affect the normal) 
				float4 worldVertex = mul(unity_ObjectToWorld, v.vertex);
				float3 worldNormal = normalize(mul(transpose((float3x3)unity_WorldToObject), v.normal.xyz));

				// Transform vertex in world coordinates to camera coordinates, and pass colour
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.color = v.color;

				// Pass out the world vertex position and world normal to be interpolated
				// in the fragment shader (and utilised)
				o.worldVertex = worldVertex;
				o.worldNormal = worldNormal;

				TRANSFER_VERTEX_TO_FRAGMENT(o);

				return o;
			}

			sampler2D _MainTex;
			fixed4 _Color;

			// Implementation of the fragment shader
			fixed4 frag(vertOut v) : SV_Target
			{
				fixed atten = LIGHT_ATTENUATION(v);

				// Our interpolated normal might not be of length 1
				float3 interpNormal = normalize(v.worldNormal);

				// Calculate ambient RGB intensities
				float Ka = _AmbientCoeff; // (May seem inefficient, but compiler will optimise)
				float3 amb = v.color.rgb * UNITY_LIGHTMODEL_AMBIENT.rgb * Ka;

				// Sum up lighting calculations for each light (only diffuse/specular; ambient does not depend on the individual lights)
				float3 dif_and_spe_sum = float3(0.0, 0.0, 0.0);
				for (int i = 0; i < _NumPointLights; i++)
				{
					// Calculate diffuse RBG reflections, we save the results of L.N because we will use it again
					// (when calculating the reflected ray in our specular component)
					float fAtt = 1;
					float Kd = _DiffuseCoeff;
					float3 L = normalize(_PointLightPositions[i] - v.worldVertex.xyz);
					float LdotN = dot(L, interpNormal);
					float3 dif = fAtt * _PointLightColors[i].rgb * Kd * v.color.rgb * saturate(LdotN);

					// Calculate specular reflections
					float Ks = _SpecularCoeff;
					float specN = _SpecularPower; // Values>>1 give tighter highlights
					float3 V = normalize(_WorldSpaceCameraPos - v.worldVertex.xyz);
					// Using Blinn-Phong approximation (note, this is a modification of normal Phong illumination):
					float3 H = normalize(V + L);
					float3 spe = fAtt * _PointLightColors[i].rgb * Ks * pow(saturate(dot(interpNormal, H)), specN);

					dif_and_spe_sum += dif + spe;
				}

				// Combine Phong illumination model components
				float4 returnColor = float4(0.0f, 0.0f, 0.0f, 0.0f);
				returnColor.rgb = amb.rgb + dif_and_spe_sum.rgb;
				returnColor.a = v.color.a;

				fixed4 c;
				c.rgb = (UNITY_LIGHTMODEL_AMBIENT.rgb * 2 * returnColor.rgb);   // Ambient term. Only do this in Forward Base. It only needs calculating once.
				c.rgb += returnColor.rgb * (atten * 2); // Diffuse and specular.
				c.a = returnColor.a * atten;

				return c;
			}
			ENDCG
		}
	}
	FallBack "VertexLit"    // Use VertexLit's shadow caster/receiver passes.
}
