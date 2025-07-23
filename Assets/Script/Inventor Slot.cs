using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class InventorSlot : MonoBehaviour
{
    [SerializeField] private Image Boarder;

    [SerializeField] private Image ItemImage;
    public RectTransform Transformation;
    public Image Item;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
       Transformation = GetComponent<RectTransform>(); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
