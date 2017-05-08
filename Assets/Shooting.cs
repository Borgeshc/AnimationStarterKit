using UnityEngine;
using System.Collections;
using RootMotion;

public class Shooting : MonoBehaviour
{
    RaycastHit hit;
    RootMotion.FinalIK.BipedIK bipedIK;

    private void Start()
    {
        bipedIK = GetComponent<RootMotion.FinalIK.BipedIK>();
    }

    private void Update()
    {
        if(Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 1000))
        {
            //bipedIK.SetLookAtPosition(hit.point);
        }
    }
}
