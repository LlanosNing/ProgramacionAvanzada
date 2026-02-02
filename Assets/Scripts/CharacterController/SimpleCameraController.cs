using UnityEngine;

/// <summary>
/// Sistema de cámara en tercera persona simplificado
/// Optimizado para trabajar con cápsula sin animaciones
/// </summary>
public class SimpleCameraController : MonoBehaviour
{
    [Header("Objetivo")]
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset = new Vector3(0, 1.5f, 0);

    [Header("Distancia y Posición")]
    [SerializeField] private float distance = 5f;
    [SerializeField] private float minDistance = 2f;
    [SerializeField] private float maxDistance = 10f;

    [Header("Sensibilidad")]
    [SerializeField] private float mouseSensitivity = 100f;
    [SerializeField] private float scrollSensitivity = 2f;

    [Header("Límites de Rotación")]
    [SerializeField] private float minYAngle = -30f;
    [SerializeField] private float maxYAngle = 70f;

    [Header("Suavizado")]
    [SerializeField] private float positionSmoothing = 10f;
    [SerializeField] private float rotationSmoothing = 5f;

    [Header("Colisión")]
    [SerializeField] private bool useCollision = true;
    [SerializeField] private float collisionRadius = 0.3f;
    [SerializeField] private LayerMask collisionLayers = -1;

    // Variables privadas
    private float currentX = 0f;
    private float currentY = 20f;
    private float currentDistance;
    private Vector3 currentPosition;
    private Quaternion currentRotation;

    void Start()
    {
        // Buscar el objetivo si no está asignado
        if (target == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
                target = player.transform;
            else
                Debug.LogError("No se encontró un GameObject con tag 'Player'. Asigna el objetivo manualmente.");
        }

        // Inicializar distancia
        currentDistance = distance;

        // Inicializar rotación desde la cámara actual
        Vector3 angles = transform.eulerAngles;
        currentX = angles.y;
        currentY = angles.x;

        // Bloquear cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Inicializar posición y rotación
        currentPosition = transform.position;
        currentRotation = transform.rotation;
    }

    void LateUpdate()
    {
        if (target == null) return;

        HandleInput();
        CalculateCameraPosition();
        ApplyCameraTransform();
    }

    void HandleInput()
    {
        // Input del mouse
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        currentX += mouseX;
        currentY -= mouseY;
        currentY = Mathf.Clamp(currentY, minYAngle, maxYAngle);

        // Zoom con la rueda del mouse
        float scroll = Input.GetAxis("Mouse ScrollWheel") * scrollSensitivity;
        distance -= scroll;
        distance = Mathf.Clamp(distance, minDistance, maxDistance);

        // Suavizar el zoom
        currentDistance = Mathf.Lerp(currentDistance, distance, Time.deltaTime * 5f);

        // Desbloquear cursor con ESC
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        // Bloquear cursor al hacer clic
        if (Input.GetMouseButtonDown(0))
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    void CalculateCameraPosition()
    {
        // Punto objetivo (posición del personaje + offset)
        Vector3 targetPosition = target.position + offset;

        // Calcular rotación deseada
        Quaternion desiredRotation = Quaternion.Euler(currentY, currentX, 0);

        // Calcular dirección de la cámara
        Vector3 direction = desiredRotation * Vector3.back;

        // Distancia final (puede cambiar por colisión)
        float finalDistance = currentDistance;

        // Detectar colisiones
        if (useCollision)
        {
            RaycastHit hit;
            if (Physics.SphereCast(targetPosition, collisionRadius, direction, out hit, currentDistance, collisionLayers))
            {
                finalDistance = Mathf.Max(hit.distance - collisionRadius, minDistance);
            }
        }

        // Calcular posición deseada
        Vector3 desiredPosition = targetPosition + direction * finalDistance;

        // Suavizar rotación
        currentRotation = Quaternion.Slerp(currentRotation, desiredRotation, rotationSmoothing * Time.deltaTime);

        // Suavizar posición
        currentPosition = Vector3.Lerp(currentPosition, desiredPosition, positionSmoothing * Time.deltaTime);
    }

    void ApplyCameraTransform()
    {
        transform.position = currentPosition;
        transform.rotation = currentRotation;
    }

    // Método público para establecer el objetivo
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    // Método público para resetear la cámara
    public void ResetCamera()
    {
        currentX = 0f;
        currentY = 20f;
        distance = 5f;
        currentDistance = distance;
    }

    // Visualización en el editor
    void OnDrawGizmosSelected()
    {
        if (target == null) return;

        // Dibujar línea hacia el objetivo
        Gizmos.color = Color.yellow;
        Vector3 targetPos = target.position + offset;
        Gizmos.DrawLine(transform.position, targetPos);
        Gizmos.DrawWireSphere(targetPos, 0.3f);

        // Dibujar radio de colisión
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, collisionRadius);
    }
}
