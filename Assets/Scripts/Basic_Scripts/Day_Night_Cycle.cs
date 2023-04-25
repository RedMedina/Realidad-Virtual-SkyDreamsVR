using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Day_Night_Cycle : MonoBehaviour
{

    public float dayCycleDuration = 1440.0f; // Duración en minutos de un ciclo completo de día y noche
    public AnimationCurve lightIntensityCurve; // Curva de intensidad de la luz
    public Gradient lightColorGradient; // Gradiente de color de la luz
    public Gradient skyColorGradient; // Gradiente de color del cielo
    public Material MaterialSky;
    public Material Clouds1;
    public Material Clouds2;

    public float dayDuration = 10.0f;
    public float nightDuration = 10.0f;

    private Light directionalLight; // La Direccional Light en la escena

    float ColorChange = 1.0f;
    bool ColorChangeBool = true;

    private GameObject sol;
    private GameObject luna;

    // Start is called before the first frame update
    void Start()
    {
        directionalLight = GetComponent<Light>();
        luna = directionalLight.transform.GetChild(0).gameObject;
        sol = directionalLight.transform.GetChild(1).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        // Calcula el tiempo del día como un valor entre 0 y 1, donde 0 es la medianoche y 1 es la medianoche del día siguiente
        float timeOfDay = Mathf.Repeat(Time.time / dayCycleDuration, 1.0f);

        if (ColorChangeBool)
        {
            ColorChange += Time.deltaTime / dayCycleDuration;
        }
        else
        {
            ColorChange -= Time.deltaTime / dayCycleDuration;
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

        MaterialSky.SetFloat("_TransitionDuration", ColorChange);
        Clouds1.SetFloat("_TransitionDuration", ColorChange);
        Clouds2.SetFloat("_TransitionDuration", ColorChange);

        // Ajusta la intensidad y el color de la Direccional Light en función del tiempo del día
        float intensity = lightIntensityCurve.Evaluate(ColorChange);
        //directionalLight.intensity = intensity < 0.2f ? 0.2f : intensity;
        //directionalLight.intensity = ColorChange;

        Color color = lightColorGradient.Evaluate(ColorChange);
        directionalLight.color = color;

        // Ajusta la rotación de la Direccional Light para que se mueva a lo largo del horizonte en función del tiempo del día
        float sunAngle = timeOfDay * 360.0f;
        Quaternion rotation = Quaternion.Euler(sunAngle, 135.0f, 0.0f);
        //directionalLight.transform.rotation = rotation;

        float rotationSpeed = 360.0f / dayCycleDuration;
        directionalLight.transform.Rotate((rotationSpeed/2) * Time.deltaTime, 0,0);

        sol.GetComponent<SpriteRenderer>().color = Color.Lerp(Color.white, color, 0.25f);
        luna.GetComponent<SpriteRenderer>().color = Color.Lerp(Color.white, color, 0.25f);

        //Debug.Log(ColorChange);
        
    }
}
