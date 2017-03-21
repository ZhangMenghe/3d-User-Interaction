// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

Shader "Unlit/PlaneShader"
{
	
	SubShader
	{
		Tags{ "Queue" = "Geometry" "RenderType" = "Opaque" }
		Pass
			{
				Tags{ "LightMode" = "ForwardBase" }                      // This Pass tag is important or Unity may not give it the correct light information.
				CGPROGRAM
				#pragma target 3.0
				#pragma vertex vert
				#pragma fragment frag
				#pragma multi_compile_fwdbase                       // This line tells Unity to compile this pass for forward base.

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

				uniform sampler2D _NormalMapTex;

				struct vertex_input
				{
					float4 vertex   : POSITION;
					float3 normal   : NORMAL;
					float2 texcoord : TEXCOORD0;
					float4 tangent  : TANGENT;
				};

				struct vertex_output
				{
					float4  pos         : SV_POSITION;
					float2  uv          : TEXCOORD0;
					float4	worldVertex : TEXCOORD1;
					float3  worldNormal	: TEXCOORD2;
					float3  worldTangent : TEXCOORD3;
					float3  worldBinormal : TEXCOORD4;
					LIGHTING_COORDS(5,6)    // Macro to send shadow & attenuation to the vertex shader.
				};

				sampler2D _MainTex;
				float4 _MainTex_ST;

				vertex_output vert(vertex_input v)
				{
					vertex_output o;
					float4 worldVertex = mul(unity_ObjectToWorld, v.vertex);
					float3 worldNormal = normalize(mul(transpose((float3x3)unity_WorldToObject), v.normal.xyz));
					float3 worldTangent = normalize(mul(transpose((float3x3)unity_WorldToObject), v.tangent.xyz));
					float3 worldBinormal = normalize(cross(worldTangent, worldNormal));

					o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
					o.uv = v.texcoord.xy;

					o.worldVertex = worldVertex;
					o.worldNormal = worldNormal;
					o.worldTangent = worldTangent;
					o.worldBinormal = worldBinormal;

					TRANSFER_VERTEX_TO_FRAGMENT(o);                 // Macro to send shadow & attenuation to the fragment shader.

					return o;
				}

				fixed4 frag(vertex_output v) : COLOR
				{
					//Sample colour from texture.
					float4 surfaceColor = tex2D(_MainTex, v.uv);
					fixed atten = LIGHT_ATTENUATION(v); // Macro to get you the combined shadow & attenuation value.

					float3 bump = (tex2D(_NormalMapTex, v.uv) - float3(0.5, 0.5, 0.5)) * 2.0;
					float3 bumpNormal = (bump.x * normalize(v.worldTangent)) +
						(bump.y * normalize(v.worldBinormal)) +
						(bump.z * normalize(v.worldNormal));
					bumpNormal = normalize(bumpNormal);

					// Calculate ambient RGB intensities
					float Ka = _AmbientCoeff; // (May seem inefficient, but compiler will optimise)
					float3 amb = surfaceColor * UNITY_LIGHTMODEL_AMBIENT.rgb * Ka;

					// Sum up lighting calculations for each light (only diffuse/specular; ambient does not depend on the individual lights)
					float3 dif_and_spe_sum = float3(0.0, 0.0, 0.0);
					for (int i = 0; i < _NumPointLights; i++)
					{
						// Calculate diffuse RBG reflections, we save the results of L.N because we will use it again
						// (when calculating the reflected ray in our specular component)
						float fAtt = 1;
						float Kd = _DiffuseCoeff;
						float3 L = normalize(_PointLightPositions[i] - v.worldVertex.xyz);
						float LdotN = dot(L, bumpNormal);
						float3 dif = fAtt * _PointLightColors[i].rgb * Kd * surfaceColor * saturate(LdotN);

						// Calculate specular reflections
						float Ks = _SpecularCoeff;
						float specN = _SpecularPower; // Values>>1 give tighter highlights
						float3 V = normalize(_WorldSpaceCameraPos - v.worldVertex.xyz);
						// Using Blinn-Phong approximation (note, this is a modification of normal Phong illumination):
						float3 H = normalize(V + L);
						float3 spe = fAtt * _PointLightColors[i].rgb * Ks * pow(saturate(dot(bumpNormal, H)), specN);

						dif_and_spe_sum += dif + spe;
					}

					// Combine Phong illumination model components
					float4 returnColor = float4(0.0f, 0.0f, 0.0f, 0.0f);
					returnColor.rgb = amb.rgb + dif_and_spe_sum.rgb;
					returnColor.a = surfaceColor.a;	

					fixed4 c;
					c.rgb = (UNITY_LIGHTMODEL_AMBIENT.rgb * 2 * surfaceColor.rgb * returnColor.rgb);   // Ambient term. Only do this in Forward Base. It only needs calculating once.
					c.rgb += (surfaceColor.rgb * returnColor.rgb) * (atten * 2); // Diffuse and specular.
					c.a = surfaceColor.a + returnColor.a * atten;
					return c;
				}
				ENDCG
			}

		Pass
			{
				Tags{ "LightMode" = "ForwardAdd" }              // Again, this pass tag is important otherwise Unity may not give the correct light information.
				Blend One One                                   // Additively blend this pass with the previous one(s). This pass gets run once per pixel light.
				CGPROGRAM
				#pragma target 3.0
				#pragma vertex vert
				#pragma fragment frag
				#pragma multi_compile_fwdadd_fullshadows        // This line tells Unity to compile this pass for forward add, giving attenuation information for the light with full shadow information.

				#include "UnityCG.cginc"
				#include "AutoLight.cginc"
				uniform sampler2D _NormalMapTex;

				uniform float _AmbientCoeff;
				uniform float _DiffuseCoeff;
				uniform float _SpecularCoeff;
				uniform float _SpecularPower;

				uniform float3 _PointLightColor;
				uniform float3 _PointLightPosition;

				struct v2f
				{
					float4  pos         : SV_POSITION;
					float2  uv          : TEXCOORD0;
					float4	worldVertex : TEXCOORD1;
					float3  worldNormal	: TEXCOORD2;
					float3  worldTangent : TEXCOORD3;
					float3  worldBinormal : TEXCOORD4;
					LIGHTING_COORDS(5,6)    // Macro to send shadow & attenuation to the vertex shader.
				};

				v2f vert(appdata_tan v)
				{
					v2f o;
					float4 worldVertex = mul(unity_ObjectToWorld, v.vertex);
					float3 worldNormal = normalize(mul(transpose((float3x3)unity_WorldToObject), v.normal.xyz));
					float3 worldTangent = normalize(mul(transpose((float3x3)unity_WorldToObject), v.tangent.xyz));
					float3 worldBinormal = normalize(cross(worldTangent, worldNormal));

					o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
					o.uv = v.texcoord.xy;

					o.worldVertex = worldVertex;
					o.worldNormal = worldNormal;
					o.worldTangent = worldTangent;
					o.worldBinormal = worldBinormal;

					TRANSFER_VERTEX_TO_FRAGMENT(o);                 // Macro to send shadow & attenuation to the fragment shader.

					return o;
				}

				sampler2D _MainTex;

				fixed4 frag(v2f i) : COLOR
				{
					//Sample colour from texture.
					float4 surfaceColor = tex2D(_MainTex, i.uv);

					fixed atten = LIGHT_ATTENUATION(i); // Macro to get you the combined shadow & attenuation value.

					float3 bump = (tex2D(_NormalMapTex, i.uv) - float3(0.5, 0.5, 0.5)) * 2.0;
					float3 bumpNormal = (bump.x * normalize(i.worldTangent)) +
						(bump.y * normalize(i.worldBinormal)) +
						(bump.z * normalize(i.worldNormal));
					bumpNormal = normalize(bumpNormal);

					// Calculate ambient RGB intensities
					float Ka = _AmbientCoeff; // (May seem inefficient, but compiler will optimise)
					float3 amb = surfaceColor * UNITY_LIGHTMODEL_AMBIENT.rgb * Ka;

					// Calculate diffuse RBG reflections, we save the results of L.N because we will use it again
					// (when calculating the reflected ray in our specular component)
					float fAtt = 1;
					float Kd = _DiffuseCoeff;
					float3 L = normalize(_PointLightPosition - i.worldVertex.xyz);
					float LdotN = dot(L, bumpNormal);
					float3 dif = fAtt * _PointLightColor.rgb * Kd * surfaceColor * saturate(LdotN);

					// Calculate specular reflections
					float Ks = _SpecularCoeff;
					float specN = _SpecularPower; // Values>>1 give tighter highlights
					float3 V = normalize(_WorldSpaceCameraPos - i.worldVertex.xyz);
					// Using Blinn-Phong approximation (note, this is a modification of normal Phong illumination):
					float3 H = normalize(V + L);
					float3 spe = fAtt * _PointLightColor.rgb * Ks * pow(saturate(dot(bumpNormal, H)), specN);

					// Combine Phong illumination model components
					float4 returnColor = float4(0.0f, 0.0f, 0.0f, 0.0f);
					returnColor.rgb = amb.rgb + dif.rgb + spe.rgb;
					returnColor.a = surfaceColor.a;

					fixed4 c;
					c.rgb = (UNITY_LIGHTMODEL_AMBIENT.rgb * 2 * surfaceColor.rgb * returnColor.rgb);   // Ambient term. Only do this in Forward Base. It only needs calculating once.
					c.rgb += (surfaceColor.rgb * returnColor.rgb) * (atten * 2); // Diffuse and specular.
					c.a = surfaceColor.a + returnColor.a * atten;
					return c;
				}
				ENDCG
			}
	}
	FallBack "VertexLit"    // Use VertexLit's shadow caster/receiver passes.
}