using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private Transform player;
    [SerializeField] private GameObject[] platformPrefabs;

    [Header("Objetos")]
    [SerializeField] private GameObject leakPrefab;
    [SerializeField] private GameObject enemyPrefab;

    [Header("Probabilidades")]
    [Range(0f, 1f)]
    [SerializeField] private float leakChance = 0.25f;

    [Range(0f, 1f)]
    [SerializeField] private float enemyChance = 0.20f;

    [Header("Posición de fugas")]
    [SerializeField] private float leakOffsetY = 0.1f;
    [SerializeField] private float leakHorizontalMargin = 0.5f;

    [Header("Posición de enemigos")]
    [SerializeField] private float enemyOffsetY = 0.1f;
    [SerializeField] private float enemyHorizontalMargin = 0.5f;

    [Header("Plataformas")]
    [SerializeField] private float minX = -7f;
    [SerializeField] private float maxX = 7f;

    [SerializeField] private float minDistanceY = 2f;
    [SerializeField] private float maxDistanceY = 5f;

    [SerializeField] private float maxHorizontalStep = 3f;

    [SerializeField] private int initialPlatforms = 20;
    [SerializeField] private float spawnAheadDistance = 35f;

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

        // 10% de probabilidad de caída larga
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

        SpawnPlatformObject(platform);
    }

    private void SpawnPlatformObject(GameObject platform)
    {
        float randomValue = Random.value;

        // Primero comprueba si genera una fuga
        if (
            leakPrefab != null &&
            randomValue < leakChance
        )
        {
            SpawnObjectOnPlatform(
                platform,
                leakPrefab,
                leakOffsetY,
                leakHorizontalMargin
            );

            return;
        }

        // Si no generó fuga, comprueba el enemigo
        if (
            enemyPrefab != null &&
            randomValue < leakChance + enemyChance
        )
        {
            SpawnObjectOnPlatform(
                platform,
                enemyPrefab,
                enemyOffsetY,
                enemyHorizontalMargin
            );
        }
    }

    private void SpawnObjectOnPlatform(
        GameObject platform,
        GameObject objectPrefab,
        float offsetY,
        float horizontalMargin
    )
    {
        Collider2D platformCollider =
            platform.GetComponentInChildren<Collider2D>();

        if (platformCollider == null)
        {
            return;
        }

        float minimumX =
            platformCollider.bounds.min.x +
            horizontalMargin;

        float maximumX =
            platformCollider.bounds.max.x -
            horizontalMargin;

        float randomX =
            platformCollider.bounds.center.x;

        if (minimumX < maximumX)
        {
            randomX = Random.Range(
                minimumX,
                maximumX
            );
        }

        float y =
            platformCollider.bounds.max.y +
            offsetY;

        Instantiate(
            objectPrefab,
            new Vector3(randomX, y, 0f),
            Quaternion.identity
        );
    }
}