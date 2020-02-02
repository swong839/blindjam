using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TunnelGeneration : MonoBehaviour
{
    [SerializeField]
    private GameObject tunnelGO;

    [SerializeField]
    private GameObject horizontalTunnel;

    [SerializeField]
    private GameObject verticalTunnel;

    [SerializeField]
    private GameObject nwCornerTunnel;

    [SerializeField]
    private GameObject neCornerTunnel;

    [SerializeField]
    private GameObject seCornerTunnel;

    [SerializeField]
    private GameObject swCornerTunnel;

    [SerializeField]
    private GameObject tunnelParent;

    float tileWidth;

    private void Awake()
    {
        tileWidth = gameObject.GetComponent<DungeonGeneration>().TileWidth;
    }

    public GameObject createHorizontalTunnel(Room rm1, Room rm2)
    {
        GameObject tunnelParentObject = Instantiate(tunnelParent);
        tunnelParentObject.transform.position = Vector2.zero;
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
        GameObject tunnel = Instantiate(horizontalTunnel);
        tunnel.transform.parent = tunnelParentObject.transform;
        tunnel.transform.position = lastPos + new Vector2(tileWidth, 0);
        lastPos = tunnel.transform.position;

        // Get number of extra pieces and instantiate them
        int extraTiles = Random.Range(1, tunnelTiles - 1);
        for (int i = 0; i < extraTiles - 1; i++)
        {
            tunnel = Instantiate(horizontalTunnel);
            tunnel.transform.parent = tunnelParentObject.transform;
            tunnel.transform.position = lastPos + new Vector2(tileWidth, 0);
            lastPos = tunnel.transform.position;
        }

        int vertTunnelLen = Mathf.RoundToInt(Mathf.Abs(rm1EastDoor.y - rm2WestDoor.y) / tileWidth);

        // Start making vertical pieces
        if (vertTunnelLen != 0)
        {
            int modifier = 1;
            if (rm1EastDoor.y > rm2WestDoor.y)
            {
                modifier = -1;

                tunnel = Instantiate(neCornerTunnel);
                tunnel.transform.parent = tunnelParentObject.transform;
                tunnel.transform.position = lastPos + new Vector2(tileWidth, 0);
                lastPos = tunnel.transform.position;
            }
            else
            {
                tunnel = Instantiate(seCornerTunnel);
                tunnel.transform.parent = tunnelParentObject.transform;
                tunnel.transform.position = lastPos + new Vector2(tileWidth, 0);
                lastPos = tunnel.transform.position;
            }

            for (int i = 0; i < vertTunnelLen - 1; i++)
            {
                tunnel = Instantiate(verticalTunnel);
                tunnel.transform.parent = tunnelParentObject.transform;
                tunnel.transform.position = lastPos + new Vector2(0, modifier * tileWidth);
                lastPos = tunnel.transform.position;
            }

            if (modifier == -1)
            {
                tunnel = Instantiate(swCornerTunnel);
                tunnel.transform.parent = tunnelParentObject.transform;
                tunnel.transform.position = lastPos + new Vector2(0, modifier * tileWidth);
                lastPos = tunnel.transform.position;
            }
            else
            {
                tunnel = Instantiate(nwCornerTunnel);
                tunnel.transform.parent = tunnelParentObject.transform;
                tunnel.transform.position = lastPos + new Vector2(0, modifier * tileWidth);
                lastPos = tunnel.transform.position;
            }
        }
        else
        {
            tunnel = Instantiate(horizontalTunnel);
            tunnel.transform.parent = tunnelParentObject.transform;
            tunnel.transform.position = lastPos + new Vector2(tileWidth, 0);
            lastPos = tunnel.transform.position;
        }

        // Set remaining tile pieces
        for (int i = 0; i < tunnelTiles - 1 - extraTiles; i++)
        {
            tunnel = Instantiate(horizontalTunnel);
            tunnel.transform.parent = tunnelParentObject.transform;
            tunnel.transform.position = lastPos + new Vector2(tileWidth, 0);
            lastPos = tunnel.transform.position;
        }

        return tunnelParentObject;
    }

    public GameObject createVerticalTunnel(Room rm1, Room rm2)
    {
        GameObject tunnelParentObject = Instantiate(tunnelParent);
        int topRoom = rm1.RoomNum;
        int bottomRoom = rm2.RoomNum;
        int topSize = rm1.RoomSize;
        int bottomSize = rm2.RoomSize;
        
        Vector2 rm1SouthDoor = rm1.Doors["south"].transform.position;
        Vector2 rm2NorthDoor = rm2.Doors["north"].transform.position;

        int tunnelTiles = (int)((rm1SouthDoor.y - rm2NorthDoor.y - tileWidth) / tileWidth);

        Vector2 lastPos = rm1SouthDoor;

        // Place first tile outside of room 1
        GameObject tunnel = Instantiate(verticalTunnel);
        tunnel.transform.parent = tunnelParentObject.transform;
        tunnel.transform.position = lastPos + new Vector2(0, -1 * tileWidth);
        lastPos = tunnel.transform.position;

        // Get number of extra pieces and instantiate them
        int extraTiles = Random.Range(1, tunnelTiles - 1);
        for (int i = 0; i < extraTiles - 1; i++)
        {
            tunnel = Instantiate(verticalTunnel);
            tunnel.transform.parent = tunnelParentObject.transform;
            tunnel.transform.position = lastPos + new Vector2(0, -1 * tileWidth);
            lastPos = tunnel.transform.position;
        }

        int horiTunnelLength = Mathf.RoundToInt(Mathf.Abs(rm1SouthDoor.x - rm2NorthDoor.x) / tileWidth);

        // Start making horizontal pieces
        if (horiTunnelLength != 0)
        {
            int modifier = 1;
            if (rm1SouthDoor.x > rm2NorthDoor.x)
            {
                modifier = -1;
                tunnel = Instantiate(seCornerTunnel);
                tunnel.transform.parent = tunnelParentObject.transform;
                tunnel.transform.position = lastPos + new Vector2(0, -1 * tileWidth);
                lastPos = tunnel.transform.position;
            }
            else
            {
                tunnel = Instantiate(swCornerTunnel);
                tunnel.transform.parent = tunnelParentObject.transform;
                tunnel.transform.position = lastPos + new Vector2(0, -1 * tileWidth);
                lastPos = tunnel.transform.position;
            }

            for (int i = 0; i < horiTunnelLength - 1; i++)
            {
                tunnel = Instantiate(horizontalTunnel);
                tunnel.transform.parent = tunnelParentObject.transform;
                tunnel.transform.position = lastPos + new Vector2(modifier * tileWidth, 0);
                lastPos = tunnel.transform.position;
            }

            if (modifier == -1)
            {
                tunnel = Instantiate(nwCornerTunnel);
                tunnel.transform.parent = tunnelParentObject.transform;
                tunnel.transform.position = lastPos + new Vector2(modifier * tileWidth, 0);
                lastPos = tunnel.transform.position;
            }
            else
            {
                tunnel = Instantiate(neCornerTunnel);
                tunnel.transform.parent = tunnelParentObject.transform;
                tunnel.transform.position = lastPos + new Vector2(modifier * tileWidth, 0);
                lastPos = tunnel.transform.position;
            }
        }
        else
        {
            tunnel = Instantiate(verticalTunnel);
            tunnel.transform.parent = tunnelParentObject.transform;
            tunnel.transform.position = lastPos + new Vector2(0, -1 * tileWidth);
            lastPos = tunnel.transform.position;
        }

        

        // Set remaining tile pieces
        for (int i = 0; i < tunnelTiles - 1 - extraTiles; i++)
        {
            tunnel = Instantiate(verticalTunnel);
            tunnel.transform.parent = tunnelParentObject.transform;
            tunnel.transform.position = lastPos + new Vector2(0, -1 * tileWidth);
            lastPos = tunnel.transform.position;
        }

        return tunnelParentObject;

    }
}
