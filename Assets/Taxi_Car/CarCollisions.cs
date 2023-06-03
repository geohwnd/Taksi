using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car_S : MonoBehaviour
{
    [SerializeField] private Transform carTransform;
    [SerializeField] private Collider carCollider;
    [SerializeField] private Collider capsuleCollider;
    [SerializeField] private Collider metaCollider;
    [SerializeField] private GameObject capsulePrefab; // Prefab de la c�psula (persona)

    private bool capsuleDestroyed = false;
    private bool justOnce = false;
    private GameObject currentCapsule; // Referencia al objeto de la c�psula actual

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider == capsuleCollider && carCollider != null)
        {
            Debug.Log("Colision� con la persona");

            if (!capsuleDestroyed)
            {
                capsuleDestroyed = true;
                currentCapsule = collision.gameObject; // Guardar referencia al objeto de la c�psula actual
                currentCapsule.SetActive(false); // Ocultar la c�psula actual en lugar de destruirla
            }
        }
        else if (collision.collider == metaCollider && capsuleDestroyed && !justOnce)
        {
            Debug.Log("El jugador acaba de entregar a la persona");
            justOnce = true;

            if (capsulePrefab != null)
            {
                currentCapsule.SetActive(true); // Mostrar la c�psula actual nuevamente
                currentCapsule.transform.position = capsuleCollider.transform.position; // Posicionar la c�psula actual en la posici�n correcta
                currentCapsule.transform.rotation = capsuleCollider.transform.rotation; // Asignar la rotaci�n correcta a la c�psula actual
                currentCapsule.GetComponent<Collider>().enabled = true; // Activar el collider de la c�psula actual

                /*GameObject newCapsule = Instantiate(capsulePrefab, capsuleCollider.transform.position, capsuleCollider.transform.rotation);
                newCapsule.GetComponent<Collider>().enabled = true; // Activar el collider de la nueva c�psula*/
            }
            else
            {
                Debug.LogWarning("Prefab de la c�psula no asignado en Car_S script!");
            }

            capsuleDestroyed = false;
            justOnce = false; // Restablecer el valor de justOnce a false
        }
    }
}
