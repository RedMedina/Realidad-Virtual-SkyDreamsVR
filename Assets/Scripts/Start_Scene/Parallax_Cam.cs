using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax_Cam : MonoBehaviour
{
    public float velocidad = 1.0f;     // Velocidad de movimiento de la cámara
    public float alturaMaxima = 5.0f;  // Altura máxima a la que se moverá la cámara
    public float alturaMinima = 1.0f;  // Altura mínima a la que se moverá la cámara

    private float direccion = 1.0f;    // Dirección del movimiento (1 para arriba, -1 para abajo)
    private float alturaActual;        // Altura actual de la cámara

    // Start is called before the first frame update
    void Start()
    {
        alturaActual = transform.position.y;  // Obtener la altura inicial de la cámara
    }

    // Update is called once per frame
    void Update()
    {
        // Calcular la nueva posición de la cámara
        alturaActual += velocidad * direccion * Time.deltaTime;

        // Cambiar la dirección si alcanza los límites de altura
        if (alturaActual >= alturaMaxima || alturaActual <= alturaMinima)
        {
            direccion *= -1;
        }

        // Actualizar la posición de la cámara
        transform.position = new Vector3(transform.position.x, alturaActual, transform.position.z);

    }
}
