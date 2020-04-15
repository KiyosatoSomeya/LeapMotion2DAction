using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPosition : MonoBehaviour
{
    protected Vector3 firstPosition;
    protected Quaternion firstRotation;
    protected Rigidbody rb;

    public virtual void Reset() { }
}
