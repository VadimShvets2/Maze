using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float damage = 10f;
    [SerializeField] private float lifetime = 3f;
    
    private void Start()
    {
        // Automatically destroy the projectile after its lifetime expires
        Destroy(gameObject, lifetime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Try to find a health component on the object we hit
        if (collision.gameObject.TryGetComponent(out PlayerHealth health))
        {
            health.TakeDamage(damage);
        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            // If there's an enemy script, we can call it here.
            // For now, let's assume we might want to damage enemies later.
            // collision.gameObject.SendMessage("TakeDamage", damage, SendMessageOptions.DontRequireReceiver);
        }

        // Destroy the projectile on impact
        Destroy(gameObject);
    }
}
