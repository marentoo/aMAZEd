using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class MazeNavMeshBuilder : MonoBehaviour
{
    public void BuildNavMesh()
    {
        var navMeshSurface = gameObject.AddComponent<NavMeshSurface>();
        // Configure navMeshSurface settings if needed
        // e.g., navMeshSurface.collectObjects = CollectObjects.Children;

        navMeshSurface.BuildNavMesh();
    }
}
