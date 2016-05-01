using UnityEngine;

public class CachedRigidbody : CachedBehaviour<CachedRigidbody> {

	public Rigidbody body { get; private set; }

	void Awake() {
		body = GetComponent<Rigidbody>();
	}

}
