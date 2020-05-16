using UnityEngine;
using System.Collections;


public class ShootSync : MonoBehaviour {

	ParticleSystem gunParticles;                    // Reference to the particle system.
	LineRenderer gunLine;                           // Reference to the line renderer.
	AudioSource gunAudio;                           // Reference to the audio source.
	Light gunLight;                                 // Reference to the light component.

	float timer;                                    // A timer to determine when to fire.
	float effectsDisplayTime = 0.03f;                // The proportion of the timeBetweenBullets that the effects will display for.

	// Update is called once per frame
	void Update () {

		// Add the time since Update was last called to the timer.
		timer += Time.deltaTime;

		// If the timer has exceeded the proportion of timeBetweenBullets that the effects should be displayed for...
		if(timer >= effectsDisplayTime)
		{
			// ... disable the effects.
			ShootOf ();
		}
	}

	void Awake(){
		// Set up the references.
		gunParticles = GetComponent<ParticleSystem> ();
		gunLine = GetComponent <LineRenderer> ();
		gunAudio = GetComponent<AudioSource> ();
		gunLight = GetComponent<Light> ();
	}

	public void ShootOf ()
	{
		// Disable the line renderer and the light.
		gunLine.enabled = false;
		gunLight.enabled = false;
	}


	public void ShootOn (Vector3 target)
	{
		// Reset the timer.
		timer = 0f;

		// Play the gun shot audioclip.
		gunAudio.Play ();

		// Enable the light.
		gunLight.enabled = true;

		// Stop the particles from playing if they were, then start the particles.
		gunParticles.Stop ();
		gunParticles.Play ();

		// Enable the line renderer and set it's first position to be the end of the gun.
		gunLine.enabled = true;
		gunLine.SetPosition (0, transform.position);
		gunLine.SetPosition (1, target);
	}

}
