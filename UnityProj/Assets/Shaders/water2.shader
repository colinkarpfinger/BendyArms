// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Unlit/water2"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
// Upgrade NOTE: excluded shader from DX11; has structs without semantics (struct v2f members worldVertex)
#pragma exclude_renderers d3d11
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 worldNormal: NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 subNormal: TEXCOORD1;
                float4 worldVertex: TEXCOORD2;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            float hash1( float n )
            {
                return frac( n*17.0*frac( n*0.3183099 ) );
            }

            // return value noise (in x) and its derivatives (in yzw)
            float4 noise(float3 x)
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

            float fbm_4( float3 x )
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
            }

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.worldVertex = mul(unity_ObjectToWorld, v.vertex);
                o.subNormal = v.worldNormal;
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                //UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                // apply fog
                //UNITY_APPLY_FOG(i.fogCoord, col);
                float l = dot(i.subNormal, float3(1, 1, 1));
                float lowerMod = clamp(((i.worldVertex.y + 1.0) / 1.0), 0.0, 1.0) * 0.5 + 0.5;
                float2 preFract = float2(i.worldVertex.x, i.worldVertex.z) / 20.0;
                fixed4 col = tex2D(_MainTex, frac(preFract));
                return col;
            }
            ENDCG
        }
    }
}
