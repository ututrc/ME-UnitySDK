using UnityEngine;
using System.Collections;

public class DebugMove : MonoBehaviour
{
	public KeyCode walkForward = KeyCode.W;
	public KeyCode walkBackward = KeyCode.S;
	public KeyCode turnLeft = KeyCode.A;
	public KeyCode turnRight = KeyCode.D;
	public KeyCode strafeLeft = KeyCode.Q;
	public KeyCode strafeRight = KeyCode.E;
	public KeyCode ascend = KeyCode.R;
	public KeyCode descend = KeyCode.F;

	public bool invertVertical = false;

	public float walkSpeed = 12.0f;
	public float turnSpeed = 90.0f;
	public float verticalSpeed = 4.0f;

	private float modifiedVerticalSpeed;

	public bool ignoreApplicationManager = false;

	void Start ()
	{
		modifiedVerticalSpeed = (invertVertical ? -1.0f : 1.0f)*verticalSpeed;
	}

	void Update ()
	{
		if (ignoreApplicationManager || ApplicationManager.Instance.IsInEditor) {

			float strafeSpeed = walkSpeed / 2.0f;

			if (Input.GetKey (walkForward)) {
				transform.Translate (walkSpeed * Vector3.forward * Time.deltaTime);
			}
			if (Input.GetKey (walkBackward)) {
				transform.Translate (walkSpeed * Vector3.back * Time.deltaTime);
			}

			if (Input.GetKey (turnLeft)) {
				transform.Rotate (Vector3.up, -turnSpeed * Time.deltaTime);
			}
			if (Input.GetKey (turnRight)) {
				transform.Rotate (Vector3.up, turnSpeed * Time.deltaTime);
			}

			if (Input.GetKey (strafeLeft)) {
				transform.Translate (strafeSpeed * Vector3.left * Time.deltaTime);
			}
			if (Input.GetKey (strafeRight)) {
				transform.Translate (strafeSpeed * Vector3.right * Time.deltaTime);
			}

			if (Input.GetKey (ascend)) {
				transform.Translate (modifiedVerticalSpeed * Vector3.up * Time.deltaTime);
			}
			if (Input.GetKey (descend)) {
				transform.Translate (modifiedVerticalSpeed * Vector3.down * Time.deltaTime);
			}
		}
	}
}

//FS
