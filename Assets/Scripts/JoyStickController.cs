using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoyStickController : MonoBehaviour
{

    [SerializeField] private Transform car;

    private void OnTriggerStay(Collider collider)
    {
        if (collider.gameObject.tag == "Hand")
        {
            float handPos = collider.transform.parent.parent.parent.position.x;
            float joystickPos = transform.GetChild(2).position.x;
            float joystickRot = transform.rotation.z;
            float offset = 0f;

            // Dont rotate further than 0.2f
            if (handPos < joystickPos && joystickRot < 0.2f)
            {
                offset = (joystickPos - handPos);
                transform.Rotate(0, 0, offset*5, Space.Self);
            } else if (handPos > joystickPos && joystickRot > -0.2f)
            {
                offset = -(handPos - joystickPos);
                transform.Rotate(0, 0, offset*5, Space.Self);
            }

            Debug.Log(joystickRot);

            if (joystickRot < 0)
            {
                car.Rotate(0, Time.deltaTime * 20 * (joystickRot*10), 0, Space.Self);
            } else
            {
                car.Rotate(0, Time.deltaTime * 20 * (joystickRot * 10), 0, Space.Self);
            }
        }
    }
    private void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.tag == "Hand")
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
    }
}
