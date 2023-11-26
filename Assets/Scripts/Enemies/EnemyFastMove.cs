/*
EnemyMove cs should handle the movement logic, specifically how zombies navigate the maze
and interact with the player. This includes following the player,
attacking, and possibly retreating or other complex movements.
 */

using UnityEngine;
using UnityEngine.AI;

public class EnemyFastMove : MonoBehaviour
{
    private Transform player;
    private NavMeshAgent agent;
    private Animator animator;

    // New properties specific to the FastZombie
    public float fastAttackRange = 0.6f;
    public float fastMovementSpeed = 1.0f;
    public float attackDamage = 0.3f; // This could be different for the FastZombie

    private void Start()
    {
        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }

        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>(); // Get the Animator component

        // Set the movement speed to be faster for the FastZombie
        agent.speed = fastMovementSpeed;
    }

    void Update()
    {
        if (player != null && agent != null && agent.isActiveAndEnabled && agent.isOnNavMesh)
        {
            agent.SetDestination(player.position);
            LookAtPlayer();

            if (Vector3.Distance(transform.position, player.position) <= fastAttackRange)
            {
                animator.SetTrigger("Attack"); // Assume you have a different attack animation called "FastAttack"
                /**/
                PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(attackDamage);
                }
            }
            else
            {
                animator.SetBool("Run", true); // Assume you have a boolean parameter in Animator to control the walking animation
            }
        }

        // Fixing the enemy floating (existing code)
        RaycastHit hit;
        if (Physics.Raycast(transform.position, -Vector3.up, out hit))
        {
            // Assuming the maze's floor is at y = 0
            float groundHeight = hit.point.y;
            Vector3 currentPosition = transform.position;
            currentPosition.y = groundHeight;
            transform.position = currentPosition;
        }
    }

    private void LookAtPlayer()
    {

        if (player != null)
        {
            Vector3 direction = player.position - transform.position;
            direction.y = 0; // Keep the zombie upright, ignore vertical difference
            Quaternion rotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 5.0f); // Smooth rotation towards the player
        }
    }
}