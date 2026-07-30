using UnityEngine;

public class RepeatBackground : MonoBehaviour
{
    public Transform cameraTransform;
    public float backgroundHeight;

    void Update()
    {
        if (cameraTransform.position.y < transform.position.y - backgroundHeight)
        {
            transform.position += Vector3.down * backgroundHeight * 2f;
        }
    }
}
