using UnityEngine;

public class SpeedBlock : MonoBehaviour
{
    [Header("New Speeds")]
    [SerializeField] private float newWalkSpeed = 12f;
    [SerializeField] private float newSprintSpeed = 18f;
    [SerializeField] private float newJumpHeight = 6f;

    private void OnCollisionEnter(Collision collision)
    {
        HandleSpeedChange(collision.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        HandleSpeedChange(other.gameObject);
    }

    private void HandleSpeedChange(GameObject obj)
    {
        if (obj.CompareTag("Player") && obj.TryGetComponent(out Controller player))
        {
            player.SetMovementSpeeds(newWalkSpeed, newSprintSpeed, newJumpHeight);
            Debug.Log($"Speeds updated! Walk: {newWalkSpeed}, Sprint: {newSprintSpeed}, Jump: {newJumpHeight}");
        }
    }
}
