using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Vector3 offset;
    private Transform target;
    public float followSpeed = 0.125f;
    public float zoomSpeed = 1f;
    private float zoom = 12f;
    public float maxZoom = 25f;
    public float minZoom = 10f;

    // Start is called before the first frame update
    private void Start() {
        target = GameObject.FindWithTag("Player").transform;
        zoom = (maxZoom + minZoom) / 2f;
        transform.position = target.position + (zoom * offset);
    }

    private void LateUpdate() {
        zoom = Mathf.Clamp(-Input.GetAxis("Mouse ScrollWheel") * zoomSpeed + zoom, minZoom, maxZoom);
        Vector3 targetPos = target.position + (zoom * offset);
        Vector3 lerpPos = Vector3.Lerp(transform.position, targetPos, followSpeed);
        transform.position = lerpPos;
        transform.LookAt(target);
    }

    // Update is called once per frame
    private void Update() {

    }
}
