Shader "Hidden/PostProcessChain"
{
  Properties
  {
    _MainTex ("Base (RGB)", 2D) = "white" {}
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
      uniform sampler2D _MainTex;
      struct appdata_t
      {
          float4 vertex :POSITION;
          float4 texcoord :TEXCOORD0;
      };
      
      struct OUT_Data_Vert
      {
          float2 xlv_TEXCOORD0 :TEXCOORD0;
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
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          float4 c_1;
          float4 tmpvar_2;
          tmpvar_2 = tex2D(_MainTex, in_f.xlv_TEXCOORD0);
          c_1 = tmpvar_2;
          c_1.xyz = float3((c_1.xyz * c_1.xyz));
          float _tmp_dvx_0 = sqrt(c_1.xyz);
          c_1.xyz = float3(_tmp_dvx_0, _tmp_dvx_0, _tmp_dvx_0);
          c_1.w = 1;
          out_f.color = c_1;
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
      uniform sampler2D _MainTex;
      uniform float4 _bloomParameter;
      struct appdata_t
      {
          float4 vertex :POSITION;
          float4 texcoord :TEXCOORD0;
      };
      
      struct OUT_Data_Vert
      {
          float4 xlv_TEXCOORD0 :TEXCOORD0;
          float4 xlv_TEXCOORD1 :TEXCOORD1;
          float4 vertex :SV_POSITION;
      };
      
      struct v2f
      {
          float4 xlv_TEXCOORD0 :TEXCOORD0;
          float4 xlv_TEXCOORD1 :TEXCOORD1;
      };
      
      struct OUT_Data_Frag
      {
          float4 color :SV_Target0;
      };
      
      OUT_Data_Vert vert(appdata_t in_v)
      {
          OUT_Data_Vert out_v;
          float4 tmpvar_1;
          float4 tmpvar_2;
          float4 tmpvar_3;
          tmpvar_3.w = 1;
          tmpvar_3.xyz = float3(in_v.vertex.xyz);
          tmpvar_1.xy = float2((in_v.texcoord.xy + _MainTex_TexelSize.xy));
          tmpvar_1.zw = (in_v.texcoord.xy - _MainTex_TexelSize.xy);
          tmpvar_2.xy = float2((in_v.texcoord.xy + (_MainTex_TexelSize.xy * float2(1, (-1)))));
          tmpvar_2.zw = (in_v.texcoord.xy + (_MainTex_TexelSize.xy * float2(-1, 1)));
          out_v.vertex = mul(unity_MatrixVP, mul(unity_ObjectToWorld, tmpvar_3));
          out_v.xlv_TEXCOORD0 = tmpvar_1;
          out_v.xlv_TEXCOORD1 = tmpvar_2;
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          float3 C3_1;
          float3 C2_2;
          float3 C1_3;
          float3 C0_4;
          float3 tmpvar_5;
          tmpvar_5 = tex2D(_MainTex, in_f.xlv_TEXCOORD0.xy).xyz.xyz;
          C0_4 = tmpvar_5;
          float3 tmpvar_6;
          tmpvar_6 = tex2D(_MainTex, in_f.xlv_TEXCOORD0.zw).xyz.xyz;
          C1_3 = tmpvar_6;
          float3 tmpvar_7;
          tmpvar_7 = tex2D(_MainTex, in_f.xlv_TEXCOORD1.xy).xyz.xyz;
          C2_2 = tmpvar_7;
          float3 tmpvar_8;
          tmpvar_8 = tex2D(_MainTex, in_f.xlv_TEXCOORD1.zw).xyz.xyz;
          C3_1 = tmpvar_8;
          C0_4 = (C0_4 * C0_4);
          C1_3 = (C1_3 * C1_3);
          C2_2 = (C2_2 * C2_2);
          C3_1 = (C3_1 * C3_1);
          float3 tmpvar_9;
          tmpvar_9 = max(((((C0_4 * 0.25) + (C1_3 * 0.25)) + (C2_2 * 0.25)) + (C3_1 * 0.25)), float3(0, 0, 0));
          float3 rgb_10;
          rgb_10 = tmpvar_9;
          float tmpvar_11;
          tmpvar_11 = dot(rgb_10, float3(0.22, 0.707, 0.071));
          float3 color_12;
          color_12 = (tmpvar_9 * clamp(((tmpvar_11 - _bloomParameter.y) * _bloomParameter.z), 0, 1));
          float4 RGBT_13;
          float tmpvar_14;
          tmpvar_14 = min(max(max(color_12.x, color_12.y), max(color_12.z, 1E-06)), 4);
          RGBT_13.w = ((1.25 * tmpvar_14) / (1 + tmpvar_14));
          RGBT_13.w = (ceil((RGBT_13.w * 255)) / 255);
          RGBT_13.xyz = float3((color_12 / (RGBT_13.w / (1.25 - RGBT_13.w))));
          out_f.color = RGBT_13;
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
      uniform float4 _bloomParameter;
      uniform float4 _MainTex_TexelSize;
      uniform sampler2D _MainTex;
      struct appdata_t
      {
          float4 vertex :POSITION;
          float4 texcoord :TEXCOORD0;
      };
      
      struct OUT_Data_Vert
      {
          float4 xlv_TEXCOORD0 :TEXCOORD0;
          float4 xlv_TEXCOORD1 :TEXCOORD1;
          float4 xlv_TEXCOORD2 :TEXCOORD2;
          float4 xlv_TEXCOORD3 :TEXCOORD3;
          float4 xlv_TEXCOORD4 :TEXCOORD4;
          float4 xlv_TEXCOORD5 :TEXCOORD5;
          float4 xlv_TEXCOORD6 :TEXCOORD6;
          float2 xlv_TEXCOORD7 :TEXCOORD7;
          float4 vertex :SV_POSITION;
      };
      
      struct v2f
      {
          float4 xlv_TEXCOORD0 :TEXCOORD0;
          float4 xlv_TEXCOORD1 :TEXCOORD1;
          float4 xlv_TEXCOORD2 :TEXCOORD2;
          float4 xlv_TEXCOORD3 :TEXCOORD3;
          float4 xlv_TEXCOORD4 :TEXCOORD4;
          float4 xlv_TEXCOORD5 :TEXCOORD5;
          float4 xlv_TEXCOORD6 :TEXCOORD6;
          float2 xlv_TEXCOORD7 :TEXCOORD7;
      };
      
      struct OUT_Data_Frag
      {
          float4 color :SV_Target0;
      };
      
      OUT_Data_Vert vert(appdata_t in_v)
      {
          OUT_Data_Vert out_v;
          float4 tmpvar_1;
          float4 tmpvar_2;
          float4 tmpvar_3;
          float4 tmpvar_4;
          float4 tmpvar_5;
          float4 tmpvar_6;
          float4 tmpvar_7;
          float2 tmpvar_8;
          float4 tmpvar_9;
          tmpvar_9.w = 1;
          tmpvar_9.xyz = float3(in_v.vertex.xyz);
          tmpvar_1.xy = float2(in_v.texcoord.xy);
          float2 tmpvar_10;
          tmpvar_10 = (_bloomParameter.x * _MainTex_TexelSize.xy);
          tmpvar_1.zw = (in_v.texcoord.xy + (float2(0.04271346, 0.6652969) * tmpvar_10));
          tmpvar_2.xy = float2((in_v.texcoord.xy + (float2(0.327145, 0.5808792) * tmpvar_10)));
          tmpvar_2.zw = (in_v.texcoord.xy + (float2(0.5467815, 0.3814112) * tmpvar_10));
          tmpvar_3.xy = float2((in_v.texcoord.xy + (float2(0.6581212, 0.1064001) * tmpvar_10)));
          tmpvar_3.zw = (in_v.texcoord.xy + (float2(0.639112, (-0.1896849)) * tmpvar_10));
          tmpvar_4.xy = float2((in_v.texcoord.xy + (float2(0.4935188, (-0.4482004)) * tmpvar_10)));
          tmpvar_4.zw = (in_v.texcoord.xy + (float2(0.2501782, (-0.6179444)) * tmpvar_10));
          tmpvar_5.xy = float2((in_v.texcoord.xy + (float2(-0.0427132, (-0.665297)) * tmpvar_10)));
          tmpvar_5.zw = (in_v.texcoord.xy + (float2(-0.3271449, (-0.5808792)) * tmpvar_10));
          tmpvar_6.xy = float2((in_v.texcoord.xy + (float2(-0.5467814, (-0.3814113)) * tmpvar_10)));
          tmpvar_6.zw = (in_v.texcoord.xy + (float2(-0.6581211, (-0.1064003)) * tmpvar_10));
          tmpvar_7.xy = float2((in_v.texcoord.xy + (float2(-0.639112, 0.1896848) * tmpvar_10)));
          tmpvar_7.zw = (in_v.texcoord.xy + (float2(-0.493519, 0.4482002) * tmpvar_10));
          tmpvar_8 = (in_v.texcoord.xy + (float2(-0.2501783, 0.6179444) * tmpvar_10));
          out_v.vertex = mul(unity_MatrixVP, mul(unity_ObjectToWorld, tmpvar_9));
          out_v.xlv_TEXCOORD0 = tmpvar_1;
          out_v.xlv_TEXCOORD1 = tmpvar_2;
          out_v.xlv_TEXCOORD2 = tmpvar_3;
          out_v.xlv_TEXCOORD3 = tmpvar_4;
          out_v.xlv_TEXCOORD4 = tmpvar_5;
          out_v.xlv_TEXCOORD5 = tmpvar_6;
          out_v.xlv_TEXCOORD6 = tmpvar_7;
          out_v.xlv_TEXCOORD7 = tmpvar_8;
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          float4 tmpvar_1;
          tmpvar_1 = tex2D(_MainTex, in_f.xlv_TEXCOORD0.xy);
          float4 encodedhdr_2;
          encodedhdr_2 = tmpvar_1;
          float4 RGBT_3;
          RGBT_3.xyz = float3(encodedhdr_2.xyz);
          RGBT_3.w = (encodedhdr_2.w / (1.25 - encodedhdr_2.w));
          float4 tmpvar_4;
          tmpvar_4 = tex2D(_MainTex, in_f.xlv_TEXCOORD0.zw);
          float4 encodedhdr_5;
          encodedhdr_5 = tmpvar_4;
          float4 RGBT_6;
          RGBT_6.xyz = float3(encodedhdr_5.xyz);
          RGBT_6.w = (encodedhdr_5.w / (1.25 - encodedhdr_5.w));
          float4 tmpvar_7;
          tmpvar_7 = tex2D(_MainTex, in_f.xlv_TEXCOORD1.xy);
          float4 encodedhdr_8;
          encodedhdr_8 = tmpvar_7;
          float4 RGBT_9;
          RGBT_9.xyz = float3(encodedhdr_8.xyz);
          RGBT_9.w = (encodedhdr_8.w / (1.25 - encodedhdr_8.w));
          float4 tmpvar_10;
          tmpvar_10 = tex2D(_MainTex, in_f.xlv_TEXCOORD1.zw);
          float4 encodedhdr_11;
          encodedhdr_11 = tmpvar_10;
          float4 RGBT_12;
          RGBT_12.xyz = float3(encodedhdr_11.xyz);
          RGBT_12.w = (encodedhdr_11.w / (1.25 - encodedhdr_11.w));
          float4 tmpvar_13;
          tmpvar_13 = tex2D(_MainTex, in_f.xlv_TEXCOORD2.xy);
          float4 encodedhdr_14;
          encodedhdr_14 = tmpvar_13;
          float4 RGBT_15;
          RGBT_15.xyz = float3(encodedhdr_14.xyz);
          RGBT_15.w = (encodedhdr_14.w / (1.25 - encodedhdr_14.w));
          float4 tmpvar_16;
          tmpvar_16 = tex2D(_MainTex, in_f.xlv_TEXCOORD2.zw);
          float4 encodedhdr_17;
          encodedhdr_17 = tmpvar_16;
          float4 RGBT_18;
          RGBT_18.xyz = float3(encodedhdr_17.xyz);
          RGBT_18.w = (encodedhdr_17.w / (1.25 - encodedhdr_17.w));
          float4 tmpvar_19;
          tmpvar_19 = tex2D(_MainTex, in_f.xlv_TEXCOORD3.xy);
          float4 encodedhdr_20;
          encodedhdr_20 = tmpvar_19;
          float4 RGBT_21;
          RGBT_21.xyz = float3(encodedhdr_20.xyz);
          RGBT_21.w = (encodedhdr_20.w / (1.25 - encodedhdr_20.w));
          float4 tmpvar_22;
          tmpvar_22 = tex2D(_MainTex, in_f.xlv_TEXCOORD3.zw);
          float4 encodedhdr_23;
          encodedhdr_23 = tmpvar_22;
          float4 RGBT_24;
          RGBT_24.xyz = float3(encodedhdr_23.xyz);
          RGBT_24.w = (encodedhdr_23.w / (1.25 - encodedhdr_23.w));
          float4 tmpvar_25;
          tmpvar_25 = tex2D(_MainTex, in_f.xlv_TEXCOORD4.xy);
          float4 encodedhdr_26;
          encodedhdr_26 = tmpvar_25;
          float4 RGBT_27;
          RGBT_27.xyz = float3(encodedhdr_26.xyz);
          RGBT_27.w = (encodedhdr_26.w / (1.25 - encodedhdr_26.w));
          float4 tmpvar_28;
          tmpvar_28 = tex2D(_MainTex, in_f.xlv_TEXCOORD4.zw);
          float4 encodedhdr_29;
          encodedhdr_29 = tmpvar_28;
          float4 RGBT_30;
          RGBT_30.xyz = float3(encodedhdr_29.xyz);
          RGBT_30.w = (encodedhdr_29.w / (1.25 - encodedhdr_29.w));
          float4 tmpvar_31;
          tmpvar_31 = tex2D(_MainTex, in_f.xlv_TEXCOORD5.xy);
          float4 encodedhdr_32;
          encodedhdr_32 = tmpvar_31;
          float4 RGBT_33;
          RGBT_33.xyz = float3(encodedhdr_32.xyz);
          RGBT_33.w = (encodedhdr_32.w / (1.25 - encodedhdr_32.w));
          float4 tmpvar_34;
          tmpvar_34 = tex2D(_MainTex, in_f.xlv_TEXCOORD5.zw);
          float4 encodedhdr_35;
          encodedhdr_35 = tmpvar_34;
          float4 RGBT_36;
          RGBT_36.xyz = float3(encodedhdr_35.xyz);
          RGBT_36.w = (encodedhdr_35.w / (1.25 - encodedhdr_35.w));
          float4 tmpvar_37;
          tmpvar_37 = tex2D(_MainTex, in_f.xlv_TEXCOORD6.xy);
          float4 encodedhdr_38;
          encodedhdr_38 = tmpvar_37;
          float4 RGBT_39;
          RGBT_39.xyz = float3(encodedhdr_38.xyz);
          RGBT_39.w = (encodedhdr_38.w / (1.25 - encodedhdr_38.w));
          float4 tmpvar_40;
          tmpvar_40 = tex2D(_MainTex, in_f.xlv_TEXCOORD6.zw);
          float4 encodedhdr_41;
          encodedhdr_41 = tmpvar_40;
          float4 RGBT_42;
          RGBT_42.xyz = float3(encodedhdr_41.xyz);
          RGBT_42.w = (encodedhdr_41.w / (1.25 - encodedhdr_41.w));
          float4 tmpvar_43;
          tmpvar_43 = tex2D(_MainTex, in_f.xlv_TEXCOORD7);
          float4 encodedhdr_44;
          encodedhdr_44 = tmpvar_43;
          float4 RGBT_45;
          RGBT_45.xyz = float3(encodedhdr_44.xyz);
          RGBT_45.w = (encodedhdr_44.w / (1.25 - encodedhdr_44.w));
          float3 tmpvar_46;
          tmpvar_46 = ((((((((((((((((encodedhdr_2.xyz * RGBT_3.w) * 0.06666667) + ((encodedhdr_5.xyz * RGBT_6.w) * 0.06666667)) + ((encodedhdr_8.xyz * RGBT_9.w) * 0.06666667)) + ((encodedhdr_11.xyz * RGBT_12.w) * 0.06666667)) + ((encodedhdr_14.xyz * RGBT_15.w) * 0.06666667)) + ((encodedhdr_17.xyz * RGBT_18.w) * 0.06666667)) + ((encodedhdr_20.xyz * RGBT_21.w) * 0.06666667)) + ((encodedhdr_23.xyz * RGBT_24.w) * 0.06666667)) + ((encodedhdr_26.xyz * RGBT_27.w) * 0.06666667)) + ((encodedhdr_29.xyz * RGBT_30.w) * 0.06666667)) + ((encodedhdr_32.xyz * RGBT_33.w) * 0.06666667)) + ((encodedhdr_35.xyz * RGBT_36.w) * 0.06666667)) + ((encodedhdr_38.xyz * RGBT_39.w) * 0.06666667)) + ((encodedhdr_41.xyz * RGBT_42.w) * 0.06666667)) + ((encodedhdr_44.xyz * RGBT_45.w) * 0.06666667));
          float4 RGBT_47;
          float tmpvar_48;
          tmpvar_48 = min(max(max(tmpvar_46.x, tmpvar_46.y), max(tmpvar_46.z, 1E-06)), 4);
          RGBT_47.w = ((1.25 * tmpvar_48) / (1 + tmpvar_48));
          RGBT_47.w = (ceil((RGBT_47.w * 255)) / 255);
          RGBT_47.xyz = float3((tmpvar_46 / (RGBT_47.w / (1.25 - RGBT_47.w))));
          out_f.color = RGBT_47;
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
  }
  FallBack Off
}