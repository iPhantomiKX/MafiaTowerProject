using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public int TileCost;
    public int AccCost;
    public Vector3 m_pos;
    public Vector2 m_GridPos;

    public Node ParentNode = null;
	public bool IsInClosedList = false;
    public bool IsInOpenList = false;

    public void Init(int cost, Vector3 pos, float x, float y)
    {
        ParentNode = null;
        TileCost = cost;
        m_pos = pos;
        IsInClosedList = false;
		IsInOpenList = false;

        m_GridPos.x = x;
        m_GridPos.y = y;
    }

    public int CalculateAccCost()
    {
        if (ParentNode != null)
        {
            AccCost = this.TileCost + this.ParentNode.CalculateAccCost();
            return AccCost;
        }
        else
        {
            AccCost = this.TileCost;
            return AccCost;
        }
    }

    public void Reset()
    {
        AccCost = 0;
        ParentNode = null;
        IsInClosedList = false;
		IsInOpenList = false;
    }
}