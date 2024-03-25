using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputHandler : MonoBehaviour
{

    [SerializeField] Creature playerCreature;
    ProjectileThrower projectileThrower;

    void Start()
    {
        projectileThrower = playerCreature.GetComponent<ProjectileThrower>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 input = Vector3.zero;

        // if (Input.GetKey(KeyCode.W))
        // {
        //     input.y += 1;
        // }

        // if (Input.GetKey(KeyCode.S))
        // {
        //     input.y += -1;
        // }

        if (Input.GetKey(KeyCode.A))
        {
            input.x += -1;
        }

        if (Input.GetKey(KeyCode.D))
        {
            input.x += 1;
        }

        if(Input.GetKey(KeyCode.W)){
            input.y += 1;
        }

        if(Input.GetKey(KeyCode.S)){
            input.y -= 1;
        }

        /* if (Input.GetKeyDown(KeyCode.Q))
        {
            playerCreature.GetComponent<SpriteRenderer>().color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
        } */

        if (Input.GetKeyDown(KeyCode.Space))
        {
            playerCreature.Jump();
        }

        // if(Input.GetKeyDown(KeyCode.E)){
        //     projectileThrower.Launch(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        // }

        // Launch projectile on arrow key press
        if(Input.GetKey(KeyCode.UpArrow)){
            projectileThrower.TryLaunch(Vector3.up);
        }
        else if(Input.GetKey(KeyCode.DownArrow)){
            projectileThrower.TryLaunch(Vector3.down);
        }
        else if(Input.GetKey(KeyCode.LeftArrow)){
            projectileThrower.TryLaunch(Vector3.left);
        }
        else if(Input.GetKey(KeyCode.RightArrow)){
            projectileThrower.TryLaunch(Vector3.right);
        }


        playerCreature.MoveCreature(input);


    }
}
