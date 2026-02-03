using UnityEngine;

/// <summary>
/// Sistema de cámara en tercera persona simplificado
/// Optimizado para trabajar con cápsula sin animaciones
/// </summary>
public class SimpleCameraController : MonoBehaviour
{
    [Header("Objetivo")]
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 targetOffset = new Vector3(0, 1.5f, 0);

    [Header("Distancia")]
    [SerializeField] private float distance = 4f;
    [SerializeField] private float minDistance = 2f;
    [SerializeField] private float maxDistance = 8f;
    [SerializeField] private float zoomSpeed = 1f;

    [Header("Sensibilidad")]
    [SerializeField] private float mouseSensitivityX = 2f;
    [SerializeField] private float mouseSensitivityY = 2f;

    [Header("Límites de Rotación")]
    [SerializeField] private float minVerticalAngle = -30f;
    [SerializeField] private float maxVerticalAngle = 60f;

    [Header("Suavizado (Menos = Más Estable)")]
    [SerializeField] private float positionSmoothSpeed = 12f; // Mayor = más directo
    [SerializeField] private float rotationSmoothSpeed = 10f; // Mayor = más directo

    [Header("Colisión")]
    [SerializeField] private bool checkCollision = true;
    [SerializeField] private float collisionRadius = 0.2f;
    [SerializeField] private LayerMask collisionMask = -1;
    [SerializeField] private float collisionSmoothSpeed = 15f; // Mayor = más rápido al acercarse

    [Header("Estabilidad")]
    [SerializeField] private bool lockCursorOnStart = true;
    [SerializeField] private bool useFixedOffset = true; // Offset fijo sin variación

    // Variables privadas
    private float currentX = 0f;
    private float currentY = 20f;
    private float currentDistance;
    private float targetDistance;
    private Vector3 desiredPosition;

    void Start()
    {
        // Buscar objetivo
        if (target == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
                target = player.transform;
        }

        // Inicializar
        currentDistance = distance;
        targetDistance = distance;

        // Inicializar rotación desde la posición actual
        Vector3 angles = transform.eulerAngles;
        currentX = angles.y;
        currentY = angles.x;

        // Cursor
        if (lockCursorOnStart)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    void LateUpdate()
    {
        if (target == null) return;

        HandleInput();
        UpdateCameraPosition();
    }

    void HandleInput()
    {
        // Rotación con mouse (sin suavizado en el input)
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivityX;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivityY;

        currentX += mouseX;
        currentY -= mouseY;
        currentY = Mathf.Clamp(currentY, minVerticalAngle, maxVerticalAngle);

        // Zoom con rueda del mouse
        float scroll = Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
        targetDistance -= scroll;
        targetDistance = Mathf.Clamp(targetDistance, minDistance, maxDistance);

        // Control de cursor
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        if (Input.GetMouseButtonDown(0) && Cursor.lockState == CursorLockMode.None)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    void UpdateCameraPosition()
    {
        // Punto objetivo (centro de enfoque)
        Vector3 targetPosition = target.position + targetOffset;

        // Calcular rotación deseada (SIN suavizado en la rotación)
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);

        // Dirección desde el objetivo hacia la cámara
        Vector3 direction = rotation * Vector3.back;

        // Suavizar zoom (solo el zoom, no la posición)
        currentDistance = Mathf.Lerp(currentDistance, targetDistance, Time.deltaTime * 8f);

        // Distancia final después de colisión
        float finalDistance = currentDistance;

        // Detección de colisión
        if (checkCollision)
        {
            RaycastHit hit;
            if (Physics.SphereCast(targetPosition, collisionRadius, direction, out hit, currentDistance, collisionMask))
            {
                finalDistance = Mathf.Max(hit.distance - collisionRadius, minDistance);
            }
        }

        // Calcular posición deseada
        desiredPosition = targetPosition + direction * finalDistance;

        // Aplicar posición CON suavizado mínimo (más directo que antes)
        transform.position = Vector3.Lerp(transform.position, desiredPosition, positionSmoothSpeed * Time.deltaTime);

        // Aplicar rotación (directa, sin Slerp para máxima estabilidad)
        transform.rotation = rotation;
    }

    // Métodos públicos
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    public void SetDistance(float newDistance)
    {
        distance = newDistance;
        targetDistance = newDistance;
    }

    public void ResetCamera()
    {
        if (target == null) return;
        currentX = 0f;
        currentY = 20f;
        currentDistance = distance;
        targetDistance = distance;
    }

    public float GetCurrentDistance()
    {
        return currentDistance;
    }

    // Visualización
    void OnDrawGizmosSelected()
    {
        if (target == null || !Application.isPlaying) return;

        // Punto objetivo
        Vector3 targetPos = target.position + targetOffset;
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(targetPos, 0.2f);

        // Línea hacia la cámara
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(targetPos, transform.position);

        // Radio de colisión
        if (checkCollision)
        {
            Gizmos.color = new Color(1, 0, 0, 0.3f);
            Gizmos.DrawWireSphere(transform.position, collisionRadius);
        }
    }
}
