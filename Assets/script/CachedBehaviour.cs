using UnityEngine;
using System.Collections.Generic;

public class CachedBehaviour<T> : MonoBehaviour where T : CachedBehaviour<T> {

	static HashSet<T> _all;
	public static HashSet<T> all {
		get {
			if (_all == null) {
				_all = new HashSet<T>();
			}
			return _all;
		}
	}

	protected virtual void OnEnable() {
		all.Add((T) this);
	}

	protected virtual void OnDisable() {
		all.Remove((T) this);
	}

}
