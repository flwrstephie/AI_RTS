using System.Collections;
using UnityEngine;

public class VassalActions : MonoBehaviour
{
    public float exploreDuration = 5f; // EDITABLE: Time they stay in the cave
    private static Transform caveTransform;

    private void Awake()
    {
        // Cache the Cave's transform once
        if (caveTransform == null)
        {
            GameObject cave = GameObject.Find("Cave");
            if (cave != null)
                caveTransform = cave.transform;
            else
                Debug.LogWarning("No GameObject named 'Cave' found in the scene.");
        }
    }

    public void OnExploreClicked()
    {
        if (VassalController.Selected != null && caveTransform != null)
        {
            VassalController vassal = VassalController.Selected;

            // Start coroutine on the vassal’s MonoBehaviour context
            vassal.StartCoroutine(ExploreRoutine(vassal));

            // Deselect right away
            VassalController.DeselectAll();
        }
    }

    private IEnumerator ExploreRoutine(VassalController vassal)
    {
        // Disable wandering so it doesn’t override the path
        if (vassal.WanderScript != null)
            vassal.WanderScript.enabled = false;

        // Move to cave
        vassal.Agent.SetDestination(caveTransform.position);

        // Wait until they arrive
        while (vassal.Agent.pathPending || vassal.Agent.remainingDistance > vassal.Agent.stoppingDistance)
        {
            yield return null;
        }

        // Fully stop agent and disable it
        vassal.Agent.ResetPath();
        vassal.Agent.velocity = Vector3.zero;
        vassal.Agent.isStopped = true;
        vassal.Agent.enabled = false;

        // Disable interactivity and visuals
        Collider col = vassal.GetComponent<Collider>();
        if (col != null) col.enabled = false;

        MeshRenderer mesh = vassal.GetComponent<MeshRenderer>();
        if (mesh == null) mesh = vassal.GetComponentInChildren<MeshRenderer>();
        if (mesh != null) mesh.enabled = false;

        // Wait inside cave
        yield return new WaitForSeconds(exploreDuration);

        // Reactivate everything
        if (mesh != null) mesh.enabled = true;
        if (col != null) col.enabled = true;
        if (vassal.WanderScript != null) vassal.WanderScript.enabled = true;

        // Re-enable movement
        vassal.Agent.enabled = true;
        vassal.Agent.isStopped = false;
        
    }
}
