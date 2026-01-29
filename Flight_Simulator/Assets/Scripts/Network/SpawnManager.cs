using Unity.Netcode;
using UnityEngine;

public class SpawnManager : NetworkBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private float checkRadius = 5f;
    [SerializeField] private string backupPointName = "SpawnPoint5";

    private GameObject[] spawnPoints;
    private Transform backupPoint;

    void Start()
    {
        spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");

        foreach (var point in spawnPoints)
        {
            if (point.name == backupPointName)
            {
                backupPoint = point.transform;
                break;
            }
        }
    }

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
        }
    }

    private void OnClientConnected(ulong clientId)
    {
        Transform bestPoint = GetFreeSpawnPoint();
        GameObject playerInstance = Instantiate(playerPrefab, bestPoint.position, bestPoint.rotation);
        playerInstance.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId);
    }

    private Transform GetFreeSpawnPoint()
    {
        foreach (GameObject point in spawnPoints)
        {
            if (point.name == backupPointName) continue;

            if (!IsPointOccupied(point.transform.position))
            {
                return point.transform;
            }
        }

        return backupPoint != null ? backupPoint : spawnPoints[0].transform;
    }

    private bool IsPointOccupied(Vector3 position)
    {
        Collider[] colliders = Physics.OverlapSphere(position, checkRadius);
        foreach (var col in colliders)
        {
            if (col.CompareTag("Player")) return true;
        }
        return false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        if (spawnPoints != null)
        {
            foreach (var p in spawnPoints)
                Gizmos.DrawWireSphere(p.transform.position, checkRadius);
        }
    }
}