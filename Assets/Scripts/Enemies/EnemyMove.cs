/*
EnemyMove cs should handle the movement logic, specifically how zombies navigate the maze
and interact with the player. This includes following the player,
attacking, and possibly retreating or other complex movements.

They only attac when they see the player.
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
    public float sightRange = 10f; // Sight range for spotting the player
    public float wanderRadius = 5f; // Radius for random wandering
    
    public float attackDamage = 0.2f;
    public float wanderTimer = 5f; // Time interval for changing wander destination

    private float timer; // To keep track of wandering time

    public float stoppingDistance = 0.8f;
    public AudioSource attackAudioSource; // Assign this in the inspector
    public AudioSource seeAudioSource;
    private void Start()
    {
        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }

        agent = GetComponent<NavMeshAgent>();
        agent.stoppingDistance = stoppingDistance;
        // Enable local avoidance
        agent.avoidancePriority = Random.Range(0, 100);

        animator = GetComponent<Animator>(); // Get the Animator component

        timer = wanderTimer; // Initialize timer

    }

    void Update()
    {
        if (player != null && agent != null && agent.isActiveAndEnabled && agent.isOnNavMesh)
        {
            float distance = Vector3.Distance(transform.position, player.position);

            // Check for line of sight
            if (distance <= sightRange && CanSeePlayer())
            {
                // Player is in sight range and visible
                agent.SetDestination(player.position);
                LookAtPlayer();

                // Check the distance to the player and attack if in range
                if (distance <= attackRange)
                {
                    AttackPlayer();
                }
            }
            else
            {
                // Player is not in sight, wander around
                Wander();
            }
        }

        FixFloating();
    }

    private bool CanSeePlayer()
    {
        RaycastHit hit;
        Vector3 direction = player.position - transform.position;

        if (Physics.Raycast(transform.position, direction, out hit, sightRange))
        {
            if (hit.transform == player)
            {
                seeAudioSource.Play(); // Play the attack sound

                return true; // Player is in line of sight

            }
        }
        return false; // No line of sight to player
    }

    private void Wander()
    {
        animator.SetTrigger("Walk");
        timer += Time.deltaTime;

        if (timer >= wanderTimer)
        {
            Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
            agent.SetDestination(newPos);
            timer = 0;
        }
    }

    private static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;

        randDirection += origin;
        NavMeshHit navHit;

        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

        return navHit.position;
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

    //private void AttackPlayer()
    //{
    //    animator.SetTrigger("Attack"); // Trigger the attack animation
    //    // Implement attack logic here
    //    PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
    //    if (playerHealth != null)
    //    {
    //        playerHealth.TakeDamage(attackDamage);
    //    }
    //}
    private void AttackPlayer()
    {
        animator.SetTrigger("Attack"); // Trigger the attack animation
                                       


        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(attackDamage);
            attackAudioSource.Play(); // Play the attack sound

        }
    }

    private void FixFloating()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, -Vector3.up, out hit))
        {
            float groundHeight = hit.point.y;
            Vector3 currentPosition = transform.position;
            currentPosition.y = groundHeight;
            transform.position = currentPosition;
        }
    }
}




