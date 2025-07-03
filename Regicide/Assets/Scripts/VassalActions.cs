using System.Collections;
using UnityEngine;

public class VassalActions : MonoBehaviour
{
    public float exploreDuration = 5f;
    private static Transform caveTransform;
    private static Transform kitchenTransform;
    private static Transform factoryTransform;
    private static Transform cannonTransform;
    private static Transform castleWallTransform;

    private void Awake()
    {
        if (caveTransform == null)
        {
            GameObject cave = GameObject.Find("Cave");
            if (cave != null) caveTransform = cave.transform;
        }

        if (kitchenTransform == null)
        {
            GameObject kitchen = GameObject.Find("Kitchen");
            if (kitchen != null) kitchenTransform = kitchen.transform;
        }

        if (factoryTransform == null)
        {
            GameObject factory = GameObject.Find("Factory");
            if (factory != null) factoryTransform = factory.transform;
        }

        if (cannonTransform == null)
        {
            GameObject cannon = GameObject.Find("Cannon");
            if (cannon != null) cannonTransform = cannon.transform;
        }

        if (castleWallTransform == null)
        {
            GameObject[] castleWalls = GameObject.FindGameObjectsWithTag("CastleWall");
            GameObject chosenWall = castleWalls[Random.Range(0, castleWalls.Length)];
            castleWallTransform = chosenWall.transform;
        }
    }

    public void OnExploreClicked()
    {
        if (VassalController.Selected != null && caveTransform != null)
        {
            VassalController vassal = VassalController.Selected;
            vassal.StartCoroutine(ExploreRoutine(vassal));
            vassal.DeselectButHold();
        }
    }

    public void OnCookClicked()
    {
        if (VassalController.Selected != null && kitchenTransform != null)
        {
            VassalController vassal = VassalController.Selected;
            vassal.StartCoroutine(KitchenRoutine(vassal));
            vassal.DeselectButHold();
        }
    }

    public void OnProcessClicked()
    {
        if (VassalController.Selected != null && factoryTransform != null)
        {
            VassalController vassal = VassalController.Selected;
            vassal.StartCoroutine(FactoryRoutine(vassal));
            vassal.DeselectButHold();
        }
    }

    public void OnBuildClicked()
    {
        if (VassalController.Selected != null && cannonTransform != null)
        {
            VassalController vassal = VassalController.Selected;
            vassal.StartCoroutine(CannonRoutine(vassal));
            vassal.DeselectButHold();
        }
    }

    public void OnDefendClicked()
    {
        if (VassalController.Selected != null && castleWallTransform != null)
        {
            VassalController vassal = VassalController.Selected;
            vassal.StartCoroutine(DefendRoutine(vassal));
            vassal.DeselectButHold();
        }
    }

    private IEnumerator ExploreRoutine(VassalController vassal)
    {
        yield return MoveAndHide(vassal, caveTransform);
        yield return new WaitForSeconds(5f);

        DangerResourceManager resourceManager = FindObjectOfType<DangerResourceManager>();
        if (resourceManager != null)
        {
            int roll = Random.Range(0, 2);
            if (roll == 0)
            {
                int foodGain = Random.Range(2, 6);
                resourceManager.RawFood += foodGain;
                Debug.Log($"[Explore] Found {foodGain} Raw Food!");
            }
            else
            {
                int matGain = Random.Range(1, 5);
                resourceManager.RawMaterial += matGain;
                Debug.Log($"[Explore] Found {matGain} Raw Material!");
            }
        }

        ReactivateVassal(vassal);
    }

    private IEnumerator KitchenRoutine(VassalController vassal)
    {
        yield return MoveAndHide(vassal, kitchenTransform);
        yield return new WaitForSeconds(3f);

        DangerResourceManager resourceManager = FindObjectOfType<DangerResourceManager>();
        if (resourceManager != null && resourceManager.RawFood >= 3)
        {
            resourceManager.RawFood -= 3;
            resourceManager.HungerLevel += 6;
            Debug.Log($"[Kitchen] Cooked 3 Raw Food into 6 Hunger!");
        }

        ReactivateVassal(vassal);
    }

