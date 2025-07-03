Shader "Custom/ListenModeTransparent"
{
    Properties {
        [Enum(UnityEngine.Rendering.CompareFunction)] _ZTest("ZTest", Float) = 0
        _OutlineColor("Outline Color", Color) = (1,1,1,1)
        _OutlineWidth("Outline Width", Range(0,10)) = 2
        _Alpha("Transparency", Range(0,1)) = 1
        _BlurRadius("Blur Radius", Range(0,5)) = 1
    }
    SubShader {
        Tags { "Queue"="Transparent+110" "RenderType"="Transparent" }

        // Outline pass
        Pass {
            Name "Outline"
            Cull Off
            ZWrite Off
            ZTest [_ZTest]
            Blend SrcAlpha OneMinusSrcAlpha
            ColorMask RGBA
            Stencil { Ref 1 Comp NotEqual }

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment fragOutline
            #include "UnityCG.cginc"

            struct appdata { float4 vertex:POSITION; float3 normal:NORMAL; float3 smoothNormal:TEXCOORD3; UNITY_VERTEX_INPUT_INSTANCE_ID };
            struct v2f { float4 pos:SV_POSITION; fixed4 col:COLOR; UNITY_VERTEX_OUTPUT_STEREO };

            uniform float _OutlineWidth;
            uniform fixed4 _OutlineColor;
            uniform float _Alpha;

            v2f vert(appdata v) {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
                float3 n = any(v.smoothNormal) ? v.smoothNormal : v.normal;
                float3 viewPos = UnityObjectToViewPos(v.vertex);
                float3 viewNorm = normalize(mul((float3x3)UNITY_MATRIX_IT_MV, n));
                o.pos = UnityViewToClipPos(viewPos + viewNorm * -viewPos.z * _OutlineWidth / 1000.0);
                o.col = fixed4(_OutlineColor.rgb, _OutlineColor.a * _Alpha);
                return o;
            }

            fixed4 fragOutline(v2f i):SV_Target { return i.col; }
            ENDCG
        }

        // Capture screen
        GrabPass { }

        // Blur pass
        Pass {
            Name "Blur"
            Cull Off
            ZWrite Off
            ZTest Always
            Blend SrcAlpha OneMinusSrcAlpha
            ColorMask RGBA
            Stencil { Ref 1 Comp Equal }

            CGPROGRAM
            #pragma vertex vertBlur
            #pragma fragment fragBlur
            #include "UnityCG.cginc"

            struct appdata_blur { float4 vertex:POSITION; UNITY_VERTEX_INPUT_INSTANCE_ID };
            struct v2f_blur { float4 pos:SV_POSITION; float4 grabPos: TEXCOORD0; UNITY_VERTEX_OUTPUT_STEREO };

            sampler2D _GrabTexture;
            float4 _GrabTexture_TexelSize; // x = 1/width, y=1/height
            float _BlurRadius;

            v2f_blur vertBlur(appdata_blur v) {
                v2f_blur o;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
                o.pos = UnityObjectToClipPos(v.vertex);
                o.grabPos = ComputeGrabScreenPos(o.pos);
                return o;
            }

            fixed4 fragBlur(v2f_blur i):SV_Target {
                // grabPos.zw stores the UV to sample GrabTexture
                float2 uv = i.grabPos.zw;
                fixed4 sum = fixed4(0,0,0,0);
                float count = 0;
                int steps = 3;
                for(int x=-steps; x<=steps; x++) {
                    for(int y=-steps; y<=steps; y++) {
                        float2 offset = float2(x, y) * _BlurRadius * _GrabTexture_TexelSize.xy;
                        sum += tex2D(_GrabTexture, uv + offset);
                        count += 1;
                    }
                }
                return sum / count;
            }
            ENDCG
        }
    }
    Fallback Off
}
