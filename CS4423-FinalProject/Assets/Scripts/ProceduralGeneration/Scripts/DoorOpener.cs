using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DoorOpener : MonoBehaviour
{
    public UnityEvent onDoorOpen;
    private bool isDoorInteractionAvailable = true;
    private float doorInteractionCooldown = 2f;

    //this is how we can pass along data to functions through our events
    // [System.Serializable]
    // public class MyEvent : UnityEvent<int> {}
    // public MyEvent myPickupEvent;

    Vector2 roomSizeInTiles = new Vector2(9,17);

    void Start(){
        onDoorOpen.AddListener(PrintDoor);
        // TutorialMovement cam = FindObjectOfType<TutorialMovement>();
    }

    void PrintDoor(){
        Debug.Log("Touched Door!");
    }

    IEnumerator DoorInteractionCooldown()
    {
        
        isDoorInteractionAvailable = false;
        SetAllDoorsColor(false); // Set to cooldown color immediately

        yield return new WaitForSeconds(doorInteractionCooldown);

        isDoorInteractionAvailable = true;
        // After cooldown, recheck the enemy status in the room instead of just setting to green
        Debug.Log("r");
        /* RoomInstance currentRoom = GetComponentInParent<RoomInstance>();
        if (currentRoom != null && currentRoom.enemiesInRoom.Count == 0)
        {
            Debug.Log("rr");
            SetAllDoorsColor(true); // Assuming true indicates the door can be interacted with
        } */
        PlayerLocation playerLocation = GetComponent<PlayerLocation>();
        if (playerLocation != null && playerLocation.currentRoomInstance != null)
            {
                // Log PlayerLocation and currentRoomInstance for debugging
                Debug.Log($"PlayerLocation is not null. Current room instance: {playerLocation.currentRoomInstance.name}, Grid Position: {playerLocation.currentRoomInstance.gridPos}");

                // Use the room instance tracked by PlayerLocation after cooldown
                SetDoorColorBasedOnEnemies(playerLocation.currentRoomInstance);
            }
            else
            {
                // Log when PlayerLocation or currentRoomInstance is null
                Debug.Log("PlayerLocation is null or currentRoomInstance is not set.");
            }
    }
    void SetAllDoorsColor(bool interactionAvailable)
    {
        RoomInstance currentRoom = GetComponentInParent<RoomInstance>(); // Again, adjust based on your architecture
        if (currentRoom != null)
        {
            SetDoorColorBasedOnEnemies(currentRoom);
        }
        else
        {
            // Fallback behavior if the room isn't directly accessible
            Door[] allDoors = FindObjectsOfType<Door>();
            foreach (Door door in allDoors)
            {
                if(interactionAvailable)
                {
                    // Optionally, only reset colors if there are no enemies, or handle differently
                    door.ResetColor();
                }
                else
                {
                    door.SetCooldownColor();
                }
            }
        }
    }
    void OnTriggerEnter2D(Collider2D other){
        if(isDoorInteractionAvailable && other.GetComponent<Door>() != null)
        { 
            // Assuming your doors have a "Door" tag
            // Get the RoomInstance object this door belongs to
            RoomInstance room = other.GetComponentInParent<RoomInstance>();

            if(room != null){
                SetDoorColorBasedOnEnemies(room);
                // Calculate the difference between the door's position and the room's position
                Vector3 positionDifference = other.transform.position - room.transform.position;

                // Use the positionDifference to determine the door's identity
                string doorIdentity = GetDoorIdentity(positionDifference, room);

                // Invoke event or handle door interaction based on doorIdentity
                // Debug.Log("Door Identity: " + doorIdentity);
                // HandleDoorInteraction(doorIdentity, room);
                // StartCoroutine(DoorInteractionCooldown());
                if (room.enemiesInRoom.Count == 0) // Check if there are no enemies left in the room
                {
                    HandleDoorInteraction(doorIdentity, room);
                    StartCoroutine(DoorInteractionCooldown());
                }
                else
                {
                    //Debug.Log("Cannot use the door, enemies present.");
                }
            }
        }
    }
    public void SetDoorColorBasedOnEnemies(RoomInstance room)
    {
        Door[] doorsInRoom = room.GetComponentsInChildren<Door>(); // Assuming doors are children of the room
        foreach (Door door in doorsInRoom)
        {
            if (room.enemiesInRoom.Count > 0)
            {

                door.SetCooldownColor();
            }
            else
            {
                door.ResetColor();
            }
        }
    }

    string GetDoorIdentity(Vector3 positionDifference, RoomInstance room){
        float threshold = 1f; // Adjust based on your door's expected position variance

        if(Mathf.Abs(positionDifference.x) < threshold && positionDifference.y > 0)
            return "Top";
        if(Mathf.Abs(positionDifference.x) < threshold && positionDifference.y < 0)
            return "Bottom";
        if(positionDifference.x > 0 && Mathf.Abs(positionDifference.y) < threshold)
            return "Right";
        if(positionDifference.x < 0 && Mathf.Abs(positionDifference.y) < threshold)
            return "Left";

        return "Unknown"; // Fallback in case the position doesn't match expected values
    }

    void HandleDoorInteraction(string doorIdentity, RoomInstance currentRoom){
        
        if (currentRoom.enemiesInRoom.Count > 0) {
            Debug.Log("Cannot use the door, enemies present.");
            return; // Exit the method if there are still enemies
        }
        Vector3 moveJump = Vector2.zero;
	    float horMove, vertMove;
        SheetAssigner SA = FindObjectOfType<SheetAssigner>();
		Vector2 tempJump = (SA.roomDimensions / 2) + (SA.gutterSize / 2);
        // Vector2 tempJump = SA.gutterSize;
		moveJump = new Vector3(tempJump.x, tempJump.y, 0); //distance b/w rooms: to be used for movement
        Vector3 tempPos = transform.position;
        TutorialMovement cam = FindObjectOfType<TutorialMovement>();

        // Vector3 spawnPosition = Vector3.zero;

        LevelGeneration levelGen = FindObjectOfType<LevelGeneration>();
        Room nextRoom = null;
        Vector2 gridOffset = Vector2.zero;
        Vector2 nextRoomGridPos = currentRoom.gridPos;
        
        PlayerLocation playerLocation = GetComponent<PlayerLocation>();
        if (playerLocation != null)
        {
            Vector2 gridChange = Vector2.zero;
            switch (doorIdentity)
            {
                case "Top": gridChange = new Vector2(0, 1); break;
                case "Bottom": gridChange = new Vector2(0, -1); break;
                case "Left": gridChange = new Vector2(-1, 0); break;
                case "Right": gridChange = new Vector2(1, 0); break;
            }
            playerLocation.UpdateGridPos(gridChange);
        }
        switch(doorIdentity){
            case "Top":
                gridOffset = new Vector2(0, 8);
                horMove = System.Math.Sign(0);//capture input
			    vertMove = System.Math.Sign(1);
			    // Vector3 tempPos = transform.position;
			    tempPos += Vector3.right * horMove * moveJump.x; //jump bnetween rooms based opn input
			    tempPos += Vector3.up * vertMove * moveJump.y;
			    transform.position = tempPos;
                nextRoomGridPos.y++;
                cam.MoveCamera("Top");
                break;
            case "Bottom":
                gridOffset = new Vector2(0, -8);
                horMove = System.Math.Sign(0);//capture input
			    vertMove = System.Math.Sign(-1);
			    // Vector3 tempPos = transform.position;
			    tempPos += Vector3.right * horMove * moveJump.x; //jump bnetween rooms based opn input
			    tempPos += Vector3.up * vertMove * moveJump.y;
			    transform.position = tempPos;
                nextRoomGridPos.y--;
                cam.MoveCamera("Bottom");
                break;
            case "Left":
                gridOffset = new Vector2(-16, 0);
                horMove = System.Math.Sign(-1);//capture input
			    vertMove = System.Math.Sign(0);
			    // Vector3 tempPos = transform.position;
			    tempPos += Vector3.right * horMove * moveJump.x; //jump bnetween rooms based opn input
			    tempPos += Vector3.up * vertMove * moveJump.y;
			    transform.position = tempPos;
                nextRoomGridPos.x--;
                cam.MoveCamera("Left");
                break;
            case "Right":
                gridOffset = new Vector2(16, 0);
                horMove = System.Math.Sign(1);//capture input
			    vertMove = System.Math.Sign(0);
			    // Vector3 tempPos = transform.position;
			    tempPos += Vector3.right * horMove * moveJump.x; //jump bnetween rooms based opn input
			    tempPos += Vector3.up * vertMove * moveJump.y;
			    transform.position = tempPos;
                nextRoomGridPos.x++;
                cam.MoveCamera("Right");
                break;
        }
        // Assuming LevelGeneration has a method to get a room by grid position
        // nextRoom = levelGen.GetRoomAtGridPosition(nextRoomGridPos);
        // nextRoom = levelGen.GetRoomInstanceAtGridPosition(currentRoom.gridPos + nextRoomGridPos);
        Debug.Log("Current room grid position: " + currentRoom.gridPos);
        Debug.Log("Next room grid grid position: " + nextRoomGridPos);

        levelGen.SetCurrentRoom(currentRoom.gridPos, nextRoomGridPos);
        // nextRoom = levelGen.GetRoomInstanceAtGridPosition(nextRoomGridPos);
        
        //PlayerLocationManager.Instance.SetCurrentRoom(nextRoomGridPos);
    }
}