using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownCreature : MonoBehaviour
{
    Rigidbody2D rb;
    SpriteRenderer spriteRenderer;
    public float speed = 2.5f;
    public AnimationStateChanger asc;

    public bool platformingCreature = false; //when true, change behavior to work like 2D platformer char

    void Start(){
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }


    public void Move(Vector3 offset){
        if(offset != Vector3.zero){
            offset.Normalize();
            //offset *= Time.fixedDeltaTime;
            //rb.MovePosition(transform.position + ((offset)*speed));
            Vector3 vel = offset *= speed;
            if(platformingCreature){
                rb.velocity = new Vector2(vel.x,rb.velocity.y);
            }else{
                rb.velocity = vel;
            }


            asc.ChangeAnimationState("Walking");
            if(offset.x < 0){
                spriteRenderer.flipX = true;
            }else{
                spriteRenderer.flipX = false;
            }
        }else{
            Stop();
        //asc.ChangeAnimationState("Idle");
        }
    }

    public void Stop(){
        //return;
        if(platformingCreature){
            rb.velocity = new Vector2(0,rb.velocity.y);
        }else{
            rb.velocity = Vector3.zero;
        }
        asc.ChangeAnimationState("Idle");


    }

    public void MoveToward(Vector3 position){
        Move(position - transform.position);
    }


}
