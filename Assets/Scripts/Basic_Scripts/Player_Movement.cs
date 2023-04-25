using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Movement : MonoBehaviour
{

    public float speed = 20.0f;
    public Camera cam;

    private Rigidbody rb;
    private Vector3 moveDirection;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

        //Movimiento por teclado temporal.
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        // Calcula la dirección de movimiento usando la rotación de la cámara
        Vector3 forward = cam.transform.forward;
        Vector3 right = cam.transform.right;
        forward.y = 0.0f;
        right.y = 0.0f;
        forward.Normalize();
        right.Normalize();
        moveDirection = (forward * verticalInput + right * horizontalInput).normalized;

        if (moveDirection != Vector3.zero)
        {
            rb.MovePosition(transform.position + moveDirection * speed * Time.deltaTime);
        }
    }
}
