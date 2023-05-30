using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickUp : MonoBehaviour
{
    void OnTriggerEnter( Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            Player player = other.gameObject.GetComponent<Player>();

            if (player != null) {
                player.SendMessageUpwards("Heal", 25);
                Destroy(gameObject);
            }
        }
    }
}
