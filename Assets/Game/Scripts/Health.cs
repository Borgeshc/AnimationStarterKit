using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Health : NetworkBehaviour
{
    public float baseHealth;

    [SyncVar]
    float health;
    Animator anim;
    Shooting shooting;
    Movement movement;
    GameObject collisionDetection;

    void Start()
    {
        health = baseHealth;                                                                                //Set our health to the baseHealth.
        anim = GetComponent<Animator>();                                                                    //Reference the animator.
        shooting = GetComponent<Shooting>();                                                                //Reference the shooting script.
        movement = GetComponent<Movement>();                                                                //Reference the movement script.
        collisionDetection = transform.FindChild("CollisionDetection").gameObject;
    }

    [Command]
    public void CmdTookDamage(float damage, CollisionDetection.CollisionFlag collisionLocation)
    {
        RpcTookDamage(damage, collisionLocation);
    }

    [Command]
    public void CmdHeadshotDamage(float headshotDamage, CollisionDetection.CollisionFlag collisionLocation)
    {
        RpcHeadshotDamage(headshotDamage, collisionLocation);
    }

    [ClientRpc]
    public void RpcTookDamage(float damage, CollisionDetection.CollisionFlag collisionLocation)                //This is called from CollisionDetection to determine the damage and the location of the incoming collision.
    {
        print("took damage called");
        health -= damage;

        if(health <= 0)
        {
            Died(collisionLocation);
        }
    }

    [ClientRpc]
    public void RpcHeadshotDamage(float headshotDamage, CollisionDetection.CollisionFlag collisionLocation)    //This is called from CollisionDetection to determine the damage and the location of the incoming collision.
    {                                                                                                       //This is a seperate method from TookDamage in order to have different damage for headshots.
        health -= headshotDamage;

        if (health <= 0)
        {
            Died(collisionLocation);                                                                        
        }
    }

    void Init()                                                                                              //Used to reReference scripts as needed
    {
        shooting = GetComponent<Shooting>();
        movement = GetComponent<Movement>();
        anim = GetComponent<Animator>();
    }

    void Died(CollisionDetection.CollisionFlag collisionLocation)                                           //Died gets called when health is or goes below 0.
    {
        Init();                                                                                            
        shooting.canShoot = false;                                                                          //Stop all shooting and movement
        movement.canMove = false;
        Destroy(collisionDetection);

        switch (collisionLocation)
        {
            case CollisionDetection.CollisionFlag.FrontHeadShot:
                anim.SetTrigger("FrontHeadShot");
                break;
            case CollisionDetection.CollisionFlag.BackHeadShot:
                anim.SetTrigger("BackHeadShot");
                break;
            case CollisionDetection.CollisionFlag.Front:
                anim.SetTrigger("Front");
                break;
            case CollisionDetection.CollisionFlag.Back:
                anim.SetTrigger("Back");
                break;
            case CollisionDetection.CollisionFlag.Left:
                anim.SetTrigger("Left");
                break;
            case CollisionDetection.CollisionFlag.Right:
                anim.SetTrigger("Right");
                break;
        }

        Destroy(gameObject, 10);                                                                            //Destroy the gameobject after 10seconds.
    }
}
