using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityChanControlForApp : MonoBehaviour
{
	public float animSpeed = 1.5f;              // アニメーション再生速度設定
	public float speed = 7.0f;
	public float jumpPower = 5.0f;
	public float cameraPositionY = 1f;
	public float minAnchorMin = -10f;
	public float minX = -50f, rangeX = 100f;

	[SerializeField]
	private StageManager stage;

	[SerializeField]
	private RectTransform backgroundTransform;

    [SerializeField]
	private AudioSource jumpAudio;

	private Vector3 cameraPositionOffset;

    private Animator anim;

	// キャラクターコントローラ（カプセルコライダ）の参照
	private CapsuleCollider col;
	private Rigidbody rb;

	private GameObject cameraObject;    // メインカメラへの参照

	private bool jumping;


	void Start() {
		// Animatorコンポーネントを取得する
		anim = GetComponent<Animator>();
		anim.speed = animSpeed;                             // animSpeedを設定する

		col = GetComponent<CapsuleCollider>();
		rb = GetComponent<Rigidbody>();

		//メインカメラを取得する
		cameraObject = GameObject.FindWithTag("MainCamera");
		cameraPositionOffset = cameraObject.transform.position - transform.position;
	}


    void FixedUpdate() {
		// 背景の位置の更新
		backgroundTransform.anchorMin =
            new Vector2(minAnchorMin * (transform.position.x - minX) / rangeX, backgroundTransform.anchorMin.y);

        
		if (stage.gameEnd) {
			rb.isKinematic = true;
			rb.rotation = Quaternion.Euler(0, 180, 0);
		} else {
			// キャラクターの左右移動
			Vector3 velocity = new Vector3(0, rb.velocity.y, 0);
			if (Input.GetKey(KeyCode.A)) {
				// 左方向
				velocity.x -= speed;
			}
			if (Input.GetKey(KeyCode.D)) {
				// 右方向
				velocity.x += speed;
			}

			if (Mathf.Abs(velocity.x) > 0.01f) {
				if (velocity.x > 0) {
					transform.rotation = Quaternion.Euler(new Vector3(0, 90, 0));
				} else {
					transform.rotation = Quaternion.Euler(new Vector3(0, -90, 0));
				}

				anim.SetBool("Run", true);
				rb.velocity = velocity;

				stage.GameStart();  // ゲーム開始のUI変更
			} else {
				anim.SetBool("Run", false);
			}


			if (jumping) {
				// ジャンプ中の時着地判定
				bool isGrounded = Physics.Raycast(gameObject.transform.position + 0.1f * gameObject.transform.up, -gameObject.transform.up, 0.15f);
				if (isGrounded || Mathf.Abs(rb.velocity.y) < 0.01f) {
					jumping = false;
				}
			} else if (Input.GetButtonDown("Jump")) {
				// ジャンプしていない時ジャンプ可能
				jumpAudio.Play();
				rb.AddForce(Vector3.up * jumpPower, ForceMode.VelocityChange);
				jumping = true;
			}

			// 最後にカメラ位置補正
			Vector3 newPosition = transform.position + cameraPositionOffset;
			newPosition.y = cameraPositionY;
			cameraObject.transform.position = newPosition;
		}
	}

	private void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Finish") {
			// ゲーム終了処理
			stage.GameEnd();
			anim.SetTrigger("End");
		}
	}
}
