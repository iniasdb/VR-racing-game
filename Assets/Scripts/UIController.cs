using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;


public class UIController : MonoBehaviour
{

    [SerializeField] private List<Material> materials;
    [SerializeField] private List<string> licenseplates;
    [SerializeField] private GameObject carBody;
    [SerializeField] private List<GameObject> carRims;
    [SerializeField] private TextMeshProUGUI licenseplateText;

    private List<string> materialNames;
    private TMP_Dropdown bodyColorDropdown, rimColorDropdown, licensePlateDropdown;
    private TMP_InputField licenseplateInput;
    private Button saveButton, backButton;

    private string chosenLicenseplate;
    private Material chosenBody;
    private Material chosenRim;


    // Start is called before the first frame update
    void Start()
    {
        materialNames = new List<string>();
        foreach (Material mat in materials)
        {
            materialNames.Add(mat.name);
        }

        bodyColorDropdown = GameObject.Find("BodyColorDropdown").GetComponent<TMP_Dropdown>();
        rimColorDropdown = GameObject.Find("RimColorDropdown").GetComponent<TMP_Dropdown>();
        licensePlateDropdown = GameObject.Find("LicenseplateDropdown").GetComponent<TMP_Dropdown>();
        licenseplateInput = GameObject.Find("LicenseplateInput").GetComponent<TMP_InputField>();
        saveButton = GameObject.Find("SaveButton").GetComponent<Button>();
        backButton = GameObject.Find("BackButton").GetComponent<Button>();

        bodyColorDropdown.ClearOptions();
        bodyColorDropdown.AddOptions(materialNames);
        rimColorDropdown.ClearOptions();
        rimColorDropdown.AddOptions(materialNames);
        licensePlateDropdown.ClearOptions();
        licensePlateDropdown.AddOptions(licenseplates);

        bodyColorDropdown.onValueChanged.AddListener(delegate {
            int value = bodyColorDropdown.value;
            chosenBody = materials[value];

            MeshRenderer mr = carBody.GetComponent<MeshRenderer>();
            Material[] bodyMaterials = mr.sharedMaterials;
            bodyMaterials[1] = chosenBody;
            mr.sharedMaterials = bodyMaterials;
        });

        rimColorDropdown.onValueChanged.AddListener(delegate {
            int value = rimColorDropdown.value;
            chosenRim = materials[value];

            foreach (GameObject carRim in carRims)
            {
                MeshRenderer mr = carRim.GetComponent<MeshRenderer>();
                Material[] rimMaterials = mr.sharedMaterials;
                rimMaterials[0] = chosenRim;
                mr.sharedMaterials = rimMaterials;
            }
        });

        licensePlateDropdown.onValueChanged.AddListener(delegate {
            int value = licensePlateDropdown.value;
            chosenLicenseplate = licenseplates[value];

            licenseplateText.text = chosenLicenseplate;
        });

        licenseplateInput.onValueChanged.AddListener(delegate
        {
            chosenLicenseplate = licenseplateInput.text;
            licenseplateText.text = chosenLicenseplate;
        });

        saveButton.onClick.AddListener(delegate {
            Debug.Log("saved");
            Debug.Log(chosenBody.name);
            CarDesign.Instance.SaveDesign(chosenBody, chosenRim, chosenLicenseplate);
        });


        backButton.onClick.AddListener(delegate {
            SceneManager.LoadScene("Garage");
        });
    }

}
