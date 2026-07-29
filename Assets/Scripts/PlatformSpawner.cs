using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private Transform player;
    [SerializeField] private GameObject[] platformPrefabs;
    [SerializeField] private GameObject leakPrefab;

    [Header("Plataformas")]
    [SerializeField] private float minX = -7f;
    [SerializeField] private float maxX = 7f;
    [SerializeField] private float minDistanceY = 2f;
    [SerializeField] private float maxDistanceY = 5f;
    [SerializeField] private float maxHorizontalStep = 3f;
    [SerializeField] private int initialPlatforms = 20;
    [SerializeField] private float spawnAheadDistance = 35f;

    [Header("Fugas")]
    [Range(0f, 1f)]
    [SerializeField] private float leakChance = 0.25f;
    [SerializeField] private float leakOffsetY = 0.1f;
    [SerializeField] private float leakHorizontalMargin = 0.5f;

    private float nextY;
    private float lastX;

    private void Start()
    {
        for (int i = 0; i < initialPlatforms; i++)
        {
            SpawnPlatform();
        }
    }

    private void Update()
    {
        while (player.position.y - spawnAheadDistance < nextY)
        {
            SpawnPlatform();
        }
    }

    private void SpawnPlatform()
    {
        float yDistance = Random.Range(
            minDistanceY,
            maxDistanceY
        );

        if (Random.value < 0.1f)
        {
            yDistance = Random.Range(6f, 8f);
        }

        nextY -= yDistance;

        float x = lastX + Random.Range(
            -maxHorizontalStep,
            maxHorizontalStep
        );

        x = Mathf.Clamp(x, minX, maxX);
        lastX = x;

        GameObject prefab = platformPrefabs[
            Random.Range(0, platformPrefabs.Length)
        ];

        GameObject platform = Instantiate(
            prefab,
            new Vector3(x, nextY, 0f),
            Quaternion.identity
        );

        if (leakPrefab != null && Random.value < leakChance)
        {
            SpawnLeak(platform);
        }
    }

    private void SpawnLeak(GameObject platform)
    {
        Collider2D platformCollider =
            platform.GetComponentInChildren<Collider2D>();

        if (platformCollider == null)
        {
            return;
        }

        float minimumX =
            platformCollider.bounds.min.x +
            leakHorizontalMargin;

        float maximumX =
            platformCollider.bounds.max.x -
            leakHorizontalMargin;

        float randomX = platformCollider.bounds.center.x;

        if (minimumX < maximumX)
        {
            randomX = Random.Range(minimumX, maximumX);
        }

        float y =
            platformCollider.bounds.max.y +
            leakOffsetY;

        // Solo genera la fuga.
        // No necesita saber si tiene un script Leak.
        Instantiate(
            leakPrefab,
            new Vector3(randomX, y, 0f),
            Quaternion.identity
        );
    }
}