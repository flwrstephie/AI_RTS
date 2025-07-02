using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float moveSpeed = 6f;
    private bool isAttacking = false;

    private void Update()
    {
        if (!isAttacking)
        {
            transform.position += transform.forward * moveSpeed * Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("AttackZone"))
        {
            isAttacking = true;
            // Optional: play animation, call attack logic, etc.
            Debug.Log($"{name} reached attack zone and stopped.");
        }
    }
}
