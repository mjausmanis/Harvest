using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gates : MonoBehaviour
{
    private bool isRotating = false;

    private Transform gateLeft;
    private Transform gateRight;

    void Start() {
        gateLeft = transform.GetChild(0);
        gateRight = transform.GetChild(1);
    }

    public void OpenGate() {
        if (!isRotating) {
            StartCoroutine(rotateGate(90, -90));
        }
    }

    public void CloseGate() {
        if (!isRotating) {
            StartCoroutine(rotateGate(-90, 90));
        }
    }

    public IEnumerator rotateGate(float leftRot, float rightRot) {
        isRotating = true;

        Quaternion startRotationLeft = gateLeft.rotation;
        Quaternion startRotationRight = gateRight.rotation;
        
        Quaternion targetRotationLeft = Quaternion.Euler(gateLeft.eulerAngles + new Vector3(0f, leftRot, 0f));
        Quaternion targetRotationRight = Quaternion.Euler(gateRight.eulerAngles + new Vector3(0f, rightRot, 0f));

        float timePassed = 0f;

        while (timePassed < 3f) {
            float t = timePassed / 3f;
            
            Quaternion newRotationLeft = Quaternion.Lerp(startRotationLeft, targetRotationLeft, t);
            Quaternion newRotationRight = Quaternion.Lerp(startRotationRight, targetRotationRight, t);
            gateLeft.rotation = newRotationLeft;
            gateRight.rotation = newRotationRight;

            timePassed += Time.deltaTime;
            yield return null;
        }
        isRotating = false;
    }
}
