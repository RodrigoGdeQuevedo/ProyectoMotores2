using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Configuración de Cámara")]
    public Transform target; // El transform del jugador que la cámara seguirá
    public float distance = 5.0f; // Distancia de la cámara al jugador
    public float height = 0.4f; // Altura de la cámara sobre el jugador
    public float mouseSensitivity = 2.0f;
    public float pitchMin = -40.0f; // Límite de rotación vertical (hacia abajo)
    public float pitchMax = 80.0f;  // Límite de rotación vertical (hacia arriba)

    [Header("Suavizado")]
    public float rotationSmoothTime = 0.12f; // Suavizado de la rotación
    public float positionSmoothTime = 0.2f;  // Suavizado del seguimiento

    private float yaw = 0.0f;   // Rotación horizontal (Eje Y)
    private float pitch = 0.0f; // Rotación vertical (Eje X)

    private Vector3 currentVelocity;
    private Vector3 currentRotationSmoothVelocity;
    private float currentYawSmooth;
    private float currentPitchSmooth;

    void Start()
    {
        if (target)
        {
            yaw = target.eulerAngles.y;
            pitch = 10f;
        }
    }

    void LateUpdate()
    {
        if (!target)
        {
            Debug.LogWarning("La cámara no tiene un 'Target' (objetivo) asignado.");
            return;
        }

        yaw += Input.GetAxis("Mouse X") * mouseSensitivity;
        pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity;

        pitch = Mathf.Clamp(pitch, pitchMin, pitchMax);

        // Suavizar la rotación
        currentYawSmooth = Mathf.SmoothDampAngle(currentYawSmooth, yaw, ref currentRotationSmoothVelocity.x, rotationSmoothTime);
        currentPitchSmooth = Mathf.SmoothDampAngle(currentPitchSmooth, pitch, ref currentRotationSmoothVelocity.y, rotationSmoothTime);

        Quaternion desiredRotation = Quaternion.Euler(currentPitchSmooth, currentYawSmooth, 0);

        Vector3 targetPosition = target.position + Vector3.up * height;
        Vector3 desiredPosition = targetPosition - (desiredRotation * Vector3.forward * distance);

        // Aplicar la posición y rotación suavizadas
        transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, rotationSmoothTime);

        // Usamos SmoothDamp para el movimiento para evitar tirones
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref currentVelocity, positionSmoothTime);
    }
}
