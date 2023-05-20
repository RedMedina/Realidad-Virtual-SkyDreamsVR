using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax_Cam : MonoBehaviour
{
    public float velocidad = 1.0f;     // Velocidad de movimiento de la c�mara
    public float alturaMaxima = 5.0f;  // Altura m�xima a la que se mover� la c�mara
    public float alturaMinima = 1.0f;  // Altura m�nima a la que se mover� la c�mara

    private float direccion = 1.0f;    // Direcci�n del movimiento (1 para arriba, -1 para abajo)
    private float alturaActual;        // Altura actual de la c�mara

    // Start is called before the first frame update
    void Start()
    {
        alturaActual = transform.position.y;  // Obtener la altura inicial de la c�mara
    }

    // Update is called once per frame
    void Update()
    {
        // Calcular la nueva posici�n de la c�mara
        alturaActual += velocidad * direccion * Time.deltaTime;

        // Cambiar la direcci�n si alcanza los l�mites de altura
        if (alturaActual >= alturaMaxima || alturaActual <= alturaMinima)
        {
            direccion *= -1;
        }

        // Actualizar la posici�n de la c�mara
        transform.position = new Vector3(transform.position.x, alturaActual, transform.position.z);

    }
}
