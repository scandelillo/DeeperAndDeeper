using UnityEngine;

public class DestroyPlatform : MonoBehaviour
{
    [SerializeField] private float destroyDistance = 15f;

    private Transform player;

    private void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            player = playerObject.transform;
        }
    }

    private void Update()
    {
        if (player == null)
            return;

        if (transform.position.y > player.position.y + destroyDistance)
        {
            Destroy(gameObject);
        }
    }
}