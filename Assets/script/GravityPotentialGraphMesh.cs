using UnityEngine;
using System.Collections.Generic;

public class GravityPotentialGraphMesh : MonoBehaviour {

	public int gridSize = 16;
	public GravityReciever reciever;

	MeshFilter filter;
	Mesh mesh;
	Vector3[] verts;

	void Awake() {
		filter = GetComponent<MeshFilter>();
		InitializeMesh();
	}

	void FixedUpdate() {
		if (reciever) {
			Vector3 position = reciever.body.position;
			position.y = 0;
			transform.position = position;
			verts[0] = transform.InverseTransformPoint(reciever.body.position);
			verts[1] = Vector3.zero;
			for (int i = 1; i < verts.Length; i ++) {
				Vector3 pos = verts[i];
				pos.y = 0;
				pos = transform.TransformPoint(pos);
				verts[i].y = reciever.GetGravityPotential(pos);
			}
			mesh.vertices = verts;
		}
	}

	void InitializeMesh() {
		mesh = new Mesh();
		mesh.name = GetType().Name;
		verts = new Vector3[(gridSize + 1) * (gridSize + 1) + 2];
		Color[] colors = new Color[verts.Length];
		List<int> indices = new List<int>();
		indices.Add(0);
		indices.Add(1);
		Vector3 center = verts[0] = verts[1] = Vector3.zero;
		colors[0] = colors[1] = Color.white;
		for (int i = 0; i <= gridSize; i ++) {
			for (int j = 0; j <= gridSize; j ++) {
				int v = 2 + j * (gridSize + 1) + i;
				verts[v] = new Vector3(((float) i / (gridSize)) - 0.5f, 0, ((float) j / (gridSize)) - 0.5f);
				Color color = Color.white;
				color.a = 1 - ((verts[v] - center).magnitude) * 2;
				colors[v] = color;
				if (i > 0) {
					indices.Add(v);
					indices.Add(v - 1);
				}
				if (j > 0) {
					indices.Add(v);
					indices.Add(v - (gridSize + 1));
				}
			}
		}
		mesh.vertices = verts;
		mesh.colors = colors;
		mesh.SetIndices(indices.ToArray(), MeshTopology.Lines, 0);
		filter.sharedMesh = mesh;

	}

	void OnValidate() {
		if (filter) {
			InitializeMesh();
		}
	}

}
