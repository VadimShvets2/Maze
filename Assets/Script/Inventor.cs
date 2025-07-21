using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Inventor : MonoBehaviour
{
    private int ActiveSlot = 0;
    private int LateSlot = 0;
    private Vector3 UnActive = new Vector3(1, 1, 1);
    private Vector3 Active = new Vector3(2, 2, 2);
    [SerializeField] private float Taking_Distance;
    
    [SerializeField] List<InventorSlot> slots = new List<InventorSlot>();
    private void Scroll()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if (ActiveSlot != slots.Count - 1)
            {
                LateSlot = ActiveSlot;
            }

            ActiveSlot = Mathf.Clamp(ActiveSlot + 1, 0, slots.Count - 1);
            SelectSlot();
        }
        
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if (ActiveSlot != 0)
            {
                LateSlot = ActiveSlot;
            }
            ActiveSlot = Mathf.Clamp(ActiveSlot - 1, 0, slots.Count - 1);
            SelectSlot();
        }
        
    }

    private void Switch()
    {
        if (Input.GetKey(KeyCode.Alpha1)&& ActiveSlot!= 0)
        {
            LateSlot = ActiveSlot;
            ActiveSlot = 0;
            SelectSlot();
        }

        if (Input.GetKey(KeyCode.Alpha2)&& ActiveSlot!= 1)
        {
            LateSlot = ActiveSlot;
            ActiveSlot = 1;
            SelectSlot();
        }

        if (Input.GetKey(KeyCode.Alpha3)&& ActiveSlot!= 2)
        {
            LateSlot = ActiveSlot;
            ActiveSlot = 2;
            SelectSlot();
        }

        if (Input.GetKey(KeyCode.Alpha4)&& ActiveSlot!= 3)
        {
            LateSlot = ActiveSlot;
            ActiveSlot = 3;
            SelectSlot();
        }
    }

    private void SelectSlot()
    {
        slots[ActiveSlot].Transformation.localScale = Active; 
        slots[LateSlot].Transformation.localScale = UnActive;
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
      Scroll(); 
      Switch();
      Interact();
    }
    void Interact()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            LayerMask Ray = LayerMask.GetMask("Interactable");
            RaycastHit hit;
            Debug.DrawRay(transform.position, transform.forward * Taking_Distance, Color.red);
            if (Physics.Raycast(transform.position, transform.forward, out hit, Taking_Distance,Ray))
            {
            Interactable interactable = hit.collider.GetComponent<Interactable>();
            interactable.Interact(slots[ActiveSlot]); 
            }   
        }
        
    }
}

