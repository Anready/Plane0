using UnityEngine;

public class AircraftGizmos : MonoBehaviour
{
    public bool ShowGizmos = true;
    public float ScaleMultiplier = 0.01f; // Масштаб стрелок, чтобы они не были на весь экран

    private AircraftPhysics _physics;
    private Rigidbody _rb;

    void OnDrawGizmos()
    {
        if (!ShowGizmos) return;

        _physics = GetComponent<AircraftPhysics>();
        _rb = GetComponent<Rigidbody>();

        if (_physics == null || _rb == null) return;

        // 1. Показываем Центр Масс и Вес
        Vector3 com = _rb.worldCenterOfMass;
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(com, 0.2f);

        // Вес в Ньютонах (Масса * Гравитация)
        float weightForce = _rb.mass * Physics.gravity.magnitude;
        GUIStyle style = new GUIStyle();
        style.normal.textColor = Color.green;

        // 2. Отрисовка сил для каждой поверхности
        if (_physics.AeroSurfaces != null)
        {
            foreach (var surface in _physics.AeroSurfaces)
            {
                if (surface == null) continue;

                // Подъемная сила (Синяя)
                Gizmos.color = Color.blue;
                Gizmos.DrawRay(surface.transform.position, surface.CurrentLift * ScaleMultiplier);

                // Сопротивление (Красное)
                Gizmos.color = Color.red;
                Gizmos.DrawRay(surface.transform.position, surface.CurrentDrag * ScaleMultiplier);
            }
        }

        // 3. Отрисовка Тяги Двигателя (Желтая)
        Gizmos.color = Color.yellow;
        Vector3 thrustForce = transform.forward * _physics.ThrustMultiplier * _physics.ThrustPercent;
        Gizmos.DrawRay(transform.position, thrustForce * ScaleMultiplier);

        // Рисуем направление "вперед" для самолета
        Gizmos.color = Color.white;
        Gizmos.DrawRay(transform.position, transform.forward * 2f);
    }
}