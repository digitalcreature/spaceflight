using UnityEngine;

public class CachedRigidbodyPath : MonoBehaviour {

	public int pathPoints = 1000;
	public int stepsPerPoint = 15;

	public CachedRigidbody cachedBody;

	LineRenderer lineRenderer;

	void Awake() {
		lineRenderer = GetComponent<LineRenderer>();
		lineRenderer.SetVertexCount(pathPoints);
	}

	void FixedUpdate() {
		if (cachedBody && cachedBody.body) {
			Vector3 pos = cachedBody.body.position;
			Vector3 vel = cachedBody.body.velocity;
			for (int p = 0; p < pathPoints; p ++) {
				lineRenderer.SetPosition(p, pos);
				for (int s = 0; s < stepsPerPoint; s ++) {
					Vector3 acc = Vector3.zero;
					foreach (GravitySource source in GravitySource.all) {
						acc += source.GetAcceleration(pos);
					}
					vel += acc * Time.fixedDeltaTime;
					pos += vel * Time.fixedDeltaTime;
				}
			}
		}
	}

	void OnValidate() {
		if (lineRenderer) {
			lineRenderer.SetVertexCount(pathPoints);
		}
	}

}
