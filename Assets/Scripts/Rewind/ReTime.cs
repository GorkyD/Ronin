using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReTime : MonoBehaviour 
{
	[HideInInspector] public bool isRewinding = false;
	[HideInInspector] public bool UseInputTrigger;
	[HideInInspector] public float RewindSeconds = 5;
	[HideInInspector] public float RewindSpeed = 1;
	[HideInInspector] public bool PauseEnd;
	
	public string KeyTrigger = "R";
	public Animator animator;
	
	private LinkedList<PointInTime> PointsInTime;
	private bool hasAnimator = false;
	private bool hasRb = false;
	private bool isFeeding = true;
	private ParticleSystem Particles;
	
	private void Start () 
	{
		PointsInTime = new LinkedList<PointInTime> ();

		if (GetComponent<ParticleSystem>())
		{
			Particles = GetComponent<ParticleSystem> ();
		}

		if (UseInputTrigger)
		{
			KeyTrigger = KeyTrigger.ToLower ();
		}
		
		if (GetComponent<Animator> ()) 
		{
			hasAnimator = true;
			animator = GetComponent<Animator> ();
		}

		if (GetComponent<Rigidbody>())
		{
			hasRb = true;
		}
		
		foreach(Transform child in transform)
		{
			child.gameObject.AddComponent<ReTime> ();
			child.GetComponent<ReTime> ().UseInputTrigger = UseInputTrigger;
			child.GetComponent<ReTime> ().KeyTrigger = KeyTrigger;
			child.GetComponent<ReTime> ().RewindSeconds = RewindSeconds;
			child.GetComponent<ReTime> ().RewindSpeed = RewindSpeed;
			child.GetComponent<ReTime> ().PauseEnd = PauseEnd;
		}
	}

	private void Update () 
	{
		if (UseInputTrigger)
		{
			if (Input.GetKey(KeyTrigger))
			{
				StartRewind ();
			}
			else
			{
				StopRewind ();
			}
		}
	}

	private void FixedUpdate()
	{
		ChangeTimeScale (RewindSpeed);

		if (isRewinding) 
		{
			Rewind ();
		}
		else
		{
			Time.timeScale = 1f;
			if(isFeeding)
				Record ();
		}
	}
	
	private void Rewind()
	{
		if (PointsInTime.Count > 0 ) 
		{
			PointInTime PointInTime = PointsInTime.First.Value;
			transform.position = PointInTime.position;
			transform.rotation = PointInTime.rotation;
			PointsInTime.RemoveFirst();
		} 
		else 
		{
			if (PauseEnd)
			{
				Time.timeScale = 0;
			}
			else
			{
				StopRewind ();
			}
		}
	}
	
	private void Record()
	{
		if(PointsInTime.Count > Mathf.Round(RewindSeconds / Time.fixedDeltaTime))
		{
			PointsInTime.RemoveLast ();
		}
		PointsInTime.AddFirst (new PointInTime (transform.position, transform.rotation));
		if (Particles)
		{
			if (Particles.isPaused) 
			{
				Particles.Play ();
			}
		}
			
	}

	private void StartRewind()
	{
		isRewinding = true;
		if (hasAnimator)
		{
			animator.enabled = false;
		}

		if (hasRb)
		{
			GetComponent<Rigidbody> ().isKinematic = true;
		}
	}

	void StopRewind()
	{
		Time.timeScale = 1;
		isRewinding = false;
		if (hasAnimator)
		{
			animator.enabled = true;
		}

		if (hasRb)
		{
			GetComponent<Rigidbody> ().isKinematic = false;
		}
	}
		
	private void ChangeTimeScale(float speed){
		Time.timeScale = speed;
		if (speed > 1)
		{
			Time.fixedDeltaTime = 0.02f / speed;
		}
		else
		{
			Time.fixedDeltaTime = speed * 0.02f;
		}
	}
	
	public void StartTimeRewind()
	{
		isRewinding = true;

		if (hasAnimator)
		{
			animator.enabled = false;
		}

		if (hasRb)
		{
			GetComponent<Rigidbody> ().isKinematic = true;
		}
		
		if(transform.childCount > 0)
		{
			foreach (Transform child in transform)
			{
				child.GetComponent<ReTime> ().StartRewind ();
			}
		}
	}
	
	public void StopTimeRewind(){
		isRewinding = false;
		Time.timeScale = 1;
		if (hasAnimator)
		{
			animator.enabled = true;
		}

		if (hasRb)
		{
			GetComponent<Rigidbody> ().isKinematic = false;
		}

		if(transform.childCount > 0)
		{
			foreach (Transform child in transform) 
			{
				child.GetComponent<ReTime> ().StopTimeRewind ();
			}
		}
	}


	public void StopFeeding(){
		isFeeding = false;

		if(transform.childCount > 0){
			foreach (Transform child in transform) 
			{
				child.GetComponent<ReTime> ().StopFeeding ();
			}
		}
	}
	
	public void StartFeeding(){
		isFeeding = true;

		if(transform.childCount > 0)
		{
			foreach (Transform child in transform) 
			{
				child.GetComponent<ReTime> ().StartFeeding ();
			}
		}
	}
	
	void Reset()
	{
		if (GetComponent<ParticleSystem>())
		{
			gameObject.AddComponent<ReTimeParticles> ();
		}
		
		foreach (Transform child in transform) 
		{
			if (child.GetComponent<ParticleSystem>())
			{
				child.gameObject.AddComponent<ReTimeParticles> ();
			}
		}
	}
}
