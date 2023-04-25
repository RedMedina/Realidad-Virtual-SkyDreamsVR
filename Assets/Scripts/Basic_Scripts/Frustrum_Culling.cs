using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frustrum_Culling : MonoBehaviour
{
    private Camera mainCamera;
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        // Get the planes of the camera's frustum
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(mainCamera);
        // Check if an object is inside the frustum
        if (GeometryUtility.TestPlanesAABB(planes, transform.GetComponent<Renderer>().bounds))
        {
            // The object is inside the frustum
            transform.GetComponent<Renderer>().enabled = true;
        }
        else
        {
            // The object is outside the frustum
            transform.GetComponent<Renderer>().enabled = false;
        }
    }
}
