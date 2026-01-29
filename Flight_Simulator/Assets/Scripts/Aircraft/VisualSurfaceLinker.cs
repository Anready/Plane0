using UnityEngine;

public class VisualSurfaceLinker : MonoBehaviour
{
    [SerializeField] private AircraftPhysics physics;

    [Header("Ailerons")]
    [SerializeField] private Transform eleron_left;
    [SerializeField] private Transform eleron_right;

    [Header("Tail")]
    [SerializeField] private Transform ruli;
    [SerializeField] private Transform rudder;

    [Header("Flaps")]
    [SerializeField] private Transform flaps1;
    [SerializeField] private Transform flaps2;

    [SerializeField] private float multiplier = 1f;

    void Update()
    {
        if (physics == null) return;

        foreach (var control in physics.ControlSurfaces)
        {
            if (control.Surface == null) continue;

            float angle = control.Surface.FlapAngle * Mathf.Rad2Deg * multiplier;

            switch (control.Type)
            {
                case ControlSurfaceType.Roll:
                    ApplyRotation(eleron_left, -angle);
                    ApplyRotation(eleron_right, angle);
                    break;

                case ControlSurfaceType.Pitch:
                    ApplyRotation(ruli, angle);
                    break;

                case ControlSurfaceType.Yaw:
                    ApplyRotation(rudder, angle, true);
                    break;

                case ControlSurfaceType.Flap:
                    ApplyRotation(flaps1, angle);
                    ApplyRotation(flaps2, angle);
                    break;
            }
        }
    }

    private void ApplyRotation(Transform target, float angle)
    {
        ApplyRotation(target, angle, false);
    }

    private void ApplyRotation(Transform target, float angle, bool isRul)
    {
        if (target != null)
        {
            if (isRul)
            {
                target.localRotation = Quaternion.Euler(-90, angle, 0);
            }
            else
            {
                target.localRotation = Quaternion.Euler(-90 - angle, 0, 0);
            }
        }
    }
}