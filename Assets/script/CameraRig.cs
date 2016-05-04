using UnityEngine;

public class CameraRig : MonoBehaviour {

	public float sensitivity = 5;
	public MouseButton orbitViewButton = MouseButton.Right;
	public MouseButton panButton = MouseButton.Middle;
	public float panSpeed = 500;
	public float zoomMin = 250;
	public float zoomMax = 5000;
	public float zoomInterval = 50;
	public float zoomSmoothing = 15;

	Vector2 look;
	float zoom;

	Camera cam;

	void Awake() {
		cam = GetComponentInChildren<Camera>();
		look.y = transform.eulerAngles.y;
		look.x = transform.eulerAngles.x;
		zoom = cam.orthographicSize;
	}

	void Update() {
		if (Input.GetMouseButton((int) orbitViewButton)) {
			look.x -= Input.GetAxis("Mouse Y") * sensitivity;
			look.y += Input.GetAxis("Mouse X") * sensitivity;
			if (look.x > 90) look.x = 90;
			if (look.x < -90) look.x = -90;
			transform.rotation = Quaternion.Euler(look.x, look.y, 0);
		}
		if (Input.GetMouseButton((int) panButton)) {
			Vector3 delta = new Vector3(
				Input.GetAxis("Mouse X"),
				0,
				Input.GetAxis("Mouse Y")
			);
			delta = Quaternion.Euler(0, cam.transform.eulerAngles.y, 0) * delta;
			transform.position -= delta * panSpeed;
		}
		zoom -= Input.mouseScrollDelta.y * zoomInterval;
		zoom = Mathf.Clamp(zoom, zoomMin, zoomMax);
		cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, zoom, Time.deltaTime * zoomSmoothing);
	}

}

public enum MouseButton { Left, Right, Middle }
