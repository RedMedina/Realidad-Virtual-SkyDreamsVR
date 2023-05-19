using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System.Threading;

public class Globo : MonoBehaviour
{
    public float speed = 15f;
    public float UpSpeed = 3f;
    public float maxHeight = 100f;
    public float maxRight = -300f;
    public GameObject Player;
    private SerialPort port;
    private float Up;

    private bool isMoving = false;
    private bool MovUp = true;
    private bool MovRight = true;
    private BoxCollider[] colliders;
    private Rigidbody rig;
    private float Tiempo=0;
    private int DireccionActual = -1;

    private bool StationColl;
    private bool TerrainColl;

    public enum Direcciones 
    {
        Left,
        Right,
        Foward,
        Back
    }

    public enum Alturas
    {
        Altura1 = 110,
        Altura2 = 150,
        Altura3 = 200,
        Altura4 = 300
    }

    //El tiempo aproximado en que cambiará el viento
    public float TimeWindVariation = 15.0f;
    private float alturaAnterior;

    // Start is called before the first frame update
    void Start()
    {
       colliders = gameObject.GetComponents<BoxCollider>();
       rig = gameObject.GetComponent<Rigidbody>();
       colliders[0].isTrigger = false;
       colliders[1].isTrigger = false;
       colliders[2].isTrigger = false;
       colliders[3].isTrigger = false;
       colliders[4].isTrigger = false;
       colliders[5].isTrigger = false;
       isMoving = true;
       alturaAnterior = transform.position.y;
       DireccionActual = GetRandomDirection(DireccionActual);
       StationColl = false;
       TerrainColl = false;

        try
        {
            port = new SerialPort();
            port.PortName = "COM4";
            port.BaudRate = 9600;
            port.ReadTimeout = 500;
            port.Open();
        }
        catch
        {

        }

        Thread thread = new Thread(SerialCommunication);
        thread.Start();
    }

    // Update is called once per frame
    void Update()
    {
        try
        {
            if (port.IsOpen)
                port.WriteLine(DireccionActual.ToString());
        }catch { }
        //Debug.Log(data);

        //float Up = Input.GetAxisRaw("Jump");

        transform.Translate(Vector3.up * Up * UpSpeed * Time.deltaTime);

        if (isMoving)
        {
            if (MovUp)
            {
                transform.Translate(Vector3.up * speed * Time.deltaTime);
            }

            if (transform.position.y >= maxHeight || !MovUp)
            {
                MovUp = false;
                rig.useGravity = true;
                Tiempo += Time.deltaTime;
                if(Tiempo >= TimeWindVariation) 
                {
                    DireccionActual = GetRandomDirection(DireccionActual);
                    Debug.Log("Tiempo: " + Tiempo + " Direccion: " + DireccionActual);
                    Tiempo = 0.0f;
                }

                float alturaActual = transform.position.y;

                if (alturaActual > alturaAnterior)
                {
                    // el objeto ha subido de altura
                    if (alturaActual >= (float)Alturas.Altura2 && alturaAnterior < (float)Alturas.Altura2)
                    {
                        Debug.Log("El objeto ha subido a la altura 2");
                        alturaAnterior = (float)Alturas.Altura2;
                        DireccionActual = GetRandomDirection(DireccionActual);
                    }
                    else if (alturaActual >= (float)Alturas.Altura3 && alturaAnterior < (float)Alturas.Altura3)
                    {
                        Debug.Log("El objeto ha subido a la altura 3");
                        alturaAnterior = (float)Alturas.Altura3;
                        DireccionActual = GetRandomDirection(DireccionActual);
                    }
                    else if (alturaActual >= (float)Alturas.Altura4 && alturaAnterior < (float)Alturas.Altura4)
                    {
                        Debug.Log("El objeto ha subido a la altura 4");
                        alturaAnterior = (float)Alturas.Altura4;
                        DireccionActual = GetRandomDirection(DireccionActual);
                    }
                }
                else if (alturaActual < alturaAnterior)
                {
                    // el objeto ha bajado de altura
                    if (alturaActual < (float)Alturas.Altura2 && alturaAnterior >= (float)Alturas.Altura2)
                    {
                        Debug.Log("El objeto ha bajado a la altura 1");
                        alturaAnterior = (float)Alturas.Altura1;
                        DireccionActual = GetRandomDirection(DireccionActual);
                    }
                    else if (alturaActual < (float)Alturas.Altura3 && alturaAnterior >= (float)Alturas.Altura3)
                    {
                        Debug.Log("El objeto ha bajado a la altura 2");
                        alturaAnterior = (float)Alturas.Altura2;
                        DireccionActual = GetRandomDirection(DireccionActual);
                    }
                    else if (alturaActual < (float)Alturas.Altura4 && alturaAnterior >= (float)Alturas.Altura4)
                    {
                        Debug.Log("El objeto ha bajado a la altura 3");
                        alturaAnterior = (float)Alturas.Altura3;
                        DireccionActual = GetRandomDirection(DireccionActual);
                    }
                }

                if (transform.position.x > 450 || transform.position.x < -450 || transform.position.z > 450 || transform.position.z < -450) 
                {
                    switch (DireccionActual)
                    {
                        case (int)Direcciones.Left:
                            DireccionActual = 1;
                            break;
                        case (int)Direcciones.Right:
                            DireccionActual = 0;
                            break;
                        case (int)Direcciones.Foward:
                            DireccionActual = 3;
                            break;
                        case (int)Direcciones.Back:
                            DireccionActual = 2;
                            break;
                        default:
                            break;
                    }
                    Debug.Log("Cambio Direccion Contraria: " + DireccionActual);
                }
                
                if (StationColl && TerrainColl)
                {
                    Debug.Log("El objeto ha entrado en el trigger y en el collider al mismo tiempo");
                }
                else if (TerrainColl)
                {
                    Debug.Log("El objeto ha entrado solo en el collider");
                }

                DireccionDelViento(DireccionActual);
            }
        }

  
    }

