using UnityEngine;
using UnityEngine.EventSystems;

public class VassalSelectionManager : MonoBehaviour
{
    void Update()
    {
        // Left-click
        if (Input.GetMouseButtonDown(0))
        {
            // Ignore clicks on UI
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
                return;

            // Cast a ray to detect what was clicked
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                // If it's a vassal, let its OnMouseDown handle selection
                if (hit.collider.GetComponent<VassalController>() != null)
                    return;
            }

            // If it's not a vassal or UI, deselect the current vassal
            VassalController.DeselectAll();
        }
    }
}
