
Shader "Custom/MudShader" {
    Properties {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        [MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
        _Color ("Color To Apply", Color) = (1,1,1,1)

		[HideInInspector] _RendererColor ("RendererColor", Color) = (1,1,1,1)
		[HideInInspector] _Flip ("Flip", Vector) = (1,1,1,1)
		[PerRendererData] _AlphaTex ("External Alpha", 2D) = "white" {}
		[PerRendererData] _EnableExternalAlpha ("Enable External Alpha", Float) = 0
    }
    
    SubShader {
        Tags {
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

                v2f vert (appdata_t IN)
                {
                    UNITY_SETUP_INSTANCE_ID (IN);
                    UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
                    v2f OUT;
                    OUT.vertex = UnityObjectToClipPos(IN.vertex);
                    OUT.texcoord = TRANSFORM_TEX(IN.texcoord, _MainTex);
                    OUT.color = fixed4(_Color.r, _Color.g, _Color.b, step(0.9, 1 - IN.vertex.y));

                    #ifdef PIXELSNAP_ON
                        OUT.vertex = UnityPixelSnap (OUT.vertex);
                    #endif
                    return OUT;
                }
                
                fixed4 frag (v2f i) : SV_Target
                {
                    fixed4 color = tex2D(_MainTex, i.texcoord);
                    i.color.a *= color.a * 0.5;
                    i.color = lerp(color, i.color, step(0.4, i.color.a));
                    return i.color;
                }
            ENDCG
        }
    }
}