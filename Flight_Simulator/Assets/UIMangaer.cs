using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    // Статическая ссылка, доступная из любого скрипта
    public static UIManager Instance { get; private set; }

    [Header("Flight Data Displays")]
    [SerializeField] private TextMeshProUGUI thrustText;
    [SerializeField] private TextMeshProUGUI speedText; // Бонус: для скорости

    private void Awake()
    {
        // Реализация паттерна Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    // Метод для обновления текста тяги
    public void UpdateThrust(float percentage)
    {
        if (thrustText != null)
            thrustText.text = $"Thrust: {Mathf.RoundToInt(percentage)}%";
    }
}