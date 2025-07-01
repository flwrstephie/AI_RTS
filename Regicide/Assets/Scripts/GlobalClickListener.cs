using UnityEngine;

public class VassalClickManager : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // left-click
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (!Physics.Raycast(ray))
            {
                VassalController.DeselectAll(); // no object hit = deselect
            }
        }
    }
}
