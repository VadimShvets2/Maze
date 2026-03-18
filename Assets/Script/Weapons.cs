using UnityEngine;
using DG.Tweening;

using DG.Tweening;
    
[RequireComponent(typeof(AudioSource))]
public class Weapons : Interactable
{
    

    [SerializeField] private Sprite icon;
    [SerializeField] private Vector3 holdPosition = new Vector3(-4, 0, 8);
    [SerializeField] private Vector3 holdRotation = new Vector3(-15, -10, 0);
    
    [Header("Shooting")]
    [SerializeField] private AudioClip fireSound;
    [SerializeField] private float fireRate = 0.2f;
    [SerializeField] private Vector3 recoilStrength = new Vector3(0, 0, -0.2f);
    [SerializeField] private Vector3 recoilRotationStrength = new Vector3(-10, 5, 0);
    [SerializeField] private float recoilDuration = 0.1f;
    
    [Header("Projectile Settings")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform muzzlePoint;
    [SerializeField] private float projectileForce = 5000f; // Increased for realism

    [Header("VFX")]
    [SerializeField] private GameObject muzzleFlashPrefab; // Assign your smoke/fire effect here
    [SerializeField] private float muzzleFlashDuration = 0.1f;

    [Header("Aim Alignment")]
    [SerializeField] private float aimSmoothing = 15f;
    [SerializeField] private float maxAimDistance = 100f;

    
    private Quaternion currentAimRotation;
    private bool isHeld = false;
    private float nextFireTime = 0f;
    private AudioSource audioSource;
    private Vector3 currentRecoil;
    private Vector3 currentRecoilRotation;

    public override void Interact(InventorSlot Slot, Transform holder)
    {
        // 0. Stop idle animation
        transform.DOKill();

        // 1. Parent/Hold logic
        transform.SetParent(holder);
        transform.localPosition = holdPosition;
        currentAimRotation = Quaternion.Euler(holdRotation);
        transform.localRotation = currentAimRotation;
        
        // 2. Disable Physics
        if (TryGetComponent(out Rigidbody rb)) rb.isKinematic = true;
        if (TryGetComponent(out Collider col)) col.enabled = false;

        // 3. UI logic
        Slot.Item.sprite = icon;
        Slot.Item.enabled = true;
        Slot.ConnectedItem = gameObject;
        
        isHeld = true;
        if(audioSource == null) audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (isHeld)
        {
            HandleShooting();
            HandleAimAlignment();
            
            transform.localPosition = holdPosition + currentRecoil;
            transform.localRotation = currentAimRotation * Quaternion.Euler(currentRecoilRotation);
        }
    }

    private void HandleAimAlignment()
    {
        if (muzzlePoint == null || Camera.main == null) return;

        // 1. Find the target point (crosshair hit point)
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        Vector3 targetPoint = ray.GetPoint(maxAimDistance);
        
        // Raycast to find actual hit point, ignoring the player/gun if possible
        // For now, a simple raycast
        if (Physics.Raycast(ray, out RaycastHit hit, maxAimDistance))
        {
            targetPoint = hit.point;
        }

        // 2. Calculate the rotation required for the gun to point at that target
        // We want muzzlePoint.forward to point at targetPoint
        Vector3 dirToTarget = (targetPoint - muzzlePoint.position).normalized;
        
        if (dirToTarget != Vector3.zero)
        {
            Quaternion targetWorldRotation = Quaternion.LookRotation(dirToTarget);
            
            // We need to account for the muzzle's local offset within the weapon
            // WeaponRotation * muzzleLocalRotation = targetWorldRotation
            // WeaponRotation = targetWorldRotation * Inverse(muzzleLocalRotation)
            Quaternion desiredWeaponRotation = targetWorldRotation * Quaternion.Inverse(muzzlePoint.localRotation);
            
            // Convert to local rotation relative to the holder (parent)
            Quaternion desiredLocalRotation = Quaternion.Inverse(transform.parent.rotation) * desiredWeaponRotation;

            // 3. Smoothly Lerp to the target rotation
            currentAimRotation = Quaternion.Slerp(currentAimRotation, desiredLocalRotation, Time.deltaTime * aimSmoothing);
        }
    }

    void HandleShooting()
    {
        if (Input.GetMouseButtonDown(0) && Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireRate;
            Shoot();
        }
    }

    void Shoot()
    {
        // Sound
        if (fireSound) audioSource.PlayOneShot(fireSound);
        
        // Recoil
        // Punch the currentRecoil vector (kick back and return)
        // We kill any previous recoil to keep it snappy
        DOTween.Kill(this, "recoil"); 
        currentRecoil = Vector3.zero;
        currentRecoilRotation = Vector3.zero;
        
        // Position Recoil
        DOTween.Punch(() => currentRecoil, x => currentRecoil = x, recoilStrength, recoilDuration, 10, 1)
            .SetId("recoil");
            
        // Rotation Recoil
        DOTween.Punch(() => currentRecoilRotation, x => currentRecoilRotation = x, recoilRotationStrength, recoilDuration, 10, 1)
            .SetId("recoil");

        // Muzzle Flash
        if (muzzleFlashPrefab && muzzlePoint)
        {
            GameObject flash = Instantiate(muzzleFlashPrefab, muzzlePoint.position, muzzlePoint.rotation);
            flash.transform.SetParent(muzzlePoint); // Follow the gun during recoil
            Destroy(flash, muzzleFlashDuration);
        }

        // Projectile Instantiation
        if (projectilePrefab && muzzlePoint)
        {
            GameObject bullet = Instantiate(projectilePrefab, muzzlePoint.position, muzzlePoint.rotation);
            if (bullet.TryGetComponent(out Rigidbody rb))
            {
                // Set to continuous to prevent tunneling through walls at high speeds
                rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
                rb.AddForce(muzzlePoint.forward * projectileForce);
            }
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    // Update is called once per frame
    
    
        
    
}

