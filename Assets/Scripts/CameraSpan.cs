using UnityEngine;

public class CameraSpan : MonoBehaviour
{
	private new Camera camera;
	[SerializeField] private Rect confinedIn = new(0, 0, 16, 9);

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
	}

	private Rect GetCameraRect() => new Rect
	{
		height = camera.orthographicSize * 2f,
		position = new Vector2(-camera.orthographicSize * camera.aspect, -camera.orthographicSize),
		width = camera.orthographicSize * camera.aspect * 2f
	};
}