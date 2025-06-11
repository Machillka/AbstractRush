Shader "Custom/RandomTextureOverlay" {
    Properties {
        // 贴图属性，建议使用带透明通道的图片效果更佳
        _MainTex ("贴图", 2D) = "white" {}
        // 控制叠加采样次数（采样次数越多视觉上越“叠加”）
        _OverlayCount ("叠加次数", Range(1, 10)) = 5
        // 控制每次采样的随机偏移幅度，数值越大偏移越明显
        _Spread ("偏移范围", Range(0, 0.2)) = 0.05
    }
    SubShader {
        // 使用透明渲染队列及混合模式
        Tags { "Queue"="Transparent" "RenderType"="Transparent"}
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float _OverlayCount;
            float _Spread;

            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert(appdata v) {
                v2f o;
                // 将模型空间顶点转换到裁剪空间
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            // 简单的伪随机函数，在 [0, 1] 范围内返回一个随机数
            float rand(float2 co) {
                return frac(sin(dot(co, float2(12.9898, 78.233))) * 43758.5453);
            }

            fixed4 frag(v2f i) : SV_Target {
                fixed4 color = fixed4(0, 0, 0, 0);
                // 根据 _OverlayCount 进行多次采样并叠加
                for (int j = 0; j < (int)_OverlayCount; j++) {
                    // 利用 UV 和循环索引生成伪随机偏移，使每次采样略有差异
                    float2 offset = (float2(rand(i.uv + j), rand(i.uv - j)) - 0.5) * _Spread;
                    fixed4 sampleColor = tex2D(_MainTex, i.uv + offset);
                    color += sampleColor;
                }
                // 对累计结果进行平均化
                color /= _OverlayCount;
                return color;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
