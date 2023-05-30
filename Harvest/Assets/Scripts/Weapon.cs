using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using StarterAssets;
using TMPro;

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

    private GameObject canvas;
    private TextMeshProUGUI ammoCount;


    Camera mainCam;

    void Awake()
    {
        canvas = GameObject.Find("PlayerHUD");
        mainCam = Camera.main; 
        animator = GetComponent<Animator>();

        ammoCount = canvas.GetComponentInChildren<TextMeshProUGUI>();
    }

    void Start() {
        _input = transform.root.GetComponent<StarterAssetsInputs>();
        bullets = clipSize;
        ChangeAmmoText(bullets);
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
            StartCoroutine(Reload());
        } else if (bullets == clipSize) {
            _input.reload = false;
        }
    }

    private void Shoot()
    {
        MuzzleFlash.Play();
        animator.SetBool("Shoot", true);
        bullets--;
        ChangeAmmoText(bullets);
        
        if (bullets == 0) {
            animator.SetBool("Empty", true);
        }
        
        if (Physics.Raycast(mainCam.transform.position, mainCam.transform.forward, out RaycastHit hit, weaponRange, hittableLayer))
        {
            if(hit.collider.gameObject.tag == "Enemy") {
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("EnemyHead")) {
                    Debug.Log("Head hit");
                    hit.collider.gameObject.SendMessageUpwards("TakeDamage", weaponDamage * 2);
                } else if (hit.collider.gameObject.layer == LayerMask.NameToLayer("EnemyLegs")) {
                    hit.collider.gameObject.SendMessageUpwards("TakeDamage", weaponDamage * 0.7);
                } else if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Enemy")) {
                    hit.collider.gameObject.SendMessageUpwards("TakeDamage", weaponDamage);
                }
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

    private IEnumerator Reload() {
            reloading = true;
            canShoot = false;
            thresholdTime = Time.time + reloadTime;
            _input.reload = false;

            yield return new WaitForSeconds(reloadTime);

            bullets = clipSize;
            ChangeAmmoText(bullets);
            reloading = false;
            canShoot = true;
            animator.SetBool("Empty", false);
    }

    void ChangeAmmoText(int bulletCount) {
        ammoCount.text = bulletCount+"/"+clipSize;
    }
}
