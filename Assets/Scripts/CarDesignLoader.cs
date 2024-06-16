using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;


public class CarDesignLoader : MonoBehaviour
{

    [SerializeField] private GameObject carBody;
    [SerializeField] private List<GameObject> carRims;
    [SerializeField] private TextMeshProUGUI licenseplateText;

    private void Start()
    {
        Material chosenBody = CarDesign.Instance.LoadBody();
        Material chosenRim = CarDesign.Instance.LoadRim();
        string chosenLicenseplate = CarDesign.Instance.LoadLicenseplate();

        attachBody(chosenBody);
        attachRim(chosenRim);
        licenseplateText.text = chosenLicenseplate;
    }

    private void attachBody(Material chosenBody)
    {
        MeshRenderer mr = carBody.GetComponent<MeshRenderer>();
        Material[] bodyMaterials = mr.sharedMaterials;
        bodyMaterials[1] = chosenBody;
        mr.sharedMaterials = bodyMaterials;
    }

    private void attachRim(Material chosenRim)
    {
        foreach (GameObject carRim in carRims)
        {
            MeshRenderer mr = carRim.GetComponent<MeshRenderer>();
            Material[] rimMaterials = mr.sharedMaterials;
            rimMaterials[0] = chosenRim;
            mr.sharedMaterials = rimMaterials;
        }
    }
}
