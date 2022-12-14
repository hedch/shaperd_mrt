// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/cube_shader" {
	Properties{
		_LineColor("Line Color", Color) = (1,1,1,1)
		_GridColor("Grid Color", Color) = (1,1,1,0)
		_LineWidth("Line Width", float) = 0.1
	}
		
	SubShader{

		pass {
			Tags { "RenderType" = "Transparent" }
			Blend SrcAlpha OneMinusSrcAlpha
			Cull Front

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
		    
			uniform float4 _LineColor;
			uniform float4 _GridColor;
			uniform float _LineWidth;

			struct appdata {
				float4 vertex : POSITION;
				float4 texcoord1 : TEXCOORD0;
				float4 color : COLOR;

			};

			struct v2f {
				float4 pos : POSITION;
				float4 texcoord1 : TEXCOORD0;
				float4 color : COLOR;
			};

			v2f vert(appdata v) {
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.texcoord1 = v.texcoord1;
				o.color = v.color;
				return o;
			}

			fixed4 frag(v2f i) : COLOR {
				fixed4 answer;
				float lx = step(_LineWidth, i.texcoord1.x);
				float ly = step(_LineWidth, i.texcoord1.y);
				float hx = step(i.texcoord1.x, 1.0 - _LineWidth);
				float hy = step(i.texcoord1.y, 1.0 - _LineWidth);
				answer = lerp(_LineColor, _GridColor, lx*ly*hx*hy);
				return answer;
			}
			
			ENDCG

		}

	    pass {
		
			Tags{ "RenderType" = "Transparent" }    
			Blend SrcAlpha OneMinusSrcAlpha//这句可以注释掉，能够避免线框太粗出现的模糊效果。    
			//AlphaTest Greater 0.5    
			Cull Back//only this is different         
			CGPROGRAM    
            #pragma vertex vert    
            #pragma fragment frag         
			uniform float4 _LineColor;    
			uniform float4 _GridColor;    
			uniform float _LineWidth;         
			struct appdata {    
				float4 vertex : POSITION;    
				float4 texcoord1 : TEXCOORD0;    
				float4 color : COLOR;    
			};         
			struct v2f {    
				float4 pos : POSITION;    
				float4 texcoord1 : TEXCOORD0;    
				float4 color : COLOR;    
			};         
			v2f vert (appdata v) {    
				v2f o;    
				o.pos = UnityObjectToClipPos( v.vertex);    
				o.texcoord1 = v.texcoord1;    
				o.color = v.color;    
				return o;    
			}         
			fixed4 frag(v2f i) : COLOR {    
				fixed4 answer;         
			    float lx = step(_LineWidth, i.texcoord1.x);    
				float ly = step(_LineWidth, i.texcoord1.y);    
				float hx = step(i.texcoord1.x, 1.0 - _LineWidth);    
				float hy = step(i.texcoord1.y, 1.0 - _LineWidth);         
				answer = lerp(_LineColor, _GridColor, lx*ly*hx*hy);         
				return answer;    
			}    
			
			ENDCG
			
		
		
		}
	}
	Fallback "Vertex Colored", 1
}
