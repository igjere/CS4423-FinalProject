using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float fMoveSpeed = 0f;
    public Transform movePoint;
    public bool bAbleToMove = true;

    public LayerMask whatStopsMovement;

    float AxxH, AxxV;

    // Start is called before the first frame update
    void Start()
    {
        movePoint.parent = null;
        movePoint.position = new Vector3(0.32f, 0.32f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, fMoveSpeed * Time.deltaTime);
        if(Vector3.Distance(transform.position,movePoint.position) == 0f)
        { 
            if(Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 1)
            {
            AxxH = Input.GetAxisRaw("Horizontal");
                if(AxxH > 0f)
                {
                    if (!Physics2D.OverlapCircle(new Vector3(transform.position.x + 0.64f, transform.position.y, transform.position.z), 0.2f, whatStopsMovement))
                    { movePoint.position = new Vector3(transform.position.x + 0.64f, transform.position.y, transform.position.z); }
                }
                if (AxxH < 0f)
                {
                    if (!Physics2D.OverlapCircle(new Vector3(transform.position.x - 0.64f, transform.position.y, transform.position.z), 0.2f, whatStopsMovement))
                    { movePoint.position = new Vector3(transform.position.x - 0.64f, transform.position.y, transform.position.z); }
                }
            
            }
            if (Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1)
            {
            AxxV = Input.GetAxisRaw("Vertical");
                if(AxxV > 0f)
                {
                    if (!Physics2D.OverlapCircle(new Vector3(transform.position.x, transform.position.y + 0.64f, transform.position.z), 0.2f, whatStopsMovement))
                    { movePoint.position = new Vector3(transform.position.x, transform.position.y + 0.64f, transform.position.z); }
                }
                if (AxxV < 0f)
                {
                    if (!Physics2D.OverlapCircle(new Vector3(transform.position.x, transform.position.y - 0.64f, transform.position.z), 0.2f, whatStopsMovement))
                    { movePoint.position = new Vector3(transform.position.x, transform.position.y - 0.64f, transform.position.z); }
                }
                    
            }
        }
    }
}
