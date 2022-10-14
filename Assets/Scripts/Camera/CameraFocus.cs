using System.Collections.Generic;
using UnityEngine;

/*
 * CameraFocus will keep its focus on the targets you give it and readjust once a target is removed.
 * 
 * It has to be attached to a GameObject with the Camera component.
 * This class requires the developer to remove the target from the list using the relevant methods once
 * it is no longer needed.
 */
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

	[SerializeField] public List<Transform> _targets;
	private Vector3 _velocity;
	private Camera _cam;

	private void Start()
	{
		offset = transform.position;
		_cam = GetComponent<Camera>();

		GameObject[] players = GameObject.FindGameObjectsWithTag("Player"); // Used for when changing level
		foreach(GameObject tr in players)
        {
			_targets.Add(tr.transform);
		}
		
 		
	}

	private void LateUpdate()
	{

		if (_targets.Count == 0) {return;}

		Bounds bounds = GetTargetsBounds();
		Reposition(bounds);
		Focus(bounds);
	}

	/*
	 * Adds a target for the camera to follow.
	 */
	public void AddTarget(Transform t)
	{
		_targets.Add(t);
	}
	
	/*
	 * Removes a target for the camera to stop following.
	 */
	public void RemoveTarget(Transform t)
	{
		_targets.Remove(t);
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
		if (_targets.Count == 1)
		{
			return _targets[0].position;
		}
		
		return bounds.center;
	}

	/*
	 * Gets the a bounding box encapsulating all targets.
	 */
	private Bounds GetTargetsBounds()
	{
		Bounds bounds = new Bounds(_targets[0].position, Vector3.zero);
		for (int i = 0; i < _targets.Count; i++)
		{
			bounds.Encapsulate(_targets[i].position);
		}

		return bounds;
	}
}
