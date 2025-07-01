using UnityEngine;
using UnityEngine.AI;

public class VassalController : MonoBehaviour
{
    public GameObject buttonPanelPrefab;

    private WanderingAgent wanderScript;
    public WanderingAgent WanderScript => wanderScript;
    private NavMeshAgent agent;
    public NavMeshAgent Agent => agent;
    private bool isSelected = false;

    private static VassalController selectedVassal;
    private static GameObject buttonPanelInstance;

    private void Start()
    {
        wanderScript = GetComponent<WanderingAgent>();
        agent = GetComponent<NavMeshAgent>();

        // Instantiate shared button panel ONCE
        if (buttonPanelInstance == null && buttonPanelPrefab != null)
        {
            buttonPanelInstance = Instantiate(buttonPanelPrefab);
            buttonPanelInstance.SetActive(false);
        }
    }

    private void OnMouseDown()
    {
        // Deselect old vassal if different
        if (selectedVassal != null && selectedVassal != this)
            selectedVassal.Deselect();

        // Toggle selection
        if (isSelected)
            Deselect();
        else
            Select();
    }

    private void Select()
    {
        isSelected = true;
        selectedVassal = this;

        // Stop movement
        if (wanderScript != null) wanderScript.enabled = false;
        if (agent != null) agent.ResetPath();

        // Move panel to above this vassal and activate
        if (buttonPanelInstance != null)
        {
            buttonPanelInstance.SetActive(true);
            buttonPanelInstance.transform.SetParent(transform); // follow vassal
            buttonPanelInstance.transform.localPosition = new Vector3(0, 5f, 0); // adjust height as needed
        }
    }

    public void Deselect()
    {
        isSelected = false;

        if (wanderScript != null) wanderScript.enabled = true;
        selectedVassal = null;

        if (buttonPanelInstance != null)
        {
            buttonPanelInstance.SetActive(false);
            buttonPanelInstance.transform.SetParent(null); // unparent
        }
    }

    public static void DeselectAll()
    {
        if (selectedVassal != null)
            selectedVassal.Deselect();
    }
    public static VassalController Selected => selectedVassal;
}
