using UnityEngine;
using Photon.Pun;

// Script responsável pelos movimentos do jogador
// Classe roda no prefab (deve está na pasta Resource - por conta do Photon Pun) player no child Char1
public class PlayerController : MonoBehaviourPun
{
    //public float acceleration = 1f;        // A taxa de aceleração
    public float moveSpeed = 6f;     // Velocidade máxima ao caminhar
    public Animator animator;              // Referência ao Animator
    private CharacterController characterController;  // Componente CharacterController
    private Transform cameraTransform;
    private float verticalSpeed = -9.81f;
    private float gravity = -9.81f;
    private LayerMask floor;

    private Transform feet;
    private bool isGrounded;

    public float rotationSpeed = 10f;


    void Start()
    {
        feet = transform.Find("Feet").transform;
        characterController = GetComponent<CharacterController>();
        cameraTransform = FindObjectOfType<Camera>().transform;
        floor = LayerMask.GetMask("Floor");
    }

    void Update()
    {
        if (photonView.IsMine)
        {
            Movement();
        }
    }

    private void Movement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(horizontal, 0, vertical);

        movement = cameraTransform.TransformDirection(movement);
        movement.y = 0;

        characterController.Move(moveSpeed * Time.deltaTime * movement);

        if (movement != Vector3.zero)
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), Time.deltaTime * rotationSpeed);

        animator.SetInteger("movementState", movement != Vector3.zero ? 1 : 0);

        isGrounded = Physics.CheckSphere(feet.position, 0.3f, floor);
        animator.SetBool("isGrounded", isGrounded);
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Keypad0)) && isGrounded) // keypad0 == X no Playstation e A no xbox
        {
            verticalSpeed = 5f;
            animator.SetTrigger("Jump");
        }
        if (verticalSpeed > gravity) verticalSpeed += gravity * Time.deltaTime;
        characterController.Move(new Vector3(0, verticalSpeed, 0) * Time.deltaTime);
    }
}
