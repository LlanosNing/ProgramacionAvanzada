using System.Collections;
using System.Collections.Generic;
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

    [Header("Control de Movimiento")]
    [SerializeField] private float acceleration = 20f;
    [SerializeField] private float deceleration = 25f;
    [SerializeField] private bool instantStop = false;

    [Header("Rotación del Cuerpo")]
    [SerializeField] private float maxBodyRotation = 90f; // Máximo 90 grados (±90°)
    [SerializeField] private float bodyRotationSpeed = 4f; // Velocidad de rotación hacia cámara
    [SerializeField] private float returnToForwardSpeed = 3f; // Velocidad para volver al frente
    [SerializeField] private float rotationDeadZone = 5f; // Zona muerta
    [SerializeField] private float moveRotationSpeed = 12f; // Velocidad al moverse

    [Header("Salto y Gravedad")]
    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] private float gravity = -20f;

    [Header("Detección de Suelo")]
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private float groundCheckDistance = 0.1f;
    [SerializeField] private LayerMask groundMask;

    [Header("Referencias")]
    [SerializeField] private Transform cameraTransform;

    // Componentes
    private CharacterController characterController;

    // Variables de movimiento
    private Vector3 currentVelocity;
    private Vector3 moveDirection;
    private float currentSpeed;
    private bool isGrounded;
    private float verticalVelocity;
    private bool isMoving;

    // Variables de rotación
    private Quaternion forwardRotation; // Rotación "hacia delante" del personaje
    private bool shouldReturnToForward = false;

    void Start()
    {
        characterController = GetComponent<CharacterController>();

        if (cameraTransform == null)
        {
            if (Camera.main != null)
                cameraTransform = Camera.main.transform;
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        forwardRotation = transform.rotation;

        //añadir la funcion del consumible al callback de consumible utilizado
        ConsumibleSystem.onConsumibleUsed += ConsumibleUsed;
    }

    void Update()
    {
        CheckGround();
        HandleMovement();
        HandleBodyRotation();
        HandleJump();
        ApplyGravity();
    }

    void CheckGround()
    {
        Vector3 checkPosition = transform.position - new Vector3(0, characterController.height / 2f - groundCheckDistance, 0);
        isGrounded = Physics.CheckSphere(checkPosition, groundCheckRadius, groundMask);

        if (isGrounded && verticalVelocity < 0)
        {
            verticalVelocity = -2f;
        }
    }

    void HandleMovement()
    {
        // Obtener input
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 inputDirection = new Vector3(horizontal, 0f, vertical).normalized;

        // Calcular dirección relativa a la cámara
        Vector3 cameraForward = cameraTransform.forward;
        Vector3 cameraRight = cameraTransform.right;
        cameraForward.y = 0f;
        cameraRight.y = 0f;
        cameraForward.Normalize();
        cameraRight.Normalize();

        Vector3 desiredMoveDirection = (cameraForward * vertical + cameraRight * horizontal).normalized;

        // Determinar velocidad objetivo
        float targetSpeed = 0f;

        if (inputDirection.magnitude >= 0.1f)
        {
            isMoving = true;

            // Determinar velocidad
            if (Input.GetKey(KeyCode.LeftShift))
                targetSpeed = sprintSpeed;
            else if (Input.GetKey(KeyCode.LeftControl))
                targetSpeed = walkSpeed;
            else
                targetSpeed = runSpeed;

            // Actualizar dirección de movimiento
            moveDirection = desiredMoveDirection;

            // Al moverse, actualizar la rotación "hacia delante"
            forwardRotation = Quaternion.LookRotation(moveDirection);
            shouldReturnToForward = false;

            // Rotar personaje hacia la dirección de movimiento
            transform.rotation = Quaternion.Slerp(transform.rotation, forwardRotation, moveRotationSpeed * Time.deltaTime);
        }
        else
        {
            isMoving = false;
        }

        // Aplicar aceleración o desaceleración
        if (instantStop && inputDirection.magnitude < 0.1f)
        {
            currentSpeed = 0f;
        }
        else if (inputDirection.magnitude >= 0.1f)
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, acceleration * Time.deltaTime);
        }
        else
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, 0f, deceleration * Time.deltaTime);
        }

        // Calcular velocidad final
        currentVelocity = moveDirection * currentSpeed;

        // Mover el personaje
        characterController.Move(currentVelocity * Time.deltaTime);
    }

    void HandleBodyRotation()
    {
        // Solo rotar cuando NO se está moviendo
        if (!isMoving)
        {
            // Obtener dirección de la cámara
            Vector3 cameraDirection = cameraTransform.forward;
            cameraDirection.y = 0f;
            cameraDirection.Normalize();

            // Obtener dirección "hacia delante" del personaje
            Vector3 forwardDirection = forwardRotation * Vector3.forward;
            forwardDirection.y = 0f;
            forwardDirection.Normalize();

            // Calcular ángulo entre el "frente" del personaje y la dirección de la cámara
            // Positivo = derecha, Negativo = izquierda
            float angleToCamera = Vector3.SignedAngle(forwardDirection, cameraDirection, Vector3.up);

            // Debug opcional
            // Debug.Log($"Ángulo a cámara: {angleToCamera:F1}°");

            // CLAVE: Si el ángulo es mayor que maxBodyRotation (en cualquier dirección), volver al frente
            if (Mathf.Abs(angleToCamera) > maxBodyRotation)
            {
                shouldReturnToForward = true;
            }

            // Si debe volver al frente
            if (shouldReturnToForward)
            {
                // Volver suavemente a mirar hacia delante
                transform.rotation = Quaternion.Slerp(transform.rotation, forwardRotation, returnToForwardSpeed * Time.deltaTime);

                // Verificar si ya está casi mirando al frente
                float currentAngle = Quaternion.Angle(transform.rotation, forwardRotation);
                if (currentAngle < 1f)
                {
                    shouldReturnToForward = false;
                }
            }
            else
            {
                // Rotación normal limitada
                if (Mathf.Abs(angleToCamera) > rotationDeadZone)
                {
                    // Calcular rotación objetivo limitada
                    // Clamp mantiene el signo (+ o -)
                    float clampedAngle = Mathf.Clamp(angleToCamera, -maxBodyRotation, maxBodyRotation);

                    // Aplicar rotación desde el "frente" del personaje
                    Quaternion targetRotation = forwardRotation * Quaternion.Euler(0, clampedAngle, 0);

                    // Aplicar rotación suave
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, bodyRotationSpeed * Time.deltaTime);
                }
                else
                {
                    // En zona muerta - volver suavemente al frente
                    transform.rotation = Quaternion.Slerp(transform.rotation, forwardRotation, bodyRotationSpeed * 0.5f * Time.deltaTime);
                }
            }
        }
    }

    void HandleJump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    void ApplyGravity()
    {
        verticalVelocity += gravity * Time.deltaTime;
        characterController.Move(new Vector3(0, verticalVelocity, 0) * Time.deltaTime);
    }

    // Métodos públicos
    public bool IsMoving()
    {
        return isMoving;
    }

    public float GetCurrentSpeed()
    {
        return currentSpeed;
    }

    public bool IsGrounded()
    {
        return isGrounded;
    }

    public float GetAngleToCamera()
    {
        Vector3 cameraDir = cameraTransform.forward;
        cameraDir.y = 0f;
        cameraDir.Normalize();
        return Vector3.SignedAngle(forwardRotation * Vector3.forward, cameraDir, Vector3.up);
    }

    // Visualización en el editor
    void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying) return;

        // Ground check
        Vector3 checkPosition = transform.position - new Vector3(0, characterController.height / 2f - groundCheckDistance, 0);
        Gizmos.color = isGrounded ? Color.green : Color.red;
        Gizmos.DrawWireSphere(checkPosition, groundCheckRadius);

        // Dirección "hacia delante" del personaje
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position + Vector3.up, forwardRotation * Vector3.forward * 2f);

        // Dirección actual del personaje
        Gizmos.color = Color.cyan;
        Gizmos.DrawRay(transform.position + Vector3.up * 0.8f, transform.forward * 1.5f);

        // Dirección de la cámara
        if (cameraTransform != null)
        {
            Vector3 cameraDir = cameraTransform.forward;
            cameraDir.y = 0f;
            cameraDir.Normalize();
            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(transform.position + Vector3.up * 0.5f, cameraDir * 2f);
        }

        // Visualizar límites de rotación (45° a cada lado)
        if (!isMoving)
        {
            Gizmos.color = new Color(1, 0, 0, 0.3f);
            Vector3 leftLimit = forwardRotation * Quaternion.Euler(0, -maxBodyRotation, 0) * Vector3.forward * 1.5f;
            Vector3 rightLimit = forwardRotation * Quaternion.Euler(0, maxBodyRotation, 0) * Vector3.forward * 1.5f;

            Gizmos.DrawRay(transform.position + Vector3.up * 0.5f, leftLimit);
            Gizmos.DrawRay(transform.position + Vector3.up * 0.5f, rightLimit);
        }
    }

    //MODIFICACION PARA USAR LOS CONSUMIBLES
    void ConsumibleUsed(ItemInfo consumible)
    {
        //hay que transformar el ItemIn
        Consumible cons = consumible as Consumible;
        //solo cambia la velocidad de movimiento si es un buff de este tipo
        if (cons.moveSpeedAmount != 0)
        {
            //llamar a la corrutina que cambia la velocidad de movimiento con los valores del consumible
            StartCoroutine(MoveSpeedChangeCrt(cons.moveSpeedAmount, cons.duration));
        }
    }

    IEnumerator MoveSpeedChangeCrt(float moveSpeedChange, float duration)
    {
        //modificar la velocidad
        walkSpeed += moveSpeedChange;
        //esperar la duracion del consumible
        yield return new WaitForSeconds(duration);
        //restaurar la velocidad
        walkSpeed -= moveSpeedChange;

    }

}
