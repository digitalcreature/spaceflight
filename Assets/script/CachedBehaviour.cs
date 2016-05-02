using UnityEngine;
using System.Collections.Generic;

//a behaviour of which all active instances are cached for easy access
//accessed through 'all' property
public class CachedBehaviour<T> : MonoBehaviour where T : CachedBehaviour<T> {

	static HashSet<T> _all;
	public static IEnumerable<T> all {
		get {
			if (_all == null) {
				_all = new HashSet<T>();
			}
			return _all;
		}
	}

	protected virtual void OnEnable() {
		((HashSet<T>) all).Add((T) this);
	}

	protected virtual void OnDisable() {
		((HashSet<T>) all).Remove((T) this);
	}

}
