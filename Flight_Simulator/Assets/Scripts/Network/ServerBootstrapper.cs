using Unity.Netcode;
using Unity.Netcode.Transports.UTP; // Required to access UnityTransport
using UnityEngine;
using TMPro;

public class ServerBootstrapper : MonoBehaviour
{
    public TMP_InputField ipInputField; // Drag your UI InputField here

    void Start()
    {
        Debug.Log(Application.isBatchMode);
        Debug.Log("--------------");
        if (Application.isBatchMode)
        {
            Debug.Log("Server starting on 0.0.0.0...");
            var transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
            transport.ConnectionData.Address = "0.0.0.0"; // Listen to everyone
            NetworkManager.Singleton.StartServer();
        }
        else
        {
            var transport = NetworkManager.Singleton.GetComponent<UnityTransport>();

            // Get the IP from the UI InputField, or use a default if empty
            string targetIP = ipInputField != null && !string.IsNullOrEmpty(ipInputField.text)
                              ? ipInputField.text
                              : "192.168.0.6"; // Your PC IP here

            transport.ConnectionData.Address = targetIP;
            Debug.Log($"Connecting to {targetIP}...");

            NetworkManager.Singleton.StartClient();
        }
    }

    public void JoinGame()
    {

    }
}