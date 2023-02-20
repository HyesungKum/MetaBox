using System;
using UnityEngine;

public enum NodeDirection
{
    East,       // -> 
    EastSouth,  // \
    South,      // |
    SouthWest,  // </
    West,       // <-
    WestNorth,  // \
    North,      // |
    NorthEast   // />
}

[Serializable]
public class NavNode
{
    #region Neibhor Node
    public NavNode EastNode;
    public NavNode EastSouthNode;
    public NavNode SouthNode;
    public NavNode SouthWestNode;
    public NavNode WestNode;
    public NavNode WestNorthNode;
    public NavNode NorthNode;
    public NavNode NorthEastNode;
    #endregion

    public NavNode parentNode;

    //=========================================Position & Index==========================================
    public Vector2 position;
    public int x;
    public int y;
    public bool obstacle = false;

    //==========================================Huristic Value===========================================
    [field:SerializeField] public int GScore { get; set; } //cost for start - current
    [field: SerializeField] public int HScore { get; set; } //cost for current - end
    public int FScore => GScore + HScore; //Huristic Function value

    //==============================================Constructor===========================================
    public NavNode()
    {
    }
    public NavNode(Vector3 pos)
    {
        this.position = pos;
    }

    //========================================Neighbor Node Controll======================================
    public void SetNeighbor(NavNode targetNode, NodeDirection dir)
    {
        switch (dir)
        {
            case NodeDirection.East:
                {
                    EastNode = new();
                    EastNode = targetNode;
                } 
                break;
            case NodeDirection.EastSouth:
                {
                    EastSouthNode = new();
                    EastSouthNode = targetNode;
                }
                break;
            case NodeDirection.South:
                {
                    SouthNode = new();
                    SouthNode = targetNode;
                }
                 break;
            case NodeDirection.SouthWest:
                {
                    SouthWestNode = new();
                    SouthWestNode = targetNode;
                }
                 break;
            case NodeDirection.West: 
                {
                    WestNode = new();
                    WestNode = targetNode;
                }
                break;
            case NodeDirection.WestNorth:
                {
                    WestNorthNode = new();
                    WestNorthNode = targetNode;
                }
                break;
            case NodeDirection.North: 
                {
                    NorthNode = new();
                    NorthNode = targetNode;
                }
                break;
            case NodeDirection.NorthEast: 
                {
                    NorthEastNode = new();
                    NorthEastNode = targetNode;
                }
                break;
        }
    }

    //=============================================Get Distance===========================================
    public int CalH(NavNode targetNode)
    {
        int x = targetNode.x - this.x;
        int y = targetNode.y - this.y;

        if (x < 0) x *= -1;
        if (y < 0) y *= -1;

        return (x + y) * 10;
    }

    //==============================================Score Clear===========================================
    public void Clear()
    {
        parentNode = null;
        GScore = 0;
        HScore = 0;
    }
}