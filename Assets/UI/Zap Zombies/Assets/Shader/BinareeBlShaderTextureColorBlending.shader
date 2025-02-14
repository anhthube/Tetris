Shader "Binaree/BlShaderTextureColorBlending"
{
  Properties
  {
    _MainTex ("Base (RGB), Alpha (A)", 2D) = "white" {}
    _Color ("Blend Color", Color) = (1,1,1,0.5)
    _Opacity ("Opacity", float) = 1
    _PremultipliedBlendingColor ("Premultiplied Blending Color", Color) = (0.5,0.5,0.5,0.5)
    _OpacityForTexture ("OpacityForTexture", float) = 0.5
  }
  SubShader
  {
    Tags
    { 
      "QUEUE" = "Transparent+1"
      "RenderType" = "Opaque"
    }
    Pass // ind: 1, name: 
    {
      Tags
      { 
        "QUEUE" = "Transparent+1"
        "RenderType" = "Opaque"
      }
      ZClip Off
      ZWrite Off
      Blend SrcAlpha OneMinusSrcAlpha
      // m_ProgramMask = 6
      CGPROGRAM
      #pragma multi_compile DUMMY
      //#pragma target 4.0
      
      #pragma vertex vert
      #pragma fragment frag
      
      #include "UnityCG.cginc"
      
      
      #define CODE_BLOCK_VERTEX
      //uniform float4x4 unity_ObjectToWorld;
      //uniform float4x4 unity_MatrixVP;
      uniform sampler2D _MainTex;
      uniform float _Opacity;
      uniform float4 _PremultipliedBlendingColor;
      uniform float _OpacityForTexture;
      struct appdata_t
      {
          float4 vertex :POSITION;
          float4 texcoord :TEXCOORD0;
          float4 texcoord1 :TEXCOORD1;
      };
      
      struct OUT_Data_Vert
      {
          float2 xlv_TEXCOORD0 :TEXCOORD0;
          float2 xlv_TEXCOORD1 :TEXCOORD1;
          float4 vertex :SV_POSITION;
      };
      
      struct v2f
      {
          float2 xlv_TEXCOORD0 :TEXCOORD0;
      };
      
      struct OUT_Data_Frag
      {
          float4 color :SV_Target0;
      };
      
      OUT_Data_Vert vert(appdata_t in_v)
      {
          OUT_Data_Vert out_v;
          float4 tmpvar_1;
          tmpvar_1.w = 1;
          tmpvar_1.xyz = float3(in_v.vertex.xyz);
          out_v.vertex = mul(unity_MatrixVP, mul(unity_ObjectToWorld, tmpvar_1));
          out_v.xlv_TEXCOORD0 = in_v.texcoord.xy;
          out_v.xlv_TEXCOORD1 = in_v.texcoord1.xy;
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          float4 textureColor_1;
          float4 tmpvar_2;
          tmpvar_2 = tex2D(_MainTex, in_f.xlv_TEXCOORD0);
          textureColor_1.xyz = float3(tmpvar_2.xyz);
          textureColor_1.w = (tmpvar_2.w * _Opacity);
          if((_OpacityForTexture<=0.99))
          {
              textureColor_1.xyz = float3(((tmpvar_2.xyz * float3(_OpacityForTexture, _OpacityForTexture, _OpacityForTexture)) + _PremultipliedBlendingColor.xyz));
          }
          out_f.color = textureColor_1;
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
  }
  FallBack Off
}