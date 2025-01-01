using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float speed = 5.0f;
    public Transform pointA;
    public Transform pointB;
    private Vector3 target;
    // Start is called before the first frame update
    void Start()
    {
        target = pointB.position;
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
        if (Vector3.Distance(transform.position, target) < 0.1f)
        {
            target = target == pointA.position ? pointB.position : pointA.position;
        }
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player is on the platform");
            other.transform.SetParent(transform);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.transform.SetParent(null);
            Debug.Log("Player is off the platform");
        }
    }
    
    
}
