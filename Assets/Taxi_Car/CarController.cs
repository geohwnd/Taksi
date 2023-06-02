using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    private float horizontalInput, verticalInput;
    private float currentSteerAngle, currentbreakForce;
    private bool isBreaking;
    private Vector3 initialRotation = new Vector3(0f, 0f, 0f);

    // Settings
    [SerializeField] private float motorForce, breakForce, maxSteerAngle;

    // Wheel Colliders
    [SerializeField] private WheelCollider frontLeftWheelCollider, frontRightWheelCollider;
    [SerializeField] private WheelCollider rearLeftWheelCollider, rearRightWheelCollider;

    [SerializeField] private Transform startingPosition;
    [SerializeField] private float maxTiltAngle = 60f;

    // Wheels
    [SerializeField] private Transform frontLeftWheelTransform, frontRightWheelTransform;
    [SerializeField] private Transform rearLeftWheelTransform, rearRightWheelTransform;

    [SerializeField] private float driftForce = 500f;
    [SerializeField] private float maxDriftAngle = 45f;
    [SerializeField] private float driftDuration = 1f;
    [SerializeField] private float driftCooldown = 2f;

    private bool isDrifting = false;
    private float driftStartTime = 0f;

    private void FixedUpdate()
    {
        /*GetInput();
        HandleMotor();
        HandleSteering();
        UpdateWheels();*/
        GetInput();
        HandleMotor();

        if (!isDrifting)
        {
            HandleSteering();
        }
        else
        {
            HandleDriftSteering();
        }

        UpdateWheels();
    }

    private void GetInput()
    {
        /* // Steering Input
         horizontalInput = Input.GetAxis("Horizontal");

         // Acceleration Input
         verticalInput = Input.GetAxis("Vertical");

         // Breaking Input
         isBreaking = Input.GetKey(KeyCode.Space);*/
        // Steering Input
        horizontalInput = Input.GetAxis("Horizontal");

        // Acceleration Input
        verticalInput = Input.GetAxis("Vertical");

        // Breaking Input
        isBreaking = Input.GetKey(KeyCode.Space);

        // Drift Input
        if (Input.GetKeyDown(KeyCode.Space) && Time.time - driftStartTime > driftCooldown)
        {
            StartDrift();
        }
    }

    private void StartDrift()
    {
        isDrifting = true;
        driftStartTime = Time.time;
    }

    private void HandleMotor()
    {
        frontLeftWheelCollider.motorTorque = -verticalInput * motorForce;
        frontRightWheelCollider.motorTorque = -verticalInput * motorForce;
        currentbreakForce = isBreaking ? breakForce : 0f;
        ApplyBreaking();
    }

    private void ApplyBreaking()
    {
        frontRightWheelCollider.brakeTorque = currentbreakForce;
        frontLeftWheelCollider.brakeTorque = currentbreakForce;
        rearLeftWheelCollider.brakeTorque = currentbreakForce;
        rearRightWheelCollider.brakeTorque = currentbreakForce;
    }

    private void HandleSteering()
    {
        /*currentSteerAngle = maxSteerAngle * horizontalInput;
        frontLeftWheelCollider.steerAngle = currentSteerAngle;
        frontRightWheelCollider.steerAngle = currentSteerAngle;*/
        currentSteerAngle = maxSteerAngle * horizontalInput;
        frontLeftWheelCollider.steerAngle = currentSteerAngle;
        frontRightWheelCollider.steerAngle = currentSteerAngle;

    }

    private void HandleDriftSteering()
    {
        if (Time.time - driftStartTime > driftDuration)
        {
            isDrifting = false;
            HandleSteering();
            return;
        }

        float driftAngle = currentSteerAngle + (horizontalInput > 0 ? maxDriftAngle : -maxDriftAngle);
        frontLeftWheelCollider.steerAngle = driftAngle;
        frontRightWheelCollider.steerAngle = driftAngle;

        // Apply a sideways force to create the drifting effect
        Vector3 driftForceVector = -transform.right * (horizontalInput > 0 ? 1f : -1f) * driftForce;
        GetComponent<Rigidbody>().AddForce(driftForceVector, ForceMode.Acceleration);
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

    private void Update()
    {
        CheckIfCarFlipped();
    }

    /*private void ResetCarPosition()
    {
        // Teleport the car to the starting position
        transform.position = startingPosition.position;
        transform.rotation = Quaternion.Euler(initialRotation);
    }*/

    private void CheckIfCarFlipped()
    {
        // Obtener el ángulo de inclinación actual en el eje Z
        float currentTiltAngle = transform.eulerAngles.z;

        // Ajustar el ángulo negativo a un valor en el rango de 0 a 360 grados
        if (currentTiltAngle > 180f)
        {
            currentTiltAngle -= 360f;
        }

        // Si el ángulo de inclinación excede el valor máximo permitido (considerado volcado)
        if (Mathf.Abs(currentTiltAngle) > maxTiltAngle)
        {
            // Teletransportar el carro al inicio con rotación (0, 0, 0)
            transform.position = startingPosition.position;
            transform.rotation = Quaternion.identity;
            // Puedes usar Quaternion.Euler(0f, 0f, 0f) en lugar de Quaternion.identity si prefieres la notación de ángulos de Euler

            // Reiniciar la velocidad lineal y angular del carro
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        }
    }

}