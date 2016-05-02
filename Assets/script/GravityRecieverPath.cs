using UnityEngine;

public class GravityRecieverPath : MonoBehaviour {

	public int steps = 1000;
	public Gradient accelerationColors;
	public float accelerationColorsStart = 1000;
	public float accelerationColorsEnd = 5000;
	public Transform center;

	public GravityReciever reciever;

	MeshFilter filter;
	Mesh mesh;

	void Awake() {
		filter = GetComponent<MeshFilter>();
		InitializeMesh();
		transform.parent = null;
	}

	void FixedUpdate() {
		if (center) {
			transform.position = center.position;
		}
		else {
			transform.position = Vector3.zero;
		}
		if (reciever && reciever.body) {
			Vector3 pos = reciever.body.position;
			Vector3 vel = reciever.body.velocity;
			Vector3[] verts = mesh.vertices;
			Color[] colors = mesh.colors;
			for (int i = 0; i < steps; i ++) {
				Vector3 acc = reciever.GetGravity(pos);
				float accColorT = Mathf.Clamp(acc.magnitude, accelerationColorsStart, accelerationColorsEnd) / (accelerationColorsEnd - accelerationColorsStart);
				colors[i] = accelerationColors.Evaluate(accColorT);
				vel += acc * Time.fixedDeltaTime;
				pos += vel * Time.fixedDeltaTime;
				verts[i] = pos - transform.position;
			}
			mesh.vertices = verts;
			mesh.colors = colors;
			filter.mesh = mesh;
		}
	}

	void InitializeMesh() {
		mesh = new Mesh();
		mesh.name = this.GetType().Name;
		mesh.vertices = new Vector3[steps];
		Color[] colors = new Color[steps];
		int[] indices = new int[steps];
		Color clear = new Color(1f, 1f, 1f, 0f);
		for (int i = 0; i < steps; i ++) {
			colors[i] = Color.Lerp(Color.white, clear, (float) i / steps);
			indices[i] = i;
		}
		mesh.colors = colors;
		mesh.SetIndices(indices, MeshTopology.LineStrip, 0);
		filter.mesh = mesh;
	}

	void OnValidate() {
		if (filter) {
			InitializeMesh();
		}
	}

}
