using UnityEngine;

public class Resume : Button
{
    protected override void OnClick()
    {
        Time.timeScale = 1f; 
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        GameSettings.Pause = false;
        
        // Find the EscPanel script in the scene to hide its specific panel
        EscPanel panelScript = Object.FindFirstObjectByType<EscPanel>();
        if (panelScript != null && panelScript.escPanel != null)
        {
            panelScript.escPanel.SetActive(false);
        }
        else
        {
            // Fallback: hierarchy climb
            Transform current = transform;
            while (current.parent != null && current.parent.name != "Canvas")
            {
                current = current.parent;
            }
            current.gameObject.SetActive(false);
        }
    } 
}
