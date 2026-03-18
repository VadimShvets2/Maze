using UnityEngine;

public class BouncyPad : MonoBehaviour
{
    public float BounceForce = 15f;
    public bool useVelocity = false; // Toggle between methods

    void OnCollisionEnter(Collision collision)
    {
        HandleBounce(collision.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        HandleBounce(other.gameObject);
    }

    private void HandleBounce(GameObject obj)
    {
        if (obj.CompareTag("Player") && obj.TryGetComponent(out Controller player))
        {
            player.ApplyBounce(BounceForce);
            Debug.Log("Bounce applied to CharacterController!");
        }
    }
}