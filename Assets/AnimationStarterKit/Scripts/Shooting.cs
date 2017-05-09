using UnityEngine;
using System.Collections;
using InControl;

public class Shooting : MonoBehaviour
{
    public GameObject muzzleFlash;
    public GameObject bulletHole;
    public float fireFreq;
    public float maxAmmo;
    public float reloadTime;
    public LayerMask layermask;

    float ammo;
    bool reloading;
    bool firing;

    InputDevice inputDevice;
    RaycastHit hit;

    void Start()
    {
        ammo = maxAmmo;
    }

    void Update()
    {
        inputDevice = InputManager.ActiveDevice;

        if(inputDevice.Action3.WasPressed && !reloading)
        {
            reloading = true;
            StartCoroutine(Reload());
        }

        if(inputDevice.RightTrigger && Physics.Raycast(Camera.main.transform.position,  Camera.main.transform.forward, out hit, 1000, layermask) && !reloading)
        {
            if(!firing)
            {
                firing = true;
                Instantiate(bulletHole, hit.point + (hit.normal * .1f), Quaternion.LookRotation(hit.normal));
                StartCoroutine(MuzzleFlash());
                StartCoroutine(Fire());
            }
        }
    }

    IEnumerator Fire()
    {
        ammo--;

        if(ammo <= 0)
        {
            reloading = true;
            StartCoroutine(Reload());
        }
        yield return new WaitForSeconds(fireFreq);
        firing = false;
    }

    IEnumerator Reload()
    {
        yield return new WaitForSeconds(reloadTime);
        ammo = maxAmmo;
        reloading = false;
    }

    IEnumerator MuzzleFlash()
    {
        muzzleFlash.SetActive(true);
        yield return new WaitForSeconds(.05f);
        muzzleFlash.SetActive(false);
    }
}
