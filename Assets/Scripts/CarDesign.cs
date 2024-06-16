using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarDesign : MonoBehaviour
{
    public static CarDesign Instance;

    private string licenseplate = "BMW";
    [SerializeField] private Material body, rims ;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SaveDesign(Material body, Material rims, string licenseplate)
    {
        this.licenseplate = licenseplate;
        this.body = body;
        this.rims = rims;
    }

    public Material LoadBody()
    {
        return this.body;
    }

    public Material LoadRim()
    {
        return this.rims;
    }

    public string LoadLicenseplate()
    {
        return this.licenseplate;
    }
}
