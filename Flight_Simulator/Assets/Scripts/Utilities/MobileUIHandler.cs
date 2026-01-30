using UnityEngine;

public class MobileUIHandler : MonoBehaviour
{
    void Awake()
    {
        // Check if we are on a mobile platform (Android/iOS)
        // Or if we are in WebGL and the device is a mobile browser
        if (Application.isMobilePlatform)
        {
            gameObject.SetActive(true); // Show joystick
        }
        else
        {
            gameObject.SetActive(false); // Hide joystick for PC/Desktop
        }
    }
}