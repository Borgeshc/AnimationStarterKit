using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Health : NetworkBehaviour
{
    public float baseHealth;
    public float respawnTime;

    [SyncVar]
    float health;
    Animator anim;
    Shooting shooting;
    Movement movement;
    GameObject collisionDetection;
    [SyncVar]
    bool isDead;

    void Start()
    {
        Init();
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
    {
        if (isDead)
            return;

        health -= headshotDamage;

        if (health <= 0)
        {
            Died(collisionLocation);
        }
    }

    public void Init()                                                                                      //Used to reReference scripts as needed
    {
        isDead = false;
        health = baseHealth;
        anim = GetComponent<Animator>();                                                                    //Set our health to the baseHealth.
        shooting = GetComponent<Shooting>();
        movement = GetComponent<Movement>();
        shooting.canShoot = true;
        movement.canMove = true;
        collisionDetection = transform.FindChild("CollisionDetection").gameObject;
        foreach (Transform go in collisionDetection.GetComponentsInChildren<Transform>())
            go.gameObject.layer = LayerMask.NameToLayer("Collision");
    }

    void Died(CollisionDetection.CollisionFlag collisionLocation)                                           //Died gets called when health is or goes below 0.
    {
        print("Died");
        Init();
        isDead = true;                                                                                 
        shooting.canShoot = false;                                                                          //Stop all shooting and movement
        movement.canMove = false;

        foreach(Transform go in collisionDetection.GetComponentsInChildren<Transform>())
            go.gameObject.layer = LayerMask.NameToLayer("Default");

        switch (collisionLocation)
        {
            case CollisionDetection.CollisionFlag.FrontHeadShot:
                anim.SetBool("FrontHeadShot", true);
                break;
            case CollisionDetection.CollisionFlag.BackHeadShot:
                anim.SetBool("BackHeadShot", true);
                break;
            case CollisionDetection.CollisionFlag.Front:
                anim.SetBool("Front", true);
                break;
            case CollisionDetection.CollisionFlag.Back:
                anim.SetBool("Back", true);
                break;
            case CollisionDetection.CollisionFlag.Left:
                anim.SetBool("Left", true);
                break;
            case CollisionDetection.CollisionFlag.Right:
                anim.SetBool("Right", true);
                break;
        }
        StartCoroutine(Respawn());
    }

    IEnumerator Respawn()
    {
        print("Respawning..");
        yield return new WaitForSeconds(respawnTime);
        Transform respawnpoint = NetworkManager.singleton.GetStartPosition();
        transform.position = respawnpoint.position;
        transform.rotation = respawnpoint.rotation;

        ResetDeathAnims();
        Init();
        shooting.ResetAmmo();
    }

    void ResetDeathAnims()
    {
        anim.SetBool("FrontHeadShot", false);
        anim.SetBool("BackHeadShot", false);
        anim.SetBool("Front",false);
        anim.SetBool("Back", false);
        anim.SetBool("Left", false);
        anim.SetBool("Right", false);
    }
}
