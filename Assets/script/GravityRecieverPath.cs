using UnityEngine;

public class GravityRecieverPath : MonoBehaviour {

	public int pathPoints = 1000;
	public int stepsPerPoint = 5;

	public GravityReciever reciever;

	LineRenderer lineRenderer;

	void Awake() {
		lineRenderer = GetComponent<LineRenderer>();
		lineRenderer.SetVertexCount(pathPoints);
	}

	void FixedUpdate() {
		if (reciever && reciever.body) {
			Vector3 pos = reciever.body.position;
			Vector3 vel = reciever.body.velocity;
			for (int p = 0; p < pathPoints; p ++) {
				lineRenderer.SetPosition(p, pos);
				for (int s = 0; s < stepsPerPoint; s ++) {
					Vector3 acc = reciever.GetGravity(pos);
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
