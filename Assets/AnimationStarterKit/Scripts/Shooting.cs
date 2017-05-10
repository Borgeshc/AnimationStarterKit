using UnityEngine;
using System.Collections;
using InControl;

public class Shooting : MonoBehaviour
{
    public GameObject muzzleFlash;
    public GameObject bulletHole;
    public float bulletDamage;
    public float headshotDamage;
    public float fireFreq;
    public float maxAmmo;
    public float reloadTime;
    public LayerMask layermask;

    [HideInInspector]
    public bool canShoot;

    float ammo;
    bool reloading;
    bool firing;

    InputDevice inputDevice;
    RaycastHit hit;

    void Start()
    {
        canShoot = true;
        ammo = maxAmmo;                                                                                         //Set the ammo to the max ammo.                                                          
    }

    void Update()
    {
        inputDevice = InputManager.ActiveDevice;                                                                //Reference the current active device.

        if(inputDevice.Action3.WasPressed && !reloading)                                                        //Check to see if the player wants to reload.
        {
            reloading = true;
            StartCoroutine(Reload());
        }

        if(inputDevice.RightTrigger && Physics.Raycast(Camera.main.transform.position,  Camera.main.transform.forward, out hit, 1000, layermask) && !reloading && canShoot)
        {                                                                                                       //Check to see if we are shooting. Raycast from the camera to see what the player is shooting at.
            if(!firing)
            {
                firing = true;


                if (hit.transform.tag == "Collision")
                    hit.transform.gameObject.GetComponent<CollisionDetection>().OnHit(transform.gameObject);    //If the raycast hit a player collider, let the CollisionDetection script on that object know.
                else
                Instantiate(bulletHole, hit.point + (hit.normal * .1f), Quaternion.LookRotation(hit.normal));   //Spawn the bullet hole where the raycast hit.
                
                                                                                                                //We pass this gameobject so the CollisionDetection can know how much damage to apply.
                StartCoroutine(MuzzleFlash());                                                                  //Activate the MuzzleFlash
                StartCoroutine(Fire());                                                                         
            }
        }
    }

    IEnumerator Fire()                                                                                          //Subtracts ammo and checks to see if we need to reload.
    {                                                                                                           //Apply the fire frequency.
        ammo--;

        if(ammo <= 0)
        {
            reloading = true;
            StartCoroutine(Reload());
        }
        yield return new WaitForSeconds(fireFreq);
        firing = false;
    }

    IEnumerator Reload()                                                                                        //Wait the length of the reload time and reset ammo count to max ammo.
    {
        yield return new WaitForSeconds(reloadTime);
        ammo = maxAmmo;
        reloading = false;
    }

    IEnumerator MuzzleFlash()                                                                                   //Activate and DeActivate the muzzle flash
    {
        muzzleFlash.SetActive(true);
        yield return new WaitForSeconds(.05f);
        muzzleFlash.SetActive(false);
    }
}
