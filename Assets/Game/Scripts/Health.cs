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

    public void TookDamage(float damage, CollisionDetection.CollisionFlag collisionLocation)                //This is called from CollisionDetection to determine the damage and the location of the incoming collision.
    {
        print("took damage called");
        health -= damage;

        if(health <= 0)
        {
            Died(collisionLocation);
        }
    }

    public void HeadshotDamage(float headshotDamage, CollisionDetection.CollisionFlag collisionLocation)    //This is called from CollisionDetection to determine the damage and the location of the incoming collision.
    {                                                                                                       //This is a seperate method from TookDamage in order to have different damage for headshots.
        health -= headshotDamage;

        if (health <= 0)
        {
            Died(collisionLocation);                                                                        
        }
    }

    void Died(CollisionDetection.CollisionFlag collisionLocation)                                           //Died gets called when health is or goes below 0.
    {                                                                                                       //We take in the collision location in order to determine which death animation we want to play.
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
