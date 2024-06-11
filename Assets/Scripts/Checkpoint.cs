using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{

    private Checkpoints checkpoints;
    private MeshRenderer mr;

    private void Awake()
    {
        mr = GetComponent<MeshRenderer>();
        toggleView(false);
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.TryGetComponent<CarController>(out CarController player))
        {
            checkpoints.onCarThroughCheckpoint(this, collider.transform); 
        }
    }

    public void setCheckpoints(Checkpoints checkpoints)
    {
        this.checkpoints = checkpoints;
    }

    public void toggleView(bool show)
    {
        mr.enabled = show;
    }
}
