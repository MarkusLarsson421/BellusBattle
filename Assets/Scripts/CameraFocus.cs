using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraFocus : MonoBehaviour
{
	public List<Transform> targets;
	public Vector3 offset;
	public float smoothTime = 0.5f;

	public float minZoom = 40.0f;
	public float maxZoom = 10.0f;
	public float zoomLimiter = 50.0f;

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

		Move();
		Zoom();
	}

	private void Move()
	{
		Vector3 median = GetMedian();

		Vector3 newPos = median + offset;
		
		transform.position = Vector3.SmoothDamp(transform.position, newPos, ref _velocity, smoothTime);
	}

	private void Zoom()
	{
		float newZoom = Mathf.Lerp(maxZoom, minZoom, GetGreatestDistance() / zoomLimiter);
		_cam.fieldOfView = Mathf.Lerp(_cam.fieldOfView, newZoom, Time.deltaTime);
	}

	private float GetGreatestDistance()
	{
		Bounds bounds = new Bounds(targets[0].position, Vector3.zero);
		for (int i = 0; i < targets.Count; i++)
		{
			bounds.Encapsulate(targets[i].position);
		}

		return bounds.size.x;
	}

	private Vector3 GetMedian()
	{
		if (targets.Count == 1)
		{
			return targets[0].position;
		}

		Bounds bounds = new Bounds(targets[0].position, Vector3.zero);
		for (int i = 0; i < targets.Count; i++)
		{
			bounds.Encapsulate(targets[i].position);
		}

		return bounds.center;
	}
}
