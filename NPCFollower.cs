using UnityEngine;
using Photon.Pun;
using System.Collections;

// Script responsável pelos movimentos dos npcs da party
// Classe roda no prefab (deve estar na pasta Resource - por conta do Photon Pun) player no child Char2, Char3
public class NPCFollower : MonoBehaviourPun 
{
    // Movimentos
    public float followDistance = 2.5f;
    public float moveSpeed = 4f;
    public float rotationSpeed = 10f;
    private Animator animator; // componente proprio
    private CharacterController characterController; // componente proprio
    // Pulo
    private Transform feet; // Feet dentro de Char2, Char3,Char1
    private bool isGrounded;
    private float verticalSpeed = -9.81f;
    private float gravity = -9.81f;
    private LayerMask floor; // layer do chão
    // seguir
    public Transform player;             // Referência ao jogador que será seguido
    private bool hasStartedFollowing = false; // iniciou a seguir
    public float avoidDistance = 1.5f;    // Distância mínima para evitar colisão fictícia com outros NPCs
    public float startFollowDelay = 1f; // tempo para comelar a seguir
    void Start()
    {
        // Encontrar o jogador ao qual o NPC deve seguir (nesse caso, o jogador local)
        if (!player)
        {
            player = FindObjectOfType<PlayerController>().transform;
        }
        // Buscar componentes necessários
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        feet = transform.Find("Feet").transform;
        floor = LayerMask.GetMask("Floor");

        StartCoroutine(StartFollowingAfterDelay());
    }

    void Update()
    {
        if (photonView.IsMine && hasStartedFollowing)
        {
            FollowPlayer();
        }

    }

    private void FollowPlayer()
    {
        // Verificar a distância entre o NPC e o jogador
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Se a distância for maior que a distância mínima, mover o NPC em direção ao jogador
        if (distanceToPlayer > followDistance)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            direction.y = 0; // Manter a direção no plano horizontal

            AvoidOtherNPCs(ref direction);

            // Mover NPC em direção ao jogador
            characterController.Move(direction * moveSpeed * Time.deltaTime);

            // Rotacionar o NPC para ficar voltado ao jogador
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        if (characterController.velocity.magnitude > 0)
        {
            animator.SetInteger("movementState", 1);
        }
        else
        {
            animator.SetInteger("movementState", 0);
        }

        // Verificar se o NPC está no chão
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

    private void AvoidOtherNPCs(ref Vector3 direction)
    {
        NPCFollower[] npcs = FindObjectsOfType<NPCFollower>();

        foreach (var npc in npcs)
        {
            if (npc != this)
            {
                float distanceToNPC = Vector3.Distance(transform.position, npc.transform.position);

                if (distanceToNPC < avoidDistance)
                {
                    // Calcular uma direção de desvio para evitar colisão fictícia
                    Vector3 avoidDirection = (transform.position - npc.transform.position).normalized;
                    avoidDirection.y = 0;

                    // Ajustar a direção principal somando uma pequena porção da direção de desvio
                    direction += avoidDirection * 0.5f;
                }
            }
        }

        direction = direction.normalized; // Normalizar a direção resultante
    }

    private IEnumerator StartFollowingAfterDelay()
    {
        yield return new WaitForSeconds(startFollowDelay);
        hasStartedFollowing = true; // Agora pode começar a seguir o jogador
    }
}