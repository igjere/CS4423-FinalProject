using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheetAssigner : MonoBehaviour {
    [SerializeField] Texture2D[] sheetsNormal;
    [SerializeField] GameObject RoomObj;
    public Vector2 roomDimensions = new Vector2(16 * 17, 16 * 9);
    public Vector2 gutterSize = new Vector2(16 * 9, 16 * 4);
    private List<RoomInstance> roomInstances = new List<RoomInstance>();

    private List<Room> potentialItemAndShopRooms = new List<Room>();
    private List<Room> potentialBossRooms = new List<Room>();

    public void Assign(Room[,] rooms) {
		// First clear previous potential rooms lists
    	potentialItemAndShopRooms.Clear();
    	potentialBossRooms.Clear();
		foreach (Room room in rooms) {
			if (room == null) {
				continue;
			}
			// Calculate number of doors for the room
			int numDoors = 0;
			if (room.doorTop) numDoors++;
			if (room.doorBot) numDoors++;
			if (room.doorLeft) numDoors++;
			if (room.doorRight) numDoors++;

			if (room.type == 0) { // Ensure it's a normal room
				if (numDoors == 1) {
					// Potential boss room
					potentialBossRooms.Add(room);
				} else {
					// Potential item or shop room
					potentialItemAndShopRooms.Add(room);
				}
			}
		}

		// Assign special room types
		AssignSpecialRooms(rooms);

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
			if (room.type == 5){
				myRoom.Setup(sheetsNormal[6], room.gridPos, room.type, room.doorTop, room.doorBot, room.doorLeft, room.doorRight);
			}
			else if (room.type == 1){
				myRoom.Setup(sheetsNormal[5], room.gridPos, room.type, room.doorTop, room.doorBot, room.doorLeft, room.doorRight);
			}
			else if (room.type == 2){
				myRoom.Setup(sheetsNormal[5], room.gridPos, room.type, room.doorTop, room.doorBot, room.doorLeft, room.doorRight);
			}
			else if (room.type == 3){
				myRoom.Setup(sheetsNormal[5], room.gridPos, room.type, room.doorTop, room.doorBot, room.doorLeft, room.doorRight);
			}
			else{
				myRoom.Setup(sheetsNormal[Random.Range(0, 4)], room.gridPos, room.type, room.doorTop, room.doorBot, room.doorLeft, room.doorRight);
			}
			roomInstances.Add(myRoom);

			// Decide whether it's a potential boss room or item/shop room based on the number of doors
		}
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

	/* private void AssignSpecialRooms() {
		if (potentialBossRooms.Count > 0) {
			foreach (Room room in potentialBossRooms) {
				room.type = 5;  // Confirm Boss room
				// Debug.Log($"Assigned Boss Room at {room.gridPos}");
        	}
		}

		if (potentialItemAndShopRooms.Count > 0) {
			 foreach (Room room in potentialItemAndShopRooms) {
				int roomType = (Random.value < 0.5) ? 2 : 3;  // Randomly choose between item (2) and shop (3)
				room.type = roomType;
				// Debug.Log($"Assigned {roomType} at {room.gridPos}");
			}
		}
	} */
	private void AssignSpecialRooms(Room[,] rooms) {
		bool bossAssigned = false;
		bool itemAssigned = false;
		bool shopAssigned = false;

		Vector2 bossRoomPos = Vector2.zero;
		Vector2 itemRoomPos = Vector2.zero;
		Vector2 shopRoomPos = Vector2.zero;
		Vector2 spawnRoomPos = Vector2.zero;

		// Assign Boss Room
		foreach (Room room in potentialBossRooms) {
			if (!bossAssigned && room.type == 0 && NotNeighborOf(room, spawnRoomPos)) {
				room.type = 5;  // Assign as Boss room
				bossAssigned = true;
				bossRoomPos = room.gridPos;
				Debug.Log($"Assigned Boss Room at {room.gridPos}");
				break;
			}
		}

		// Remove the boss room from potentialItemAndShopRooms to avoid double assignment
		potentialItemAndShopRooms.RemoveAll(r => r.type == 5);

		// Assign Item Room
		foreach (Room room in potentialItemAndShopRooms) {
			if (!itemAssigned && room.type == 0 && NotNeighborOf(room, bossRoomPos) && NotNeighborOf(room, spawnRoomPos)) {
				room.type = 2;  // Assign as Item room
				itemAssigned = true;
				itemRoomPos = room.gridPos;
				Debug.Log($"Assigned Item Room at {room.gridPos}");
				break;  // Stop after assigning one item room
			}
		}

		// Assign Shop Room
		foreach (Room room in potentialItemAndShopRooms) {
			if (!shopAssigned && room.type == 0 && NotNeighborOf(room, bossRoomPos) && NotNeighborOf(room, itemRoomPos) && NotNeighborOf(room, spawnRoomPos)) {
				room.type = 3;  // Assign as Shop room
				shopAssigned = true;
				Debug.Log($"Assigned Shop Room at {room.gridPos}");
				break;  // Stop after assigning one shop room
			}
		}
	}

	private bool NotNeighborOf(Room room, Vector2 neighborPos) {
		Vector2[] directions = new Vector2[] { Vector2.up, Vector2.down, Vector2.left, Vector2.right };
		Vector2 checkPos;
		foreach (Vector2 dir in directions) {
			checkPos = room.gridPos + dir;
			if (checkPos == neighborPos) {
				// Debug.Log("r");
				return false;  // Found a non-normal room neighbor
			}
		}
		// Debug.Log("rr");
		return true;  // No non-normal neighbors found
	}

	// Helper method to check if a room is isolated (i.e., surrounded only by normal rooms)
	private bool IsIsolated(Room room, Room[,] rooms) {
		Vector2[] directions = new Vector2[] { Vector2.up, Vector2.down, Vector2.left, Vector2.right };
		foreach (Vector2 dir in directions) {
			Vector2 neighborPos = room.gridPos + dir;
			if (RoomExists(neighborPos, rooms) && rooms[(int)neighborPos.x, (int)neighborPos.y].type != 0) {
				//Debug.Log("r");
				return false;  // Found a non-normal room neighbor
			}
		}
		//Debug.Log("rr");
		return true;  // No non-normal neighbors found
	}

	// Existing method from previous snippets
	private bool RoomExists(Vector2 gridPos, Room[,] rooms) {
		int x = Mathf.FloorToInt(gridPos.x);
		int y = Mathf.FloorToInt(gridPos.y);
		return x >= 0 && x < rooms.GetLength(0) && y >= 0 && y < rooms.GetLength(1) && rooms[x, y] != null;
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
    /* private bool RoomExists(Vector2 gridPos, Room[,] rooms) {
        int x = Mathf.FloorToInt(gridPos.x);
        int y = Mathf.FloorToInt(gridPos.y);
        return x >= 0 && x < rooms.GetLength(0) && y >= 0 && y < rooms.GetLength(1) && rooms[x, y] != null;
    } */
}
