using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickUp : MonoBehaviour
{
    [SerializeField] float teleportTimer;

    private Vector3 lowerRange;
    private Vector3 upperRange;
    Vector3 basePosition;
    private float timer;

    void Start() {
        Transform low = transform.Find("LowerRange");
        Transform up = transform.Find("UpperRange");
        basePosition = transform.position;
        
        lowerRange = low.position;
        upperRange = up.position;

        timer = Time.time + teleportTimer;
    }

    void Update() {
        if(timer < Time.time) {
            Teleport();
            timer = Time.time + teleportTimer;
        }
    }

    void Teleport() {
        float randomX = Random.Range(lowerRange.x, upperRange.x);
        float y = basePosition.y;
        float randomZ = Random.Range(lowerRange.z, upperRange.z);

        Vector3 randomPos = new Vector3(randomX, y, randomZ);
        transform.position = randomPos;
    }

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
