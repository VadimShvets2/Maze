using UnityEngine;
using UnityEngine.UI;

public class CrosshairManager : MonoBehaviour
{
    [Header("Crosshair Settings")]
    [SerializeField] private RectTransform crosshairRect;
    [SerializeField] private Image crosshairImage;
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color interactColor = Color.green;
    [SerializeField] private float interactDistance = 5f;

    void Start()
    {
        if (crosshairRect == null) crosshairRect = GetComponent<RectTransform>();
        if (crosshairImage == null) crosshairImage = GetComponent<Image>();
        
        // Ensure it's centered
        if (crosshairRect != null)
        {
            crosshairRect.anchoredPosition = Vector2.zero;
        }
    }

    void Update()
    {
        if (Camera.main == null) return;
        CheckForInteractables();
    }

    private void CheckForInteractables()
    {
        // Simple raycast from camera to see if we are looking at something interactable
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        if (Physics.Raycast(ray, out RaycastHit hit, interactDistance))
        {
            if (hit.collider.TryGetComponent(out Interactable _))
            {
                SetCrosshairColor(interactColor);
                return;
            }
        }
        
        SetCrosshairColor(normalColor);
    }

    private void SetCrosshairColor(Color color)
    {
        if (crosshairImage != null)
        {
            crosshairImage.color = color;
        }
    }
}

