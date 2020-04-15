using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringBlockPosition : ResetPosition
{
    [SerializeField]
    private Vector3 leftPosition, rightPosition;
    [SerializeField]
    private float firstMoveVelocity = 2f, moveTouchSpan = 0.5f, speedDecayRate = 1f;
    [SerializeField]
    private Animator springAnim;

    /* 動いている速さ */
    private float moveVelocity = 0;

    /* 0はタッチされていない
     * 1は左側にタッチされた直後
     * 2は右側にタッチされた直後 */
    private int touchFlag = 0;
    private float touchTime = 0;

    void Start() {
        rb = GetComponent<Rigidbody>();
        firstPosition = rb.position;
        firstRotation = rb.rotation;
    }

    void FixedUpdate() {
        float moveVelocityAbs = Mathf.Abs(moveVelocity);
        if (moveVelocityAbs < 0.01f) {
            // 移動停止
            moveVelocity = 0f;
        } else {
            if (moveVelocity > 0) {
                if (moveVelocity * Time.deltaTime > Vector3.Distance(rb.position, rightPosition)) {
                    rb.MovePosition(rightPosition);
                } else {
                    rb.MovePosition(transform.position + (new Vector3(moveVelocity, 0, 0) * Time.deltaTime));
                    moveVelocity -= speedDecayRate * Time.fixedDeltaTime;
                }
            } else {
                if(-moveVelocity * Time.deltaTime > Vector3.Distance(rb.position, leftPosition)) {
                    rb.MovePosition(leftPosition);
                } else {
                    rb.MovePosition(transform.position + (new Vector3(moveVelocity, 0, 0) * Time.deltaTime));
                    moveVelocity += speedDecayRate * Time.fixedDeltaTime;
                }
            }
        }
    }

    public override void Reset() {
        rb.position = firstPosition;
        rb.rotation = firstRotation;
        moveVelocity = 0;
    }

    private void ToRight() {
        springAnim.SetTrigger("Right");
        moveVelocity = firstMoveVelocity;
    }

    private void ToLeft() {
        springAnim.SetTrigger("Left");
        moveVelocity = -firstMoveVelocity;
    }

    /* newTouchFlagは左の時1、右の時2 */
    public void Touch(int newTouchFlag) {
        if (touchFlag != 0 && touchFlag != newTouchFlag && Time.timeSinceLevelLoad - touchTime <= moveTouchSpan) {
            if(newTouchFlag == 1) {
                ToRight();
            } else {
                ToLeft();
            }
        }
        touchTime = Time.timeSinceLevelLoad;
        touchFlag = newTouchFlag;
    }
}
