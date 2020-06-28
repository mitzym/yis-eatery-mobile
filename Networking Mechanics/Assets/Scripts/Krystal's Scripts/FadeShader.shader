Shader "Custom/FadeToColour"
{
     Properties
     {
        _MainTex ("Texture", 2D) = "white" {}
        _colorToFade("Fade to Color", Color) = (1,1,1,1)
        _useLightingInFade("Use Lighting in Fade", Int) = 1
        _colorModifier("Color Modifier", Color) = (0.5,0.5,0.5,0.5)
        _fadeDepth("Fade Depth", Float) = 1.0
        _fadeStart("Fade Start Y", Float) = 0.0
     }
 
     SubShader
     {
        Tags {"Queue"="Geometry" "LightMode"="ForwardBase"}
        LOD 200
 
           Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            CGPROGRAM
 
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
 
            sampler2D _MainTex;
 
            half4 _colorToFade;
            half4 _colorModifier;
            float _fadeDepth;
            float _fadeStart;
            fixed _useLightingInFade;
 
            struct v2f
            {
                float4 position : POSITION;
                float2 uv : TEXCOORD0;
                float4 wPos : TEXCOORD1;
                float4 col : COLOR0;
                float3 normal : NORMAL;
            };
            v2f vert (v2f v)
            {
                v2f o;
                // set Wpos to be vertex world postion for y checking
                o.wPos = mul (unity_ObjectToWorld, v.position);
 
                o.position = UnityObjectToClipPos(v.position);
                o.uv = v.uv;
                o.normal = v.normal;
         
                //lighting - basic vertex shading with one light, no shadows
                float4x4 modelMatrixInverse = unity_WorldToObject;
                float3 normalDirection = normalize(float3(mul(float4(v.normal, 0.0), modelMatrixInverse).xyz));
                float3 lightDirection = normalize(float3(_WorldSpaceLightPos0.xyz));
                float3 diffuseReflection = float3(_LightColor0.xyz) * max(0.0, dot(normalDirection, lightDirection));
                o.col = float4(diffuseReflection, 1.0) + UNITY_LIGHTMODEL_AMBIENT;
 
                //return the v2f output structure
                return o;
            }
 
            half4 frag (v2f i) : SV_Target
            {
                //get the texture colour
                half4 texel = tex2D (_MainTex, i.uv);
                //get the world height
                half vertHeight = i.wPos.y;
                //fade if below fade height
                if (vertHeight < _fadeStart)
                {
                    //work out how deep we are in the fadeDepth for use in lerp
                    half vertDepth = abs(_fadeStart - vertHeight);
                    //calculate the colour tint - 50% gray = no tint
                    half4 tint = _colorModifier - (0.5,0.5,0.5,0.5);
 
                    if (_useLightingInFade > 0)
                        //lerp between texture color and complete fade depending on depth - with lighting.
                        return (lerp (_colorToFade, (texel * i.col ) + tint, saturate ((_fadeDepth-vertDepth)/_fadeDepth))) ;
                    else
                        //or ignore lighting - flat shade the Fade
                        return (lerp (_colorToFade, texel + tint, saturate ((_fadeDepth-vertDepth)/_fadeDepth))) ;
                }
                else
                {
                    //otherwise just return the texture colour affected by lighting
                    return texel * i.col;
                }
            }
            ENDCG
        }
     }
     Fallback "Diffuse"
}