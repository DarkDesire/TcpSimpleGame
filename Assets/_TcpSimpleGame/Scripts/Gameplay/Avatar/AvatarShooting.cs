using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class AvatarShooting : MonoBehaviour
{
    public int damagePerShot = 20;                  // The damage inflicted by each bullet.
    public float timeBetweenBullets = 0.15f;        // The time between each shot.

    public float range = 100f;                      // The distance the gun can fire.

	int shootableMask;                              // A layer mask so the raycast only hits things on the shootable layer.

    float timer;                                    // A timer to determine when to fire.
    Ray shootRay;                                   // A ray from the gun end forwards.
    RaycastHit shootHit;                            // A raycast hit to get information about what was hit.


	private NetworkManager networkManager;

    void Awake ()
	{
		networkManager = FindObjectOfType<NetworkManager>();
        // Create a layer mask for the Shootable layer.
        shootableMask = LayerMask.GetMask ("Shootable");

    }


    void Update ()
    {
        // Add the time since Update was last called to the timer.
        timer += Time.deltaTime;

#if !MOBILE_INPUT
        // If the Fire1 button is being press and it's time to fire...
		if(Input.GetButton ("Fire1") && timer >= timeBetweenBullets && Time.timeScale != 0)
        {
            // ... shoot the gun.
            Shoot ();
        }
#else
        // If there is input on the shoot direction stick and it's time to fire...
        if ((CrossPlatformInputManager.GetAxisRaw("Mouse X") != 0 || CrossPlatformInputManager.GetAxisRaw("Mouse Y") != 0) && timer >= timeBetweenBullets)
        {
            // ... shoot the gun
            Shoot();
        }
#endif
    }
		

    void Shoot ()
	{
        // Reset the timer.
        timer = 0f;

        // Set the shootRay so that it starts at the end of the gun and points forward from the barrel.
        shootRay.origin = transform.position;
        shootRay.direction = transform.forward;

        // Perform the raycast against gameobjects on the shootable layer and if it hits something...
        if(Physics.Raycast (shootRay, out shootHit, range, shootableMask))
		{
			// Try and find an Avatar script on the gameobject hit.
			Avatar enemyAvatar = shootHit.collider.GetComponent <Avatar> ();

			// If the Avatar component exist...
			if(enemyAvatar != null)
			{
                // ... take his id and send shoot to server
				networkManager.SendShootToAvatar (enemyAvatar.AvatarId);
			}
		} 
		else 
		{
			networkManager.SendShoot (shootRay.origin + shootRay.direction * range);
		}
    }
}