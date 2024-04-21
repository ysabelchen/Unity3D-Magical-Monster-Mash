using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerAnimator : MonoBehaviour
{
    private Animator anim;
    private NavMeshAgent agent;
    private bool moving = true;
    private float gradual;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift)) {
            if (gradual < 2) {
                gradual += 0.025f;
                gradual = Mathf.Clamp(gradual, 0f, 2f);
            }
            anim.SetFloat("Speed", gradual);
        } else if (Input.GetKey(KeyCode.Space)) {
            anim.SetFloat("Speed", gradual);
        }
        if (moving) {
            anim.SetFloat("Speed", agent.velocity.magnitude / agent.speed);
        }
    }

    public void SetTrigger(string trig) {
        anim.SetTrigger(trig);
    }
}
