Shader "Custom/BufferStencil" {
       SubShader {
         // draw after all opaque objects (queue = 2001):
         Tags { "Queue"="Geometry+1" }
         Pass {
           Blend Zero One // keep the image be$$anonymous$$nd it
         }
       } 
         FallBack "Diffuse"
     }