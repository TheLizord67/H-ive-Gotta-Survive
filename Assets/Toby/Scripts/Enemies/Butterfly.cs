using Pathfinding;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Pathfinding;

public class Butterfly : MonoBehaviour
{
    public enum States {Chasing, AttackLight, AttackTelegraphed, Watching, Retreating}

    [SerializeField] private States state;
    
    [SerializeField] private List<GameObject> players;

    [SerializeField] private Path butterflyPath;

    [SerializeField] private float nextWaypointDistance = 3f;

    [SerializeField] private int currentWaypoint = 10;

    [SerializeField] private bool reachedEndOfPath = false;

    [SerializeField] private Rigidbody rb;

    [SerializeField] private Seeker seeker;

    [SerializeField] private float speedCap;
        
    [SerializeField] private float slerp;

    [SerializeField] private float targetDistance;

    [SerializeField] EnemyType butterfly;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        players = GameObject.FindGameObjectsWithTag("Player").ToList();
        players = (List<GameObject>)players.Shuffle();
        butterfly.enemyType.currentTarget = players[0];
    }

    public void UpdatePath()
    {
        if (seeker.IsDone())
        {
            seeker.StartPath(rb.position, butterfly.enemyType.currentTarget.transform.position, OnPathComplete);
        }
    }
    
    public void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            butterflyPath = p;
            currentWaypoint = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        targetDistance = Vector3.Distance(transform.position, butterfly.enemyType.currentTarget.transform.position);
        UpdatePath();
        CapVelocity();

    }

    public void IsChasing()
    {

    }

    public void IsLight()
    {

    }

    public void IsTelegraphed()
    {

    }

    public void IsWatching()
    {

    }

    public void IsRetreating()
    {

    }
    public void CapVelocity()
    {
        if (rb.linearVelocity.x < -speedCap)
        {
            rb.linearVelocity = new(-speedCap, rb.linearVelocity.y);
            if (rb.linearVelocity.z < -speedCap)
            {
                rb.linearVelocity = new(-speedCap, rb.linearVelocity.y, -speedCap);
            }
        }
        if (rb.linearVelocity.z < -speedCap)
        {
            rb.linearVelocity = new(rb.linearVelocity.x, rb.linearVelocity.y, -speedCap);
            if (rb.linearVelocity.x < -speedCap)
            {
                rb.linearVelocity = new(-speedCap, rb.linearVelocity.y, -speedCap);
            }
        }
        else if (rb.linearVelocity.x > speedCap)
        {
            rb.linearVelocity = new(speedCap, rb.linearVelocity.y);
            if (rb.linearVelocity.z > speedCap)
            {
                rb.linearVelocity = new(speedCap, rb.linearVelocity.y, speedCap);
            }
        }
        else if (rb.linearVelocity.z > speedCap)
        {
            rb.linearVelocity = new(rb.linearVelocity.x, rb.linearVelocity.y, speedCap);
            if (rb.linearVelocity.z > speedCap)
            {
                rb.linearVelocity = new(speedCap, rb.linearVelocity.y, speedCap);
            }
        }
    }

}