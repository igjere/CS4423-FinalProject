using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocation : MonoBehaviour
{
    public Vector2 currentGridPos = Vector2.zero; // Initialize at grid position (0,0)
    public RoomInstance currentRoomInstance;

    void Start()
    {
        UpdateRoomInstance();
    }

    public void UpdateGridPos(Vector2 gridChange)
    {
        currentGridPos += gridChange;
        //Debug.Log($"Player grid position updated to: {currentGridPos}");
        UpdateRoomInstance();
    }

    void UpdateRoomInstance()
    {
        //Debug.Log($"Attempting to find RoomInstance at grid position: {currentGridPos}");
        RoomInstance[] allRooms = FindObjectsOfType<RoomInstance>();
        foreach (var room in allRooms)
        {
            //Debug.Log($"Checking RoomInstance at grid position: {room.gridPos}");
            if (room.gridPos == currentGridPos)
            {
                currentRoomInstance = room;
                //Debug.Log($"Player current room instance updated to room at grid position: {currentGridPos}");
                return;
            }
        }
        //Debug.LogWarning("Matching RoomInstance for updated grid position not found.");
    }
}
