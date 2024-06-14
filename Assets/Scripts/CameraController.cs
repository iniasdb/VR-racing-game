using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    [SerializeField] private Camera povCamera, followCamera;
    private CarController carController;

    // Start is called before the first frame update
    void Start()
    {
        povCamera.enabled = true;
        followCamera.enabled = false;
        carController = GetComponent<CarController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            povCamera.enabled = !povCamera.enabled;
            followCamera.enabled = !followCamera.enabled;
            Debug.Log(povCamera.enabled);
            carController.SetFPV(povCamera.enabled);
        }
    }
}
