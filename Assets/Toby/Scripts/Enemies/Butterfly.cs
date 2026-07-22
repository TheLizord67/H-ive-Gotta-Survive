using Pathfinding;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Pathfinding;
using System.Collections;

public class Butterfly : MonoBehaviour
{
    public enum States {Chasing, Attacking, Watching, Retreating, Roaming}

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

    [SerializeField] private EnemyType butterfly;

    [SerializeField] private GameObject spawn;

    [SerializeField] private GameObject retreatPoint, node;

    [SerializeField] private GameObject attackBox;

    [SerializeField] private float watchTimeMin, watchTimeMax, watchTimeCurrent;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        retreatPoint = Instantiate(node, transform.position, Quaternion.identity);
        //players = GameObject.FindGameObjectsWithTag("Player").ToList();
        //players = (List<GameObject>)players.Shuffle();
        //butterfly.enemyType.currentTarget = players[0];
        butterfly.enemyType.currentTarget = spawn;
        state = States.Roaming;
    }

    // 0 - searching for a target : 1 - searching for if it can attack
    public void FindTarget(int searchType)
    {
        if (searchType == 0)
        {
            RaycastHit[] hits = Physics.SphereCastAll(transform.position, butterfly.enemyType.searchDistance, Vector3.up, butterfly.enemyType.searchDistance);
            foreach (RaycastHit hit in hits)
            {
                if (hit.collider.gameObject.CompareTag("Player"))
                {
                    state = States.Watching;
                    watchTimeCurrent = Random.Range(watchTimeMin, watchTimeMax);
                    butterfly.enemyType.currentTarget = hit.collider.gameObject;
                    break;
                }
                else if (hit.collider.gameObject.CompareTag("Structure"))
                {
                    state = States.Chasing;
                    butterfly.enemyType.currentTarget = hit.collider.gameObject;

                }
                else
                {
                    if (state == States.Chasing)
                    {
                        break;
                    }
                    else
                    {
                        butterfly.enemyType.currentTarget = spawn;
                        state = States.Roaming;
                    }
                }
            }
        }
        if (searchType == 1)
        {
            if (targetDistance <= butterfly.enemyType.hitDistance)
            {
                state = States.Attacking;
                int result = Statics.RollDice(1, 7);
                if (result <= 4)
                {
                    StartCoroutine(Attack(0));
                }
                else
                {
                    StartCoroutine(Attack(1));
                }
            }
        }
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
        FaceTarget(butterfly.enemyType.currentTarget.transform);
        targetDistance = Vector3.Distance(transform.position, butterfly.enemyType.currentTarget.transform.position);
        UpdatePath();
        CapVelocity();
        if (state == States.Roaming)
        {
            IsRoaming();
        }
        if (state == States.Watching)
        {
            IsWatching();
        }
        if (state == States.Retreating)
        {
            IsRetreating();
        }
        if (state == States.Chasing)
        {
            IsChasing();
        }
        if (butterfly.enemyType.retreatHealthCurrent <= 0)
        {
            butterfly.enemyType.currentTarget = retreatPoint;
            state = States.Retreating;
        }
    }

    public void IsChasing()
    {
        if (butterfly.enemyType.currentTarget == null)
        {
            butterfly.enemyType.currentTarget = spawn;
            state = States.Roaming;
        }
        if (targetDistance > butterfly.enemyType.searchDistance)
        {
            butterfly.enemyType.followTime -= Time.deltaTime;
            if (butterfly.enemyType.followTime <= 0)
            {
                butterfly.enemyType.currentTarget = spawn;
                state = States.Roaming;
            }
            if (butterfly.enemyType.followTime > 0)
            {
                if (butterflyPath == null)
                {
                    return;
                }
                if (currentWaypoint >= butterflyPath.vectorPath.Count)
                {
                    reachedEndOfPath = true;
                    return;
                }
                else
                {
                    reachedEndOfPath = false;
                }
                Vector3 direction = (butterflyPath.vectorPath[currentWaypoint + 1] - transform.position).normalized;
                rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, direction * butterfly.enemyType.speed, Time.deltaTime * slerp);
                float distance = Vector3.Distance(rb.position, butterflyPath.vectorPath[currentWaypoint + 1]);
                if (distance < nextWaypointDistance)
                {
                    currentWaypoint++;
                }
                FindTarget(0);

            }
        }
        else
        {
            butterfly.enemyType.followTime = butterfly.enemyType.followTimeMax;
            FindTarget(1);
            if (butterflyPath == null)
            {
                return;
            }
            if (currentWaypoint >= butterflyPath.vectorPath.Count)
            {
                reachedEndOfPath = true;
                return;
            }
            else
            {
                reachedEndOfPath = false;
            }
            Vector3 direction = (butterflyPath.vectorPath[currentWaypoint + 1] - transform.position).normalized;
            rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, direction * butterfly.enemyType.speed, Time.deltaTime * slerp);
            float distance = Vector3.Distance(rb.position, butterflyPath.vectorPath[currentWaypoint + 1]);
            if (distance < nextWaypointDistance)
            {
                currentWaypoint++;
            }
        }
    }

    // 0 - Telegraphed Attack : 1 - Quick Attack
    public IEnumerator Attack(int type)
    {
        if (type == 0)
        {
            yield return new WaitForSeconds(butterfly.enemyType.attackSpeed);
            attackBox.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            attackBox.SetActive(false);
            state = States.Chasing;
        }
        else if (type == 1)
        {
            yield return new WaitForSeconds(butterfly.enemyType.attackSpeed / 2);
            attackBox.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            attackBox.SetActive(false);
            state = States.Chasing;
        }
    }
    public void IsWatching()
    {
        watchTimeCurrent -= Time.deltaTime;
        if (watchTimeCurrent <= 0)
        {
            state = States.Chasing;
        }
    }
    public void IsRetreating()
    {
        butterfly.enemyType.retreatHealthCurrent += 1;
        if (butterfly.enemyType.retreatHealthCurrent < butterfly.enemyType.retreatHealthMax)
        {
            butterfly.enemyType.followTime = butterfly.enemyType.followTimeMax;
            FindTarget(1);
            if (butterflyPath == null)
            {
                return;
            }
            if (currentWaypoint >= butterflyPath.vectorPath.Count)
            {
                reachedEndOfPath = true;
                return;
            }
            else
            {
                reachedEndOfPath = false;
            }
            Vector3 direction = (butterflyPath.vectorPath[currentWaypoint + 1] - transform.position).normalized;
            rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, direction * (butterfly.enemyType.speed * 2), Time.deltaTime * slerp);
            float distance = Vector3.Distance(rb.position, butterflyPath.vectorPath[currentWaypoint + 1]);
            if (distance < nextWaypointDistance)
            {
                currentWaypoint++;
            }
        }
        if (butterfly.enemyType.retreatHealthCurrent >= butterfly.enemyType.retreatHealthMax)
        {
            butterfly.enemyType.retreatHealthCurrent = butterfly.enemyType.retreatHealthMax;
            FindTarget(0);
        }
    }
    public void IsRoaming()
    {
        FindTarget(0);
        if (butterflyPath == null)
        {
            return;
        }
        if (currentWaypoint >= butterflyPath.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }
        Vector3 direction = (butterflyPath.vectorPath[currentWaypoint + 1] - transform.position).normalized;
        rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, direction * (butterfly.enemyType.speed * 0.7f), Time.deltaTime * slerp);
        float distance = Vector3.Distance(rb.position, butterflyPath.vectorPath[currentWaypoint + 1]);
        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }
    }
    public void FaceTarget(Transform target)
    {
        Vector3 directionToTarget = target.position - transform.position;

        Quaternion lookRotation = Quaternion.LookRotation(directionToTarget);

        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, butterfly.enemyType.rotateSpeed * Time.deltaTime);
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