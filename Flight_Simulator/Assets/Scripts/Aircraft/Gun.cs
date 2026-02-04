/*
using UnityEngine;
using TMPro;

public class GunSystem : MonoBehaviour
{
    public int damage = 20;
    public float timeBetweenShooting = 0.1f;
    public float spread = 0.05f;
    public float range = 500f;
    public float reloadTime = 2f;
    public float timeBetweenShots = 0.05f;
    public int magazineSize = 50;
    public int bulletsPerTap = 1;
    public bool allowButtonHold = true;

    int bulletsLeft;
    int bulletsShot;
    bool shooting, readyToShoot, reloading;

    public Transform attackPoint; 
    public LayerMask whatIsEnemy;
    public TextMeshProUGUI ammoText;

    public GameObject muzzleFlash;
    public GameObject bulletHoleGraphic;
    public CamShake camShake;
    public float camShakeMagnitude = 0.1f;
    public float camShakeDuration = 0.05f;

    private RaycastHit rayHit;

    private void Awake()
    {
        bulletsLeft = magazineSize;
        readyToShoot = true;
    }

    private void Update()
    {
        MyInput();

        if (ammoText != null)
            ammoText.SetText(bulletsLeft + " / " + magazineSize);
    }

    private void MyInput()
    {
        if (allowButtonHold) shooting = Input.GetKey(KeyCode.Mouse0);
        else shooting = Input.GetKeyDown(KeyCode.Mouse0);

        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !reloading) Reload();

        if (readyToShoot && shooting && !reloading && bulletsLeft > 0)
        {
            bulletsShot = bulletsPerTap;
            Shoot();
        }
    }

    private void Shoot()
    {
        readyToShoot = false;

        // Spread relative to where the gun is pointing
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);
        Vector3 direction = attackPoint.forward + (attackPoint.right * x) + (attackPoint.up * y);

        // Raycast from Gun nozzle forward
        if (Physics.Raycast(attackPoint.position, direction, out rayHit, range, whatIsEnemy))
        {
            Debug.Log("Hit: " + rayHit.collider.name);
            AircraftHealth targetHealth = rayHit.collider.GetComponent<AircraftHealth>();
            if (targetHealth != null)
            {
                targetHealth.TakeDamage(damage);
            }

            if (bulletHoleGraphic != null)
                Instantiate(bulletHoleGraphic, rayHit.point, Quaternion.LookRotation(rayHit.normal));
        }

        if (muzzleFlash != null)
            Instantiate(muzzleFlash, attackPoint.position, attackPoint.rotation);

        if (camShake != null)
            camShake.Shake(camShakeDuration, camShakeMagnitude);

        bulletsLeft--;
        bulletsShot--;

        Invoke("ResetShot", timeBetweenShooting);

        if (bulletsShot > 0 && bulletsLeft > 0)
            Invoke("Shoot", timeBetweenShots);
    }

    private void ResetShot() => readyToShoot = true;

    private void Reload()
    {
        reloading = true;
        Invoke("ReloadFinished", reloadTime);
    }

    private void ReloadFinished()
    {
        bulletsLeft = magazineSize;
        reloading = false;
    }
}
*/