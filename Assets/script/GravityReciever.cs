using UnityEngine;

public class GravityReciever : MonoBehaviour {

	public Rigidbody body { get; private set; }

	void Awake() {
		body = GetComponent<Rigidbody>();
	}

	public Vector3 GetGravity() { return GetGravity(body.position); }
	public Vector3 GetGravity(Vector3 position) {
		Vector3 gMax = Vector3.zero;
		foreach (GravitySource source in GravitySource.all) {
			Vector3 g = source.GetGravity(position);
			if (g.sqrMagnitude > gMax.sqrMagnitude) {
				gMax = g;
			}
		}
		return gMax;
	}

	public float GetGravityPotential() { return GetGravityPotential(body.position); }
	public float GetGravityPotential(Vector3 position) {
		float uGMax = 0;
		foreach (GravitySource source in GravitySource.all) {
			float uG = source.GetGravityPotential(position);
			if (uG < uGMax) {
				uGMax = uG;
			}
		}
		return uGMax;
	}

	void FixedUpdate() {
		body.AddForce(GetGravity(), ForceMode.Acceleration);
	}

}
