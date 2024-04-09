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
	GameObject doorU, doorD, doorL, doorR, doorWall;
	[SerializeField]
	ColorToGameObject[] mappings;
	float tileSize = 16;
	Vector2 roomSizeInTiles = new Vector2(9,17);
	[SerializeField]
    GameObject[] enemyPrefabs;
	[SerializeField]
	GameObject coinPrefab;
	[SerializeField]
	GameObject trashcanPrefab;
	[SerializeField]
	GameObject[] itemPrefabs;

	List<Vector3> validSpawnPoints = new List<Vector3>();
	public List<GameObject> enemiesInRoom = new List<GameObject>();

	public void Setup(Texture2D _tex, Vector2 _gridPos, int _type, bool _doorTop, bool _doorBot, bool _doorLeft, bool _doorRight){
		tex = _tex;
		gridPos = _gridPos;
		type = _type;
		doorTop = _doorTop;
		doorBot = _doorBot;
		doorLeft = _doorLeft;
		doorRight = _doorRight;
		MakeDoors();
		GenerateRoomTiles();
		if (type == 1) {
			SpawnBookshelf();
			return; // Skip spawning enemies and coins for type 1 rooms
    	}
		SpawnEnemies();
		SpawnCoins();
	}
	void MakeDoors(){
		//top door, get position then spawn
		Vector3 spawnPos = transform.position + Vector3.up*(roomSizeInTiles.y/4 * tileSize) - Vector3.up*(tileSize/4);
		PlaceDoor(spawnPos, doorTop, doorU);
		//bottom door
		spawnPos = transform.position + Vector3.down*(roomSizeInTiles.y/4 * tileSize) - Vector3.down*(tileSize/4);
		PlaceDoor(spawnPos, doorBot, doorD);
		//right door
		spawnPos = transform.position + Vector3.right*(roomSizeInTiles.x * tileSize) - Vector3.right*(tileSize);
		PlaceDoor(spawnPos, doorRight, doorR);
		//left door
		spawnPos = transform.position + Vector3.left*(roomSizeInTiles.x * tileSize) - Vector3.left*(tileSize);
		PlaceDoor(spawnPos, doorLeft, doorL);
	}
	void PlaceDoor(Vector3 spawnPos, bool door, GameObject doorSpawn){
		// check whether its a door or wall, then spawn
		if (door){
			Instantiate(doorSpawn, spawnPos, Quaternion.identity).transform.parent = transform;
		}else{
			Instantiate(doorWall, spawnPos, Quaternion.identity).transform.parent = transform;
		}
	}
	void GenerateRoomTiles(){
		//loop through every pixel of the texture
		for(int x = 0; x < tex.width; x++){
			for (int y = 0; y < tex.height; y++){
				GenerateTile(x,y);
			}
		}
	}
	void GenerateTile(int x, int y){
		Color pixelColor = tex.GetPixel(x,y);
		//skip clear spaces in texture
		if (pixelColor.a == 0){
			if(x != tex.width-1 && x > 1 && y != tex.height-1 && y > 1){
				Vector3 spawnPos = positionFromTileGrid(x, y);
				validSpawnPoints.Add(spawnPos);
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
	void SpawnEnemies() {
		 if (type == 1) return;
		int enemiesToSpawn = Random.Range(3, 6); // Spawns 3 to 6 enemies
		for (int i = 0; i < enemiesToSpawn; i++) {
			if(validSpawnPoints.Count > 0) {
				int spawnIndex = Random.Range(0, validSpawnPoints.Count);
				Vector3 spawnPos = validSpawnPoints[spawnIndex];
				GameObject enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
				GameObject enemyInstance = Instantiate(enemyPrefab, spawnPos, Quaternion.identity, transform);
				Creature enemyCreature = enemyInstance.GetComponent<Creature>();
				if (enemyCreature != null) {
					enemyCreature.SetRoomInstance(this);
				}
				enemiesInRoom.Add(enemyInstance);
				// GameObject enemyInstance = Instantiate(enemyPrefab, spawnPos, Quaternion.identity, transform);
				// CreatureAI aiComponent = enemyInstance.AddComponent<CreatureAI>();
				// Optionally remove the spawn point from the list to avoid spawning multiple enemies in the same spot
				validSpawnPoints.RemoveAt(spawnIndex);
			}
		}
	}
	void SpawnCoins() {
		if (type == 1) return;
		int coinsToSpawn = Random.Range(1, 3); 
		for (int i = 0; i < coinsToSpawn; i++) {
			if(validSpawnPoints.Count > 0) {
				int spawnIndex = Random.Range(0, validSpawnPoints.Count);
				Vector3 spawnPos = validSpawnPoints[spawnIndex];
				Instantiate(coinPrefab, spawnPos, Quaternion.identity, transform);
				validSpawnPoints.RemoveAt(spawnIndex);
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
