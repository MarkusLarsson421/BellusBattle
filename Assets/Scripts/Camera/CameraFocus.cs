using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraFocus : MonoBehaviour
{
	[SerializeField] [Tooltip("Offset relative to its' starting position.")]
	private Vector3 offset;
	[SerializeField] [Tooltip("How smooth the camera repositions itself.")]
	private float smoothTime = 0.5f;
	[SerializeField] [Tooltip("The furthest out the camera can zoom out.")]
	private float minZoom = 40.0f;
	[SerializeField] [Tooltip("The closest in the camera can zoom in.")]
	private float maxZoom = 10.0f;
	[SerializeField]
	private float zoomLimiter = 50.0f;
	[SerializeField] [Tooltip("List of targets to track.")]
	private List<Transform> targets;
	[SerializeField] [Tooltip("Whether or not to automatically add players throughout the game.")]
	private bool autoAddPlayers = true;
	
	private Vector3 _velocity;
	private Camera _cam;

	private void Start()
	{
		offset = transform.position;
		_cam = GetComponent<Camera>();
	}

    private void Update()
    {
	    if (autoAddPlayers)
	    {
		    AutoAdd();
	    }
    }

    private void LateUpdate()
	{
		if (targets.Count == 0) {return;}

		Bounds bounds = GetTargetsBounds();
		Reposition(bounds);
		Focus(bounds);
	}

	/*
	 * Adds a target for the camera to follow.
	 */
	public bool AddTarget(Transform t)
	{
		if (targets.Contains(t)){return false;}
		
		targets.Add(t);
		return true;
	}
	
	/*
	 * Removes a target for the camera to stop following.
	 */
	public bool RemoveTarget(Transform t)
	{
		if (targets.Contains(t) == false) {return false;}

		targets.Remove(t);
		return true;
	}

	/*
	 * Automatically adds players to the list.
	 */
	private void AutoAdd()
	{
		GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
		foreach (GameObject player in players)
		{
			AddTarget(player.transform);
		}
	}

	/*
	 * Repositions the camera over time to the center of the bounds offset with the given offset.
	 */
	private void Reposition(Bounds bounds)
	{
		Vector3 median = GetMedian(bounds);
		Vector3 newPos = median + offset;
		transform.position = Vector3.SmoothDamp(transform.position, newPos, ref _velocity, smoothTime);
	}

	/*
	 * Focuses the camera over time based on the width of the bounding box scaled with the zoom limiter.
	 */
	private void Focus(Bounds bounds)
	{
		float newZoom = Mathf.Lerp(maxZoom, minZoom, (bounds.size.x + bounds.size.y) / zoomLimiter);
		_cam.fieldOfView = Mathf.Lerp(_cam.fieldOfView, newZoom, Time.deltaTime);
	}

	/*
	 * Returns the center of the bounding box.
	 */
	private Vector3 GetMedian(Bounds bounds)
	{
		if (targets.Count == 1)
		{
			return targets[0].position;
		}
		
		return bounds.center;
	}

	/*
	 * Gets the a bounding box encapsulating all targets.
	 */
	private Bounds GetTargetsBounds()
	{
		Bounds bounds = new Bounds(targets[0].position, Vector3.zero);
		for (int i = 0; i < targets.Count; i++)
		{
			bounds.Encapsulate(targets[i].position);
		}

		return bounds;
	}
}
