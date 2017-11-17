using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteraction : MonoBehaviour {
	public Text d;
	public float sensitivity = 150.0f;
	public Camera cam;
	private Rigidbody rigidbody;
	public float speed = 10.0f;
	public float gravity = 10.0f;
	public float maxVelocityChange = 10.0f;
	public bool canJump = true;
	public float jumpHeight = 2.0f;
	private bool grounded = false;



	void Awake () {
		rigidbody = gameObject.GetComponent<Rigidbody> ();
		rigidbody.freezeRotation = true;
		rigidbody.useGravity = false;
	}

	void FixedUpdate () {
		var camy = Input.GetAxis("Mouse Y") * Time.deltaTime * sensitivity ;
		cam.transform.Rotate(camy, 0, 0);
		if (grounded) {
			// Calculate how fast we should be moving
			Vector3 targetVelocity = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
			targetVelocity = transform.TransformDirection(targetVelocity);
			targetVelocity *= speed;

			// Apply a force that attempts to reach our target velocity
			Vector3 velocity = rigidbody.velocity;
			Vector3 velocityChange = (targetVelocity - velocity);
			velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
			velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
			velocityChange.y = 0;
			rigidbody.AddForce(velocityChange, ForceMode.VelocityChange);

			// Jump
			if (canJump && Input.GetButton("Jump")) {
				rigidbody.velocity = new Vector3(velocity.x, CalculateJumpVerticalSpeed(), velocity.z);
			}
		}

		// We apply gravity manually for more tuning control
		rigidbody.AddForce(new Vector3 (0, -gravity * rigidbody.mass, 0));

		grounded = false;
	}

	void OnCollisionStay () {
		grounded = true;    
	}

	float CalculateJumpVerticalSpeed () {
		// From the jump height and gravity we deduce the upwards speed 
		// for the character to reach at the apex.
		return Mathf.Sqrt(2 * jumpHeight * gravity);
	}

	void OnCollisionEnter(Collision col){
		if (col.gameObject.tag == "Storage") {
			openDialouge ();
		}
	}

	void onCollisionExit(Collision col){
		endDialouge ();
	}
	void endDialouge(){
		d.text = "";
	}
	void openDialouge(){
		d.text = "Open Chest?";
	}
}
