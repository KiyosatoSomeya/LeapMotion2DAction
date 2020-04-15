using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidbodyResetPosition : ResetPosition
{

    void Start() {
        rb = GetComponent<Rigidbody>();
        firstPosition = rb.position;
        firstRotation = rb.rotation;
    }

    public override void Reset() {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.ResetInertiaTensor();
        rb.position = firstPosition;
        rb.rotation = firstRotation;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Area") {
            Reset();
        }
    }
}
