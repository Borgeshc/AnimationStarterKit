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
    
    //[Command]
    //public void CmdOnHit(GameObject other)
    //{
    //    RpcOnHit(other);
    //}
    
    public void OnHit(GameObject other)                                                                                            //Gets called from the shooting scripts raycast, we use other to determine how much damage we will take.
    {
        print("collision called");
        switch(collisionLocation)                                                                                           //Find the collisionLocation this collider is marked with.
        {
            case CollisionFlag.FrontHeadShot:
                if(health.isServer)
                    health.RpcHeadshotDamage(other.GetComponent<Shooting>().headshotDamage, CollisionFlag.FrontHeadShot);
                else
                health.CmdHeadshotDamage(other.GetComponent<Shooting>().headshotDamage, CollisionFlag.FrontHeadShot);              //Tell our health script how much damage we took from the enemies shooting script and the location we were hit from.
                break;
            case CollisionFlag.BackHeadShot:
                if (health.isServer)
                    health.RpcHeadshotDamage(other.GetComponent<Shooting>().headshotDamage, CollisionFlag.BackHeadShot);
                else
                    health.CmdHeadshotDamage(other.GetComponent<Shooting>().headshotDamage, CollisionFlag.BackHeadShot);
                break;
            case CollisionFlag.Front:
                if (health.isServer)
                    health.RpcTookDamage(other.GetComponent<Shooting>().bulletDamage, CollisionFlag.Front);
                else
                    health.CmdTookDamage(other.GetComponent<Shooting>().bulletDamage, CollisionFlag.Front);
                break;
            case CollisionFlag.Back:
                if (health.isServer)
                    health.RpcTookDamage(other.GetComponent<Shooting>().bulletDamage, CollisionFlag.Back);
                else
                    health.CmdTookDamage(other.GetComponent<Shooting>().bulletDamage, CollisionFlag.Back);
                break;
            case CollisionFlag.Left:
                if (health.isServer)
                    health.RpcTookDamage(other.GetComponent<Shooting>().bulletDamage, CollisionFlag.Left);
                else
                    health.CmdTookDamage(other.GetComponent<Shooting>().bulletDamage, CollisionFlag.Left);
                break;
            case CollisionFlag.Right:
                if (health.isServer)
                    health.RpcTookDamage(other.GetComponent<Shooting>().bulletDamage, CollisionFlag.Right);
                else
                    health.CmdTookDamage(other.GetComponent<Shooting>().bulletDamage, CollisionFlag.Right);
                break;
        }
    }

}