    private void OnApplicationQuit()
    {
        if (port != null || port.IsOpen)
        {
            port.Write("\0");
            port.Close();
            Debug.Log("Close");
        }
    }

    void SerialCommunication()
    {
        while (port.IsOpen)
        {
            string data = "";
            try
            {
                data = port.ReadLine();
                //Debug.Log(data);
                Up = int.Parse(data);
               

            }
            catch { }
        }
    }

    int GetRandomDirection(int DirActual) 
    {
        int Direccion = Random.Range(0, 4);
        while (Direccion == DirActual)
        {
            Direccion = Random.Range(0, 4);
        }
        DirActual = Direccion;

        return DirActual;
    }

    void DireccionDelViento(int DirActual) 
    {
        switch (DirActual)
        {
            case (int)Direcciones.Left:
                    transform.Translate(Vector3.left * speed * Time.deltaTime);
                break;
            case (int)Direcciones.Right:
                    transform.Translate(Vector3.right * speed * Time.deltaTime);
                break;
            case (int)Direcciones.Foward:
                    transform.Translate(Vector3.forward * speed * Time.deltaTime);
                break;
            case (int)Direcciones.Back:
                    transform.Translate(Vector3.back * speed * Time.deltaTime);
                break;

            default:
                break;
        }
    }

    
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Base_Station"))
        {
            StationColl = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (!isMoving)
            {
                other.transform.position = transform.position;
                other.transform.Translate(new Vector3(0, 0.7f, 0));

                colliders[0].isTrigger = false;
                colliders[1].isTrigger = false;
                colliders[2].isTrigger = false;
                colliders[3].isTrigger = false;
                colliders[4].isTrigger = false;
                colliders[5].isTrigger = false;
                isMoving = true;
            }
        }

        if (other.gameObject.CompareTag("Base_Station"))
        {
            StationColl = true;
        }
    }
   
    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("terrain"))
        {
            TerrainColl = false;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("terrain"))
        {
            TerrainColl = true;
        }
    }
}
