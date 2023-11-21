/*
EnemyMove cs should handle the movement logic, specifically how zombies navigate the maze
and interact with the player. This includes following the player,
attacking, and possibly retreating or other complex movements.
 */

using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;

public class EnemyMove : MonoBehaviour
{
    private Transform player;
    private NavMeshAgent agent;
    private Animator animator; // Reference to the Animator component
    public float attackRange = 0.6f; // Attack range for the zombie
    public float attackDamage = 0.2f; // Variable for damage inflicted by the enemy

    private void Start()
    {
        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }

        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>(); // Get the Animator component
    }

    void Update()
    {
        if (player != null && agent != null && agent.isActiveAndEnabled && agent.isOnNavMesh)
        {
            // Continuously update the destination to the player's position
            agent.SetDestination(player.position);

            LookAtPlayer();

            // Check the distance to the player and attack if in range
            if (Vector3.Distance(transform.position, player.position) <= attackRange)
            {
                animator.SetTrigger("Attack"); // Trigger the attack animation
                
                /**/

                // Safely call the TakeDamage method on the player
                PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(attackDamage);
                }
                else
                {
                    Debug.LogError("PlayerHealth component not found on the player!");
                }
                
            }
            else
            {
                animator.SetTrigger("Walk"); // Trigger the walk when player isn't near
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






/*

using UnityEngine;
using UnityEngine.AI;

public class enemymove : MonoBehaviour
{
    private Transform player;
    private NavMeshAgent agent;
    private Animator animator; // Reference to the Animator component

    public float attackRange; // Variable for attack range
    public float attackDamage; // Variable for damage inflicted by the enemy
    private bool playerInAttackRange; // This should be updated based on your game logic

    private void Start()
    {
        //get the player object
        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null) {  player = playerObject.transform; }

        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (player != null && agent != null && agent.isActiveAndEnabled && agent.isOnNavMesh)
        {
            // Continuously update the destination to the player's position
            agent.SetDestination(player.position);
            LookAtPlayer();

            playerInAttackRange = Vector3.Distance(player.position, transform.position) <= attackRange;
            if (playerInAttackRange){ AttackPlayer(); }
        }

       
        
        //fixing the enemy floating:
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

    private void AttackPlayer()
    {
        // Trigger the attack animation
        animator.SetTrigger("Attack");
        //Debug.Log("Attack animation trigger");

        /*
        // Safely call the TakeDamage method on the player
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(attackDamage);
        }
        else
        {
            Debug.LogError("PlayerHealth component not found on the player!");
        }
        *//*
    }


} */
