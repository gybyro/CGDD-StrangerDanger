using UnityEngine;
using System.Collections.Generic;

public class TreeSpawner : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public GameObject[] treePrefabs;

    [Header("Spawn Settings")]
    public float spawnDistanceAhead = 30f;
    public float spawnInterval = 0.3f;   // how often new trees spawn

    [Header("Random Spawn Range")]
    public float minX = -30f;
    public float maxX = 30f;
    public float yPosition = 0f;

    [Header("No Spawn Zone (X range)")]
    public float noSpawnMinX = -12f;
    public float noSpawnMaxX = 12f;

    [Header("Random Z Offset (per spawned tree)")]
    public float minZOffset = -5f;
    public float maxZOffset = 20f;

    [Header("Initial Forest")]
    public int initialTrees = 60;
    public float initialMinZOffset = -10f;
    public float initialMaxZOffset = 80f;

    [Header("Movement & Cleanup")]
    public float moveSpeed = 10f;
    public float deleteDistance = 60f;

    private List<GameObject> activeTrees = new List<GameObject>();

    void Start()
    {
        // Spawn forest before gameplay begins
        SpawnInitialForest();

        // Start spawning new trees over time
        InvokeRepeating(nameof(SpawnTree), spawnInterval, spawnInterval);
    }

    void Update()
    {
        MoveAndCleanupTrees();
    }

    //------------------------------------------
    // INITIAL FOREST
    //------------------------------------------

    void SpawnInitialForest()
    {
        for (int i = 0; i < initialTrees; i++)
        {
            float x = GetValidRandomX();
            float z = player.position.z + Random.Range(initialMinZOffset, initialMaxZOffset);

            Vector3 pos = new Vector3(x, yPosition, z);

            GameObject prefab = treePrefabs[Random.Range(0, treePrefabs.Length)];
            GameObject tree = Instantiate(prefab, pos, Quaternion.identity);

            activeTrees.Add(tree);
        }
    }

    //------------------------------------------
    // ONGOING SPAWNING
    //------------------------------------------

    void SpawnTree()
    {
        float x = GetValidRandomX();

        float z = player.position.z +
                  spawnDistanceAhead +
                  Random.Range(minZOffset, maxZOffset);

        Vector3 pos = new Vector3(x, yPosition, z);

        GameObject prefab = treePrefabs[Random.Range(0, treePrefabs.Length)];
        GameObject tree = Instantiate(prefab, pos, Quaternion.identity);

        activeTrees.Add(tree);
    }

    //------------------------------------------
    // RANDOM VALID X (avoids no-spawn lane)
    //------------------------------------------

    float GetValidRandomX()
    {
        float x;
        do
        {
            x = Random.Range(minX, maxX);
        }
        while (x >= noSpawnMinX && x <= noSpawnMaxX);

        return x;
    }

    //------------------------------------------
    // MOVE & DELETE TREES
    //------------------------------------------

    void MoveAndCleanupTrees()
    {
        for (int i = activeTrees.Count - 1; i >= 0; i--)
        {
            GameObject tree = activeTrees[i];
            if (tree == null)
            {
                activeTrees.RemoveAt(i);
                continue;
            }

            // Move tree backwards
            tree.transform.Translate(Vector3.back * moveSpeed * Time.deltaTime);

            // Delete trees far behind player
            if (player.position.z - tree.transform.position.z > deleteDistance)
            {
                Destroy(tree);
                activeTrees.RemoveAt(i);
            }
        }
    }
}
