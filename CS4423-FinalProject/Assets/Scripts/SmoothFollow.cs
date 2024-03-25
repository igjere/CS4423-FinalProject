using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothFollow : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] Vector3 offset;
    [SerializeField] float smoothSpeed = 5;
    // Start is called before the first frame update

    void Awake(){

    }

    void Start()
    {

    }



    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 desiredPosition = target.position + offset;
        transform.position = Vector3.Lerp(transform.position,desiredPosition,smoothSpeed*Time.deltaTime);
        //transform.position = target.transform.position + offset;
    }

    public void AssignTarget(Transform newTarget){
        target = newTarget;
    }
}
