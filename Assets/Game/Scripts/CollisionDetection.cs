using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class CollisionDetection : NetworkBehaviour
{
    public enum CollisionFlag                                                                                              //List of possible collision locations
    {
        FrontHeadShot,
        BackHeadShot,
        Front,
        Back,
        Left,
        Right
    };

    public CollisionFlag collisionLocation = CollisionFlag.FrontHeadShot;

    Health health;

    void Start()
    {
        health = GetComponentInParent<Health>();                                                                            //References to the health and shooting scripts
    }
    
    public void OnHit(GameObject other)                                                                                            //Gets called from the shooting scripts raycast, we use other to determine how much damage we will take.
    {
        print("collision called");
        switch(collisionLocation)                                                                                           //Find the collisionLocation this collider is marked with.
        {
            case CollisionFlag.FrontHeadShot:
                health.HeadshotDamage(other.GetComponent<Shooting>().headshotDamage, CollisionFlag.FrontHeadShot);              //Tell our health script how much damage we took from the enemies shooting script and the location we were hit from.
                break;
            case CollisionFlag.BackHeadShot:
                health.HeadshotDamage(other.GetComponent<Shooting>().headshotDamage, CollisionFlag.BackHeadShot);
                break;
            case CollisionFlag.Front:
                health.TookDamage(other.GetComponent<Shooting>().bulletDamage, CollisionFlag.Front);
                break;
            case CollisionFlag.Back:
                health.TookDamage(other.GetComponent<Shooting>().bulletDamage, CollisionFlag.Back);
                break;
            case CollisionFlag.Left:
                health.TookDamage(other.GetComponent<Shooting>().bulletDamage, CollisionFlag.Left);
                break;
            case CollisionFlag.Right:
                health.TookDamage(other.GetComponent<Shooting>().bulletDamage, CollisionFlag.Right);
                break;
        }
    }

}
