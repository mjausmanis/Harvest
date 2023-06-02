using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using StarterAssets;
using TMPro;

public class Throwable : MonoBehaviour
{
    private StarterAssetsInputs _input;

    [Header("References")]
    public Transform cam;
    public Transform attackPoint;
    public GameObject objectToThrow;

    [Header("Settings")]
    public int totalThrows;
    public float throwCooldown;

    [Header("Throwing")]
    public float throwForce;
    public float throwUpwardForce;

    bool readyToThrow;

    private GameObject canvas;
    private TextMeshProUGUI nadeCountUI;

    private int nadeCount;

    private void Start() {
        nadeCount = totalThrows;
        canvas = GameObject.Find("PlayerHUD");
        _input = transform.root.GetComponent<StarterAssetsInputs>();
        readyToThrow = true;
        nadeCountUI = GameObject.Find("NadeCount").GetComponentInChildren<TextMeshProUGUI>();
        UpdateNadeCount();
    }

    private void Update() {
        if (_input.throwgrenade && readyToThrow && nadeCount > 0) {
            Throw();
            _input.throwgrenade = false;
        }
    }

    private void Throw() {
        nadeCount--;
        UpdateNadeCount();
        GameObject projectile = Instantiate(objectToThrow, attackPoint.position, cam.rotation);

        Rigidbody projectileRB = projectile.GetComponent<Rigidbody>();

        Vector3 forceToAdd = cam.transform.forward * throwForce + transform.up * throwUpwardForce;
    
        projectileRB.AddForce(forceToAdd, ForceMode.Impulse);

    }

    void UpdateNadeCount() {
        nadeCountUI.text = nadeCount+"/"+totalThrows;
    }
}
