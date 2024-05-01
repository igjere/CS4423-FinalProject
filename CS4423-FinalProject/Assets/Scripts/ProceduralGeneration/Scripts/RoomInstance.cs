using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomInstance : MonoBehaviour {
	public Texture2D tex;
	[HideInInspector]
	public Vector2 gridPos;
	public int type; // 0: normal, 1: enter
	[HideInInspector]
	public bool doorTop, doorBot, doorLeft, doorRight;
	[SerializeField]
	GameObject doorU, doorD, doorL, doorR, doorWall, wallTile;
	[SerializeField]
	ColorToGameObject[] mappings;
	float tileSize = 16;
	Vector2 roomSizeInTiles = new Vector2(9,17);
	[SerializeField]
    GameObject[] enemyPrefabs;
	[SerializeField]
	GameObject bossPrefab;
	[SerializeField]
	GameObject coinPrefab;
	[SerializeField]
	GameObject spikePrefab;
	[SerializeField]
	GameObject heartPrefab;
	[SerializeField]
	GameObject trashcanPrefab;
	[SerializeField]
	GameObject shopcanPrefab;
	[SerializeField]
	GameObject[] itemPrefabs;

	// List<Vector3> validSpawnPoints = new List<Vector3>();
	List<Vector3> validEnemySpawnPoints = new List<Vector3>();
    List<Vector3> validCoinSpawnPoints = new List<Vector3>();
	public List<GameObject> enemiesInRoom = new List<GameObject>();

	//public List<RoomInstance> potentialItemAndShopRooms = new List<RoomInstance>();
    //public List<RoomInstance> potentialBossRooms = new List<RoomInstance>();
	//RoomInstance myRoom;
	int numDoors;

	public void Setup(Texture2D _tex, Vector2 _gridPos, int _type, bool _doorTop, bool _doorBot, bool _doorLeft, bool _doorRight){
		tex = _tex;
		gridPos = _gridPos;
		type = _type;
		doorTop = _doorTop;
		doorBot = _doorBot;
		doorLeft = _doorLeft;
		doorRight = _doorRight;
		List<string> doors = new List<string>();

		numDoors = 0;
		if (doorTop){
			doors.Add("top");
			numDoors++;
		} 
		if (doorBot) {
			doors.Add("bottom");
			numDoors++;
		} 
		if (doorLeft) {
			doors.Add("left");
			numDoors++;
		} 
		if (doorRight) {
			doors.Add("right");
			numDoors++;
		} 

		// Join the list into a single string
		string doorDescriptions = string.Join(", ", doors);
		if (doorDescriptions.Length > 0) {
			doorDescriptions = " with a door on the " + doorDescriptions;
		}
		
		
		MakeDoors();
		GenerateRoomTiles();

		//AssignBossRoom();
		//AssignSpecialRooms();

		if (type == 1) {
			SpawnBookshelf();
			return; // Skip spawning enemies and coins for type 1 rooms
		}
		if (type == 2){
			SpawnBookshelf();
			SpawnHearts();
			return; // Skip spawning enemies and coins for type 1 rooms
		}
		if (type == 3){
			SpawnShopShelf();
			SpawnHearts();
			return;
		}
		if (type == 5){
			SpawnBoss();
			SpawnSpikes();
			return;
		}
		SpawnEnemies();
		SpawnCoins(); 
		SpawnSpikes();
	}
	void MakeDoors(){
		//top door, get position then spawn
		Vector3 spawnPos = transform.position + Vector3.up*(roomSizeInTiles.y/4 * tileSize) - Vector3.up*(tileSize/4);
		PlaceDoor(spawnPos, doorTop, doorU);
		// PlaceWallTilesAroundDoor(spawnPos, "Top");
		//bottom door
		spawnPos = transform.position + Vector3.down*(roomSizeInTiles.y/4 * tileSize) - Vector3.down*(tileSize/4);
		PlaceDoor(spawnPos, doorBot, doorD);
		// PlaceWallTilesAroundDoor(spawnPos, "Bottom");
		//right door
		spawnPos = transform.position + Vector3.right*(roomSizeInTiles.x * tileSize) - Vector3.right*(tileSize);
		PlaceDoor(spawnPos, doorRight, doorR);
		// PlaceWallTilesAroundDoor(spawnPos, "Right");
		//left door
		spawnPos = transform.position + Vector3.left*(roomSizeInTiles.x * tileSize) - Vector3.left*(tileSize);
		PlaceDoor(spawnPos, doorLeft, doorL);
		// PlaceWallTilesAroundDoor(spawnPos, "Left");
	}
	void PlaceDoor(Vector3 spawnPos, bool door, GameObject doorSpawn){
		// check whether its a door or wall, then spawn
		if (door){
			// Instantiate(doorSpawn, spawnPos, Quaternion.identity).transform.parent = transform;
			GameObject instantiatedDoor = Instantiate(doorSpawn, spawnPos, Quaternion.identity);
        	instantiatedDoor.transform.parent = transform;
			// Debug.Log($"Door placed at position {spawnPos}. Door type: {(door ? doorSpawn.name.Replace("(Clone)", "").Trim() : "Wall")}"); 
			Vector3 relativePosition = spawnPos - transform.position;
			string doorOrientation = GetDoorOrientation(relativePosition);
			// Debug.Log($"Door placed at position {spawnPos}. Door orientation: {doorOrientation}");
			PlaceWallTilesForDoor(spawnPos, doorOrientation);
		}else{
			Instantiate(wallTile, spawnPos, Quaternion.identity).transform.parent = transform;
		}
		// Deduce door orientation based on spawnPos relative to room position
		// Vector3 relativePosition = spawnPos - transform.position;
		// string doorOrientation = GetDoorOrientation(relativePosition);

		// Log door orientation
		// Debug.Log($"Door placed at position {spawnPos}. Door orientation: {doorOrientation}");
	}

	// Utility method to get door orientation
	string GetDoorOrientation(Vector3 relativePosition) {
		if (relativePosition.y > 0) return "Top";
		if (relativePosition.y < 0) return "Bottom";
		if (relativePosition.x > 0) return "Right";
		if (relativePosition.x < 0) return "Left";
		return "Unknown";
	}
	void PlaceWallTilesForDoor(Vector3 doorPosition, string doorOrientation){
		Vector3 wallTilePosition;
		GameObject wall;
		SpriteRenderer renderer;
		if (doorOrientation == "Top") {
			// Place wall tiles to the left and right of the top door
			wallTilePosition = doorPosition + Vector3.left * tileSize;
			if (IsTilePositionEmpty(wallTilePosition)) {
				wall = Instantiate(doorWall, wallTilePosition, Quaternion.identity, transform);
				renderer = wall.GetComponent<SpriteRenderer>();
				if (renderer != null) {
					renderer.enabled = false; // This disables the rendering, making it invisible
				}
			}
			wallTilePosition = doorPosition + Vector3.right * tileSize;
			if (IsTilePositionEmpty(wallTilePosition)) {
				wall = Instantiate(doorWall, wallTilePosition, Quaternion.identity, transform);
				renderer = wall.GetComponent<SpriteRenderer>();
				if (renderer != null) {
					renderer.enabled = false; // This disables the rendering, making it invisible
				}
			}

			// Optional: Place an additional wall tile above the door for aesthetic completeness
			wallTilePosition = doorPosition + Vector3.up * tileSize; // Adjust as necessary
			if (IsTilePositionEmpty(wallTilePosition)) {
				wall = Instantiate(doorWall, wallTilePosition, Quaternion.identity, transform);
				renderer = wall.GetComponent<SpriteRenderer>();
				if (renderer != null) {
					renderer.enabled = false; // This disables the rendering, making it invisible
				}
			}
		}

		// Handle left door wall tiles placement
		if (doorOrientation == "Left") {
			// Place wall tiles above and below the left door
			wallTilePosition = doorPosition + Vector3.up * tileSize;
			if (IsTilePositionEmpty(wallTilePosition)) {
				wall = Instantiate(doorWall, wallTilePosition, Quaternion.identity, transform);
				renderer = wall.GetComponent<SpriteRenderer>();
				if (renderer != null) {
					renderer.enabled = false; // This disables the rendering, making it invisible
				}
			}
			wallTilePosition = doorPosition + Vector3.down * tileSize;
			if (IsTilePositionEmpty(wallTilePosition)) {
				wall = Instantiate(doorWall, wallTilePosition, Quaternion.identity, transform);
				renderer = wall.GetComponent<SpriteRenderer>();
				if (renderer != null) {
					renderer.enabled = false; // This disables the rendering, making it invisible
				}
			}

			// Optional: Place an additional wall tile to the left of the door for aesthetic completeness
			wallTilePosition = doorPosition + Vector3.left * tileSize; // Adjust as necessary
			if (IsTilePositionEmpty(wallTilePosition)) {
				wall = Instantiate(doorWall, wallTilePosition, Quaternion.identity, transform);
				renderer = wall.GetComponent<SpriteRenderer>();
				if (renderer != null) {
					renderer.enabled = false; // This disables the rendering, making it invisible
				}
			}
		}
		
		if (doorOrientation == "Bottom") {
			// Place wall tiles to the left and right of the top door
			wallTilePosition = doorPosition + Vector3.left * tileSize;
			if (IsTilePositionEmpty(wallTilePosition)) {
				wall = Instantiate(doorWall, wallTilePosition, Quaternion.identity, transform);
				renderer = wall.GetComponent<SpriteRenderer>();
				if (renderer != null) {
					renderer.enabled = false; // This disables the rendering, making it invisible
				}
			}
			wallTilePosition = doorPosition + Vector3.right * tileSize;
			if (IsTilePositionEmpty(wallTilePosition)) {
				wall = Instantiate(doorWall, wallTilePosition, Quaternion.identity, transform);
				renderer = wall.GetComponent<SpriteRenderer>();
				if (renderer != null) {
					renderer.enabled = false; // This disables the rendering, making it invisible
				}
			}

			// Optional: Place an additional wall tile above the door for aesthetic completeness
			wallTilePosition = doorPosition + Vector3.down * tileSize; // Adjust as necessary
			if (IsTilePositionEmpty(wallTilePosition)) {
				wall = Instantiate(doorWall, wallTilePosition, Quaternion.identity, transform);
				renderer = wall.GetComponent<SpriteRenderer>();
				if (renderer != null) {
					renderer.enabled = false; // This disables the rendering, making it invisible
				}
			}
		}

		// Handle left door wall tiles placement
		if (doorOrientation == "Right") {
			// Place wall tiles above and below the left door
			wallTilePosition = doorPosition + Vector3.up * tileSize;
			if (IsTilePositionEmpty(wallTilePosition)) {
				wall = Instantiate(doorWall, wallTilePosition, Quaternion.identity, transform);
				renderer = wall.GetComponent<SpriteRenderer>();
				if (renderer != null) {
					renderer.enabled = false; // This disables the rendering, making it invisible
				}
			}
			wallTilePosition = doorPosition + Vector3.down * tileSize;
			if (IsTilePositionEmpty(wallTilePosition)) {
				wall = Instantiate(doorWall, wallTilePosition, Quaternion.identity, transform);
				renderer = wall.GetComponent<SpriteRenderer>();
				if (renderer != null) {
					renderer.enabled = false; // This disables the rendering, making it invisible
				}
			}

			// Optional: Place an additional wall tile to the left of the door for aesthetic completeness
			wallTilePosition = doorPosition + Vector3.right * tileSize; // Adjust as necessary
			if (IsTilePositionEmpty(wallTilePosition)) {
				wall = Instantiate(doorWall, wallTilePosition, Quaternion.identity, transform);
				renderer = wall.GetComponent<SpriteRenderer>();
				if (renderer != null) {
					renderer.enabled = false; // This disables the rendering, making it invisible
				}
			}
		}
	}
	// Helper method to place a wall tile at a specified position if it is empty
	void PlaceWallTileIfEmpty(Vector3 position) {
		if (IsTilePositionEmpty(position)) {
			Instantiate(doorWall, position, Quaternion.identity, transform);
		}
	}

	// Existing method to check if a tile position is empty
	bool IsTilePositionEmpty(Vector3 position) {
		float checkRadius = 0.1f; // Small radius for checking
		Collider2D[] colliders = Physics2D.OverlapCircleAll(position, checkRadius);
		foreach (var collider in colliders) {
			if (collider.CompareTag("Wall")) { // Adjust this tag based on your project
				return false; // Found an object; position is not empty
			}
		}
		return true; // No objects found; position is empty
	}
	void GenerateRoomTiles(){
		//loop through every pixel of the texture
		for(int x = 0; x < tex.width; x++){
			for (int y = 0; y < tex.height; y++){
				GenerateTile(x,y);
			}
		}
	}
	void GenerateTile(int x, int y) {
        Color pixelColor = tex.GetPixel(x, y);
        if (pixelColor.a == 0) {
            Vector3 spawnPos = positionFromTileGrid(x, y);
			// Exclude perimeter of the room
			if (IsInPerimeter(spawnPos) || (spawnPos == transform.position + Vector3.up*(roomSizeInTiles.y/4 * tileSize) - Vector3.up*(tileSize/4)) || (spawnPos == transform.position + Vector3.down*(roomSizeInTiles.y/4 * tileSize) - Vector3.down*(tileSize/4)) || (spawnPos == transform.position + Vector3.right*(roomSizeInTiles.x * tileSize) - Vector3.right*(tileSize)) || (spawnPos == transform.position + Vector3.left*(roomSizeInTiles.x * tileSize) - Vector3.left*(tileSize))) {
				return; // Skip adding this position if it's in the perimeter
			}

            // Determine if within centered range for enemies
            if (spawnPos.x >= transform.position.x - 32 && spawnPos.x <= transform.position.x + 32 && spawnPos.y >= transform.position.y - 16 && spawnPos.y <= transform.position.y + 16) {
                validEnemySpawnPoints.Add(spawnPos);
            } else {
				// TODO: MAKE COINS AND SPIKES NOT SPAWN IN BORDER OF ROOM/ON DOOR TRIGGERS
                // Otherwise, it's a valid coin spawn
                validCoinSpawnPoints.Add(spawnPos);
            }
            return;
		}
		//find the color to math the pixel
		foreach (ColorToGameObject mapping in mappings){
			if (mapping.color.Equals(pixelColor)){
				Vector3 spawnPos = positionFromTileGrid(x,y);
				Instantiate(mapping.prefab, spawnPos, Quaternion.identity).transform.parent = this.transform;
			}else{
				
			}
		}
	}
	bool IsInPerimeter(Vector3 position) {
		// Exclude exact boundary positions
		float minX = transform.position.x - roomSizeInTiles.x * tileSize / 2 + tileSize;
		float maxX = transform.position.x + roomSizeInTiles.x * tileSize / 2 - tileSize;
		float minY = transform.position.y - roomSizeInTiles.y * tileSize / 2 + tileSize;
		float maxY = transform.position.y + roomSizeInTiles.y * tileSize / 2 - tileSize;

		// Adjusted to exclude the edges where x or y equals the boundary values
		if (position.x <= minX || position.x >= maxX || position.y <= minY || position.y >= maxY ||
			Mathf.Abs(position.x) == 128 || Mathf.Abs(position.y) == 64) {
			return true;
		}
		return false;
	}

	Vector3 positionFromTileGrid(int x, int y){
		Vector3 ret;
		//find difference between the corner of the texture and the center of this object
		Vector3 offset = new Vector3((-roomSizeInTiles.x + 1)*tileSize, (roomSizeInTiles.y/4)*tileSize - (tileSize/4), 0);
		//find scaled up position at the offset
		ret = new Vector3(tileSize * (float) x, -tileSize * (float) y, 0) + offset + transform.position;
		return ret;
	}

	void SpawnBookshelf() {
		Vector3 centerPos = transform.position; // Assuming this is the center of the room
		Instantiate(trashcanPrefab, centerPos, Quaternion.identity, transform);
	}

	void SpawnShopShelf() {
		Vector3 centerPos = transform.position; // Assuming this is the center of the room
    	Vector3 offset = new Vector3(tileSize, 0, 0); // Using tileSize as an arbitrary offset for separation

    	// Instantiate the first shop shelf slightly to the left of the center
    	Instantiate(shopcanPrefab, centerPos - offset, Quaternion.identity, transform);

    	// Instantiate the second shop shelf slightly to the right of the center
    	Instantiate(shopcanPrefab, centerPos + offset, Quaternion.identity, transform);
	}

	void SpawnBoss(){
		Vector3 centerPos = transform.position;
		GameObject enemyInstance = Instantiate(bossPrefab, centerPos, Quaternion.identity, transform);
		Creature enemyCreature = enemyInstance.GetComponent<Creature>();
		if (enemyCreature != null) {
			enemyCreature.SetRoomInstance(this);
		}
		enemiesInRoom.Add(enemyInstance);
	}

	void SpawnEnemies() {
        if (type == 1) return;
        int enemiesToSpawn = Random.Range(3, 6); // Spawns 3 to 6 enemies
        for (int i = 0; i < enemiesToSpawn; i++) {
            if (validEnemySpawnPoints.Count > 0) {
                int spawnIndex = Random.Range(0, validEnemySpawnPoints.Count);
                Vector3 spawnPos = validEnemySpawnPoints[spawnIndex];
                GameObject enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
                GameObject enemyInstance = Instantiate(enemyPrefab, spawnPos, Quaternion.identity, transform);
                Creature enemyCreature = enemyInstance.GetComponent<Creature>();
                if (enemyCreature != null) {
                    enemyCreature.SetRoomInstance(this);
                }
                enemiesInRoom.Add(enemyInstance);
                validEnemySpawnPoints.RemoveAt(spawnIndex);
            }
        }
		// Debug.Log($"Room at grid position ({gridPos.x}, {gridPos.y}) has {enemiesInRoom.Count} enemies.");
    }
	void SpawnHearts() {
        if (type == 1) return;
        int coinsToSpawn = Random.Range(1, 2);
        for (int i = 0; i < coinsToSpawn; i++) {
            if (validCoinSpawnPoints.Count > 0) {
                int spawnIndex = Random.Range(0, validCoinSpawnPoints.Count);
                Vector3 spawnPos = validCoinSpawnPoints[spawnIndex];
				if (!IsInPerimeter(spawnPos)) {
                	Instantiate(heartPrefab, spawnPos, Quaternion.identity, transform);
                	validCoinSpawnPoints.RemoveAt(spawnIndex);
				}
            }
        }
    }
	void SpawnCoins() {
        if (type == 1) return;
        int coinsToSpawn = Random.Range(1, 3);
        for (int i = 0; i < coinsToSpawn; i++) {
            if (validCoinSpawnPoints.Count > 0) {
                int spawnIndex = Random.Range(0, validCoinSpawnPoints.Count);
                Vector3 spawnPos = validCoinSpawnPoints[spawnIndex];
				if (!IsInPerimeter(spawnPos)) {
                	Instantiate(coinPrefab, spawnPos, Quaternion.identity, transform);
                	validCoinSpawnPoints.RemoveAt(spawnIndex);
				}
            }
        }
    }
	void SpawnSpikes() {
        if (type == 1) return;
        int spikesToSpawn = Random.Range(1, 3);
        for (int i = 0; i < spikesToSpawn; i++) {
            if (validCoinSpawnPoints.Count > 0) {
                int spawnIndex = Random.Range(0, validCoinSpawnPoints.Count);
                Vector3 spawnPos = validCoinSpawnPoints[spawnIndex];
				if (!IsInPerimeter(spawnPos)) {
                	Instantiate(spikePrefab, spawnPos, Quaternion.identity, transform);
                	validCoinSpawnPoints.RemoveAt(spawnIndex);
				}
            }
        }
    }
	
	public void CheckAndUpdateDoors()
	{
		// This method would loop through each door in the room and update its color based on enemy count
		DoorOpener[] doorOpeners = FindObjectsOfType<DoorOpener>(); // Assuming you have a way to access DoorOpener from RoomInstance
		foreach (DoorOpener doorOpener in doorOpeners)
		{
			doorOpener.SetDoorColorBasedOnEnemies(this);
		}
	}
	public void OnPlayerEntered()
	{
		// UpdateDoorColors();
		CheckAndUpdateDoors();
		MapSpriteSelector mapIcon = GetComponentInChildren<MapSpriteSelector>();
		if (mapIcon != null)
		{
			mapIcon.SetCurrentRoomColor(); // Method to be defined in MapSpriteSelector
		}
		else{
			Debug.Log("dont work");
		}
	}
	public void OnPlayerLeft()
	{
		MapSpriteSelector mapIcon = GetComponentInChildren<MapSpriteSelector>();
		if (mapIcon != null)
		{
			mapIcon.ResetColor(); // Resets the color to the default
		}
		else{
			Debug.Log("dont work");
		}
	} 
}
