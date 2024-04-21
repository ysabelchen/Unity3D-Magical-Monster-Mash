using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMotor : MonoBehaviour {
    private NavMeshAgent agent;
    private Transform target;
    public float rotationSpeed;

    private void Start() {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update() {
        if (target) {
            Quaternion targetRotation = Quaternion.LookRotation(target.position - transform.position);
            Quaternion lerpRotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed);
            transform.rotation = lerpRotation;
        }
    }

    public void Move(Vector3 location) {
        NullTarget();
        agent.isStopped = false;
        agent.SetDestination(location);
    }

    public void Stop() {
        agent.isStopped = true;
        agent.velocity = Vector3.zero;
    }

    public void SetTarget(Transform t) {
        target = t;
    }

    public void NullTarget() {
        target = null;
    }

    public void MoveToTarget() {
        agent.SetDestination(target.position);
    }
}