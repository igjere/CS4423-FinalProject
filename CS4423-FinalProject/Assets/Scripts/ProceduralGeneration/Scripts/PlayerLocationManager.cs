using UnityEngine;

public class PlayerLocationManager : MonoBehaviour {
    public static PlayerLocationManager Instance { get; private set; }
    private Vector2 currentRoomGridPos;
    private LevelGeneration levelGen; // Adjusted to reference LevelGeneration
    private SheetAssigner sa;

    void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            levelGen = FindObjectOfType<LevelGeneration>(); // Properly finding LevelGeneration instance
            sa = FindObjectOfType<SheetAssigner>();
        } else {
            Destroy(gameObject);
        }
    }

    public void SetCurrentRoom(Vector2 gridPos) {
        if (currentRoomGridPos != gridPos) {
            Debug.Log($"Player moved from {currentRoomGridPos} to {gridPos}.");
            UpdateMinimap(gridPos);
            currentRoomGridPos = gridPos;
        }
    }

    private void UpdateMinimap(Vector2 newGridPos) {
        // Implementation to reset and highlight rooms on the minimap
        // Assuming LevelGeneration script provides access to room instances
        var allRooms = sa.GetAllRoomInstances(); // This method needs to exist in LevelGeneration
        foreach (var room in allRooms) {
            var icon = room.GetComponentInChildren<MapSpriteSelector>();
            if (icon != null) {
                icon.ResetColor(); // Resets to default color
            }
        }

        var currentRoom = sa.GetRoomInstanceAtGridPosition(newGridPos);
        if (currentRoom != null) {
            var currentIcon = currentRoom.GetComponentInChildren<MapSpriteSelector>();
            if (currentIcon != null) {
                currentIcon.SetCurrentRoomColor(); // Highlights the current room
            }
        }
    }
}
