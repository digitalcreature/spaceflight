using UnityEngine;
using System;
using System.Threading;
using System.Collections.Generic;

public class GravityRecieverPath : MonoBehaviour {

	public int steps = 1000;
	public Gradient accelerationColors;
	public float accelerationColorsStart = 1000;
	public float accelerationColorsEnd = 5000;
	public Transform centerTransform;
	public bool recenter = true;

	public GravityReciever reciever;

	MeshFilter filter;
	Mesh mesh;
	Vector3[] verts;
	Color[] colors;
	Vector3 center;

	object meshLock;
	Queue<PathData> dataQueue;
	Thread updateThread;

	void Awake() {
		meshLock = new object();
		filter = GetComponent<MeshFilter>();
		InitializeMesh();
		transform.parent = null;
		transform.rotation = Quaternion.identity;
		transform.localScale = Vector3.one;
		dataQueue = new Queue<PathData>();
	}

	struct PathData {
		public Vector3[] verts;
		public Color[] colors;
	}

	void StartPathUpdate() {
		if (reciever) {
			center = transform.position;
			Vector3 pos = reciever.body.position;
			Vector3 vel = reciever.body.velocity;
			updateThread = new Thread((ThreadStart) delegate {
				PathUpdateThread(pos, vel);
			});
			updateThread.Start();
		}
	}

	void PathUpdateThread(Vector3 pos, Vector3 vel) {
		lock (meshLock) {
			if (reciever) {
				for (int i = 0; i < steps; i ++) {
					Vector3 acc = reciever.GetGravity(pos);
					float accColorT = Mathf.Clamp(acc.magnitude, accelerationColorsStart, accelerationColorsEnd) / (accelerationColorsEnd - accelerationColorsStart);
					Color color = accelerationColors.Evaluate(accColorT);
					color.a = 1 - ((float) i / steps);
					colors[i] = color;
					vel += acc * Time.fixedDeltaTime;
					pos += vel * Time.fixedDeltaTime;
					verts[i] = pos - center;
				}
				PathData data = new PathData() {
					verts = verts,
					colors = colors,
				};
				updateThread = null;
				dataQueue.Enqueue(data);
			}
		}
	}

	void FixedUpdate() {
		if (centerTransform && recenter) {
			transform.position = centerTransform.position;
		}
		else {
			transform.position = Vector3.zero;
		}
		while (dataQueue.Count > 0) {
			PathData data = dataQueue.Dequeue();
			mesh.vertices = data.verts;
			mesh.colors = data.colors;
		}
		if (updateThread == null) {
			// StartPathUpdate();
		}
	}



	void InitializeMesh() {
		lock (meshLock) {
			mesh = new Mesh();
			mesh.name = this.GetType().Name;
			verts = new Vector3[steps];
			colors = new Color[steps];
			int[] indices = new int[steps];
			Color clear = new Color(1f, 1f, 1f, 0f);
			for (int i = 0; i < steps; i ++) {
				colors[i] = Color.Lerp(Color.white, clear, (float) i / steps);
				indices[i] = i;
			}
			mesh.vertices = verts;
			mesh.colors = colors;
			mesh.SetIndices(indices, MeshTopology.LineStrip, 0);
			filter.mesh = mesh;
		}
	}

	void OnValidate() {
		if (filter) {
			InitializeMesh();
		}
	}

}
