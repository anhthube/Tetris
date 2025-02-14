Shader "Hidden/SGXBlur"
{
  Properties
  {
    _MainTex ("Albedo (RGB)", 2D) = "white" {}
    _Color ("TintColor", Color) = (1,1,1,0.1)
  }
  SubShader
  {
    Tags
    { 
    }
    Pass // ind: 1, name: 
    {
      Tags
      { 
      }
      ZClip Off
      ZTest Always
      ZWrite Off
      Cull Off
      // m_ProgramMask = 6
      CGPROGRAM
      //#pragma target 4.0
      
      #pragma vertex vert
      #pragma fragment frag
      
      #include "UnityCG.cginc"
      
      
      #define CODE_BLOCK_VERTEX
      //uniform float4x4 unity_ObjectToWorld;
      //uniform float4x4 unity_MatrixVP;
      uniform float4 _MainTex_TexelSize;
      uniform float _blurParameter;
      uniform sampler2D _MainTex;
      struct appdata_t
      {
          float4 vertex :POSITION;
          float4 texcoord :TEXCOORD0;
      };
      
      struct OUT_Data_Vert
      {
          float2 xlv_TEXCOORD0 :TEXCOORD0;
          float4 xlv_TEXCOORD1 :TEXCOORD1;
          float4 xlv_TEXCOORD1_1 :TEXCOORD1_1;
          float4 xlv_TEXCOORD1_2 :TEXCOORD1_2;
          float4 vertex :SV_POSITION;
      };
      
      struct v2f
      {
          float2 xlv_TEXCOORD0 :TEXCOORD0;
          float4 xlv_TEXCOORD1 :TEXCOORD1;
          float4 xlv_TEXCOORD1_1 :TEXCOORD1_1;
          float4 xlv_TEXCOORD1_2 :TEXCOORD1_2;
      };
      
      struct OUT_Data_Frag
      {
          float4 color :SV_Target0;
      };
      
      OUT_Data_Vert vert(appdata_t in_v)
      {
          OUT_Data_Vert out_v;
          float4 coords_1;
          float2 netFilterWidth_2;
          float4 tmpvar_3;
          float4 tmpvar_4;
          tmpvar_4.w = 1;
          tmpvar_4.xyz = float3(in_v.vertex.xyz);
          float4 tmpvar_5;
          tmpvar_5.zw = float2(1, 1);
          tmpvar_5.xy = float2(in_v.texcoord.xy);
          float2 tmpvar_6;
          tmpvar_6 = ((_MainTex_TexelSize.xy * float2(0, 1)) * _blurParameter);
          netFilterWidth_2 = tmpvar_6;
          float4 tmpvar_7;
          tmpvar_7 = ((-netFilterWidth_2.xyxy) * 3);
          coords_1 = (tmpvar_7 + netFilterWidth_2.xyxy);
          tmpvar_3 = (in_v.texcoord.xyxy + (coords_1 * float4(1, 1, (-1), (-1))));
          coords_1 = (coords_1 + netFilterWidth_2.xyxy);
          out_v.vertex = mul(unity_MatrixVP, mul(unity_ObjectToWorld, tmpvar_4));
          out_v.xlv_TEXCOORD0 = tmpvar_5.xy;
          out_v.xlv_TEXCOORD1 = (in_v.texcoord.xyxy + (tmpvar_7 * float4(1, 1, (-1), (-1))));
          out_v.xlv_TEXCOORD1_1 = tmpvar_3;
          out_v.xlv_TEXCOORD1_2 = (in_v.texcoord.xyxy + (coords_1 * float4(1, 1, (-1), (-1))));
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          float4 tapB_1;
          float4 tapA_2;
          float4 color_3;
          float4 tmpvar_4;
          tmpvar_4 = tex2D(_MainTex, in_f.xlv_TEXCOORD0);
          float4 tmpvar_5;
          tmpvar_5 = tex2D(_MainTex, in_f.xlv_TEXCOORD1.xy);
          tapA_2 = tmpvar_5;
          float4 tmpvar_6;
          tmpvar_6 = tex2D(_MainTex, in_f.xlv_TEXCOORD1.zw);
          tapB_1 = tmpvar_6;
          color_3 = ((tmpvar_4 * float4(0.324, 0.324, 0.324, 1)) + ((tapA_2 + tapB_1) * float4(0.0205, 0.0205, 0.0205, 0)));
          float4 tmpvar_7;
          tmpvar_7 = tex2D(_MainTex, in_f.xlv_TEXCOORD1_1.xy);
          tapA_2 = tmpvar_7;
          float4 tmpvar_8;
          tmpvar_8 = tex2D(_MainTex, in_f.xlv_TEXCOORD1_1.zw);
          tapB_1 = tmpvar_8;
          color_3 = (color_3 + ((tapA_2 + tapB_1) * float4(0.0855, 0.0855, 0.0855, 0)));
          float4 tmpvar_9;
          tmpvar_9 = tex2D(_MainTex, in_f.xlv_TEXCOORD1_2.xy);
          tapA_2 = tmpvar_9;
          float4 tmpvar_10;
          tmpvar_10 = tex2D(_MainTex, in_f.xlv_TEXCOORD1_2.zw);
          tapB_1 = tmpvar_10;
          color_3 = (color_3 + ((tapA_2 + tapB_1) * float4(0.232, 0.232, 0.232, 0)));
          out_f.color = color_3;
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
    Pass // ind: 2, name: 
    {
      Tags
      { 
      }
      ZClip Off
      ZTest Always
      ZWrite Off
      Cull Off
      // m_ProgramMask = 6
      CGPROGRAM
      //#pragma target 4.0
      
      #pragma vertex vert
      #pragma fragment frag
      
      #include "UnityCG.cginc"
      
      
      #define CODE_BLOCK_VERTEX
      //uniform float4x4 unity_ObjectToWorld;
      //uniform float4x4 unity_MatrixVP;
      uniform float4 _MainTex_TexelSize;
      uniform float _blurParameter;
      uniform sampler2D _MainTex;
      struct appdata_t
      {
          float4 vertex :POSITION;
          float4 texcoord :TEXCOORD0;
      };
      
      struct OUT_Data_Vert
      {
          float2 xlv_TEXCOORD0 :TEXCOORD0;
          float4 xlv_TEXCOORD1 :TEXCOORD1;
          float4 xlv_TEXCOORD1_1 :TEXCOORD1_1;
          float4 xlv_TEXCOORD1_2 :TEXCOORD1_2;
          float4 vertex :SV_POSITION;
      };
      
      struct v2f
      {
          float2 xlv_TEXCOORD0 :TEXCOORD0;
          float4 xlv_TEXCOORD1 :TEXCOORD1;
          float4 xlv_TEXCOORD1_1 :TEXCOORD1_1;
          float4 xlv_TEXCOORD1_2 :TEXCOORD1_2;
      };
      
      struct OUT_Data_Frag
      {
          float4 color :SV_Target0;
      };
      
      OUT_Data_Vert vert(appdata_t in_v)
      {
          OUT_Data_Vert out_v;
          float4 coords_1;
          float2 netFilterWidth_2;
          float4 tmpvar_3;
          float4 tmpvar_4;
          tmpvar_4.w = 1;
          tmpvar_4.xyz = float3(in_v.vertex.xyz);
          float2 tmpvar_5;
          tmpvar_5 = ((_MainTex_TexelSize.xy * float2(1, 0)) * _blurParameter);
          netFilterWidth_2 = tmpvar_5;
          float4 tmpvar_6;
          tmpvar_6 = ((-netFilterWidth_2.xyxy) * 3);
          coords_1 = (tmpvar_6 + netFilterWidth_2.xyxy);
          tmpvar_3 = (in_v.texcoord.xyxy + (coords_1 * float4(1, 1, (-1), (-1))));
          coords_1 = (coords_1 + netFilterWidth_2.xyxy);
          out_v.vertex = mul(unity_MatrixVP, mul(unity_ObjectToWorld, tmpvar_4));
          out_v.xlv_TEXCOORD0 = in_v.texcoord.xy;
          out_v.xlv_TEXCOORD1 = (in_v.texcoord.xyxy + (tmpvar_6 * float4(1, 1, (-1), (-1))));
          out_v.xlv_TEXCOORD1_1 = tmpvar_3;
          out_v.xlv_TEXCOORD1_2 = (in_v.texcoord.xyxy + (coords_1 * float4(1, 1, (-1), (-1))));
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          float4 tapB_1;
          float4 tapA_2;
          float4 color_3;
          float4 tmpvar_4;
          tmpvar_4 = tex2D(_MainTex, in_f.xlv_TEXCOORD0);
          float4 tmpvar_5;
          tmpvar_5 = tex2D(_MainTex, in_f.xlv_TEXCOORD1.xy);
          tapA_2 = tmpvar_5;
          float4 tmpvar_6;
          tmpvar_6 = tex2D(_MainTex, in_f.xlv_TEXCOORD1.zw);
          tapB_1 = tmpvar_6;
          color_3 = ((tmpvar_4 * float4(0.324, 0.324, 0.324, 1)) + ((tapA_2 + tapB_1) * float4(0.0205, 0.0205, 0.0205, 0)));
          float4 tmpvar_7;
          tmpvar_7 = tex2D(_MainTex, in_f.xlv_TEXCOORD1_1.xy);
          tapA_2 = tmpvar_7;
          float4 tmpvar_8;
          tmpvar_8 = tex2D(_MainTex, in_f.xlv_TEXCOORD1_1.zw);
          tapB_1 = tmpvar_8;
          color_3 = (color_3 + ((tapA_2 + tapB_1) * float4(0.0855, 0.0855, 0.0855, 0)));
          float4 tmpvar_9;
          tmpvar_9 = tex2D(_MainTex, in_f.xlv_TEXCOORD1_2.xy);
          tapA_2 = tmpvar_9;
          float4 tmpvar_10;
          tmpvar_10 = tex2D(_MainTex, in_f.xlv_TEXCOORD1_2.zw);
          tapB_1 = tmpvar_10;
          color_3 = (color_3 + ((tapA_2 + tapB_1) * float4(0.232, 0.232, 0.232, 0)));
          out_f.color = color_3;
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
    Pass // ind: 3, name: 
    {
      Tags
      { 
      }
      ZClip Off
      ZTest Always
      ZWrite Off
      Cull Off
      // m_ProgramMask = 6
      CGPROGRAM
      //#pragma target 4.0
      
      #pragma vertex vert
      #pragma fragment frag
      
      #include "UnityCG.cginc"
      
      
      #define CODE_BLOCK_VERTEX
      //uniform float4x4 unity_ObjectToWorld;
      //uniform float4x4 unity_MatrixVP;
      uniform float4 _MainTex_TexelSize;
      uniform float _blurParameter;
      uniform sampler2D _MainTex;
      uniform float4 _Color;
      struct appdata_t
      {
          float4 vertex :POSITION;
          float4 texcoord :TEXCOORD0;
      };
      
      struct OUT_Data_Vert
      {
          float2 xlv_TEXCOORD0 :TEXCOORD0;
          float4 xlv_TEXCOORD1 :TEXCOORD1;
          float4 xlv_TEXCOORD1_1 :TEXCOORD1_1;
          float4 xlv_TEXCOORD1_2 :TEXCOORD1_2;
          float4 vertex :SV_POSITION;
      };
      
      struct v2f
      {
          float2 xlv_TEXCOORD0 :TEXCOORD0;
          float4 xlv_TEXCOORD1 :TEXCOORD1;
          float4 xlv_TEXCOORD1_1 :TEXCOORD1_1;
          float4 xlv_TEXCOORD1_2 :TEXCOORD1_2;
      };
      
      struct OUT_Data_Frag
      {
          float4 color :SV_Target0;
      };
      
      OUT_Data_Vert vert(appdata_t in_v)
      {
          OUT_Data_Vert out_v;
          float4 coords_1;
          float2 netFilterWidth_2;
          float4 tmpvar_3;
          float4 tmpvar_4;
          tmpvar_4.w = 1;
          tmpvar_4.xyz = float3(in_v.vertex.xyz);
          float2 tmpvar_5;
          tmpvar_5 = ((_MainTex_TexelSize.xy * float2(1, 0)) * _blurParameter);
          netFilterWidth_2 = tmpvar_5;
          float4 tmpvar_6;
          tmpvar_6 = ((-netFilterWidth_2.xyxy) * 3);
          coords_1 = (tmpvar_6 + netFilterWidth_2.xyxy);
          tmpvar_3 = (in_v.texcoord.xyxy + (coords_1 * float4(1, 1, (-1), (-1))));
          coords_1 = (coords_1 + netFilterWidth_2.xyxy);
          out_v.vertex = mul(unity_MatrixVP, mul(unity_ObjectToWorld, tmpvar_4));
          out_v.xlv_TEXCOORD0 = in_v.texcoord.xy;
          out_v.xlv_TEXCOORD1 = (in_v.texcoord.xyxy + (tmpvar_6 * float4(1, 1, (-1), (-1))));
          out_v.xlv_TEXCOORD1_1 = tmpvar_3;
          out_v.xlv_TEXCOORD1_2 = (in_v.texcoord.xyxy + (coords_1 * float4(1, 1, (-1), (-1))));
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          float4 color_1;
          float4 tapB_2;
          float4 tapA_3;
          float4 color_4;
          float4 tmpvar_5;
          tmpvar_5 = tex2D(_MainTex, in_f.xlv_TEXCOORD0);
          float4 tmpvar_6;
          tmpvar_6 = tex2D(_MainTex, in_f.xlv_TEXCOORD1.xy);
          tapA_3 = tmpvar_6;
          float4 tmpvar_7;
          tmpvar_7 = tex2D(_MainTex, in_f.xlv_TEXCOORD1.zw);
          tapB_2 = tmpvar_7;
          color_4 = ((tmpvar_5 * float4(0.324, 0.324, 0.324, 1)) + ((tapA_3 + tapB_2) * float4(0.0205, 0.0205, 0.0205, 0)));
          float4 tmpvar_8;
          tmpvar_8 = tex2D(_MainTex, in_f.xlv_TEXCOORD1_1.xy);
          tapA_3 = tmpvar_8;
          float4 tmpvar_9;
          tmpvar_9 = tex2D(_MainTex, in_f.xlv_TEXCOORD1_1.zw);
          tapB_2 = tmpvar_9;
          color_4 = (color_4 + ((tapA_3 + tapB_2) * float4(0.0855, 0.0855, 0.0855, 0)));
          float4 tmpvar_10;
          tmpvar_10 = tex2D(_MainTex, in_f.xlv_TEXCOORD1_2.xy);
          tapA_3 = tmpvar_10;
          float4 tmpvar_11;
          tmpvar_11 = tex2D(_MainTex, in_f.xlv_TEXCOORD1_2.zw);
          tapB_2 = tmpvar_11;
          color_4 = (color_4 + ((tapA_3 + tapB_2) * float4(0.232, 0.232, 0.232, 0)));
          float3 tmpvar_12;
          tmpvar_12 = lerp(color_4.xyz, _Color.xyz, _Color.www);
          color_1.xyz = float3(tmpvar_12);
          color_1.w = 1;
          out_f.color = color_1;
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
  }
  FallBack Off
}