    private IEnumerator FactoryRoutine(VassalController vassal)
    {
        yield return MoveAndHide(vassal, factoryTransform);
        yield return new WaitForSeconds(3f);

        DangerResourceManager resourceManager = FindObjectOfType<DangerResourceManager>();
        if (resourceManager != null && resourceManager.RawMaterial >= 3)
        {
            resourceManager.RawMaterial -= 3;
            resourceManager.ProcessedMaterial += 1;
            Debug.Log($"[Factory] Processed 3 Raw Material into 1 Processed Material!");
        }

        ReactivateVassal(vassal);
    }

    private IEnumerator CannonRoutine(VassalController vassal)
    {
        yield return MoveAndHide(vassal, cannonTransform);
        yield return new WaitForSeconds(3f);

        DangerResourceManager resourceManager = FindObjectOfType<DangerResourceManager>();
        if (resourceManager != null && resourceManager.ProcessedMaterial >= 3)
        {
            resourceManager.ProcessedMaterial -= 3;

            int completionGain = Random.Range(1, 4); // 1 to 3 inclusive
            resourceManager.CannonCompletion += completionGain;

            Debug.Log($"[Cannon] Used 3 Processed Material → +{completionGain}% Cannon Completion");
        }

        ReactivateVassal(vassal);
    }

    private IEnumerator DefendRoutine(VassalController vassal)
    {
        // Randomly choose a CastleWall now
        GameObject[] castleWalls = GameObject.FindGameObjectsWithTag("CastleWall");
        if (castleWalls.Length == 0)
        {
            Debug.LogWarning("No CastleWalls found in scene!");
            yield break;
        }

        GameObject chosenWall = castleWalls[Random.Range(0, castleWalls.Length)];
        Transform targetWall = chosenWall.transform;

        // Move to and hide at selected wall
        yield return MoveAndHide(vassal, targetWall);
        yield return new WaitForSeconds(3.5f);

        DangerResourceManager resourceManager = FindObjectOfType<DangerResourceManager>();
        DefendZone defendZone = FindObjectOfType<DefendZone>();
        if (defendZone != null)
        {
            defendZone.KillEnemy();
        }

        ReactivateVassal(vassal);
    }

    private IEnumerator MoveAndHide(VassalController vassal, Transform target)
    {
        AudioManager.Instance.PlayVassalYes();
        foreach (MeshRenderer renderer in vassal.GetComponentsInChildren<MeshRenderer>())
        {
            foreach (Material mat in renderer.materials)
            {
                mat.color = Color.red;
            }
        }

        vassal.CanBeSelected = false;

        vassal.DisableWandering();

        vassal.Animator?.SetBool("IsWalking", true);

        vassal.Agent.SetDestination(target.position);

        Collider col = vassal.GetComponent<Collider>();
        if (col == null) col = vassal.GetComponentInChildren<Collider>();
        bool wasTrigger = false;
        if (col != null)
        {
            wasTrigger = col.isTrigger;
            col.isTrigger = true;
        }

        while (vassal.Agent.pathPending || vassal.Agent.remainingDistance > vassal.Agent.stoppingDistance)
            yield return null;

        vassal.Animator?.SetBool("IsWalking", false);
        vassal.Animator?.SetTrigger("Interact");

        vassal.Agent.ResetPath();
        vassal.Agent.velocity = Vector3.zero;
        vassal.Agent.isStopped = true;
        vassal.Agent.enabled = false;

        if (col != null)
        {
            col.isTrigger = wasTrigger;
            col.enabled = false;
        }

        MeshRenderer mesh = vassal.GetComponent<MeshRenderer>();
        if (mesh == null) mesh = vassal.GetComponentInChildren<MeshRenderer>();
        if (mesh != null) mesh.enabled = false;
    }

    private void ReactivateVassal(VassalController vassal)
    {
        foreach (MeshRenderer renderer in vassal.GetComponentsInChildren<MeshRenderer>())
        {
            foreach (Material mat in renderer.materials)
            {
                mat.color = Color.white;
            }
        }

        MeshRenderer mesh = vassal.GetComponent<MeshRenderer>();
        if (mesh == null) mesh = vassal.GetComponentInChildren<MeshRenderer>();
        if (mesh != null) mesh.enabled = true;

        Collider col = vassal.GetComponent<Collider>();
        if (col != null) col.enabled = true;

        vassal.EnableWandering();

        vassal.Agent.enabled = true;
        vassal.Agent.isStopped = false;

        vassal.CanBeSelected = true;

        vassal.Animator?.SetBool("IsWalking", false);
        vassal.Animator?.ResetTrigger("Interact");
        vassal.Animator?.ResetTrigger("Attack");
    }
}
