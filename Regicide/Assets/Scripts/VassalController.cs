using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class VassalController : MonoBehaviour
{
    public GameObject buttonPanelPrefab;

    private WanderingAgent wanderScript;
    private NavMeshAgent agent;
    private Animator animator;

    public NavMeshAgent Agent => agent;
    public Animator Animator => animator;

    private bool isSelected = false;
    private static VassalController selectedVassal;
    private static GameObject buttonPanelInstance;

    public bool CanBeSelected = true;

    private Coroutine faceCameraCoroutine;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        wanderScript = GetComponent<WanderingAgent>();
    }

    private void Start()
    {
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
        AudioManager.Instance.PlayVassalSelect();
        isSelected = true;
        selectedVassal = this;

        // Stop wandering and movement
        DisableWandering();
        if (agent != null)
        {
            agent.ResetPath();
            agent.isStopped = true;
            agent.updateRotation = false;
        }

        // Stop walking animation
        if (animator != null)
            animator.SetFloat("Speed", 0f);

        // Start turn-to-camera coroutine after delay
        faceCameraCoroutine = StartCoroutine(FaceCameraAfterDelay(0f));

        // Show button panel
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
        selectedVassal = null;

        // Stop facing coroutine if still running
        if (faceCameraCoroutine != null)
        {
            StopCoroutine(faceCameraCoroutine);
            faceCameraCoroutine = null;
        }

        // Resume wandering
        EnableWandering();

        if (agent != null)
        {
            agent.isStopped = false;
            agent.updateRotation = true;
        }

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

    public void DeselectButHold()
    {
        isSelected = false;
        selectedVassal = null;

        // Stop facing coroutine if still running
        if (faceCameraCoroutine != null)
        {
            StopCoroutine(faceCameraCoroutine);
            faceCameraCoroutine = null;
        }

        // NOTE: Do NOT enable wandering

        if (agent != null)
        {
            agent.isStopped = false;
            agent.updateRotation = true;
        }

        if (buttonPanelInstance != null)
        {
            buttonPanelInstance.SetActive(false);
            buttonPanelInstance.transform.SetParent(null);
        }
    }

    public void DisableWandering()
    {
        if (wanderScript != null)
            wanderScript.enabled = false;
    }

    public void EnableWandering()
    {
        if (wanderScript != null)
            wanderScript.enabled = true;
    }

    private IEnumerator FaceCameraAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Face toward camera smoothly
        if (Camera.main != null)
        {
            Vector3 direction = Camera.main.transform.position - transform.position;
            direction.y = 0;

            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                float rotateSpeed = 0.05f;

                float t = 0f;
                while (t < 1f)
                {
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, t);
                    t += Time.deltaTime * rotateSpeed;
                    yield return null;
                }

                transform.rotation = targetRotation;
            }
        }
    }
}
