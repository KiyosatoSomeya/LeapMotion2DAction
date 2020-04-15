using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchTrigger : MonoBehaviour
{
    [SerializeField]
    private int index = 1;
    [SerializeField]
    private SpringBlockPosition springBlock;

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "GameController") {
            springBlock.Touch(index);
        }
    }
}
