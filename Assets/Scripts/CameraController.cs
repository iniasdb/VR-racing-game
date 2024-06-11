using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    [SerializeField] private Camera povCamera, followCamera;

    // Start is called before the first frame update
    void Start()
    {
        povCamera.enabled = true;
        followCamera.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            povCamera.enabled = !povCamera.enabled;
            followCamera.enabled = !followCamera.enabled;

        }
    }
}
