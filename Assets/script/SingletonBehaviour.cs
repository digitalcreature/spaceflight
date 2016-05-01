using UnityEngine;

public class SingletonBehvaiour<T> : MonoBehaviour where T : SingletonBehvaiour<T> {

	static T _main;
	public static T main {
		get {
			if (_main == null) {
				_main = FindObjectOfType<T>();
				if (_main == null) {
					Debug.LogError(string.Format("No instance of singleton \"{0}\" present in scene!", typeof(T).Name));
				}
			}
			return _main;
		}
	}

}
