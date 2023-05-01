// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Unlit/water2"
{
    Properties
    {
        _LightFoamTex ("Texture1", 2D) = "white" {}
        _DarkFoamTex ("Texture2", 2D) = "white" {}
        _BottomTex ("Texture3", 2D) = "white" {}
        _PreWaterTex ("PreWaterTex", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
// Upgrade NOTE: excluded shader from DX11; has structs without semantics (struct v2f members worldVertex)
//#pragma exclude_renderers d3d11
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 timeTex: TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 timeTex: TEXCOORD0;
                float4 screenPos: TEXCOORD3;
                float4 worldVertex: TEXCOORD1;
            };

            sampler2D _LightFoamTex;
            sampler2D _DarkFoamTex;
            sampler2D _BottomTex;
            sampler2D _PreWaterTex;

            float hash1( float n )
            {
                return frac( n*17.0*frac( n*0.3183099 ) );
            }

            // return value noise (in x) and its derivatives (in yzw)
            float noise(float3 x)
            {
                float3 p = floor(x);
                float3 w = frac(x);

                float3 u = w*w*w*(w*(w*6.0-15.0)+10.0);
                float3 du = 30.0*w*w*(w*(w-2.0)+1.0);
                
                // quintic interpolation
                float n = p.x + 317.0*p.y + 157.0*p.z;
    
                float a = hash1(n+0.0);
                float b = hash1(n+1.0);
                float c = hash1(n+317.0);
                float d = hash1(n+318.0);
                float e = hash1(n+157.0);
                float f = hash1(n+158.0);
                float g = hash1(n+474.0);
                float h = hash1(n+475.0);

                float k0 =   a;
                float k1 =   b - a;
                float k2 =   c - a;
                float k3 =   e - a;
                float k4 =   a - b - c + d;
                float k5 =   a - c - e + g;
                float k6 =   a - b - e + f;
                float k7 = - a + b + c - d + e - f - g + h;

                return -1.0+2.0*(k0 + k1*u.x + k2*u.y + k3*u.z + k4*u.x*u.y + k5*u.y*u.z + k6*u.z*u.x + k7*u.x*u.y*u.z);
            }

            /*float fbm_4( float3 x )
            {
                float3x3 m3  = { 0.00,  0.80,  0.60,
                                  -0.80,  0.36, -0.48,
                                  -0.60, -0.48,  0.64};
                float3 z = x;
                float f = 2.0;
                float s = 0.5;
                float a = 0.0;
                float b = 0.5;
                for( int i=0; i<2; i++ )
                {
                    float n = noise(z);
                    a += b*n;
                    b *= s;
                    z = f*mul(z, m3);
                }
                return a;
            }*/

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.worldVertex = mul(unity_ObjectToWorld, v.vertex);
                o.screenPos = ComputeScreenPos(o.vertex);
                o.timeTex = v.timeTex;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 preFract = float2(i.worldVertex.x, i.worldVertex.z) / 160.0;
                // sample the texture
                float3 noiseCoord = float3(i.worldVertex.x + i.timeTex.x / 2.0, i.worldVertex.z + i.timeTex.x / 2.0, 0.0);

                
                float noiseAmountX = noise(noiseCoord);
                float noiseAmountY = noise(noiseCoord + float3(123, 1231, 21));

                float noiseAmount2X = noise(noiseCoord + float3(222, 0, 989));
                float noiseAmount2Y = noise(noiseCoord + float3(2141, 19, 99));

                float2 noiseD = float2(noiseAmountX, noiseAmountY);
                float2 noiseD2 = float2(noiseAmount2X, noiseAmount2Y);
                float4 col = tex2D(_LightFoamTex, frac(preFract + noiseD * 0.005));
                col += col * col.a + (1.0 - col.a) * tex2D(_DarkFoamTex, frac(preFract + noiseD2 * 0.005));
                float4 baseTextureHit = tex2D(_PreWaterTex, i.screenPos.xy / i.screenPos.w);
                float darken = 1.0;
                if (baseTextureHit.a > 0.05) {
                    darken = 0.5;
                }
                float4 bkColor = tex2D(_BottomTex, frac(preFract));
                col = col * col.a + (1.0 - col.a) * bkColor * darken;
                float4 blendAmt = clamp(noise(noiseCoord / 10.0 + float3(222, 0, 989)) * 4.0, 0.3, 0.7);
                float height = (i.worldVertex.y + 5.0) / 10.0 * 0.8 + 0.2;
                return (col * (1.0 - blendAmt) + bkColor * blendAmt) * height;
                //return baseTextureHit;
                //return float4(i.screenPos.xy / i.screenPos.w, 0.0, 1.0);
            }
            ENDCG
        }
    }
}
