using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    private float patrolRadius = 15f;
    private float minPatrolTime = 2f;
    private float maxPatrolTime = 4f;
    private float agroRadius = 10f;
    private float attackRadius = 5f;
    public LayerMask playerMask;

    private NavMeshAgent agent;
    private PlayerAnimator anim;
    private PlayerMotor motor;
    private GameObject player;
    //[SerializeField] private CharacterHealth other;

    private bool isPatrolling = true;
    private bool isAgroed = false;
    private bool isAttacking = false;

    private Transform target;

    private bool isAlive = true;
    private int maxHealth = 30;
    private int health = 30;

    [SerializeField] private HealthUI healthUI;

    //[SerializeField] private CharacterHealth other;

    // Start is called before the first frame update
    void Start()
    {
        motor = GetComponent<PlayerMotor>();
        anim = GetComponent<PlayerAnimator>();
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        //other = GetComponent<CharacterHealth>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isAlive && player.GetComponent<PlayerController>().isAlive) {
            if (!isAgroed) {
                CheckAgro();
                if (!isPatrolling && !isAgroed) {
                    StartCoroutine(ChosePatrolLocation());
                }
            } else {
                motor.MoveToTarget();
                if (CloseEnoughToAttack() && !isAttacking) {
                    isAttacking = true;
                    anim.SetTrigger("Attack");
                    player.GetComponent<PlayerController>().TakeDamage();
                }
            }
        }
    }

    public IEnumerator ChosePatrolLocation() {
        if(!isAgroed) {
            isPatrolling = true;
            Vector3 offset = Random.insideUnitSphere * patrolRadius;
            offset.y = 0;
            offset += transform.position;
            motor.Move(offset);
            yield return new WaitForSeconds(Random.Range(minPatrolTime, maxPatrolTime));
            StartCoroutine(ChosePatrolLocation());
        }
    }

    private void CheckAgro() {
        foreach (Collider col in Physics.OverlapSphere(transform.position, agroRadius, playerMask)) {
            if (col.transform.CompareTag("Player")) {
                motor.SetTarget(col.transform);
                target = col.transform;
                isAgroed = true;
            }
        }
    }

    private bool CloseEnoughToAttack() {
        return Vector3.Magnitude(transform.position - target.position) <= agent.stoppingDistance;
    }

    public void Attack() {
        isAttacking = false;
        foreach(Collider item in Physics.OverlapSphere(transform.position + transform.forward * attackRadius, attackRadius, playerMask)) {
            // damage
        }
    }

    private void Die() {
        isAlive = false;
        anim.SetTrigger("Die");
        motor.NullTarget();
        GetComponent<Collider>().enabled = false;
    }

    private void OnTriggerStay(Collider other) {
        if (other.CompareTag("Fireball")) {
            health -= 10;

            healthUI.UpdateHealth(maxHealth, health);

            if (health <= 0) {
                player.GetComponent<PlayerController>().killCount += 1;
                Die();
            }
        }
    }

    //public void ResetAttack() {
    //    isAttacking = false;
    //}

}
