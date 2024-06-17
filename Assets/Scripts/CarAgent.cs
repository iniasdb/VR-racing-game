using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class CarAgent : Agent
{

    [SerializeField] private Checkpoints checkpoints;
    [SerializeField] private Transform spawn;

    private CarController carController;

    private float collisionTime;

    private void Awake()
    {
        carController = GetComponent<CarController>();
        carController.SetAI(true);
    }

    public void WrongCheckpoint()
    {
        AddReward(-1f);
    }

    public void CorrectCheckpoint()
    {
        AddReward(1f);
    }

    public override void OnEpisodeBegin()
    {
        transform.position = new Vector3(787f, 0.2f, 655.5f);
        transform.forward = new Vector3(0.01f, 0f, -1f);
        checkpoints.ResetCheckpoints(transform);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        Vector3 checkpointForward = checkpoints.GetNextCheckpoint(transform).transform.forward;
        float directionDot = Vector3.Dot(transform.forward, checkpointForward);
        sensor.AddObservation(directionDot);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        float forwardAmount = 0f;
        float turnAmount = 0f;
        float brakes = 0f;

        switch (actions.DiscreteActions[0])
        {
            case 0:
                forwardAmount = 0f;
                break;
            case 1:
                forwardAmount = +1f;
                break;
            case 2:
                forwardAmount = -1f;
                break;
        }

        switch (actions.DiscreteActions[1])
        {
            case 0:
                turnAmount = 0f;
                break;
            case 1:
                turnAmount = +1f;
                break;
            case 2:
                turnAmount = -1f;
                break;
        }

        switch (actions.DiscreteActions[2])
        {
            case 0:
                brakes = 0;
                break;
            case 1:
                brakes = +1;
                break;
        }

        carController.SetInputs(forwardAmount, turnAmount, brakes);
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        int forwardAction = 0;
        if (Input.GetKey(KeyCode.UpArrow)) forwardAction = 1;
        if (Input.GetKey(KeyCode.DownArrow)) forwardAction = 2;

        int turnAction = 0;
        if (Input.GetKey(KeyCode.RightArrow)) turnAction = 1;
        if (Input.GetKey(KeyCode.LeftArrow)) turnAction = 2;

        int brakeAction = 0;
        if (Input.GetKey(KeyCode.Space)) brakeAction = 1;

        ActionSegment<int> discreteActions = actionsOut.DiscreteActions;
        discreteActions[0] = forwardAction;
        discreteActions[1] = turnAction;
        discreteActions[2] = brakeAction;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            collisionTime = 0.0f;
            AddReward(-0.5f);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            //Sometimes car got stuck against a wall with no way out
            collisionTime += Time.deltaTime;
            AddReward(-0.1f);
            if (collisionTime > 5)
            {
                EndEpisode();
            }
        }
    }
}
