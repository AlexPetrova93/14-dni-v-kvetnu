Shader "CustomRenderTexture/PaintingRenderShader"
{
    Properties
    {
        // x, y = pos; z, w - size
        _BrushInfo ("Brush Info", Vector) = (0,0,0,0)
        //_PastBrushInfo ("Past Brush Info", Vector) = (0,0,0,0)
     }

     SubShader
     {
        Lighting Off
        Blend One Zero

        Pass
        {
            CGPROGRAM
            #include "UnityCustomRenderTexture.cginc"
            #pragma vertex CustomRenderTextureVertexShader
            #pragma fragment frag
            #pragma target 3.0

            float4      _BrushInfo;

            	inline float mix(float x, float y, float a)
	{
		return x*(1-a)+y*a;
	}

	  float2 random2(float2 st){
		st = float2( dot(st,float2(127.1,311.7)),
				  dot(st,float2(269.5,183.3)) );
		return -1.0 + 2.0*frac(sin(st)*43758.5453123);
	}
            
	// Gradient Noise by Inigo Quilez - iq/2013
	// https://www.shadertoy.com/view/XdXGW8
	  float gradientNoise(float2 st) {
		float2 i = floor(st);
		float2 f = frac(st);

		float2 u = f*f*(3.0-2.0*f);

		return mix( mix( dot( random2(i + float2(0.0,0.0) ), f - float2(0.0,0.0) ),
						 dot( random2(i + float2(1.0,0.0) ), f - float2(1.0,0.0) ), u.x),
					mix( dot( random2(i + float2(0.0,1.0) ), f - float2(0.0,1.0) ),
						 dot( random2(i + float2(1.0,1.0) ), f - float2(1.0,1.0) ), u.x), u.y);
	}
            
            float4 frag(v2f_customrendertexture IN) : COLOR
            {
                /*float2 xy = float2(_CustomRenderTextureWidth * IN.localTexcoord.x, _CustomRenderTextureHeight * IN.localTexcoord.y);
                float a = distance(_PastBrushInfo.xy, _BrushInfo.xy);
                float b = distance(_PastBrushInfo.xy, xy);
                float c = distance(_BrushInfo.xy, xy);
                float s = (a + b + c)/2;
                float dist = 2 * sqrt(s*(s-a)*(s-b)*(s-c)) / a;*/
                
                float prev = tex2D(_SelfTexture2D, IN.localTexcoord.xy);
                
                float x = _CustomRenderTextureWidth * IN.localTexcoord.x;
                float y = _CustomRenderTextureHeight * IN.localTexcoord.y;

                //float2 center = (_BrushInfo.xy + _PastBrushInfo.xy)/2;
                //float2 size = _BrushInfo.zw + distance(_BrushInfo.xy, _PastBrushInfo.xy);
                
                float xDist = x - _BrushInfo.x;
                float yDist = y - _BrushInfo.y;
                float centerDist = (xDist*xDist+yDist*yDist) / (_BrushInfo.z * _BrushInfo.w);

            	float rand = gradientNoise(IN.localTexcoord.xy * 200) - 0.5;
            	
                centerDist = saturate(centerDist+rand);
                float val = 1 - centerDist;      
                //float val = 1 - saturate(dist);          
                return prev + val;
            }
            ENDCG
            }
    }
}