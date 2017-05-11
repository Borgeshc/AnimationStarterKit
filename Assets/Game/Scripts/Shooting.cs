using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using InControl;
using UnityEngine.Networking;

public class Shooting : NetworkBehaviour
{
    public GameObject muzzleFlash;
    public GameObject bulletHole;
    public Text ammoText;
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
    Animator anim;
    CameraController camControl;

    void Start()
    {
        canShoot = true;
        anim = GetComponent<Animator>();                                                                        //Reference animator.
        ammo = maxAmmo;                                                                                         //Set the ammo to the max ammo. 
        ammoText.text = ammo + "/" + maxAmmo;
        camControl = GetComponentInChildren<CameraController>();
    }

    void Update()
    {
        inputDevice = InputManager.ActiveDevice;                                                                //Reference the current active device.

        if(inputDevice.Action3.WasPressed && !reloading)                                                        //Check to see if the player wants to reload.
        {
            reloading = true;
            StartCoroutine(Reload());
        }

        if(inputDevice.RightTrigger && !reloading && canShoot && camControl.isAiming)
        {                                                                                                       //Check to see if we are shooting. Raycast from the camera to see what the player is shooting at.
            if(!firing)
            {
                firing = true;

                StartCoroutine(Fire());
                
            }
        }
    }

    //[Command]
    //void CmdFire()
    //{
    //    RpcFire();
    //}

    //[ClientRpc]
    //void RpcFire()
    //{
    //    StartCoroutine(Fire());
    //}
    
    [Client]
    IEnumerator Fire()                                                                                          //Subtracts ammo and checks to see if we need to reload.
    {
        CmdStartMuzzleFlash();
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 1000, layermask))
        {
            print(hit.transform.name);
            if (hit.transform.tag == "Collision")
            {
                Debug.LogError(hit.transform.parent.transform.parent.name);
                CmdPlayerShot(hit.transform.parent.transform.parent.name);
            }
            else
            {
                Vector3 position = hit.point + (hit.normal * .1f);
                Quaternion rotation = Quaternion.LookRotation(hit.normal);
                CmdBulletHole(position, rotation);                                                                    //Spawn the bullet hole where the raycast hit.
            }
        }

        ammo--;
        ammoText.text = ammo + "/" + maxAmmo;

        if (ammo <= 0)
        {
            reloading = true;
            StartCoroutine(Reload());
        }
        yield return new WaitForSeconds(fireFreq);
        firing = false;
    }

    [Command]
    void CmdPlayerShot(string hitPlayer)
    {
        print("called player shot");
        if(GameManager.GetPlayer(hitPlayer).transform.gameObject.GetComponentInChildren<CollisionDetection>() != null)
        GameManager.GetPlayer(hitPlayer).transform.gameObject.GetComponentInChildren<CollisionDetection>().OnHit(transform.gameObject);    //If the raycast hit a player collider, let the CollisionDetection script on that object know.
    }

    IEnumerator Reload()                                                                                        //Wait the length of the reload time and reset ammo count to max ammo.
    {
        anim.SetBool("ReloadIdle", true);
        yield return new WaitForSeconds(reloadTime);
        ammo = maxAmmo;
        ammoText.text = ammo + "/" + maxAmmo;
        reloading = false;
        
        anim.SetBool("ReloadIdle", false);
    }

    [Command]
    void CmdStartMuzzleFlash()
    {
        RpcStartMuzzleFlash();
    }

    [ClientRpc]
    void RpcStartMuzzleFlash()
    {
        StartCoroutine(MuzzleFlash());                                                                  //Activate the MuzzleFlash
    }
    
    IEnumerator MuzzleFlash()                                                                                   //Activate and DeActivate the muzzle flash
    {
        muzzleFlash.SetActive(true);
        yield return new WaitForSeconds(.05f);
        muzzleFlash.SetActive(false);
    }

    [Command]
    void CmdBulletHole(Vector3 position, Quaternion rotation)
    {
        GameObject hole = Instantiate(bulletHole, position, rotation) as GameObject;
        NetworkServer.Spawn(hole);
    }
}
