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
            GameObject castleWall = GameObject.Find("CastleWalls");
            if (castleWall != null) castleWallTransform = castleWall.transform;
        }
    }

    public void OnExploreClicked()
    {
        if (VassalController.Selected != null && caveTransform != null)
        {
            VassalController vassal = VassalController.Selected;
            vassal.StartCoroutine(ExploreRoutine(vassal));
            VassalController.DeselectAll();
        }
    }

    public void OnCookClicked()
    {
        if (VassalController.Selected != null && kitchenTransform != null)
        {
            VassalController vassal = VassalController.Selected;
            vassal.StartCoroutine(KitchenRoutine(vassal));
            VassalController.DeselectAll();
        }
    }

    public void OnProcessClicked()
    {
        if (VassalController.Selected != null && factoryTransform != null)
        {
            VassalController vassal = VassalController.Selected;
            vassal.StartCoroutine(FactoryRoutine(vassal));
            VassalController.DeselectAll();
        }
    }

    public void OnBuildClicked()
    {
        if (VassalController.Selected != null && cannonTransform != null)
        {
            VassalController vassal = VassalController.Selected;
            vassal.StartCoroutine(CannonRoutine(vassal));
            VassalController.DeselectAll();
        }
    }

    public void OnDefendClicked()
    {
        if (VassalController.Selected != null && castleWallTransform != null)
        {
            VassalController vassal = VassalController.Selected;
            vassal.StartCoroutine(DefendRoutine(vassal));
            VassalController.DeselectAll();
        }
    }

    private IEnumerator ExploreRoutine(VassalController vassal)
    {
        yield return MoveAndHide(vassal, caveTransform);

        yield return new WaitForSeconds(exploreDuration);

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

        yield return new WaitForSeconds(exploreDuration);

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

        yield return new WaitForSeconds(exploreDuration);

        DangerResourceManager resourceManager = FindObjectOfType<DangerResourceManager>();
        if (resourceManager != null && resourceManager.RawMaterial >= 3)
        {
            resourceManager.RawMaterial -= 3;
            // Assuming you have a field for processed material (add one if missing)
            resourceManager.ProcessedMaterial += 1;
            Debug.Log($"[Factory] Processed 3 Raw Material into 1 Processed Material!");
        }

        ReactivateVassal(vassal);
    }

    private IEnumerator CannonRoutine(VassalController vassal)
    {
        yield return MoveAndHide(vassal, cannonTransform);

        yield return new WaitForSeconds(exploreDuration);

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
        yield return MoveAndHide(vassal, castleWallTransform);

        yield return new WaitForSeconds(exploreDuration);

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
        vassal.CanBeSelected = false;

        if (vassal.WanderScript != null)
            vassal.WanderScript.enabled = false;

        vassal.Animator?.SetBool("IsWalking", true); // 👈 walking

        vassal.Agent.SetDestination(target.position);

        while (vassal.Agent.pathPending || vassal.Agent.remainingDistance > vassal.Agent.stoppingDistance)
            yield return null;

        vassal.Animator?.SetBool("IsWalking", false);
        vassal.Animator?.SetTrigger("Interact"); // 👈 start working

        vassal.Agent.ResetPath();
        vassal.Agent.velocity = Vector3.zero;
        vassal.Agent.isStopped = true;
        vassal.Agent.enabled = false;

        Collider col = vassal.GetComponent<Collider>();
        if (col != null) col.enabled = false;

        MeshRenderer mesh = vassal.GetComponent<MeshRenderer>();
        if (mesh == null) mesh = vassal.GetComponentInChildren<MeshRenderer>();
        if (mesh != null) mesh.enabled = false;
    }


    private void ReactivateVassal(VassalController vassal)
    {
        MeshRenderer mesh = vassal.GetComponent<MeshRenderer>();
        if (mesh == null) mesh = vassal.GetComponentInChildren<MeshRenderer>();
        if (mesh != null) mesh.enabled = true;

        Collider col = vassal.GetComponent<Collider>();
        if (col != null) col.enabled = true;

        if (vassal.WanderScript != null)
            vassal.WanderScript.enabled = true;

        vassal.Agent.enabled = true;
        vassal.Agent.isStopped = false;

        vassal.CanBeSelected = true;

        vassal.Animator?.SetBool("IsWalking", false);
        vassal.Animator?.ResetTrigger("Interact");
        vassal.Animator?.ResetTrigger("Attack");

    }
}
