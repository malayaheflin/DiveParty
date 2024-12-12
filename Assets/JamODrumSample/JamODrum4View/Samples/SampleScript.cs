using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script is for showing some basic stuff you can do with the 4-point view setup

public class SampleScript : MonoBehaviour
{
    float deltaX;
    float deltaZ;

    
    void Update()
    {
        // This part is for toggling the culling layers of the camera to show different objects
        if(Input.GetButtonDown("Fire1"))
        {
            hideLayer("Hidden");
        }

        if(Input.GetButtonDown("Jump"))
        {
            showAllLayers();
        }

        // You can also move the camera around

        deltaX = Input.GetAxis("Horizontal") * Time.deltaTime * 2.0f;
        deltaZ = Input.GetAxis("Vertical") * Time.deltaTime * 2.0f;

        moveCamera(Camera.main.transform.position.x + deltaX, Camera.main.transform.position.y, Camera.main.transform.position.z + deltaZ);
        
    }

    void moveCamera(float x, float y, float z)
    {
        Camera.main.transform.position = new Vector3(x, y, z);
    }

    // Change the camera culling layer to exclude hidden layers
    void showAllLayers()
    {
        Camera.main.cullingMask = -1;
    }

    void hideLayer(string layerName)
    {
        Camera.main.cullingMask &= ~(1 << LayerMask.NameToLayer(layerName));
    }
}
