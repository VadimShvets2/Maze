using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [Tooltip("If checked, the checkpoint will only be activated once.")]
    [SerializeField] private bool triggerOnce = true;
    
    [Tooltip("Offset applied to the checkpoint's location to determine the respawn coordinates.")]
    [SerializeField] private Vector3 respawnOffset = new Vector3(0, 1f, 0);

    private bool isActivated = false;

    private void OnTriggerEnter(Collider other)
    {
        if (triggerOnce && isActivated)
            return;

        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.SetRespawnPosition(transform.position + respawnOffset);
                isActivated = true;
            }
        }
    }
}
