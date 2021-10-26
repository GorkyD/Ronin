using UnityEngine;

public class ReTimeParticles : MonoBehaviour 
{
	[HideInInspector] public float PassedTime;
	public float ResettingPoint;
	
	#region Private Variables
	private ParticleSystem Particles;
	private ReTime CoreReTime;
	private float PlaybackTime;
	private float LoopingTime;
	private int PSLoops;
	private float t = 0f;
	private float MaxTime = 0f;
	private bool stop = false;
	private bool init = true;
	#endregion
	
	private void Start () 
	{
		Particles = GetComponent<ParticleSystem> ();
		PassedTime = 0.0f;
		LoopingTime = 0.0f;
		PSLoops = 0;
	}
		
	
	private void Update () 
	{
		if (!CoreReTime)
		{
			CoreReTime = GetComponent<ReTime> ();
		}

		if (Particles.main.loop)
		{
			ParticleSystemLoopTracker ();
		}
		
		if (!CoreReTime.isRewinding) 
		{
			init = true;
			if (!Particles.IsAlive())
			{
				PassedTime += Time.deltaTime;
			}

			if (Particles.main.loop)
			{
				LoopingTime += Time.deltaTime;
			}
			
		} 
		else 
		{
			PlaybackTime = Particles.time;
			PassedTime -= Time.deltaTime;
			Particles.Stop ();
			init = false;
			
			if (Particles.main.loop) 
			{
				if (PSLoops > 0) 
				{
					if ( PlaybackTime <= ResettingPoint) 
					{
						PlaybackTime = MaxTime;
						PSLoops--;
					}
				}
				Particles.Simulate (PlaybackTime - Time.deltaTime);
			} 
			else 
			{
				if (PassedTime <= 0.0f)
				{
					Particles.Simulate (PlaybackTime - Time.deltaTime);
				}
			}
		}
	}

	private void ParticleSystemLoopTracker()
	{
		float time = Particles.time;

		if (init) 
		{
			t = Particles.main.duration;
			
			if (t - time <= 0.1f) 
			{
				if (!stop) 
				{
					MaxTime = t;
					PSLoops++;
					stop = true;
				}
			} 
			else 
			{
				stop = false;
			}
		}
	}
}
