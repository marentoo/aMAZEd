/*
EnemyMove cs should handle the movement logic, specifically how zombies navigate the maze
and interact with the player. This includes following the player,
attacking, and possibly retreating or other complex movements.
 */


using UnityEngine;
using UnityEngine.AI;

public class enemymove : MonoBehaviour
{
    private Transform player;
    private NavMeshAgent agent;

    private void Start()
    {
        GameObject playerObject = GameObject.FindWithTag("Player");

        if (playerObject != null)
        {
            player = playerObject.transform;
        }

        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (player != null && agent != null && agent.isActiveAndEnabled && agent.isOnNavMesh)
        {
            // Continuously update the destination to the player's position
            agent.SetDestination(player.position);

            LookAtPlayer();

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

}
