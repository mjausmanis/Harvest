using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using StarterAssets;


public class Weapon : MonoBehaviour
{
    private StarterAssetsInputs _input;

    [SerializeField] ParticleSystem MuzzleFlash;
    [SerializeField] GameObject bulletHole;
    [SerializeField] float bulletHolePositionOffset;

    [Header("Weapon stats")]
    [SerializeField] float weaponDamage;
    [SerializeField] float fireRate;
    bool canShoot = true;
    float thresholdTime;
    [SerializeField] int clipSize;
    private int bullets;
    [SerializeField] float reloadTime;
    private bool reloading;

    private Animator animator;

    [Header("Raycast")]
    [SerializeField] LayerMask hittableLayer;
    [SerializeField] float weaponRange;

    Camera mainCam;

    void Awake()
    {
        //fetches the main camera and stores it in a variable
        mainCam = Camera.main; 
        animator = GetComponent<Animator>();
    }

    void Start() {
        _input = transform.root.GetComponent<StarterAssetsInputs>();
        bullets = clipSize;
    }

    void Update()
    {
        if (!canShoot && thresholdTime < Time.time)
            {
                canShoot = true;
            }
        if (_input.shoot && canShoot) {
            _input.shoot = false;
            if(bullets > 0) {
                Shoot();
                canShoot = false;
                thresholdTime = Time.time + fireRate;
            }
        } else if (_input.shoot){
            _input.shoot = false; // Disregard the click if the gun is on cooldown
        }
        if (_input.reload && !reloading && bullets < clipSize) {
            Debug.Log("Reloading");
            Reload();
        } else if (bullets == clipSize) {
            _input.reload = false;
        }
        if (reloading && thresholdTime < Time.time) {
            Debug.Log("Finished reloading");
            transform.Translate(mainCam.transform.up * 5 * Time.deltaTime);
            reloading = false;
            animator.SetBool("Empty", false);
        }
    }

    private void Shoot()
    {
        MuzzleFlash.Play();
        animator.SetBool("Shoot", true);
        bullets--;
        
        if (bullets == 0) {
            animator.SetBool("Empty", true);
        }
        
        if (Physics.Raycast(mainCam.transform.position, mainCam.transform.forward, out RaycastHit hit, weaponRange, hittableLayer))
        {
            if(hit.collider.gameObject.layer == LayerMask.NameToLayer("Enemy")) {
                hit.collider.gameObject.SendMessageUpwards("GetHit", weaponDamage);
                Debug.Log("Enemy hit");
            } else {
                //spawning bulletHoles at point of hit + (unit vector normal to the surface)*offset
                //spawned bulletHole rotation aligned with the direction of unity vector normal to the surface
                Instantiate(bulletHole, hit.point + (hit.normal * bulletHolePositionOffset), Quaternion.LookRotation(hit.normal)).transform.Rotate(0f, 180f, 0f);
                Debug.Log("Ground Hit");
            }
        }
        else
        {
            Debug.Log("Miss");
        }
        
    }
    
    public void ShootCompleteAnimationEvent() {
        // This method is called from the shoot animation when it completes

        animator.SetBool("Shoot", false);
        // Reset the shooting animation parameter
        

        // Trigger the shoot complete parameter to transition back to idle
        animator.SetTrigger("ShootComplete");
    }

    private void Reload() {
            reloading = true;
            canShoot = false;
            thresholdTime = Time.time + reloadTime;
            transform.Translate(-mainCam.transform.up * 5 * Time.deltaTime);
            bullets = clipSize;
            _input.reload = false;
    }
}
