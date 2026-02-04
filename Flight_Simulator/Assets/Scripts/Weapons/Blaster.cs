using UnityEngine;

public class Blaster : MonoBehaviour
{
    [SerializeField] Projectile projectilePrefab;
    [SerializeField] Transform muzzle;
    [SerializeField][Range(0.01f, 1f)] float cooldownTime = 0.1f;

    float cooldown;

    void Update()
    {
        cooldown -= Time.deltaTime;

        if (cooldown <= 0f && Input.GetMouseButton(0))
            Fire();
    }

    void Fire()
    {
        cooldown = cooldownTime;
        Instantiate(projectilePrefab, muzzle.position, muzzle.rotation);
    }
}
