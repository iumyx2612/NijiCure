using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;
using Pathfinding;

public class EnemyMovement : MonoBehaviour
{
    // The target
    [SerializeField] private Transform target;

    private Rigidbody2D rb;
    
    // The Seeker script handled by A*
    private Seeker seeker;
    // Path to target
    private Path path;

    public List<Vector3> pathPos;
    
    // Speed
    public float speed = 2f;
    // Distance to switch to the next waypoint
    public float nextWaypointDistance = 3f;
    // The current waypoint in the Path
    public int currentWaypoint = 0;
    // The name explains itself
    public bool reachedEndOfPath;
    
    // Const
    private float fixedDeltaTime;

    private void Start()
    {
        fixedDeltaTime = Time.fixedDeltaTime;
        seeker = gameObject.GetComponent<Seeker>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        StartCoroutine(FindPath());
    }

    private void FixedUpdate()
    {
        if (path == null)
        {
            return;
        }

        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }

        pathPos = path.vectorPath;

        Vector2 direction = ((Vector2) path.vectorPath[currentWaypoint] - rb.position).normalized;
        rb.MovePosition(rb.position + direction * speed * fixedDeltaTime);
        
        // Check if we should get the next waypoint
        float dist = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
        if (dist < nextWaypointDistance)
        {
            currentWaypoint++;
        }
    }

    private IEnumerator FindPath()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            if (seeker.IsDone())
            {
                seeker.StartPath(transform.position, target.position, OnPathComplete);
            }   
        }
    }
    

    private void OnPathComplete (Path p) 
    {
        if (!p.error) {
            path = p;
            // Reset the waypoint counter so that we start to move towards the first point in the path
            currentWaypoint = 0;
        }
    }
}
