using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Palanca_Mov : MonoBehaviour
{

    private bool IsPress;
    public float RotInicio = 0;
    public float RotFin = -90;
    private float Rotation = 0;

    // Start is called before the first frame update
    void Start()
    {
        IsPress = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space)) 
        {
            if (!IsPress) 
            {
                Rotation -= Time.deltaTime * 20;
                transform.Rotate(0, Rotation, 0);
                if (Rotation <= RotFin)
                {
                    IsPress = true;
                }
            }
            else if (IsPress) 
            {
                Rotation += Time.deltaTime * 20;
                transform.Rotate(0, Rotation, 0);
                if (Rotation >= RotInicio)
                {
                    IsPress = false;
                }
            }
        }
        if (Input.GetKey(KeyCode.L)) 
        {
            if (IsPress)
            {
                Rotation += Time.deltaTime * 20;
                transform.Rotate(0, Rotation, 0);
                if (Rotation >= RotInicio)
                {
                    IsPress = false;
                }
            }
        }
    }
}
