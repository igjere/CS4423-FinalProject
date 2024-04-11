using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheetAssigner : MonoBehaviour {
    [SerializeField] Texture2D[] sheetsNormal;
    [SerializeField] GameObject RoomObj;
    public Vector2 roomDimensions = new Vector2(16 * 17, 16 * 9);
    public Vector2 gutterSize = new Vector2(16 * 9, 16 * 4);
    private List<RoomInstance> roomInstances = new List<RoomInstance>();

    private List<RoomInstance> potentialItemAndShopRooms = new List<RoomInstance>();
    private List<RoomInstance> potentialBossRooms = new List<RoomInstance>();

    public void Assign(Room[,] rooms) {
		foreach (Room room in rooms) {
			if (room == null) {
				continue;
			}

			Vector3 pos = new Vector3(room.gridPos.x * (roomDimensions.x + gutterSize.x), room.gridPos.y * (roomDimensions.y + gutterSize.y), 0);
			RoomInstance myRoom = Instantiate(RoomObj, pos, Quaternion.identity).GetComponent<RoomInstance>();

			/* if (room.type == 0) {
				int neighbors = NumberOfNeighbors(room.gridPos, rooms);

				if (neighbors == 1) {
					potentialBossRooms.Add(myRoom); // Rooms with exactly one neighbor are potential boss rooms
				} else {
					potentialItemAndShopRooms.Add(myRoom); // All other normal rooms are potential item or shop rooms
				}
			} */

			myRoom.Setup(sheetsNormal[Random.Range(0, sheetsNormal.Length)], room.gridPos, room.type, room.doorTop, room.doorBot, room.doorLeft, room.doorRight);
			roomInstances.Add(myRoom);
			// Calculate number of doors for the room
			int numDoors = 0;
			if (room.doorTop) numDoors++;
			if (room.doorBot) numDoors++;
			if (room.doorLeft) numDoors++;
			if (room.doorRight) numDoors++;

			// Decide whether it's a potential boss room or item/shop room based on the number of doors
			if (room.type == 0) { // Ensure it's a normal room
				if (numDoors == 1) {
					// Potential boss room
					potentialBossRooms.Add(myRoom);
				} else {
					// Potential item or shop room
					potentialItemAndShopRooms.Add(myRoom);
				}
			}
		}

		// Assign special room types
		AssignSpecialRooms();
	}

	public List<RoomInstance> GetAllRoomInstances() {
		return roomInstances;
	}
	// Method to get a specific room instance by grid position
	public RoomInstance GetRoomInstanceAtGridPosition(Vector2 gridPos) {
		foreach (RoomInstance instance in roomInstances) {
			if (instance.gridPos == gridPos) {
				return instance;
			}
		}
		return null; // Return null if no matching room is found
	}

	private void AssignSpecialRooms() {
		if (potentialBossRooms.Count > 0) {
			int index = Random.Range(0, potentialBossRooms.Count);
			potentialBossRooms[index].type = 5; // Assign as Boss room
			Debug.Log($"Assigned Boss Room at {potentialBossRooms[index].gridPos}");
		}

		if (potentialItemAndShopRooms.Count > 0) {
			AssignRoomTypeRandomly(2); // Item room
			AssignRoomTypeRandomly(3); // Shop
		}
	}

    private void AssignRoomTypeRandomly(int roomType) {
		int index = Random.Range(0, potentialItemAndShopRooms.Count);
		potentialItemAndShopRooms[index].type = roomType;
		Debug.Log($"Assigned {roomType} at {potentialItemAndShopRooms[index].gridPos}");
		potentialItemAndShopRooms.RemoveAt(index); // Remove the room from the list to avoid duplicate assignments
	}

	// Helper method to count the number of neighboring rooms to a given room position
    private int NumberOfNeighbors(Vector2 gridPos, Room[,] rooms) {
        int count = 0;
        if (RoomExists(gridPos + Vector2.up, rooms)) count++;
        if (RoomExists(gridPos + Vector2.down, rooms)) count++;
        if (RoomExists(gridPos + Vector2.left, rooms)) count++;
        if (RoomExists(gridPos + Vector2.right, rooms)) count++;
        return count;
    }

    // Helper method to check if a room exists in the specified position
    private bool RoomExists(Vector2 gridPos, Room[,] rooms) {
        int x = Mathf.FloorToInt(gridPos.x);
        int y = Mathf.FloorToInt(gridPos.y);
        return x >= 0 && x < rooms.GetLength(0) && y >= 0 && y < rooms.GetLength(1) && rooms[x, y] != null;
    }
}
