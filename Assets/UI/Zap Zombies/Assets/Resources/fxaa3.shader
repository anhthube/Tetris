Shader "Hidden/FXAA3"
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
      uniform float4 _MainTex_TexelSize;
      uniform sampler2D _MainTex;
      uniform float4 _rcpFrameOpt;
      struct appdata_t
      {
          float4 vertex :POSITION;
          float4 texcoord :TEXCOORD0;
      };
      
      struct OUT_Data_Vert
      {
          float2 xlv_TEXCOORD0 :TEXCOORD0;
          float4 xlv_TEXCOORD1 :TEXCOORD1;
          float4 vertex :SV_POSITION;
      };
      
      struct v2f
      {
          float2 xlv_TEXCOORD0 :TEXCOORD0;
          float4 xlv_TEXCOORD1 :TEXCOORD1;
      };
      
      struct OUT_Data_Frag
      {
          float4 color :SV_Target0;
      };
      
      OUT_Data_Vert vert(appdata_t in_v)
      {
          OUT_Data_Vert out_v;
          float2 uv_1;
          float4 tmpvar_2;
          float4 tmpvar_3;
          tmpvar_3.w = 1;
          tmpvar_3.xyz = float3(in_v.vertex.xyz);
          float2 tmpvar_4;
          tmpvar_4 = in_v.texcoord.xy;
          uv_1 = tmpvar_4;
          float2 tmpvar_5;
          tmpvar_5.x = (-_MainTex_TexelSize.x);
          tmpvar_5.y = _MainTex_TexelSize.y;
          tmpvar_2.xy = float2((uv_1 + (tmpvar_5 * 0.5)));
          float2 tmpvar_6;
          tmpvar_6.x = _MainTex_TexelSize.x;
          tmpvar_6.y = (-_MainTex_TexelSize.y);
          tmpvar_2.zw = (uv_1 + (tmpvar_6 * 0.5));
          out_v.vertex = mul(unity_MatrixVP, mul(unity_ObjectToWorld, tmpvar_3));
          out_v.xlv_TEXCOORD0 = uv_1;
          out_v.xlv_TEXCOORD1 = tmpvar_2;
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      float4 impl_low_texture2DLodEXT(sampler2D sampler, float2 coord, float lod)
      {
          #if defined( GL_EXT_shader_texture_lod)
          {
              return tex2Dlod(sampler, float4(coord, lod));
              #else
              return tex2D(sampler, coord, lod);
              #endif
          }
      
          OUT_Data_Frag frag(v2f in_f)
          {
              float4 rgby2_1;
              float4 temp2N_2;
              float4 rgby1_3;
              float4 temp1N_4;
              float4 dir2_pos_5;
              float4 dir1_pos_6;
              float4 lumaSe_7;
              float4 lumaNw_8;
              float4 lumaSw_9;
              float4 lumaNe_10;
              float4 dir_11;
              dir_11.y = 0;
              float4 tmpvar_12;
              float4 tmpvar_13;
              tmpvar_13 = impl_low_texture2DLodEXT(_MainTex, in_f.xlv_TEXCOORD1.zy, 0);
              tmpvar_12 = tmpvar_13;
              float4 color_14;
              color_14.xyz = float3(tmpvar_12.xyz);
              color_14.w = dot(tmpvar_12.xyz, float3(0.299, 0.587, 0.114));
              lumaNe_10 = color_14;
              lumaNe_10.w = (lumaNe_10.w + 0.002604167);
              dir_11.x = (-lumaNe_10.w);
              dir_11.z = (-lumaNe_10.w);
              float4 tmpvar_15;
              float4 tmpvar_16;
              tmpvar_16 = impl_low_texture2DLodEXT(_MainTex, in_f.xlv_TEXCOORD1.xw, 0);
              tmpvar_15 = tmpvar_16;
              float4 color_17;
              color_17.xyz = float3(tmpvar_15.xyz);
              color_17.w = dot(tmpvar_15.xyz, float3(0.299, 0.587, 0.114));
              lumaSw_9 = color_17;
              dir_11.x = (dir_11.x + lumaSw_9.w);
              dir_11.z = (dir_11.z + lumaSw_9.w);
              float4 tmpvar_18;
              float4 tmpvar_19;
              tmpvar_19 = impl_low_texture2DLodEXT(_MainTex, in_f.xlv_TEXCOORD1.xy, 0);
              tmpvar_18 = tmpvar_19;
              float4 color_20;
              color_20.xyz = float3(tmpvar_18.xyz);
              color_20.w = dot(tmpvar_18.xyz, float3(0.299, 0.587, 0.114));
              lumaNw_8 = color_20;
              dir_11.x = (dir_11.x - lumaNw_8.w);
              dir_11.z = (dir_11.z + lumaNw_8.w);
              float4 tmpvar_21;
              float4 tmpvar_22;
              tmpvar_22 = impl_low_texture2DLodEXT(_MainTex, in_f.xlv_TEXCOORD1.zw, 0);
              tmpvar_21 = tmpvar_22;
              float4 color_23;
              color_23.xyz = float3(tmpvar_21.xyz);
              color_23.w = dot(tmpvar_21.xyz, float3(0.299, 0.587, 0.114));
              lumaSe_7 = color_23;
              dir_11.x = (dir_11.x + lumaSe_7.w);
              dir_11.z = (dir_11.z - lumaSe_7.w);
              float tmpvar_24;
              tmpvar_24 = dot(dir_11.xyz, dir_11.xyz);
              if((tmpvar_24<0.001))
              {
                  dir1_pos_6.xy = float2(0, 0);
              }
              else
              {
                  dir1_pos_6.xy = float2(normalize(dir_11.xyz).xz);
              }
              dir2_pos_5.xy = float2(clamp((dir1_pos_6.xy / (min(abs(dir1_pos_6.x), abs(dir1_pos_6.y)) * 8)), float2(-2, (-2)), float2(2, 2)));
              dir1_pos_6.zw = in_f.xlv_TEXCOORD0;
              dir2_pos_5.zw = in_f.xlv_TEXCOORD0;
              temp1N_4.xy = float2((dir1_pos_6.zw - (dir1_pos_6.xy * _rcpFrameOpt.zw)));
              float4 tmpvar_25;
              float4 tmpvar_26;
              tmpvar_26 = impl_low_texture2DLodEXT(_MainTex, temp1N_4.xy, 0);
              tmpvar_25 = tmpvar_26;
              float4 color_27;
              color_27.xyz = float3(tmpvar_25.xyz);
              color_27.w = dot(tmpvar_25.xyz, float3(0.299, 0.587, 0.114));
              temp1N_4 = color_27;
              rgby1_3.xy = float2((dir1_pos_6.zw + (dir1_pos_6.xy * _rcpFrameOpt.zw)));
              float4 tmpvar_28;
              float4 tmpvar_29;
              tmpvar_29 = impl_low_texture2DLodEXT(_MainTex, rgby1_3.xy, 0);
              tmpvar_28 = tmpvar_29;
              float4 color_30;
              color_30.xyz = float3(tmpvar_28.xyz);
              color_30.w = dot(tmpvar_28.xyz, float3(0.299, 0.587, 0.114));
              rgby1_3 = color_30;
              rgby1_3 = ((temp1N_4 + rgby1_3) * 0.5);
              temp2N_2.xy = float2((dir2_pos_5.zw - (dir2_pos_5.xy * _rcpFrameOpt.xy)));
              float4 tmpvar_31;
              float4 tmpvar_32;
              tmpvar_32 = impl_low_texture2DLodEXT(_MainTex, temp2N_2.xy, 0);
              tmpvar_31 = tmpvar_32;
              float4 color_33;
              color_33.xyz = float3(tmpvar_31.xyz);
              color_33.w = dot(tmpvar_31.xyz, float3(0.299, 0.587, 0.114));
              temp2N_2 = color_33;
              rgby2_1.xy = float2((dir2_pos_5.zw + (dir2_pos_5.xy * _rcpFrameOpt.xy)));
              float4 tmpvar_34;
              float4 tmpvar_35;
              tmpvar_35 = impl_low_texture2DLodEXT(_MainTex, rgby2_1.xy, 0);
              tmpvar_34 = tmpvar_35;
              float4 color_36;
              color_36.xyz = float3(tmpvar_34.xyz);
              color_36.w = dot(tmpvar_34.xyz, float3(0.299, 0.587, 0.114));
              rgby2_1 = color_36;
              rgby2_1 = ((temp2N_2 + rgby2_1) * 0.5);
              rgby2_1 = ((rgby2_1 + rgby1_3) * 0.5);
              int tmpvar_37;
              if((rgby2_1.w<min(min(lumaNw_8.w, lumaSw_9.w), min(lumaNe_10.w, lumaSe_7.w))))
              {
                  tmpvar_37 = 1;
              }
              else
              {
                  tmpvar_37 = 0;
              }
              int tmpvar_38;
              if((rgby2_1.w>max(max(lumaNw_8.w, lumaSw_9.w), max(lumaNe_10.w, lumaSe_7.w))))
              {
                  tmpvar_38 = 1;
              }
              else
              {
                  tmpvar_38 = 0;
              }
              if((tmpvar_37 || tmpvar_38))
              {
                  rgby2_1 = rgby1_3;
              }
              out_f.color = rgby2_1;
          }
      
      }
      
      
      ENDCG
      
    } // end phase
  }
  FallBack Off
}