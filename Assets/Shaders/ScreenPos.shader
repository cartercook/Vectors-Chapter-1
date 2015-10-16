Shader "Custom/ScreenPos" {
    SubShader {
      Tags { "RenderType" = "Opaque" }
      CGPROGRAM
      #pragma surface surf Lambert
      struct Input {
          float4 screenPos;
      };
      sampler2D _Detail;
      void surf (Input IN, inout SurfaceOutput o) {
          float2 screenUV = IN.screenPos.xy / IN.screenPos.w;
          o.Albedo = tex2D(_Detail, screenUV);
      }
      ENDCG
    } 
    Fallback "Diffuse"
  }