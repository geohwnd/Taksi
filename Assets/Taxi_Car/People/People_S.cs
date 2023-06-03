using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class People_S : MonoBehaviour
{
    [SerializeField] private Transform carTransform;
    [SerializeField] private Collider carCollider;
    [SerializeField] private Collider capsuleCollider;
    [SerializeField] private Collider metaCollider;

    private bool capsuleDestroyed = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider == carCollider && carTransform != null)
        {
            Debug.Log("¡Colisionó con el carro!");
            Destroy(gameObject); // Destruir la cápsula
            capsuleDestroyed = true;
            StartCoroutine(GanarJuego());
        }
        else if (collision.collider == metaCollider && capsuleDestroyed)
        {
            Debug.Log("¡Ganó!");
        }
    }

    private IEnumerator GanarJuego()
    {
        yield return new WaitForSeconds(1.0f); // Esperar un segundo

        Vector3 targetPosition = metaCollider.transform.position;

        while (Vector3.Distance(carTransform.position, targetPosition) > 0.1f)
        {
            carTransform.position = Vector3.MoveTowards(carTransform.position, targetPosition, 3.0f * Time.deltaTime);
            yield return null;
        }

        Debug.Log("¡Ganó!");
    }
}



