using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TunnelGeneration : MonoBehaviour
{
    [SerializeField]
    private GameObject tunnelGO;

    float tileWidth;

    private void Awake()
    {
        tileWidth = gameObject.GetComponent<DungeonGeneration>().TileWidth;
    }

    public void createHorizontalTunnel(Room rm1, Room rm2)
    {
        tileWidth = gameObject.GetComponent<DungeonGeneration>().TileWidth;
        int leftRoom = rm1.RoomNum;
        int rightRoom = rm2.RoomNum;
        int leftSize = rm1.RoomSize;
        int rightSize = rm2.RoomSize;

        Vector2 rm1EastDoor = rm1.Doors["east"].transform.position;
        Vector2 rm2WestDoor = rm2.Doors["west"].transform.position;

        int tunnelTiles = (int)((rm2WestDoor.x - rm1EastDoor.x - tileWidth) / tileWidth);

        Vector2 lastPos = rm1EastDoor;

        // Place first tile outside of room 1
        GameObject tunnel = Instantiate(tunnelGO);
        tunnel.transform.position = lastPos + new Vector2(tileWidth, 0);
        lastPos = tunnel.transform.position;

        // Get number of extra pieces and instantiate them
        int extraTiles = Random.Range(1, tunnelTiles - 1);
        for (int i = 0; i < extraTiles; i++)
        {
            tunnel = Instantiate(tunnelGO);
            tunnel.transform.position = lastPos + new Vector2(tileWidth, 0);
            lastPos = tunnel.transform.position;
        }

        // Start making vertical pieces
        int modifier = 1;
        if (rm1EastDoor.y > rm2WestDoor.y)
        {
            modifier = -1;
        }

        int vertTunnelLen = (int)(Mathf.Abs(rm1EastDoor.y - rm2WestDoor.y) / tileWidth);
        for (int i = 0; i < vertTunnelLen; i++)
        {
            tunnel = Instantiate(tunnelGO);
            tunnel.transform.position = lastPos + new Vector2(0, modifier * tileWidth);
            lastPos = tunnel.transform.position;
        }

        // Set remaining tile pieces
        for (int i = 0; i < tunnelTiles - 1 - extraTiles; i++)
        {
            tunnel = Instantiate(tunnelGO);
            tunnel.transform.position = lastPos + new Vector2(tileWidth, 0);
            lastPos = tunnel.transform.position;
        }

    }

    public void createVerticalTunnel(Room rm1, Room rm2)
    {
        int topRoom = rm1.RoomNum;
        int bottomRoom = rm2.RoomNum;
        int topSize = rm1.RoomSize;
        int bottomSize = rm2.RoomSize;
        
        Vector2 rm1SouthDoor = rm1.Doors["south"].transform.position;
        Vector2 rm2NorthDoor = rm2.Doors["north"].transform.position;

        int tunnelTiles = (int)((rm1SouthDoor.y - rm2NorthDoor.y - tileWidth) / tileWidth);

        Vector2 lastPos = rm1SouthDoor;

        // Place first tile outside of room 1
        GameObject tunnel = Instantiate(tunnelGO);
        tunnel.transform.position = lastPos + new Vector2(0, -1 * tileWidth);
        lastPos = tunnel.transform.position;

        // Get number of extra pieces and instantiate them
        int extraTiles = Random.Range(1, tunnelTiles - 1);
        for (int i = 0; i < extraTiles; i++)
        {
            tunnel = Instantiate(tunnelGO);
            tunnel.transform.position = lastPos + new Vector2(0, -1 * tileWidth);
            lastPos = tunnel.transform.position;
        }

        // Start making horizontal pieces
        int modifier = 1;
        if (rm1SouthDoor.x > rm2NorthDoor.x)
        {
            modifier = -1;
        }

        int horiTunnelLength = (int)(Mathf.Abs(rm1SouthDoor.x - rm2NorthDoor.x) / tileWidth);
        for (int i = 0; i < horiTunnelLength; i++)
        {
            tunnel = Instantiate(tunnelGO);
            tunnel.transform.position = lastPos + new Vector2(modifier * tileWidth, 0);
            lastPos = tunnel.transform.position;
        }

        // Set remaining tile pieces
        for (int i = 0; i < tunnelTiles - 1 - extraTiles; i++)
        {
            tunnel = Instantiate(tunnelGO);
            tunnel.transform.position = lastPos + new Vector2(0, -1 * tileWidth);
            lastPos = tunnel.transform.position;
        }

    }
}
