using UnityEngine;
using Unity.Netcode; // Используем новый сетевой движок

public class CameraScript : NetworkBehaviour
{
    public Camera AircraftCamera;
    public AudioListener AircraftAudioListener; // Рекомендуется также отключать звук

    public override void OnNetworkSpawn()
    {
        // Если это МЫ управляем этим объектом, ничего не отключаем
        if (IsOwner) return;

        // Если это чужой самолет, выключаем камеру и микрофон (AudioListener)
        if (AircraftCamera != null)
        {
            AircraftCamera.enabled = false;
        }

        if (AircraftAudioListener != null)
        {
            AircraftAudioListener.enabled = false;
        }
    }
}