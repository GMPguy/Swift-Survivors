// Unlit color shader. Very simple textured and colored shader.
// - no lighting
// - no lightmap support
// - per-material color

// Change this string to move shader to new location
Shader "Unlit/Texture Colored" {
    Properties {
        // Adds Color field we can modify
        _Color ("_TintColor", Color) = (1, 1, 1, 1)        
        _MainTex ("Base (RGB)", 2D) = "white" {}
    }

    SubShader {
        Tags { 
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        LOD 100
        
        // Enable blending & disable writing to depth to avoid sorting artifacts
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off

        Pass {
            Lighting Off

            SetTexture [_MainTex] {
                constantColor [_Color]
                // Multiply color and texture including alpha
                combine constant * texture
            }
        }
    }
}
