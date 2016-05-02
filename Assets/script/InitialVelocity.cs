using UnityEngine;

public class InitialVelocity : MonoBehaviour {

	public float initialSpeed = 5;
	public Vector3 initialVelocity {
		get {
			return transform.forward * initialSpeed;
		}
	}

	void Start() {
		Rigidbody body = GetComponent<Rigidbody>();
		if (body) {
			body.velocity = initialVelocity;
		}
	}

	void OnDrawGizmos() {
		Gizmos.color = new Color(1, 0.7f, 0);
		Gizmos.DrawLine(transform.position, transform.position + initialVelocity);
	}

}
