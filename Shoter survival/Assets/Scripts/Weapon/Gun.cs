using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public enum GunState
    {
        Idle,
        Shooting,
        Reloading
    }

    public GunState state;

    [Header("Ammo")]
    public int maxAmmo = 30;
    public int currentAmmo;
    public float reloadTime = 2f;

    [Header("Fire")]
    public float fireRate = 0.2f;
    private float nextTimeToFire;

    [Header("other")]
    public Transform shootPoint;
    public Camera cam;

    [Header("Sounds")]
    public AudioSource Shooting;
    public AudioClip AudioClip;
    
    public AudioSource Reloading;
    public AudioClip ReloadingClip;

    [Header("Effects")]
    public ParticleSystem muzleflash;
    public ParticleSystem hitEfectenemy;
    public ParticleSystem hitwall;

    void Start()
    {
        currentAmmo = maxAmmo;
        state = GunState.Idle;
    }

    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.R) && state != GunState.Reloading)
        {
            StartCoroutine(Reload());
        }

        
        if (Input.GetMouseButton(0) && Time.time >= nextTimeToFire)
        {
            if (state == GunState.Reloading) return;

            if (currentAmmo > 0)
            {
                nextTimeToFire = Time.time + fireRate;
                Shoot();
            }
            else
            {
                StartCoroutine(Reload());
            }         
        }
    }

    void Shoot()
    {
        state = GunState.Shooting;
        currentAmmo--;

        if(muzleflash != null)
        {
            muzleflash.Play();
        }

        if(Shooting != null && AudioClip != null)
        {
            Shooting.PlayOneShot(AudioClip);
        }

        RaycastHit hit;

        
        Vector3 targetPoint;

        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, 100f))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = cam.transform.position + cam.transform.forward * 100f;
        }

        Vector3 direction = (targetPoint - shootPoint.position).normalized;

        Debug.DrawRay(shootPoint.position, direction * 200f, Color.red, 5f);

        if (Physics.Raycast(shootPoint.position, direction, out hit, 100f))
        {
            if(hit.collider.CompareTag("Enemy"))
            {
                if(hitEfectenemy != null)
                {
                    Instantiate(hitEfectenemy, hit.point, Quaternion.LookRotation(hit.normal));
                }
            }
            else
            {
                if(hitwall != null)
                {
                    Instantiate(hitwall, hit.point, Quaternion.LookRotation(hit.normal));
                }
            }
        }

        state = GunState.Idle;

        StartCoroutine(ShootRoutine());
    }

    IEnumerator Reload()
    {
        state = GunState.Reloading;

        if(Reloading != null && ReloadingClip != null)
        {
        Reloading.PlayOneShot(ReloadingClip);
        }
        Debug.Log("Reloading...");

        yield return new WaitForSeconds(reloadTime);

        currentAmmo = maxAmmo;
        state = GunState.Idle;
    }

    IEnumerator ShootRoutine()
    {
        state = GunState.Shooting;

        yield return new WaitForSeconds(0.05f); 

        state = GunState.Idle;
    }
}
