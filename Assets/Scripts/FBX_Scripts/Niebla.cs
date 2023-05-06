using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Niebla : MonoBehaviour
{
    public float speed = 1.0f;   // Velocidad de la animación de las nubes
    public float density = 0.5f; // Densidad de la niebla
    public float maxDistance = 500.0f; // Distancia máxima en la que la niebla es visible

    private float noiseOffset = 0.0f;  // Offset para el ruido de la textura
    private float baseDensity = 0.0f;  // Densidad base de la niebla
    private float baseStart = 0.0f;    // Distancia de inicio base de la niebla
    public Gradient FogColorGradient;

    float ColorChange = 1.0f;
    bool ColorChangeBool = true;
    public float NoiseDuration = 1440.0f;

    // Start is called before the first frame update
    void Start()
    {
        baseDensity = RenderSettings.fogDensity;
        baseStart = RenderSettings.fogStartDistance;
    }

    // Update is called once per frame
    void Update()
    {
        
        noiseOffset += Time.deltaTime * speed;
        float dist = Vector3.Distance(transform.position, Camera.main.transform.position);
        float lerp = Mathf.Clamp01(dist / maxDistance);
        //RenderSettings.fogDensity = Mathf.Lerp(0.0f, density, lerp) + Mathf.PerlinNoise(noiseOffset, 0.0f) * 0.1f;
        RenderSettings.fogStartDistance = Mathf.Lerp(baseStart, maxDistance * 0.9f, lerp);
        

        if (ColorChangeBool)
        {
            ColorChange += 0.005f;
        }
        else
        {
            ColorChange -= 0.005f;
        }
        if (ColorChange > 1.0f)
        {
            ColorChange = 1.0f;
            ColorChangeBool = false;
        }
        else if (ColorChange < 0.0f)
        {
            ColorChange = 0.0f;
            ColorChangeBool = true;
        }

        RenderSettings.fogDensity = ColorChange/400;
        Color color = FogColorGradient.Evaluate(ColorChange);
        RenderSettings.fogColor = color;
    }
}
