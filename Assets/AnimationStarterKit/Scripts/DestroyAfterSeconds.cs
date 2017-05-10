using UnityEngine;
using System.Collections;

public class DestroyAfterSeconds : MonoBehaviour
{
    public float destroyTime;

	void Start ()
    {
        Destroy(gameObject, destroyTime);       //Destroy this object after specified time
	}
}
