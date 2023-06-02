using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class NoRotate : MonoBehaviour
{
    public Transform playerTransform;

    private CinemachineVirtualCamera virtualCamera;

    private void Start()
    {
        // Obtén la referencia al componente CinemachineVirtualCamera
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
    }

    private void LateUpdate()
    {
        Vector3 newPosition = transform.position;
        newPosition.x = playerTransform.position.x;
        newPosition.z = playerTransform.position.z;
        transform.position = newPosition;
    }
}
