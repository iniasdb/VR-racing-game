using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightsController : MonoBehaviour
{

    /*
     
     APART SCRIPT ANDERS SOMS KEY INPUT NIET GEDETECTEERD
     
     */

    private bool lightsOn = false;

    // Headlights
    [SerializeField] private Light leftHeadlight, rightHeadlight;
    // Taillights
    [SerializeField] private Light leftTaillight, rightTaillight;


    // Start is called before the first frame update
    void Start()
    {
        leftHeadlight.spotAngle = 0;
        rightHeadlight.spotAngle = 0;

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            headLights();
        }

    }

    private void headLights()
    {
        if (lightsOn)
        {
            leftHeadlight.spotAngle = 0;
            rightHeadlight.spotAngle = 0;
            lightsOn = false;
        }
        else
        {
            leftHeadlight.spotAngle = 80;
            rightHeadlight.spotAngle = 80;
            lightsOn = true;
        }
    }

    public void brakeLights(bool isBraking)
    {
        if (isBraking)
        {
            leftTaillight.intensity = 2.5f;
            rightTaillight.intensity = 2.5f;
        }
        else
        {
            leftTaillight.intensity = 0.61f;
            rightTaillight.intensity = 0.61f;
        }
    }
}
