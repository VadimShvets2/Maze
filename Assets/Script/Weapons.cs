using UnityEngine;

public class Weapons : MonoBehaviour
{
    public Gun Weapon;

    public void Interact()
    {
        Weapon.Interact();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

public class Gun : Interactable
{
    public override void Interact()
    {
        
    }
}