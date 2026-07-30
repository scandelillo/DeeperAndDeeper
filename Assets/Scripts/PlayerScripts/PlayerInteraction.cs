using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    private Leak nearbyLeak;

    // Player Input llama automáticamente este método
    // cuando se ejecuta la acción llamada "Interact".
    public void OnInteract(InputValue value)
    {
        // Evita ejecutar también cuando se suelta el botón
        if (!value.isPressed)
        {
            return;
        }

        if (nearbyLeak != null)
        {
            nearbyLeak.Repair();
            nearbyLeak = null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Leak leak = collision.GetComponentInParent<Leak>();

        if (leak != null)
        {
            nearbyLeak = leak;
            Debug.Log("Puedes reparar esta fuga.");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Leak leak = collision.GetComponentInParent<Leak>();

        if (leak != null && leak == nearbyLeak)
        {
            nearbyLeak = null;
        }
    }
}