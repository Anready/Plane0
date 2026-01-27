using UnityEngine;
using Unity.Netcode; // Новый неймспейс для Unity 6

public class OnlyLocalCanvas : NetworkBehaviour
{
    // OnNetworkSpawn вызывается, когда объект появляется в сети
    public override void OnNetworkSpawn()
    {
        // Проверяем, принадлежит ли этот объект текущему игроку
        Canvas playerCanvas = GetComponentInChildren<Canvas>();

        if (playerCanvas != null)
        {
            // IsOwner — современный аналог isLocalPlayer
            playerCanvas.enabled = IsOwner;
        }
    }
}