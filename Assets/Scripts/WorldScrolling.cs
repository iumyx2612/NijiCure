using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ScriptableObjectArchitecture;
using UnityEngine;

public class WorldScrolling : MonoBehaviour
{
    [SerializeField] private Vector2Variable playerPosRef;
    public Vector2Int playerCoord; // xy coord of where player is
    public Vector2Int currentTileCoord = new Vector2Int(0, 0); // To check if player has left a tile
    public Vector2Int onTileGridPlayerPosition;
    private float tileSize = 20f;
    private Dictionary<GameObject, Vector2Int> tileToCoordMapping = new Dictionary<GameObject, Vector2Int>();

    [SerializeField] private int terrainTileHorizontalCount;
    [SerializeField] private int terrainTileVerticalCount;

    private int fovHeight = 3;
    private int fovWidth = 3;
    
    private readonly int[] positiveMapping = {0, 1, -1};
    private readonly int[] negativeMapping = {0, -1, 1};


    private void Awake()
    {
        AssignTileIndex();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    // This will calculate the playerTilePos
    void Update()
    {
        // Map player world coord to tile xy coord
        playerCoord = WorldPosToTileCoord(playerPosRef.Value, tileSize);
        // Check if player has left the tile
        if (playerCoord != currentTileCoord)
        {
            // If yes, assign the currentTilePos to the new tile where the player is
            currentTileCoord = playerCoord;
            onTileGridPlayerPosition.x = ClipToCoord(onTileGridPlayerPosition.x, true);
            onTileGridPlayerPosition.y = ClipToCoord(onTileGridPlayerPosition.y, false);
            UpdateTilesOnScreen();
        }
    }

    // Assign each tile in the map with the corresponding index
    private void AssignTileIndex()
    {
        List<Vector2Int> coords = new List<Vector2Int>
        {
            new Vector2Int(-1, -1),
            new Vector2Int(0, -1),
            new Vector2Int(1, -1),
            new Vector2Int(-1, 0),
            new Vector2Int(0, 0),
            new Vector2Int(1, 0),
            new Vector2Int(-1, 1),
            new Vector2Int(0, 1),
            new Vector2Int(1, 1)

        };
            
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject tile = transform.GetChild(i).gameObject;
            Vector2Int coord = coords[i];
            tileToCoordMapping[tile] = coord;
        }
    }

    // Converts player world position to tile xy coord 
    private Vector2Int WorldPosToTileCoord(Vector2 playerPos, float scale)
    {
        Vector2Int result = new Vector2Int();
        if (playerPos.x >= 0)
        {
            // Offset 
            result.x = (int) ((playerPos.x + scale / 2) / scale);
        }
        else
        {
            result.x = (int) ((playerPos.x - scale / 2) / scale);
        }

        if (playerPos.y >= 0)
        {
            result.y = (int) ((playerPos.y + scale / 2) / scale);
        }
        else
        {
            result.y = (int) ((playerPos.y - scale / 2) / scale);
        }

        return result;
    }
    
    // Clips the coord outside the [-1, 1] range to
    // coord inside the [-1, 1] range
    // Example: Player on coord (3, 1) -> Clip to coord (0, 1)
    private int ClipToCoord(int currentVal, bool horizontal)
    {
        if (currentVal >= 0)
        {
            if (horizontal)
            {
                int surplus = currentVal % terrainTileHorizontalCount;
                return positiveMapping[surplus];
            }
            else
            {
                int surplus = currentVal % terrainTileVerticalCount;
                return positiveMapping[surplus];
            }
        }
        else
        {
            if (horizontal)
            {
                int surplus = Mathf.Abs(currentVal % terrainTileHorizontalCount);
                return negativeMapping[surplus];
            }
            else
            {
                int surplus = Mathf.Abs(currentVal % terrainTileVerticalCount);
                return negativeMapping[surplus];
            }
        }
    }
    
    private void UpdateTilesOnScreen()
    {
        for (int fov_X = -(fovWidth / 2); fov_X <= (fovWidth / 2); fov_X++)
        {
            for (int fov_Y = -(fovHeight / 2); fov_Y <= (fovHeight / 2); fov_Y++)
            {
                int coord_X = ClipToCoord(playerCoord.x + fov_X, true);
                int coord_Y = ClipToCoord(playerCoord.y + fov_Y, false);
                Vector2Int coord = new Vector2Int(coord_X, coord_Y);
                
                GameObject tile = tileToCoordMapping.FirstOrDefault(x => x.Value == coord).Key;
                tile.transform.position = CoordToWorldPos(
                    playerCoord.x + fov_X,
                    playerCoord.y + fov_Y
                );
            }
        }
    }
    
    private Vector2 CoordToWorldPos(int x, int y)
    {
        return new Vector2(x * tileSize, y * tileSize);
    }
}
