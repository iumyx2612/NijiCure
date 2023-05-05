using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// This scripts spread out the request over frames in order to avoid game freeze when all enemies 
// request the path to player at the same time
public class PathRequestManager : MonoBehaviour
{
    // Create a data structure that holds all of parameters for func: RequestPath
    struct PathRequest
    {
        public Vector2 pathStart;
        public Vector2 pathEnd;
        public Action<List<Vector2>, bool> callback;

        public PathRequest(Vector2 _start, Vector2 _end, Action<List<Vector2>, bool> _callback)
        {
            pathStart = _start;
            pathEnd = _end;
            callback = _callback;
        }
    }
    
    // Store all request into Queue
    Queue<PathRequest> pathRequestQueue = new Queue<PathRequest>();
    private PathRequest currentPathRequest;
    
    // Singleton architecture
    private static PathRequestManager instance;

    private PathFinding pathFinding;
    private bool isProcessingPath;
    
    
    private void Awake()
    {
        instance = this;
        pathFinding = gameObject.GetComponent<PathFinding>();
    }
    
    // This function puts all the requests into a Queue to process it one by one
    // This functions is called by Enemy script
    // param: callback will be the function to indicate how enemy follows player, which will be created in Enemy script
    public static void RequestPath(Vector2 pathStart, Vector2 pathEnd, Action<List<Vector2>, bool> callback)
    {
        // Create a new Request, which is a PathRequest
        PathRequest newRequest = new PathRequest(pathStart, pathEnd, callback);
        // Put the Request into Queue
        instance.pathRequestQueue.Enqueue(newRequest);
        // Process the request
        instance.TryProcessNext();
    }
    
    // Check if we are processing the path
    // If not we ask the PathFinding script to process the next one 
    private void TryProcessNext()
    {
        if (!isProcessingPath && pathRequestQueue.Count > 0)
        {
            currentPathRequest = pathRequestQueue.Dequeue();
            isProcessingPath = true;
            pathFinding.StartFindPath(currentPathRequest.pathStart, currentPathRequest.pathEnd);
        }
    }
    
    // Called by the PathFinding Script once finished finding the path
    public void FinishedProcessingPath(List<Vector2> path, bool success)
    {
        currentPathRequest.callback(path, success);
        isProcessingPath = false;
        TryProcessNext();
    }
}
