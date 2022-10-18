using System;
using UnityEngine;

public abstract class Event<T> where T : Event<T>
{
	public string Description;

	public delegate void EventListener(T info);
	private static event EventListener Listeners;

	private bool _hasFired;

	public static void RegisterListener(EventListener listener)
	{
		Listeners += listener;
	}

	public static void UnregisterListener(EventListener listener)
	{
		Listeners -= listener;
	}

	public void FireEvent()
	{
		if (_hasFired){throw new Exception("Event already been fired.");}

		_hasFired = true;
		Listeners?.Invoke(this as T);

		Debug.Log(Description);
	}
}

public class DebugEvent : Event<DebugEvent>
{
	public int VerbosityLevel;
}

public class ExplodeEvent : Event<ExplodeEvent>
{
	public GameObject ExplosionGo;
	/*
	 * Info about cause of death, our killer, etc...
	 * Could be struct, readonly, etc...
	 */
}