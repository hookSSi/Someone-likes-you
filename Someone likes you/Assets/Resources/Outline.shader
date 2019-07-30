Shader "Custom/Outline"
{
    Properties
    {
		_MainTex("Texture", 2D) = "white" {}
		_MaskTex("Mask", 2D) = "white" {}
		_NormalMap("Normal Map", 2D) = "bump" {}
		_OutlineColor("Outline Color", Color) = (1,1,1,1)
	}
	
    HLSLINCLUDE
    #include "Packages/com.unity.render-pipelines.lightweight/ShaderLibrary/Core.hlsl"
    ENDHLSL

	SubShader
	{
		Tags{"RenderType"="Transparent" "RenderPipeline" = "LightweightPipeline"}
		
		Blend SrcAlpha OneMinusSrcAlpha
		Cull Back
		ZWrite Off

		Pass
		{
			Tags { "LightMode" = "CombinedShapeLight" }
			HLSLPROGRAM
			#pragma prefer_hlslcc gles
			#pragma vertex Vert
            #pragma fragment Frag
            #pragma multi_compile USE_SHAPE_LIGHT_TYPE_0 __
            #pragma multi_compile USE_SHAPE_LIGHT_TYPE_1 __
            #pragma multi_compile USE_SHAPE_LIGHT_TYPE_2 __
            #pragma multi_compile USE_SHAPE_LIGHT_TYPE_3 __

			struct Attributes
			{
				float3 positionOS 	: POSITION;
				float4 color 		: COLOR;
				half2 uv 			: TEXCOORD0;
			};

			struct Varyings
			{
				float4 positionCS 	: SV_POSITION;
				float4 color 		: COLOR;
				half2 uv 			: TEXCOORD0;
				half2 lightingUV 	: TEXCOORD1;
			};

			#include "Packages/com.unity.render-pipelines.lightweight/Shaders/2D/Include/LightingUtility.hlsl"

			TEXTURE2D(_MainTex);
			SAMPLER(sampler_MainTex);
			TEXTURE2D(_MaskTex);
			SAMPLER(sampler_MaskTex);
			TEXTURE2D(_NormalMap);
			SAMPLER(sampler_NormalMap);
			half4 _MainTex_ST;
			half4 _NormalMap_ST;

			#if USE_SHAPE_LIGHT_TYPE_0
            SHAPE_LIGHT(0)
            #endif

            #if USE_SHAPE_LIGHT_TYPE_1
            SHAPE_LIGHT(1)
            #endif

            #if USE_SHAPE_LIGHT_TYPE_2
            SHAPE_LIGHT(2)
            #endif

            #if USE_SHAPE_LIGHT_TYPE_3
            SHAPE_LIGHT(3)
            #endif

			Varyings Vert(Attributes v)
			{
				Varyings o = (Varyings)0;

				o.positionCS = TransformObjectToHClip(v.positionOS);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				float4 clipVertex = o.positionCS / o.positionCS.w;
				o.lightingUV = ComputeScreenPos(clipVertex).xy;

				#if UNITY_UV_STARTS_AT_TOP
                o.lightingUV.y = 1.0 - o.lightingUV.y;
                #endif

				o.color = v.color;
				return o;
			}

			#include "Packages/com.unity.render-pipelines.lightweight/Shaders/2D/Include/CombinedShapeLightShared.hlsl"

			half4 _OutlineColor;
			half4 _MainTex_TexelSize;

			half4 Frag(Varyings i) : SV_Target
			{
				half4 c = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv);
				c.rgb *= c.a;
				half4 outlineC = _OutlineColor;
				outlineC.a *= ceil(c.a);

				half upAlpha = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv + half2(0, _MainTex_TexelSize.y)).a;
				half downAlpha = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv - half2(0, _MainTex_TexelSize.y)).a;
				half rightAlpha = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv + half2(_MainTex_TexelSize.x, 0)).a;
				half leftAlpha = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv - half2(_MainTex_TexelSize.x, 0)).a;

				half4 main = c * i.color;
				half4 outlineTex = lerp(outlineC, c, ceil(upAlpha * downAlpha * rightAlpha * leftAlpha));

				half4 mask = SAMPLE_TEXTURE2D(_MaskTex, sampler_MaskTex, i.uv);
				return CombinedShapeLightShared(outlineTex, mask, i.lightingUV);
			}
			ENDHLSL
		}

		Pass
		{
			Tags { "LightMode" = "NormalsRendering"}
			HLSLPROGRAM
            #pragma prefer_hlslcc gles
            #pragma vertex Vert
            #pragma fragment Frag

			struct Attributes
            {
                float3 positionOS   : POSITION;
                float4 color		: COLOR;
                half2  uv			: TEXCOORD0;
            };

            struct Varyings
            {
                float4  positionCS		: SV_POSITION;
                float4  color			: COLOR;
                half2	uv				: TEXCOORD0;
                float3  normalWS		: TEXCOORD1;
                float3  tangentWS		: TEXCOORD2;
                float3  bitangentWS		: TEXCOORD3;
            };

			TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);
            TEXTURE2D(_NormalMap);
            SAMPLER(sampler_NormalMap);
            float4 _NormalMap_ST;  // Is this the right way to do this?

			Varyings Vert(Attributes attributes)
            {
                Varyings o = (Varyings)0;

                o.positionCS = TransformObjectToHClip(attributes.positionOS);
                #if UNITY_UV_STARTS_AT_TOP
                    o.positionCS.y = -o.positionCS.y;
                #endif
                o.uv = TRANSFORM_TEX(attributes.uv, _NormalMap);
                o.uv = attributes.uv;
                o.color = attributes.color;
                o.normalWS = TransformObjectToWorldDir(float3(0, 0, 1));
                o.tangentWS = TransformObjectToWorldDir(float3(1, 0, 0));
                o.bitangentWS = TransformObjectToWorldDir(float3(0, 1, 0));
                return o;
            }

            #include "Packages/com.unity.render-pipelines.lightweight/Shaders/2D/Include/NormalsRenderingShared.hlsl"

			half4 _OutlineColor;
			half4 _MainTex_TexelSize;

			float4 Frag(Varyings i) : SV_Target
			{
				half4 c = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv);
				c.rgb *= c.a;
				half4 outlineC = _OutlineColor;
				outlineC.a *= ceil(c.a);

				half upAlpha = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv + half2(0, _MainTex_TexelSize.y)).a;
				half downAlpha = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv - half2(0, _MainTex_TexelSize.y)).a;
				half rightAlpha = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv + half2(_MainTex_TexelSize.x, 0)).a;
				half leftAlpha = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv - half2(_MainTex_TexelSize.x, 0)).a;

				half4 mainTex = c * i.color;
				half4 outlineTex = lerp(outlineC, c, ceil(upAlpha * downAlpha * rightAlpha * leftAlpha));

				float3 normalTS = UnpackNormal(SAMPLE_TEXTURE2D(_NormalMap, sampler_NormalMap, i.uv));
                return NormalsRenderingShared(outlineTex, normalTS, i.tangentWS.xyz, i.bitangentWS.xyz, -i.normalWS.xyz) * outlineC;
			}
			ENDHLSL
		}
	}
}
