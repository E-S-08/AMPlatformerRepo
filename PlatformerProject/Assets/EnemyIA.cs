using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class EnemyIA : MonoBehaviour
{
    GameObject player;
    //the speed at which the enemy will move
    public float chaseSpeed = 5.0F;
    //how close the player needs to be for the enemy to start chasing
    public float chaseTriggerDistance = 10;
    //do we want the enemy to return home when the player gets away?
    public bool returnHome = true;
   //my home position = my starting position
    Vector3 home;
    //should we patrol or not?
    public bool patrol = true;
    //what direction we want to patrol in?
    public Vector3 patrolDirection = Vector3.zero;
    //how far do we want to patrol
    public float patrolDistance = 3f;
    bool isHome = true;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        home = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //figure out where the player is, how far away they are
        //how far away is the player from me, the enemy???
        //destination (player position) - starting position (enemy position)
        Vector3 chasedir = player.transform.position - transform.position;
        //If the player is "close", chase the player
        if (chasedir.magnitude < chaseTriggerDistance)
        {
            //chase the player!
            chasedir.Normalize();
            GetComponent<Rigidbody2D>().velocity = chasedir * chaseSpeed;
            isHome = false;
            Debug.Log("here");
        }
        //if the returnhome variable is true, try to go home
        else if (returnHome && !isHome)
        {
            //return home
            Vector3 homeDir = home - transform.position;
            if (homeDir.magnitude > 0.2f)
            {
                //fo towards home
                homeDir.Normalize();
                GetComponent<Rigidbody2D>().velocity = homeDir * chaseSpeed;
            }
            //if we're close to home
            else
            {
                //stop moving, so we don't twitch like crazy
                GetComponent<Rigidbody2D>().velocity = Vector3.zero;
                isHome = true;
            }
        }
        //if patrol is true, we want to patrol
        else if (patrol)
        {
            //have the enemy patrol in a given direction
            //if they get too far away from thier starting position, flip their direction
            Vector3 displacement = transform.position - home;
            if (displacement.magnitude >= patrolDistance)
            {
                //we have gone too far, we need to turn back
                patrolDirection = -displacement;
            }
            //puhs the enemy RB in the direction of the patrol
            patrolDirection.Normalize();
            GetComponent<Rigidbody2D>().velocity = patrolDirection * chaseSpeed;
        }
        //otherwise stop moving
        else
        {
            GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        }

        
    }
}
