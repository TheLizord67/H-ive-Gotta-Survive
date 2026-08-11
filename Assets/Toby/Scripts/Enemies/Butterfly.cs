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

    [SerializeField] private List<float> playersDistances;

    [SerializeField] private Path butterflyPath;

    [SerializeField] private float nextWaypointDistance = 3f;

    [SerializeField] private int currentWaypoint = 10;

    [SerializeField] private bool reachedEndOfPath = false;

    [SerializeField] private Animator butterflyAnimator;

    [SerializeField] private AnimationClip fastClip, telegraphedClip;

    [SerializeField] private Rigidbody rb;

    [SerializeField] private Seeker seeker;

    [SerializeField] private float speedCap;
        
    [SerializeField] private float slerp;

    [SerializeField] private float targetDistance, attackDistance;

    [SerializeField] private EnemyType butterfly;

    [SerializeField] private GameObject spawn, currentTarget;

    [SerializeField] private GameObject retreatPoint, node;

    [SerializeField] private GameObject attackBox;

    [SerializeField] private float watchTimeMin, watchTimeMax, watchTimeCurrent, rotateSpeed, followTime;

    [SerializeField] private bool isAttacking;

    private Coroutine lookCoroutine;

    [SerializeField] private List<LookAt> lookAtList;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        retreatPoint = Instantiate(node, transform.position, Quaternion.identity);
        spawn = GameObject.FindGameObjectWithTag("Spawn");
        players = GameObject.FindGameObjectsWithTag("Player").ToList();
        //players = (List<GameObject>)players.Shuffle();
        //butterfly.enemyType.currentTarget = players[0];
        currentTarget = spawn;
        rb.AddForce(transform.forward);
        state = States.Roaming;
        followTime = butterfly.enemyType.followTimeMax;
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
                    currentTarget = hit.collider.gameObject;
                    break;
                }
                else if (hit.collider.gameObject.CompareTag("Structure") && currentTarget != null && currentTarget == spawn)
                {
                    state = States.Chasing;
                    currentTarget = hit.collider.gameObject;
                }
                else
                {
                    if (state == States.Chasing)
                    {
                        break;
                    }
                    else
                    {
                        currentTarget = spawn;
                        state = States.Roaming;
                    }
                }
            }
        }
        if (searchType == 1)
        {
            RaycastHit[] hits = Physics.SphereCastAll(transform.position, butterfly.enemyType.searchDistance, Vector3.up, butterfly.enemyType.searchDistance);
            foreach (RaycastHit hit in hits)
            {
                if (hit.collider.gameObject.CompareTag("Player"))
                {
                    if (currentTarget != hit.collider.gameObject)
                    {
                        currentTarget = hit.collider.gameObject;
                        break;
                    }
                }
            }
            if (targetDistance <= butterfly.enemyType.hitDistance && state != States.Attacking)
            {
                rb.linearVelocity = Vector3.zero;
                isAttacking = true;
                state = States.Attacking;
                int result = Statics.RollDice(1, 7);
                if (result <= 4)
                {
                    butterflyAnimator.SetInteger("Attack Mode", 0);
                    StartCoroutine(Attack(0));
                }
                else
                {
                    butterflyAnimator.SetInteger("Attack Mode", 1);
                    StartCoroutine(Attack(1));
                }
            }
        }
    }
    public void UpdatePath()
    {
        if (seeker.IsDone())
        {
            seeker.StartPath(rb.position, currentTarget.transform.position, OnPathComplete);
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
        FaceTarget();
        targetDistance = Vector3.Distance(transform.position, currentTarget.transform.position);
        UpdatePath();
        CapVelocity();
        if (state == States.Roaming)
        {
            butterflyAnimator.SetBool("IsMoving", true);
            butterflyAnimator.SetInteger("Attack Mode", 2);
            IsRoaming();
        }
        if (state == States.Watching)
        {
            butterflyAnimator.SetBool("IsMoving", false);
            butterflyAnimator.SetInteger("Attack Mode", 2);
            IsWatching();
        }
        if (state == States.Retreating)
        {
            butterflyAnimator.SetBool("IsMoving", true);
            butterflyAnimator.SetInteger("Attack Mode", 2);
            IsRetreating();
        }
        if (state == States.Chasing)
        {
            butterflyAnimator.SetBool("IsMoving", true);
            butterflyAnimator.SetInteger("Attack Mode", 2);
            IsChasing();
        }
        if (state == States.Attacking)
        {
            butterflyAnimator.SetBool("IsMoving", false);
            IsAttacking();
        }
        if (butterfly.enemyType.retreatHealthCurrent <= 0)
        {
            currentTarget = retreatPoint;
            state = States.Retreating;
        }
    }

    public void IsChasing()
    {
        if (currentTarget == null)
        {
            currentTarget = spawn;
            state = States.Roaming;
        }
        if (targetDistance >= butterfly.enemyType.searchDistance)
        {
            followTime -= Time.deltaTime;
            if (followTime <= 0)
            {
                currentTarget = spawn;
                state = States.Roaming;
            }
            if (followTime > 0)
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
                FindTarget(1);
            }
        }
        else
        {
            followTime = butterfly.enemyType.followTimeMax;
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
            yield return new WaitForSeconds(telegraphedClip.length);
            butterflyAnimator.SetInteger("Attack Mode", 2);
            //state = States.Chasing;
            isAttacking = false;
        }
        else if (type == 1)
        {
            yield return new WaitForSeconds(fastClip.length);
            butterflyAnimator.SetInteger("Attack Mode", 2);
            //state = States.Chasing;
            isAttacking = false;
        }
        
    }
    public void IsWatching()
    {
        rb.linearVelocity = Vector3.zero;
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
            followTime = butterfly.enemyType.followTimeMax;
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
    public void IsAttacking()
    {
        FindTarget(1);
        if (!isAttacking)
        {
            rb.linearVelocity = Vector3.zero;
            isAttacking = true;
            state = States.Attacking;
            int result = Statics.RollDice(1, 7);
            if (result <= 4)
            {
                butterflyAnimator.SetInteger("Attack Mode", 0);
                StartCoroutine(Attack(0));
            }
            else
            {
                butterflyAnimator.SetInteger("Attack Mode", 1);
                StartCoroutine(Attack(1));
            }
        }
        else
        {
            if (targetDistance > butterfly.enemyType.hitDistance)
            {
                state = States.Chasing;
            }
        }
    }
    public void FaceTarget()
    {
        foreach (var target in lookAtList)
        {
            target.currentTarget = currentTarget;
        }
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