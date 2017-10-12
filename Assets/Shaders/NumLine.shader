// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Upgrade NOTE: commented out 'float4x4 _Object2World', a built-in variable

//Shader "Custom/NumLine" {
//    Properties {
//        _Color ("Main Color", Color) = (1,1,1,0.5)
//        _MainTex ("Texture", 2D) = "white" { }
//    }
//    SubShader {
//        Pass {
//
//        CGPROGRAM
//// Upgrade NOTE: excluded shader from DX11 and Xbox360; has structs without semantics (struct v2f members screenPos)
//#pragma exclude_renderers d3d11 xbox360
//        #pragma vertex vert
//        #pragma fragment frag
//
//        #include "UnityCG.cginc"
//
//        fixed4 _Color;
//        sampler2D _MainTex;
//
//        struct v2f {
//            float4 pos : SV_POSITION;
//            float2 uv : TEXCOORD0;
//            float4 screenPos;
//        };
//
//        float4 _MainTex_ST;
//
//        v2f vert (appdata_base v)
//        {
//            v2f o;
//            o.pos = mul(UNITY_MATRIX_MVP, v.vertex); //put things in perspective
//            o.uv = TRANSFORM_TEX(v.texcoord, _MainTex); //v.texcoord * float2(1,1) + float2(0,0);
//            return o;
//        }
//
//        fixed4 frag (v2f i) : SV_Target
//        {
//            fixed4 texcol = _Color;
//            
//        	float2 screenUV = i.screenPos.xy / i.screenPos.w;
//        	screenUV *= float2(8,6);
//        	//o.Albedo *= tex2D (_MainTex, screenUV).rgb * 2;            
//            
//            texcol *= tex2D (_MainTex, screenUV) * 2;
//            return texcol;
//        }
//        ENDCG
//
//        }
//    }
//}

Shader "Custom/NumLine" {
    Properties {
        _Color ("Main Color", Color) = (1,1,1,1)
        _MainTex ("Texture", 2D) = "white" {}
        _quadUV ("QuadUV", Vector) = (1,1,1,1)
    }
    SubShader {
        Pass {

        CGPROGRAM
        #pragma vertex vert
        #pragma fragment frag
        #include "UnityCG.cginc"

        fixed4 _Color;
        sampler2D _MainTex;

        struct v2f {
            float4 pos : SV_POSITION;
            float2 uv : TEXCOORD0;
            // float4x4 _Object2World;
        };

        float4 _MainTex_ST;

        v2f vert (appdata_base v)
        {
            v2f o;
            o.pos = mul(unity_ObjectToWorld, v.vertex);
            
            float3 planeNormal = float3(1,0,0);
			float3 viewDir = _WorldSpaceCameraPos - o.pos.xyz; //vector pointing from camera to vertex
			float t = -dot(planeNormal, o.pos.xyz)/dot(planeNormal, viewDir); //t is the multiple of viewDir that allows it to reach the plane
			float3 intersection = o.pos.xyz + viewDir*t; //"intersection" is the point in 3-space where vertex.position + viewDir*t intersects the plane
			//now that we have the point, we will rotate it into the xy plane so that we can perform a texture-lookup on it
			//NOTE: we still have to translate our plane to the origin before the texture lookup
			float3 rotAxis = normalize(cross(planeNormal, float3(0, 0, 1))); //axis about which we will rotate our point
			
			float cosTheta = dot(normalize(planeNormal), float3(0,0,1)); //calculate angle. This only works b/c vectors are normalized
			float sinTheta = sin(acos(cosTheta)); //a thoroughly gross way of getting sinTheta from cosTheta
			float3x3 part1 = float3x3(rotAxis,
                                      intersection,
                                      cross(rotAxis, intersection));
            part1 = transpose(part1);
            
            float3 part2 = float3(dot(intersection, rotAxis)*(1 - cosTheta), cosTheta, sinTheta);
            o.uv = mul(part1, part2).xy; // NOTE: we still have to translate these from integers into uv coordinates between 0 and 1, e.g. -1 becomes 0.25
            o.pos = mul(UNITY_MATRIX_VP, o.pos);
            
            //o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
            return o;
        }

        fixed4 frag (v2f i) : SV_Target
        {
            fixed4 texcol;
            //frames the texture inside a solid colour
            if (i.uv.x < 0 || i.uv.x > 1 || i.uv.y < 0 || i.uv.y > 1) {
            	texcol = _Color;
            } else {
            	texcol = tex2D(_MainTex, i.uv);
            }
            return texcol;
        }
        ENDCG

        }
    }
}