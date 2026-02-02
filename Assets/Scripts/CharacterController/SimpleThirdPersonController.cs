using UnityEngine;

/// <summary>
/// Controlador de personaje en tercera persona sin animaciones
/// Perfecto para prototipos con solo una cápsula
/// </summary>
[RequireComponent(typeof(CharacterController))]
public class SimpleThirdPersonController : MonoBehaviour
{
    [Header("Velocidades de Movimiento")]
    [SerializeField] private float walkSpeed = 2f;
    [SerializeField] private float runSpeed = 5f;
    [SerializeField] private float sprintSpeed = 7f;
    
    [Header("Rotación")]
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private float movementSmoothing = 0.1f;

    [Header("Salto y Gravedad")]
    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] private float gravity = -20f;
    
    [Header("Detección de Suelo")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistance = 0.2f;
    [SerializeField] private LayerMask groundMask;

    [Header("Referencia de Cámara")]
    [SerializeField] private Transform cameraTransform;

    // Componentes
    private CharacterController characterController;
    
    // Variables de movimiento
    private Vector3 velocity;
    private Vector3 moveDirection;
    private float currentSpeed;
    private bool isGrounded;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        
        // Buscar la cámara principal si no está asignada
        if (cameraTransform == null)
        {
            if (Camera.main != null)
                cameraTransform = Camera.main.transform;
        }

        // Crear groundCheck si no existe
        if (groundCheck == null)
        {
            GameObject groundCheckObj = new GameObject("GroundCheck");
            groundCheckObj.transform.SetParent(transform);
            groundCheckObj.transform.localPosition = new Vector3(0, -characterController.height / 2f, 0);
            groundCheck = groundCheckObj.transform;
        }

        // Bloquear el cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        CheckGround();
        HandleMovement();
        HandleJump();
        ApplyGravity();
    }

    void CheckGround()
    {
        // Verificar si está en el suelo
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        // Resetear velocidad vertical si está en el suelo
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
    }

    void HandleMovement()
    {
        // Obtener input
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        // Obtener dirección de la cámara
        Vector3 cameraForward = cameraTransform.forward;
        Vector3 cameraRight = cameraTransform.right;

        // Aplanar vectores (eliminar componente Y)
        cameraForward.y = 0f;
        cameraRight.y = 0f;
        cameraForward.Normalize();
        cameraRight.Normalize();

        // Calcular dirección de movimiento relativa a la cámara
        Vector3 desiredMoveDirection = (cameraForward * vertical + cameraRight * horizontal).normalized;

        // Determinar velocidad según input
        float targetSpeed = 0f;
        if (desiredMoveDirection.magnitude > 0.1f)
        {
            if (Input.GetKey(KeyCode.LeftShift))
                targetSpeed = sprintSpeed;
            else if (Input.GetKey(KeyCode.LeftControl))
                targetSpeed = walkSpeed;
            else
                targetSpeed = runSpeed;
        }

        // Suavizar cambios de velocidad
        currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, movementSmoothing * 10f * Time.deltaTime);

        // Actualizar dirección de movimiento
        if (desiredMoveDirection.magnitude > 0.1f)
        {
            moveDirection = Vector3.Lerp(moveDirection, desiredMoveDirection, movementSmoothing * 10f * Time.deltaTime);
            
            // Rotar el personaje hacia la dirección de movimiento
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        // Mover el personaje
        Vector3 moveVector = moveDirection * currentSpeed * Time.deltaTime;
        characterController.Move(moveVector);
    }

    void HandleJump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    void ApplyGravity()
    {
        velocity.y += gravity * Time.deltaTime;
        Vector3 gravityVector = new Vector3(0, velocity.y, 0) * Time.deltaTime;
        characterController.Move(gravityVector);
    }

    // Métodos públicos para obtener información
    public bool IsMoving()
    {
        return currentSpeed > 0.1f;
    }

    public float GetCurrentSpeed()
    {
        return currentSpeed;
    }

    public bool IsGrounded()
    {
        return isGrounded;
    }

    // Visualización en el editor
    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = isGrounded ? Color.green : Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundDistance);
        }
    }
}
