using Cinemachine;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.InputSystem;
using TMPro; // Для вывода тяги на экран

public class AircraftController : NetworkBehaviour
{
    [Header("Physics & Movement")]
    public float RollSensitivity = 0.2f;
    public float PitchSensitivity = 0.2f;
    public float YawSensitivity = 0.2f;
    public float ThrustSensitivity = 0.5f; // Чувствительность изменения тяги
    public float FlapSensitivity = 0.15f;

    [Header("Materials")]
    public PhysicsMaterial GearsBrakesMaterial;
    public PhysicsMaterial GearsDriveMaterial;

    [Header("UI Reference")]
    public TextMeshProUGUI thrustText; // Перетащи сюда текст из UI Canvas

    // Приватные переменные состояния
    private AircraftPhysics m_physics;
    private Rotator m_propeller;
    private SphereCollider[] m_elementsSlowDown;

    private float m_currentRoll, m_currentPitch, m_currentYaw, m_currentThrust, m_currentFlap;
    private float thrust_percentage = 0;
    private bool m_isGrounded = true;

    // Ссылка на новую систему ввода
    private PlayerControls controls;

    void Awake()
    {
        // Инициализируем класс управления
        controls = new PlayerControls();
    }

    public override void OnNetworkSpawn()
    {
        // Ввод и камера работают только для локального игрока (владельца)
        if (IsOwner)
        {
            controls.Enable();

            // Подписка на разовые нажатия (Закрылки)
            controls.Flight.FlapUp.performed += _ => ChangeFlap(FlapSensitivity);
            controls.Flight.FlapDown.performed += _ => ChangeFlap(-FlapSensitivity);

            // Настройка курсора
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            // Настройка Cinemachine
            var vcam = FindFirstObjectByType<CinemachineVirtualCamera>();
            if (vcam != null)
            {
                vcam.Follow = transform;
                vcam.LookAt = transform;
            }
        }
        else
        {
            // Если это чужой самолет, отключаем его UI для нас
            if (thrustText != null) thrustText.gameObject.SetActive(false);
        }
    }

    public override void OnNetworkDespawn()
    {
        controls.Disable();
    }

    void Start()
    {
        // Компоненты нужны всем (для синхронизации физики), 
        // но инициализируем специфичные вещи
        m_physics = GetComponent<AircraftPhysics>();
        m_propeller = FindFirstObjectByType<Rotator>();
        m_elementsSlowDown = GetComponentsInChildren<SphereCollider>();

        if (IsOwner)
        {
            m_physics.ThrustPercent = 0;
            foreach (var element in m_elementsSlowDown)
            {
                element.material = GearsDriveMaterial;
            }
        }
    }

    void Update()
    {
        if (!IsOwner) return;

        // 1. Обработка Тяги (Thrust)
        float thrustInput = controls.Flight.Thrust.ReadValue<float>();
        // Изменяем процент тяги плавно (умножаем на ThrustSensitivity и Time.deltaTime)
        thrust_percentage += thrustInput * (ThrustSensitivity * 100f) * Time.deltaTime;
        thrust_percentage = Mathf.Clamp(thrust_percentage, 0f, 100f);

        m_currentThrust = thrust_percentage / 100f;

        // 2. Обновление UI
        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateThrust(thrust_percentage);
        }

        // 3. Чтение осей вращения
        m_currentPitch = controls.Flight.Pitch.ReadValue<float>() * PitchSensitivity;
        m_currentRoll = controls.Flight.Roll.ReadValue<float>() * RollSensitivity;
        m_currentYaw = controls.Flight.Yaw.ReadValue<float>() * YawSensitivity;

        // 4. Логика торможения (Brake / Space)
        if (controls.Flight.Brake.IsPressed() && m_isGrounded)
        {
            var rb = GetComponent<Rigidbody>();
            rb.linearVelocity *= 0.99f;
            rb.angularVelocity *= 0.99f;

            if (rb.linearVelocity.magnitude <= 1)
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
        }

        // 5. Визуализация пропеллера
        if (m_propeller != null)
        {
            m_propeller.Speed = m_currentThrust * 6000f;
        }
    }

    void FixedUpdate()
    {
        if (!IsOwner) return;

        // Применяем значения к физическому движку самолета
        m_physics.SetControlSurfacesAngles(m_currentPitch, m_currentRoll, m_currentYaw, m_currentFlap);
        m_physics.ThrustPercent = m_currentThrust;
    }

    private void ChangeFlap(float amount)
    {
        m_currentFlap += amount;
        m_currentFlap = Mathf.Clamp(m_currentFlap, 0f, Mathf.Deg2Rad * 40);
    }

    // Обработка приземления
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ground")) m_isGrounded = true;
    }

    void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Ground")) m_isGrounded = false;
    }
}