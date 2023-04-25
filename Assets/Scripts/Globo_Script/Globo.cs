using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Globo : MonoBehaviour
{
    public float speed = 15f;
    public float maxHeight = 100f;
    public float maxRight = -300f;
    public GameObject Player;

    private bool isMoving = false;
    private bool MovUp = true;
    private bool MovRight = true;
    private BoxCollider[] colliders;
    private Rigidbody rig;

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
    }

    // Update is called once per frame
    void Update()
    {

        float Up = Input.GetAxisRaw("Jump");
        transform.Translate(Vector3.up * Up * 3.0f * Time.deltaTime);

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

                if (MovRight) 
                {
                    transform.Translate(-Vector3.right * speed * Time.deltaTime);
                }

                if (transform.position.x <= maxRight) 
                {
                    isMoving = false;
                    MovRight = false;
                }
            }
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

    }
}
