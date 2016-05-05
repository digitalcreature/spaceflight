using UnityEngine;

public class GravityReciever : MonoBehaviour {

	public Mode gravityMode = Mode.Greatest;

	public enum Mode { Greatest, Total }

	public Rigidbody body { get; private set; }

	void Awake() {
		body = GetComponent<Rigidbody>();
	}

	public Vector3 GetGravity() { return GetGravity(body.position); }
	public Vector3 GetGravity(Vector3 position) {
		Vector3 g = Vector3.zero;
		foreach (GravitySource source in GravitySource.all) {
			Vector3 sourceG = source.GetGravity(position);
			if (gravityMode == Mode.Total)
				g += sourceG;
			if (gravityMode == Mode.Greatest && sourceG.sqrMagnitude > g.sqrMagnitude)
				g = sourceG;
		}
		return g;
	}

	public float GetGravityPotential() { return GetGravityPotential(body.position); }
	public float GetGravityPotential(Vector3 position) {
		float uG = 0;
		foreach (GravitySource source in GravitySource.all) {
			float sourceUG = source.GetGravityPotential(position);
			if (gravityMode == Mode.Total)
				uG += sourceUG;
			if (gravityMode == Mode.Greatest && sourceUG < uG)
				uG = sourceUG;
		}
		return uG;
	}

	void FixedUpdate() {
		body.AddForce(GetGravity(), ForceMode.Acceleration);
	}

}
