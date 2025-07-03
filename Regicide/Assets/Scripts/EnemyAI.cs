using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float moveSpeed = 6f;
    private bool isAttacking = false;

    private Animator animator;

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        animator?.SetBool("IsWalking", true);
    }

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
            animator?.SetBool("IsWalking", false);
            animator?.SetBool("IsAttacking", true); // 👈 switch to loop attack anim

            Debug.Log($"{name} reached attack zone and started attacking.");
        }
    }
}
