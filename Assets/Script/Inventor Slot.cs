using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class InventorSlot : MonoBehaviour
{
    [SerializeField] private Image Boarder;

    [SerializeField] private Image Item;
    public RectTransform Transformation;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       Transformation = GetComponent<RectTransform>(); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
