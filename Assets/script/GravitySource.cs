using UnityEngine;
using System.Collections.Generic;

public class GravitySource : CachedBehaviour<GravitySource> {

	public float g = 10;
	public float r = 1000;

	public float mass { get { return g * r * r; } }

	//get acceleration of object at pos
	//all vectors are in world space
	public Vector3 GetGravity(Vector3 pos) {
		pos = transform.position - pos;
		return (pos.normalized * mass) / (pos.sqrMagnitude);
	}

	public float GetGravityPotential(Vector3 pos) {
		pos = transform.position - pos;
		return  - (mass) / (pos.magnitude);
	}

}
