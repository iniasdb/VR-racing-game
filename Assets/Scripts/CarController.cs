using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CarController : MonoBehaviour
{
    private GameObject car;

    private float horizontalInput, verticalInput;
    private float currentSteerAngle, currentbrakeForce;
    private bool isBraking;
    private Rigidbody carRigidbody;
    private int kph;
    private Vector3 com;
    private bool isAI;
    private bool firstPerson = true;
    private Checkpoints checkpoints;
    
    [SerializeField] private Vector3 position;

    // Settings
    [SerializeField] private float motorForce, brakeForce, maxSteerAngle, turnSensitivity;

    // Wheel Colliders
    [SerializeField] private WheelCollider frontLeftWheelCollider, frontRightWheelCollider;
    [SerializeField] private WheelCollider rearLeftWheelCollider, rearRightWheelCollider;

    // Wheels
    [SerializeField] private Transform frontLeftWheelTransform, frontRightWheelTransform;
    [SerializeField] private Transform rearLeftWheelTransform, rearRightWheelTransform;
    [SerializeField] private Transform steeringWheelTransform;

    // Speedometer
    [SerializeField] private TextMeshProUGUI kphText;

    // Audiosources
    [SerializeField] private AudioSource interiorSound, idleSound;
    private int playing;

    public void SetAI(bool isAI)
    {
        this.isAI = isAI;
    }

    public bool IsAI()
    {
        return this.isAI;
    }

    public void SetCheckpoints(Checkpoints cps)
    {
        this.checkpoints = cps;
    }

    public void SetFPV(bool isFPV)
    {
        this.firstPerson = isFPV;
    }

    private void Awake()
    {
        carRigidbody = GetComponent<Rigidbody>();
        carRigidbody.centerOfMass = com;
        if (!isAI)
        {
            idleSound.Play();
            playing = 0;
        }

    }

    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.R) && this.checkpoints != null)
        {
            Checkpoint cp = this.checkpoints.GetLastCheckpointPassed();

            gameObject.transform.position = cp.transform.position;
        }

        if (!isAI)
        {
            kph = Convert.ToInt32(Math.Floor(carRigidbody.velocity.magnitude * 3.6));
            kphText.text = kph + " km/h";

            if (kph <= 10)
            {
                if (playing != 0)
                {
                    interiorSound.Stop();
                    idleSound.Play();
                    playing = 0;
                }
            }
            else
            {
                if (playing != 1)
                {
                    interiorSound.Play();
                    idleSound.Stop();
                    playing = 1;
                }
            }

            GetInput();
        }
        HandleMotor();
        HandleSteering();
        if (!isAI)
        {
            UpdateWheels();
            UpdateSteeringwheel();
        }
    }

    private void GetInput()
    {
        // Steering Input
        horizontalInput = Input.GetAxis("Horizontal");

        // Acceleration Input
        verticalInput = Input.GetAxis("Vertical");

        // Braking Input
        isBraking = Input.GetKey(KeyCode.Space);
        gameObject.GetComponent<LightsController>().brakeLights(isBraking);
    }

    public void SetInputs(float forwardAmount, float turnAmount, float brakes)
    {
        // Steering Input
        horizontalInput = turnAmount;

        // Acceleration Input
        verticalInput = forwardAmount;

        // Braking Input
        isBraking = Convert.ToBoolean(brakes);
        if (!isAI)
        {
            gameObject.GetComponent<LightsController>().brakeLights(isBraking);
        }
    }

    public void StopCompletely()
    {
        while (kph > 0)
        {
            isBraking = true;
            ApplyBrakes();
        }

        isBraking = false;
    }

    private void HandleMotor()
    {
        frontLeftWheelCollider.motorTorque = verticalInput * motorForce;
        frontRightWheelCollider.motorTorque = verticalInput * motorForce;
        currentbrakeForce = isBraking ? brakeForce : 0f;
        ApplyBrakes();
    }

    private void ApplyBrakes()
    {
        frontRightWheelCollider.brakeTorque = currentbrakeForce;
        frontLeftWheelCollider.brakeTorque = currentbrakeForce;
        rearLeftWheelCollider.brakeTorque = currentbrakeForce;
        rearRightWheelCollider.brakeTorque = currentbrakeForce;
    }

    private void HandleSteering()
    {
        //currentSteerAngle = maxSteerAngle * horizontalInput;

        currentSteerAngle = horizontalInput * turnSensitivity * maxSteerAngle;

        frontLeftWheelCollider.steerAngle = Mathf.Lerp(frontLeftWheelCollider.steerAngle, currentSteerAngle, 0.8f);
        frontRightWheelCollider.steerAngle = Mathf.Lerp(frontRightWheelCollider.steerAngle, currentSteerAngle, 0.8f);

    }

    private void UpdateWheels()
    {
        UpdateSingleWheel(frontLeftWheelCollider, frontLeftWheelTransform);
        UpdateSingleWheel(frontRightWheelCollider, frontRightWheelTransform);
        UpdateSingleWheel(rearRightWheelCollider, rearRightWheelTransform);
        UpdateSingleWheel(rearLeftWheelCollider, rearLeftWheelTransform);
    }

    private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);
        wheelTransform.rotation = rot;
        wheelTransform.position = pos;
    }

    private void UpdateSteeringwheel()
    {
        Quaternion rot = Quaternion.Euler(0, 0, gameObject.transform.rotation.z);

        float steeringAngle = steeringWheelTransform.transform.eulerAngles.z;

        if (currentSteerAngle <= maxSteerAngle && currentSteerAngle >= -maxSteerAngle)
        {
            steeringWheelTransform.transform.Rotate(0, 0, currentSteerAngle - steeringAngle, Space.Self);
        }
    }
}