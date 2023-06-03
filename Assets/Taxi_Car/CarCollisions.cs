using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car_S : MonoBehaviour
{
    [SerializeField] private Transform carTransform;
    [SerializeField] private Collider carCollider;
    [SerializeField] private Collider capsuleCollider;
    [SerializeField] private Collider metaCollider;
    [SerializeField] private GameObject capsulePrefab; // Prefab de la cápsula (persona)

    private bool capsuleDestroyed = false;
    private bool justOnce = false;
    private GameObject currentCapsule; // Referencia al objeto de la cápsula actual

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider == capsuleCollider && carCollider != null)
        {
            Debug.Log("Colisionó con la persona");

            if (!capsuleDestroyed)
            {
                capsuleDestroyed = true;
                currentCapsule = collision.gameObject; // Guardar referencia al objeto de la cápsula actual
                currentCapsule.SetActive(false); // Ocultar la cápsula actual en lugar de destruirla
            }
        }
        else if (collision.collider == metaCollider && capsuleDestroyed && !justOnce)
        {
            Debug.Log("El jugador acaba de entregar a la persona");
            justOnce = true;

            if (capsulePrefab != null)
            {
                currentCapsule.SetActive(true); // Mostrar la cápsula actual nuevamente
                currentCapsule.transform.position = capsuleCollider.transform.position; // Posicionar la cápsula actual en la posición correcta
                currentCapsule.transform.rotation = capsuleCollider.transform.rotation; // Asignar la rotación correcta a la cápsula actual
                currentCapsule.GetComponent<Collider>().enabled = true; // Activar el collider de la cápsula actual

                /*GameObject newCapsule = Instantiate(capsulePrefab, capsuleCollider.transform.position, capsuleCollider.transform.rotation);
                newCapsule.GetComponent<Collider>().enabled = true; // Activar el collider de la nueva cápsula*/
            }
            else
            {
                Debug.LogWarning("Prefab de la cápsula no asignado en Car_S script!");
            }

            capsuleDestroyed = false;
            justOnce = false; // Restablecer el valor de justOnce a false
        }
    }
}
