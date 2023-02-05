using UnityEngine;

public class CameraSpan : MonoBehaviour
{
	private new Camera camera;
	[SerializeField] private Rect confinedIn = new(0, 0, 16, 9);
	[SerializeField, Range(0.1f, 10f)] private float camSpeed = 1f;
	private Vector3 lastMousePos;

	private void Awake()
	{
		camera = GetComponent<Camera>();
	}

	private void OnDrawGizmos()
	{
		camera = GetComponent<Camera>();

		Gizmos.color = Color.green;
		Gizmos.DrawLine(confinedIn.position, new Vector3(confinedIn.xMax, confinedIn.yMin));
		Gizmos.DrawLine(confinedIn.position, new Vector3(confinedIn.xMin, confinedIn.yMax));
		Gizmos.DrawLine(new Vector3(confinedIn.xMax, confinedIn.yMin), new Vector3(confinedIn.xMax, confinedIn.yMax));
		Gizmos.DrawLine(new Vector3(confinedIn.xMin, confinedIn.yMax), new Vector3(confinedIn.xMax, confinedIn.yMax));
	}

	private void Update()
	{
		if (Input.GetButton("Fire1"))
		{
			var delta = (Input.mousePosition - lastMousePos) * (Time.deltaTime * camSpeed);

			//compute the rect corresponding to the camera FOV
			var camRect = GetCameraRect();
			camRect.center -= new Vector2(delta.x, delta.y);

			//make fit the delta to the confined area
			if (camRect.xMin <= confinedIn.xMin)
				delta.x -= confinedIn.xMin - camRect.xMin;
			if (camRect.xMax >= confinedIn.xMax)
				delta.x -= confinedIn.xMax - camRect.xMax;
			if (camRect.yMin <= confinedIn.yMin)
				delta.y -= confinedIn.yMin - camRect.yMin;
			if (camRect.yMax >= confinedIn.yMax)
				delta.y -= confinedIn.yMax - camRect.yMax;

			//move the camera
			camera.transform.position -= delta;
		}

		lastMousePos = Input.mousePosition;
	}

	private Rect GetCameraRect() => new()
	{
		height = camera.orthographicSize * 2f,
		position = new Vector2(
			-camera.orthographicSize * camera.aspect + camera.transform.position.x,
			-camera.orthographicSize + camera.transform.position.y
		),
		width = camera.orthographicSize * camera.aspect * 2f
	};
}