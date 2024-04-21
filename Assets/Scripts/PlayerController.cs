using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private Camera cam;
    private PlayerMotor motor;
    private PlayerAnimator anim;

    [SerializeField] private GameObject onClickSpawn;
    public LayerMask groundMask;

    public GameObject fireball;
    public float castSpeed = 0.75f;
    public Transform spawnPoint;
    private bool canCast = true;
    private float castHeight = 1f;
    private Transform target;

    private int maxHealth = 100;
    private int health = 100;
    public bool isAlive = true;

    public int killCount = 0;

    [SerializeField] private HealthUI healthUI;

    // Start is called before the first frame update
    private void Start() {
        cam = Camera.main;
        motor = GetComponent<PlayerMotor>();
        anim = GetComponent<PlayerAnimator>();
    }

    private void Die() {
        isAlive = false;
        anim.SetTrigger("Die");
        motor.NullTarget();
        GetComponent<Collider>().enabled = false;
    }

    public void TakeDamage() {
        health -= 10;

        healthUI.UpdateHealth(maxHealth, health);

        if (health <= 0) {
            Die();
            StartCoroutine(LoadGameOver(2));
        }
    }

    public IEnumerator LoadGameOver(int scene) {
        yield return new WaitForSeconds(4);
        SceneManager.LoadScene(scene);
    }

    // Update is called once per frame
    private void Update() {
        if (isAlive) {
            if (Input.GetKey(KeyCode.Space)) {
                anim.SetTrigger("Jump");
            }
            if (Input.GetKey(KeyCode.LeftShift)) {
                anim.SetTrigger("Run");
                transform.position += 7f * Time.deltaTime * transform.forward;
            }
            if (Input.GetMouseButtonDown(0)) {
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if(Physics.Raycast(ray, out hit, 100f, groundMask)) {
                    if (hit.transform.CompareTag("Enemy") && canCast) {
                        canCast = false;
                        target = hit.transform;
                        // hit enemy
                        motor.Stop();
                        motor.SetTarget(hit.transform);
                        anim.SetTrigger("Attack");
                    }
                    else {
                        motor.Move(hit.point);
                        Instantiate(onClickSpawn, hit.point + Vector3.up * 0.2f, Quaternion.LookRotation(hit.normal));
                        MovePlayer(hit);
                    }
                }
            }

            if (killCount >= 3) {
                StartCoroutine(LoadGameOver(3));
            }
        }
    }

    private void MovePlayer(RaycastHit hit) {
        motor.Move(hit.point);
        Instantiate(onClickSpawn, hit.point + Vector3.up * 0.2f, Quaternion.Euler(-90, 0, 0));
    }

    private void Cast() { // called by animation event
        Vector3 aimPoint = target.position;
        aimPoint.y = castHeight;

        Instantiate(fireball, spawnPoint.position, Quaternion.identity).transform.LookAt(target);
        StartCoroutine(ResetCastTimer());
    }

    public IEnumerator ResetCastTimer() {
        canCast = false;
        yield return new WaitForSeconds(castSpeed);
        canCast = true;
    }
}
