using System.Collections.Generic;
using UnityEngine;

/*
 * CameraFocus will keep its focus on the targets you give it and readjust once a target is removed.
 * 
 * It has to be attached to a GameObject with the Camera component.
 * This class requires the developer to remove the target from the list once it is no longer needed.
 * Meaning you have to access the targets list and remove the listed transform.
 */
[RequireComponent(typeof(Camera))]
public class CameraFocus : MonoBehaviour
{
	[SerializeField] [Tooltip("The targets for the camera to follow.")]
	private List<Transform> targets;
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

	private Vector3 _velocity;
	private Camera _cam;

	private void Start()
	{
		offset = transform.position;
		_cam = GetComponent<Camera>();
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
	public void AddTarget(Transform t)
	{
		targets.Add(t);
	}
	
	/*
	 * Removes a target for the camera to stop following.
	 */
	public void RemoveTarget(Transform t)
	{
		targets.Remove(t);

		List<Transform> newList = new List<Transform>();
		int j = 0;
		for (int i = 0; i < targets.Capacity; i++)
		{
			if (targets[i] != null)
			{
				newList[j] = targets[i];
			}
			j++;
		}
		targets = newList;
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
		float newZoom = Mathf.Lerp(maxZoom, minZoom, bounds.size.x / zoomLimiter);
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
