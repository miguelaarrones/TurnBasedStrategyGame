using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode
{
    private GridPosition gridPosition;
    private int gCost;
    private int hCost;
    private int fCost;
    private PathNode cameFromPathNode;

    public PathNode(GridPosition gridPosition)
    {
        this.gridPosition = gridPosition;
    }

    public override string ToString()
    {
        return gridPosition.ToString();
    }

    public GridPosition GetGridPosition() => gridPosition;

    public int GetGCost() => gCost;

    public int GetHCost() => hCost;

    public int GetFCost() => fCost;

    public void SetGCost(int gCost) => this.gCost = gCost;

    public void SetHCost(int hCost) => this.hCost = hCost;

    public void CalculateFCost() => fCost = gCost + hCost;

    public void ResetCameFromPathNode() => cameFromPathNode = null;

    public void SetCameFromPathNode(PathNode pathNode) => cameFromPathNode = pathNode;

    public PathNode GetCameFromPathNode() => cameFromPathNode;
}
