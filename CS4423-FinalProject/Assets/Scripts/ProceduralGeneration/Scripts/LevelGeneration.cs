using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGeneration : MonoBehaviour {
	Vector2 worldSize = new Vector2(4,4);
	public Room[,] rooms;
	List<Vector2> takenPositions = new List<Vector2>();
	int gridSizeX, gridSizeY, numberOfRooms = 13;
	public GameObject roomWhiteObj;
	// public ItemDatabase itemDatabase;
	public Transform mapRoot;

	private Dictionary<Vector2, GameObject> roomGameObjects = new Dictionary<Vector2, GameObject>();
	private GameObject currentRoomSprite;

	void Start () {
		if (numberOfRooms >= (worldSize.x * 2) * (worldSize.y * 2)){ // make sure we dont try to make more rooms than can fit in our grid
			numberOfRooms = Mathf.RoundToInt((worldSize.x * 2) * (worldSize.y * 2));
		}
		gridSizeX = Mathf.RoundToInt(worldSize.x); //note: these are half-extents
		gridSizeY = Mathf.RoundToInt(worldSize.y);
		// itemDatabase.ResetAvailableItems();
		CreateRooms(); //lays out the actual map
		SetRoomDoors(); //assigns the doors where rooms would connect
		DrawMap(); //instantiates objects to make up a map
		GetComponent<SheetAssigner>().Assign(rooms); //passes room info to another script which handles generatating the level geometry
		UpdateSpecialRoomTypes();
		// UpdateSpecialRoomColors();
		// AssignRoomNeighbors();
		//DrawMap(); //instantiates objects to make up a map

		// SetStartingRoomForPlayer();
	}
	void CreateRooms(){
		//setup
		rooms = new Room[gridSizeX * 2,gridSizeY * 2];
		rooms[gridSizeX,gridSizeY] = new Room(Vector2.zero, 1);
		takenPositions.Insert(0,Vector2.zero);
		Vector2 checkPos = Vector2.zero;
		//magic numbers
		float randomCompare = 0.2f, randomCompareStart = 0.2f, randomCompareEnd = 0.01f;
		//add rooms
		for (int i =0; i < numberOfRooms -1; i++){
			float randomPerc = ((float) i) / (((float)numberOfRooms - 1));
			randomCompare = Mathf.Lerp(randomCompareStart, randomCompareEnd, randomPerc);
			//grab new position
			checkPos = NewPosition();
			//test new position
			if (NumberOfNeighbors(checkPos, takenPositions) > 1 && Random.value > randomCompare){
				int iterations = 0;
				do{
					checkPos = SelectiveNewPosition();
					iterations++;
				}while(NumberOfNeighbors(checkPos, takenPositions) > 1 && iterations < 100);
				if (iterations >= 50)
					print("error: could not create with fewer neighbors than : " + NumberOfNeighbors(checkPos, takenPositions));
			}
			//finalize position
			rooms[(int) checkPos.x + gridSizeX, (int) checkPos.y + gridSizeY] = new Room(checkPos, 0);
			takenPositions.Insert(0,checkPos);
		}	
	}
	/* void AssignRoomNeighbors() {
		for (int x = 0; x < gridSizeX * 2; x++) {
			for (int y = 0; y < gridSizeY * 2; y++) {
				Room currentRoom = rooms[x, y];
				if (currentRoom != null) {
					// Assign neighbors, checking bounds to avoid index out of range
					currentRoom.topNeighbor = y + 1 < gridSizeY * 2 ? rooms[x, y + 1] : null;
					currentRoom.bottomNeighbor = y - 1 >= 0 ? rooms[x, y - 1] : null;
					currentRoom.leftNeighbor = x - 1 >= 0 ? rooms[x - 1, y] : null;
					currentRoom.rightNeighbor = x + 1 < gridSizeX * 2 ? rooms[x + 1, y] : null;
				}
			}
		}
	} */
	Vector2 NewPosition(){
		int x = 0, y = 0;
		Vector2 checkingPos = Vector2.zero;
		do{
			int index = Mathf.RoundToInt(Random.value * (takenPositions.Count - 1)); // pick a random room
			x = (int) takenPositions[index].x;//capture its x, y position
			y = (int) takenPositions[index].y;
			bool UpDown = (Random.value < 0.5f);//randomly pick wether to look on hor or vert axis
			bool positive = (Random.value < 0.5f);//pick whether to be positive or negative on that axis
			if (UpDown){ //find the position bnased on the above bools
				if (positive){
					y += 1;
				}else{
					y -= 1;
				}
			}else{
				if (positive){
					x += 1;
				}else{
					x -= 1;
				}
			}
			checkingPos = new Vector2(x,y);
		}while (takenPositions.Contains(checkingPos) || x >= gridSizeX || x < -gridSizeX || y >= gridSizeY || y < -gridSizeY); //make sure the position is valid
		return checkingPos;
	}
	Vector2 SelectiveNewPosition(){ // method differs from the above in the two commented ways
		int index = 0, inc = 0;
		int x =0, y =0;
		Vector2 checkingPos = Vector2.zero;
		do{
			inc = 0;
			do{ 
				//instead of getting a room to find an adject empty space, we start with one that only 
				//as one neighbor. This will make it more likely that it returns a room that branches out
				index = Mathf.RoundToInt(Random.value * (takenPositions.Count - 1));
				inc ++;
			}while (NumberOfNeighbors(takenPositions[index], takenPositions) > 1 && inc < 100);
			x = (int) takenPositions[index].x;
			y = (int) takenPositions[index].y;
			bool UpDown = (Random.value < 0.5f);
			bool positive = (Random.value < 0.5f);
			if (UpDown){
				if (positive){
					y += 1;
				}else{
					y -= 1;
				}
			}else{
				if (positive){
					x += 1;
				}else{
					x -= 1;
				}
			}
			checkingPos = new Vector2(x,y);
		}while (takenPositions.Contains(checkingPos) || x >= gridSizeX || x < -gridSizeX || y >= gridSizeY || y < -gridSizeY);
		if (inc >= 100){ // break loop if it takes too long: this loop isnt garuanteed to find solution, which is fine for this
			print("Error: could not find position with only one neighbor");
		}
		return checkingPos;
	}
	int NumberOfNeighbors(Vector2 checkingPos, List<Vector2> usedPositions){
		int ret = 0; // start at zero, add 1 for each side there is already a room
		if (usedPositions.Contains(checkingPos + Vector2.right)){ //using Vector.[direction] as short hands, for simplicity
			ret++;
		}
		if (usedPositions.Contains(checkingPos + Vector2.left)){
			ret++;
		}
		if (usedPositions.Contains(checkingPos + Vector2.up)){
			ret++;
		}
		if (usedPositions.Contains(checkingPos + Vector2.down)){
			ret++;
		}
		return ret;
	}
	void DrawMap(){
		foreach (Room room in rooms){
			if (room == null){
				continue; //skip where there is no room
			}
			Vector2 drawPos = room.gridPos;
			drawPos.x *= 16;//aspect ratio of map sprite
			drawPos.y *= 8;
			//create map obj and assign its variables
			//MapSpriteSelector mapper = Object.Instantiate(roomWhiteObj, drawPos, Quaternion.identity).GetComponent<MapSpriteSelector>();
			GameObject roomObj = Instantiate(roomWhiteObj, drawPos, Quaternion.identity, mapRoot);
       		MapSpriteSelector mapper = roomObj.GetComponent<MapSpriteSelector>();
			mapper.type = room.type;
			mapper.up = room.doorTop;
			mapper.down = room.doorBot;
			mapper.right = room.doorRight;
			mapper.left = room.doorLeft;
			mapper.gameObject.transform.parent = mapRoot;

			roomGameObjects[room.gridPos] = roomObj;
		}
	}
	void SetRoomDoors(){
		for (int x = 0; x < ((gridSizeX * 2)); x++){
			for (int y = 0; y < ((gridSizeY * 2)); y++){
				if (rooms[x,y] == null){
					continue;
				}
				Vector2 gridPosition = new Vector2(x,y);
				if (y - 1 < 0){ //check above
					rooms[x,y].doorBot = false;
				}else{
					rooms[x,y].doorBot = (rooms[x,y-1] != null);
				}
				if (y + 1 >= gridSizeY * 2){ //check bellow
					rooms[x,y].doorTop = false;
				}else{
					rooms[x,y].doorTop = (rooms[x,y+1] != null);
				}
				if (x - 1 < 0){ //check left
					rooms[x,y].doorLeft = false;
				}else{
					rooms[x,y].doorLeft = (rooms[x - 1,y] != null);
				}
				if (x + 1 >= gridSizeX * 2){ //check right
					rooms[x,y].doorRight = false;
				}else{
					rooms[x,y].doorRight = (rooms[x+1,y] != null);
				}
			}
		}
	}
	/* public void SetCurrentRoom(Vector2 oldRoom, Vector2 newRoom){
		Debug.Log($"Setting current room from {oldRoom} to {newRoom}");

		if (roomGameObjects.TryGetValue(newRoom, out GameObject oldRoomObj)) {
			Debug.Log("Destroying old room sprite.");
			Destroy(oldRoomObj);
			roomGameObjects.Remove(newRoom); // Remove the old room from the dictionary
		}

		Room targetRoom = null;
		// Attempt to find the room that matches newRoom
		foreach (Room room in rooms){
			if (room != null && room.gridPos == newRoom){
				targetRoom = room;
				break; // Exit the loop once the room is found
			}
		}

		if (targetRoom == null) {
			Debug.LogError("Target room not found in SetCurrentRoom.");
			return; // Exit the method if no matching room is found
		}

		Vector2 drawPos = targetRoom.gridPos;
		drawPos.x *= 16; // Aspect ratio of map sprite
		drawPos.y *= 8;
		// Instantiate a new room sprite and update the currentRoomSprite reference
		GameObject newRoomObj = Instantiate(roomWhiteObj, drawPos, Quaternion.identity, mapRoot);
		MapSpriteSelector mapper = newRoomObj.GetComponent<MapSpriteSelector>();
		if (mapper != null) {
			// Destroy(mapper.rend);
			mapper.type = 2; // Assuming type 2 is for the current room
			mapper.UpdateColor();
			mapper.up = targetRoom.doorTop;
			mapper.down = targetRoom.doorBot;
			mapper.right = targetRoom.doorRight;
			mapper.left = targetRoom.doorLeft;
			mapper.rend.sortingOrder = 0;
		} 
		else {
			Debug.LogError("MapSpriteSelector component not found on instantiated room object.");
		}
		roomGameObjects[newRoom] = newRoomObj;
		currentRoomSprite = newRoomObj;
	} */
	public void SetCurrentRoom(Vector2 oldRoom, Vector2 newRoom) {
		Debug.Log($"Setting current room from {oldRoom} to {newRoom}");
		
		Room leaveRoom = null;
		foreach (Room room in rooms) {
			if (room != null && room.gridPos == oldRoom) {
				leaveRoom = room;
				break; // Room found
			}
		}
		// First, handle the old room sprite if it exists
		if (roomGameObjects.TryGetValue(oldRoom, out GameObject oldRoomObj)) {
			MapSpriteSelector oldRoomMapper = oldRoomObj.GetComponent<MapSpriteSelector>();
			if (oldRoomMapper != null) {
				if (leaveRoom != null && leaveRoom.type == 5){
					oldRoomMapper.type = 6; 	
				}
				else if (leaveRoom != null && leaveRoom.type == 2){
					oldRoomMapper.type = 3; 	
				}
				else if (leaveRoom != null && leaveRoom.type == 3){
					oldRoomMapper.type = 4; 	
				}
				else if (leaveRoom != null && leaveRoom.type == 1){
					oldRoomMapper.type = 1; 
				}
				else{
					oldRoomMapper.type = 0; // Change the sprite type of the old room back to 0
				}
				oldRoomMapper.UpdateColor(); // Make sure to update the color or sprite as needed
				// Optionally adjust sortingOrder or other properties if necessary
				// oldRoomMapper.rend.sortingOrder = 0; // This ensures it's not rendered above other objects unexpectedly
			}
		}

		// Next, handle the new room sprite creation and destruction of the previous sprite if needed
		if (roomGameObjects.TryGetValue(newRoom, out GameObject newRoomObj)) {
			Debug.Log("Found existing sprite for new room. Destroying.");
			Destroy(newRoomObj);
			roomGameObjects.Remove(newRoom);
		}

		Room targetRoom = null;
		foreach (Room room in rooms) {
			if (room != null && room.gridPos == newRoom) {
				targetRoom = room;
				break; // Room found
			}
		}

		if (targetRoom == null) {
			Debug.LogError("Target room not found in SetCurrentRoom.");
			return; // Exit if no matching room is found
		}

		// Instantiate and set up the new room sprite
		Vector2 drawPos = targetRoom.gridPos;
		drawPos.x *= 16; // Aspect ratio of map sprite
		drawPos.y *= 8;
		newRoomObj = Instantiate(roomWhiteObj, drawPos, Quaternion.identity, mapRoot);
		MapSpriteSelector mapper = newRoomObj.GetComponent<MapSpriteSelector>();
		if (mapper != null) {
			mapper.type = 2; // Assuming type 2 is for the current room
			mapper.UpdateColor();
			mapper.up = targetRoom.doorTop;
			mapper.down = targetRoom.doorBot;
			mapper.right = targetRoom.doorRight;
			mapper.left = targetRoom.doorLeft;
			mapper.rend.sortingOrder = 0; // Set the sorting order to ensure it's rendered correctly
		} else {
			Debug.LogError("MapSpriteSelector component not found on instantiated room object.");
		}

		// Update the dictionary with the new room sprite
		roomGameObjects[newRoom] = newRoomObj;
		currentRoomSprite = newRoomObj;
	}
	void UpdateSpecialRoomTypes() {
		Vector2 roomPos = Vector2.zero;
		foreach (Room room in rooms) {
			if (room != null && room.type == 5) {
				// Destroy the existing room sprite
				roomPos = room.gridPos;
				if (roomGameObjects.TryGetValue(roomPos, out GameObject oldRoomObj)) {
					Debug.Log("Found existing sprite for new room. Destroying.");
					Destroy(oldRoomObj);
					roomGameObjects.Remove(roomPos);
				}

				// Create a new sprite for the updated room
				Vector2 drawPos = room.gridPos;
				drawPos.x *= 16; // Aspect ratio of map sprite
				drawPos.y *= 8;
				GameObject newRoomObj = Instantiate(roomWhiteObj, drawPos, Quaternion.identity, mapRoot);
				MapSpriteSelector mapper = newRoomObj.GetComponent<MapSpriteSelector>();
				if (mapper != null) {
					mapper.type = 6;
					mapper.UpdateColor();
					mapper.up = room.doorTop;
					mapper.down = room.doorBot;
					mapper.right = room.doorRight;
					mapper.left = room.doorLeft;
					mapper.gameObject.transform.parent = mapRoot;
				} else {
					Debug.LogError("MapSpriteSelector component not found on instantiated room object.");
				}

				// Update the dictionary with the new room sprite
				roomGameObjects[roomPos] = newRoomObj;
				break;
			}
		}

		roomPos = Vector2.zero;
		foreach (Room room in rooms) {
			if (room != null && room.type == 2) {
				// Destroy the existing room sprite
				roomPos = room.gridPos;
				if (roomGameObjects.TryGetValue(roomPos, out GameObject oldRoomObj)) {
					Debug.Log("Found existing sprite for new room. Destroying.");
					Destroy(oldRoomObj);
					roomGameObjects.Remove(roomPos);
				}

				// Create a new sprite for the updated room
				Vector2 drawPos = room.gridPos;
				drawPos.x *= 16; // Aspect ratio of map sprite
				drawPos.y *= 8;
				GameObject newRoomObj = Instantiate(roomWhiteObj, drawPos, Quaternion.identity, mapRoot);
				MapSpriteSelector mapper = newRoomObj.GetComponent<MapSpriteSelector>();
				if (mapper != null) {
					mapper.type = 3;
					mapper.UpdateColor();
					mapper.up = room.doorTop;
					mapper.down = room.doorBot;
					mapper.right = room.doorRight;
					mapper.left = room.doorLeft;
					mapper.gameObject.transform.parent = mapRoot;
				} else {
					Debug.LogError("MapSpriteSelector component not found on instantiated room object.");
				}

				// Update the dictionary with the new room sprite
				roomGameObjects[roomPos] = newRoomObj;
				break;
			}
		}
		
		roomPos = Vector2.zero;
		foreach (Room room in rooms) {
			if (room != null && room.type == 3) {
				// Destroy the existing room sprite
				roomPos = room.gridPos;
				if (roomGameObjects.TryGetValue(roomPos, out GameObject oldRoomObj)) {
					Debug.Log("Found existing sprite for new room. Destroying.");
					Destroy(oldRoomObj);
					roomGameObjects.Remove(roomPos);
				}

				// Create a new sprite for the updated room
				Vector2 drawPos = room.gridPos;
				drawPos.x *= 16; // Aspect ratio of map sprite
				drawPos.y *= 8;
				GameObject newRoomObj = Instantiate(roomWhiteObj, drawPos, Quaternion.identity, mapRoot);
				MapSpriteSelector mapper = newRoomObj.GetComponent<MapSpriteSelector>();
				if (mapper != null) {
					mapper.type = 4;
					mapper.UpdateColor();
					mapper.up = room.doorTop;
					mapper.down = room.doorBot;
					mapper.right = room.doorRight;
					mapper.left = room.doorLeft;
					mapper.gameObject.transform.parent = mapRoot;
				} else {
					Debug.LogError("MapSpriteSelector component not found on instantiated room object.");
				}

				// Update the dictionary with the new room sprite
				roomGameObjects[roomPos] = newRoomObj;
				break;
			}
		}
	}
	/* public void UpdateSpecialRoomColors() {
			foreach (var kvp in roomGameObjects) {
				var roomPos = kvp.Key;
				var roomObj = kvp.Value;

				Room room = rooms[(int)roomPos.x + gridSizeX, (int)roomPos.y + gridSizeY];
				if (room != null) {
					MapSpriteSelector mapSpriteSelector = roomObj.GetComponent<MapSpriteSelector>();
					if (mapSpriteSelector != null) {
						// Here we directly set the room type which the MapSpriteSelector
						// uses to determine the color
						mapSpriteSelector.type = room.type++;
						mapSpriteSelector.UpdateColor(); // Ensure MapSpriteSelector's UpdateColor method uses the 'type' to set the color
					}
				}
			}
	} */

}
