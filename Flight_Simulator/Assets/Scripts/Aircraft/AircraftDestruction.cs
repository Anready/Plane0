using UnityEngine;

public class AircraftDestruction : MonoBehaviour
{
    [SerializeField] int hitsToDestroy = 3;

    int hitCount;

    public void TakeDamage(int damage, Vector3 hitPoint)
    {
        hitCount++;
        Debug.Log($"Aircraft hit at {hitPoint}");

        if (hitCount >= hitsToDestroy)
        {
            Debug.Log("Aircraft destroyed");
            Destroy(gameObject);
        }
    }
}
