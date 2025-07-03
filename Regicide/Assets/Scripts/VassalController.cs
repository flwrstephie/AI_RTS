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

    public bool CanBeSelected = true; // Controls if this vassal can be clicked
    
    private Animator animator;
    public Animator Animator => animator;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }
    private void Start()
    {
        wanderScript = GetComponent<WanderingAgent>();
        agent = GetComponent<NavMeshAgent>();

        if (buttonPanelInstance == null && buttonPanelPrefab != null)
        {
            buttonPanelInstance = Instantiate(buttonPanelPrefab);
            buttonPanelInstance.SetActive(false);
        }
    }

    private void OnMouseDown()
    {
        if (!CanBeSelected)
            return;

        if (selectedVassal != null && selectedVassal != this)
            selectedVassal.Deselect();

        if (isSelected)
            Deselect();
        else
            Select();
    }

    private void Select()
    {
        isSelected = true;
        selectedVassal = this;

        if (wanderScript != null)
            wanderScript.enabled = false;

        if (agent != null)
            agent.ResetPath();

        if (buttonPanelInstance != null)
        {
            buttonPanelInstance.SetActive(true);
            buttonPanelInstance.transform.SetParent(transform);
            buttonPanelInstance.transform.localPosition = new Vector3(0, 5f, 0);
        }
    }

    public void Deselect()
    {
        isSelected = false;

        if (wanderScript != null)
            wanderScript.enabled = true;

        selectedVassal = null;

        if (buttonPanelInstance != null)
        {
            buttonPanelInstance.SetActive(false);
            buttonPanelInstance.transform.SetParent(null);
        }
    }

    public static void DeselectAll()
    {
        if (selectedVassal != null)
            selectedVassal.Deselect();
    }

    public static VassalController Selected => selectedVassal;
}
