using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TomatoGrenade : MonoBehaviour
{
    [SerializeField] ParticleSystem explosionPrefab;
    [SerializeField] float explosionForce = 10.0f;
    [SerializeField] float explosionRadius = 5f; 
    [SerializeField] private Renderer[] meshesRenderers;

    private Rigidbody rigidBody;

    private void Start() {
        rigidBody = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision other) {
        if (!other.transform.CompareTag("Fence")) {
            Explode();
            rigidBody.isKinematic = true;
        }
    }

    private void Explode() {
        Debug.Log("Boom");
        explosionPrefab.Play();

        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider hitCollider in colliders)
        {
            
            Rigidbody rb = hitCollider.GetComponent<Rigidbody>();
            if (rb != null)
            {
                // Calculate the direction from the explosion center to the object's position
                Vector3 direction = rb.transform.position - transform.position;

                // Apply explosion force with the calculated direction
                rb.AddForce(direction.normalized * explosionForce, ForceMode.Impulse);
            }

            if (hitCollider.CompareTag("Enemy")) {
                hitCollider.SendMessageUpwards("TakeDamage", 3);
            }
            if (hitCollider.CompareTag("Player")) {
                hitCollider.SendMessageUpwards("TakeDamage", 20);
            }

        }

        Collider collider = GetComponent<Collider>();
        if (collider != null) {
            collider.enabled = false;
        }

        foreach (Renderer renderer in meshesRenderers) {
            renderer.enabled = false;
        }
        Destroy(gameObject, explosionPrefab.duration);
    }
}
