// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "SharpenShader"
{
	Properties
	{
		_MainTex("MainTex", 2D) = "white" {}
		[IntRange]_Directions("Directions", Range( 1 , 50)) = 0
		[IntRange]_Quality("Quality", Range( 1 , 50)) = 0
		_Size("Size", Range( 0 , 3)) = 0
		_Strength("Strength", Range( 0 , 10)) = 1
		[HideInInspector] _texcoord( "", 2D ) = "white" {}

	}
	
	SubShader
	{
		
		
		Tags { "RenderType"="Opaque" }
	LOD 100

		CGINCLUDE
		#pragma target 3.0
		ENDCG
		Blend Off
		AlphaToMask Off
		Cull Back
		ColorMask RGBA
		ZWrite On
		ZTest LEqual
		Offset 0 , 0
		
		
		
		Pass
		{
			Name "Unlit"
			Tags { "LightMode"="ForwardBase" }
			CGPROGRAM

			

			#ifndef UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX
			//only defining to not throw compilation error over Unity 5.5
			#define UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input)
			#endif
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_instancing
			#include "UnityCG.cginc"
			

			struct appdata
			{
				float4 vertex : POSITION;
				float4 color : COLOR;
				float4 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			
			struct v2f
			{
				float4 vertex : SV_POSITION;
				#ifdef ASE_NEEDS_FRAG_WORLD_POSITION
				float3 worldPos : TEXCOORD0;
				#endif
				float4 ase_texcoord1 : TEXCOORD1;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			uniform sampler2D _MainTex;
			uniform float4 _MainTex_ST;
			uniform float _Directions;
			uniform float _Quality;
			uniform float _Size;
			float4 _MainTex_TexelSize;
			uniform float _Strength;
			float4 GaussianBlur19( sampler2D tex, float Directions, float Quality, float Size, float2 iResolution, float2 UV0, out float4 Col )
			{
				float Pi = 6.28318530718; 
				    
				   
				    float2 Radius = Size/iResolution.xy;
				    
				    float2 UW;
				UW = UV0;
				   // float4 Col;
				Col;
				    
				    // Blur calculations
				    for( float d=0.0; d<Pi; d+=Pi/Directions)
				    {
						for(float i=1.0/Quality; i<=1.0; i+=1.0/Quality)
				        {
							Col += tex2D( tex, UW+float2(cos(d),sin(d))*Radius*i);		
				        }
				    }
				    
				    Col /= Quality * Directions ;
				    return  Col;
			}
			

			
			v2f vert ( appdata v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				UNITY_TRANSFER_INSTANCE_ID(v, o);

				o.ase_texcoord1.xy = v.ase_texcoord.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord1.zw = 0;
				float3 vertexValue = float3(0, 0, 0);
				#if ASE_ABSOLUTE_VERTEX_POS
				vertexValue = v.vertex.xyz;
				#endif
				vertexValue = vertexValue;
				#if ASE_ABSOLUTE_VERTEX_POS
				v.vertex.xyz = vertexValue;
				#else
				v.vertex.xyz += vertexValue;
				#endif
				o.vertex = UnityObjectToClipPos(v.vertex);

				#ifdef ASE_NEEDS_FRAG_WORLD_POSITION
				o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				#endif
				return o;
			}
			
			fixed4 frag (v2f i ) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID(i);
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);
				fixed4 finalColor;
				#ifdef ASE_NEEDS_FRAG_WORLD_POSITION
				float3 WorldPosition = i.worldPos;
				#endif
				float2 uv_MainTex = i.ase_texcoord1.xy * _MainTex_ST.xy + _MainTex_ST.zw;
				sampler2D tex19 = _MainTex;
				float Directions19 = _Directions;
				float Quality19 = _Quality;
				float Size19 = _Size;
				float2 appendResult13 = (float2(_MainTex_TexelSize.z , _MainTex_TexelSize.w));
				float2 iResolution19 = appendResult13;
				float2 UV019 = uv_MainTex;
				float4 Col19 = float4( 0,0,0,0 );
				float4 localGaussianBlur19 = GaussianBlur19( tex19 , Directions19 , Quality19 , Size19 , iResolution19 , UV019 , Col19 );
				float4 tex2DNode24 = tex2D( _MainTex, uv_MainTex );
				float4 break28 = ( ( ( tex2D( _MainTex, uv_MainTex ) - Col19 ) * _Strength ) + tex2DNode24 );
				float4 appendResult29 = (float4(break28.r , break28.g , break28.b , tex2DNode24.a));
				
				
				finalColor = appendResult29;
				return finalColor;
			}
			ENDCG
		}
	}
	CustomEditor "ASEMaterialInspector"
	
	
}
/*ASEBEGIN
Version=18900
2301;848;2033;835;1173.198;628.058;1.3;True;True
Node;AmplifyShaderEditor.TexturePropertyNode;3;-808.5,-154.5;Inherit;True;Property;_MainTex;MainTex;0;0;Create;True;0;0;0;False;0;False;None;None;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.TexelSizeNode;12;-722.876,479.8841;Inherit;False;-1;1;0;SAMPLER2D;;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;15;-184.5123,-422.1318;Inherit;False;Property;_Quality;Quality;5;1;[IntRange];Create;True;0;0;0;False;0;False;0;5;1;50;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;16;-146.5123,-500.1318;Inherit;False;Property;_Directions;Directions;4;1;[IntRange];Create;True;0;0;0;False;0;False;0;4;1;50;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;14;-234.5122,-336.1318;Inherit;False;Property;_Size;Size;6;0;Create;True;0;0;0;False;0;False;0;0.58;0;3;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;13;-430.976,486.684;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;18;-221.6642,-80.13715;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;20;12.09473,69.72076;Inherit;True;Property;_TextureSample0;Texture Sample 0;7;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CustomExpressionNode;19;189.297,-252.4536;Inherit;False;float Pi = 6.28318530718@ $    $$   $    float2 Radius = Size/iResolution.xy@$    $$    float2 UW@$UW = UV0@$   // float4 Col@$Col@$    $    // Blur calculations$    for( float d=0.0@ d<Pi@ d+=Pi/Directions)$    {$		for(float i=1.0/Quality@ i<=1.0@ i+=1.0/Quality)$        {$			Col += tex2D( tex, UW+float2(cos(d),sin(d))*Radius*i)@		$        }$    }$    $$    Col /= Quality * Directions @$    return  Col@;4;False;7;True;tex;SAMPLER2D;;In;;Float;False;True;Directions;FLOAT;16;In;;Float;False;True;Quality;FLOAT;4;In;;Float;False;True;Size;FLOAT;10;In;;Float;False;True;iResolution;FLOAT2;1,1;In;;Float;False;True;UV0;FLOAT2;0,0;In;;Float;False;True;Col;FLOAT4;0,0,0,0;Out;;Float;False;GaussianBlur;True;False;0;7;0;SAMPLER2D;;False;1;FLOAT;16;False;2;FLOAT;4;False;3;FLOAT;10;False;4;FLOAT2;1,1;False;5;FLOAT2;0,0;False;6;FLOAT4;0,0,0,0;False;2;FLOAT4;0;FLOAT4;7
Node;AmplifyShaderEditor.RangedFloatNode;27;494.3879,66.56531;Inherit;False;Property;_Strength;Strength;7;0;Create;True;0;0;0;False;0;False;1;10;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;23;524.5585,-103.2558;Inherit;False;2;0;COLOR;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;26;746.6875,-83.53459;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;24;345.5878,325.6654;Inherit;True;Property;_TextureSample1;Texture Sample 1;7;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;25;1036.988,62.86537;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.BreakToComponentsNode;28;1034.201,235.1417;Inherit;False;COLOR;1;0;COLOR;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.TextureCoordinatesNode;5;-681.5,91.5;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;10;-377.8761,248.8841;Inherit;False;3;3;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;29;1235.701,285.8417;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;11;-612.8761,360.8841;Inherit;False;Property;_Float0;Float 0;3;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;9;-610.5,225.5;Inherit;False;Property;_BlurDirection;BlurDirection;2;0;Create;True;0;0;0;False;0;False;0,0;1,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.CustomExpressionNode;1;-117.3388,316.5435;Float;False;float fade@$$for(float i=1@ i<=Iterations@ i++)${ $fade = 1 - (i/Iterations)@$finalColor =  tex2D( tex, uv_Texture0 + (OffsetX * i) - (((Iterations) * OffsetX)/2)) + finalColor@$}$$$finalColor = finalColor/Iterations@			return finalColor@;4;False;6;True;tex;SAMPLER2D;sampler02;In;;Float;False;True;uv_Texture0;FLOAT2;0,0;In;;Float;False;True;Iterations;FLOAT;2;In;;Float;False;True;OffsetX;FLOAT2;0,0;In;;Float;False;True;noise;SAMPLER2D;_Sampler41;In;;Float;False;True;finalColor;FLOAT4;0,0,0,0;Out;;Float;False;Blur;True;False;0;6;0;SAMPLER2D;sampler02;False;1;FLOAT2;0,0;False;2;FLOAT;2;False;3;FLOAT2;0,0;False;4;SAMPLER2D;_Sampler41;False;5;FLOAT4;0,0,0,0;False;2;FLOAT4;0;FLOAT4;6
Node;AmplifyShaderEditor.RangedFloatNode;6;-558.5,-29.5;Inherit;False;Property;_Iterations;Iterations;1;0;Create;True;0;0;0;False;0;False;5;15;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;0;1330.965,41.96555;Float;False;True;-1;2;ASEMaterialInspector;100;1;SharpenShader;0770190933193b94aaa3065e307002fa;True;Unlit;0;0;Unlit;2;False;True;0;1;False;-1;0;False;-1;0;1;False;-1;0;False;-1;True;0;False;-1;0;False;-1;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;True;0;False;-1;False;True;True;True;True;True;0;False;-1;False;False;False;False;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;False;True;1;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;1;RenderType=Opaque=RenderType;True;2;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;LightMode=ForwardBase;False;0;;0;0;Standard;1;Vertex Position,InvertActionOnDeselection;1;0;1;True;False;;False;0
WireConnection;12;0;3;0
WireConnection;13;0;12;3
WireConnection;13;1;12;4
WireConnection;18;2;3;0
WireConnection;20;0;3;0
WireConnection;19;0;3;0
WireConnection;19;1;16;0
WireConnection;19;2;15;0
WireConnection;19;3;14;0
WireConnection;19;4;13;0
WireConnection;19;5;18;0
WireConnection;23;0;20;0
WireConnection;23;1;19;7
WireConnection;26;0;23;0
WireConnection;26;1;27;0
WireConnection;24;0;3;0
WireConnection;25;0;26;0
WireConnection;25;1;24;0
WireConnection;28;0;25;0
WireConnection;5;2;3;0
WireConnection;10;0;9;0
WireConnection;10;1;11;0
WireConnection;10;2;13;0
WireConnection;29;0;28;0
WireConnection;29;1;28;1
WireConnection;29;2;28;2
WireConnection;29;3;24;4
WireConnection;1;0;3;0
WireConnection;1;1;5;0
WireConnection;1;2;6;0
WireConnection;1;3;10;0
WireConnection;0;0;29;0
ASEEND*/
//CHKSM=0B1C954CC88415C4470E2D4507EF715291AC8790