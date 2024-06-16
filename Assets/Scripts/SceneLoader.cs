using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class SceneLoader : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI tipText;

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.TryGetComponent<CarController>(out CarController player))
        {
            if (gameObject.name != "Customize")
            {
                LoadScene("RaceTrack");
            }
        }
    }

    private void OnTriggerStay(Collider collider)
    {
        if (collider.TryGetComponent<CarController>(out CarController player))
        {
            if (gameObject.name == "Customize")
            {
                tipText.text = "Press [E] to customize car";
                if (Input.GetKey(KeyCode.E))
                {
                    LoadScene("CustomizeCar");
                }
            }
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.TryGetComponent<CarController>(out CarController player))
        {
            tipText.text = "";
        }
    }
}