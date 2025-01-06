Shader "Custom/AlbedoNormalOcclusionWithSelectableTransparency"
{
    Properties
    {
        _MainTex ("Albedo (RGB)", 2D) = "white" { }
        _NormalMap ("Normal Map", 2D) = "bump" { }
        _OcclusionMap ("Occlusion Map", 2D) = "white" { }
        _Transparency ("Transparency", Range(0, 1)) = 1.0 // Trasparenza controllata dallo slider
        _TransparentSide ("Transparent Side", Range(0, 1)) = 1.0 // 0 = Sinistra, 1 = Destra
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        Pass
        {
            // Attiviamo il blending per la trasparenza
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite On
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            // Proprietà del materiale
            sampler2D _MainTex;
            sampler2D _NormalMap;
            sampler2D _OcclusionMap;
            float _Transparency; // Trasparenza controllata dallo slider
            float _TransparentSide; // Seleziona il lato da rendere trasparente (0 = Sinistra, 1 = Destra)
            float4 _CameraWorldPos; // Posizione della telecamera nel mondo

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 color : COLOR;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 worldPos : TEXCOORD1; // Posizione nel mondo
            };

            // Funzione vertex
            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex); // Trasformiamo il vertice in coordinate clip
                o.uv = v.uv; // Passiamo le coordinate UV al fragment shader
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz; // Posizione nel mondo
                return o;
            }

            // Funzione fragment con trasparenza basata sulla posizione rispetto alla telecamera
            half4 frag(v2f i) : SV_Target
            {
                // Calcoliamo la posizione rispetto alla fotocamera
                float cameraToVertexX = i.worldPos.x - _CameraWorldPos.x;

                // Campioniamo le texture
                half4 albedo = tex2D(_MainTex, i.uv); // Albedo (Colore principale)
                half3 normal = tex2D(_NormalMap, i.uv).rgb * 2.0 - 1.0; // Normal Map (Convertito in [ -1, 1 ])
                half occlusion = tex2D(_OcclusionMap, i.uv).r; // Occlusion Map (Solo canale rosso)

                // Calcoliamo il colore finale
                half4 finalColor = albedo;
                finalColor.rgb *= occlusion; // Applichiamo l'occlusione

                // Aggiungiamo la trasparenza solo sulla parte selezionata
                if ((_TransparentSide == 1.0 && cameraToVertexX > 0) || (_TransparentSide == 0.0 && cameraToVertexX < 0))
                {
                    finalColor.a *= _Transparency; // Applichiamo la trasparenza
                }

                return finalColor;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
