Shader "Custom/Gradient" {
    Properties {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        [MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
        _YSize ("Sprite Y Size", Float) = 0
        _Pixel ("Pixel from below", Float) = 0
        _Color ("Gradient Color", Color) = (1,1,1,1)

        [HideInInspector] _RendererColor ("RendererColor", Color) = (1,1,1,1)
		[HideInInspector] _Flip ("Flip", Vector) = (1,1,1,1)
		[PerRendererData] _AlphaTex ("External Alpha", 2D) = "white" {}
		[PerRendererData] _EnableExternalAlpha ("Enable External Alpha", Float) = 0
    }
    
    SubShader {
        Tags
        { 
            "Queue"="Transparent" 
			"IgnoreProjector"="True" 
			"RenderType"="Transparent" 
			"PreviewType"="Plane"
			"CanUseSpriteAtlas"="True"
        }
        LOD 100
        Cull Off
        Lighting Off
        ZWrite Off
        Fog { Mode Off }        
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha 
    
        Pass {
            CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #pragma target 2.0
                #pragma multi_compile_instancing
                #pragma multi_compile _ PIXELSNAP_ON
                #pragma multi_compile _ ETC1_EXTERNAL_ALPHA
                #include "UnityCG.cginc"
        
                fixed4 _Color;

                struct appdata_t
                {
                    float4 vertex   : POSITION;
                    float4 color    : COLOR;
                    float2 texcoord : TEXCOORD0;
                    UNITY_VERTEX_INPUT_INSTANCE_ID
                };

                struct v2f
                {
                    float4 vertex   : SV_POSITION;
                    fixed4 color    : COLOR;
                    float2 texcoord : TEXCOORD0;
                    UNITY_VERTEX_OUTPUT_STEREO
                };

                sampler2D _MainTex;
                sampler2D _AlphaTex;
                float4 _MainTex_ST;
                float _YSize;
                float _Pixel;

                v2f vert (appdata_t v) {
                    UNITY_SETUP_INSTANCE_ID (v);
                    UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);

                    fixed4 color = fixed4(_Color.r, _Color.g, _Color.b, 1 - (_YSize / _Pixel) * ((v.vertex.y + _YSize / 200) / (_YSize / 100)));
                    //fixed4 color = _Color;
                    o.color = color;
                    return o;
                }        
        
                fixed4 frag (v2f i) : SV_Target {
                    float4 color = i.color;
                    color *= lerp(fixed4(1, 1, 1, 0), fixed4(1, 1, 1, 1), tex2D(_MainTex, i.texcoord).a);
                    color = lerp(tex2D(_MainTex, i.texcoord), color, color.a);
                    color.a *= 255;
                    //color = tex2D(_RampTex, i.uv);
                    return color;
                }
            ENDCG
        }
    }
}