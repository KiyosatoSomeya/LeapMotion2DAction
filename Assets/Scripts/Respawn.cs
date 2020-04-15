using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    [SerializeField]
    private List<ResetPosition> resetPositionList;

    [SerializeField]
    StageManager stage;

    private Vector3 respawnPosition;
    private Quaternion respawnRotation;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        respawnPosition = rb.position;
        respawnRotation = rb.rotation;
    }

    private void Reset() {
        //詰まないようにオブジェクトの位置は全てリセット
        foreach(ResetPosition rp in resetPositionList) {
            rp.Reset();
        }

        //自身の位置調整
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.ResetInertiaTensor();
        rb.position = respawnPosition;
        rb.rotation = respawnRotation;

        Debug.Log("transform reset");
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Area") {
            Reset();
            stage.IncMiss();
        }else if(other.gameObject.tag == "Respawn") {
            // リスポーン地点の更新
            respawnPosition = other.transform.position;
        }
    }
}
