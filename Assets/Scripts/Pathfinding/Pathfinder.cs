using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour {

    public LevelManager theLevelManager;
    public float mapRefreshRate;

    Vector3 m_Destination;
    bool b_PathFound;
    bool b_PathComplete;
    bool b_FollowPathCreated;

    List<List<Node>> NodeList;
    List<Node> OpenList;
    List<Node> ClosedList;
    List<Node> Path;

    double d_Timer;
    int i_CurrentIdx;

	// Use this for initialization
	void Start () {

        b_PathFound = false;
        b_PathComplete = false;
        b_FollowPathCreated = false;
        NodeList = new List<List<Node>>();
        OpenList = new List<Node>();
        ClosedList = new List<Node>();
        Path = new List<Node>();

        d_Timer = 0.0;
        i_CurrentIdx = 0;

        // Init List
        int SizeX = theLevelManager.columns;
        int SizeY = theLevelManager.rows;

        for (int i = 0; i < SizeX; i++)
        {
            NodeList.Add(new List<Node>());
        }

        // Fill up NodeList
        for (int i = 0; i < SizeX; i++)
        {
            for (int j = 0; j < SizeY; j++)
            {
                Node toAdd = new Node();
                toAdd.Init(theLevelManager.GetGridCost(i, j), theLevelManager.GetVec3Pos(i, j), i, j);

                NodeList[i].Add(toAdd);
            }
        } 
	}
	
	// Update is called once per frame
	void Update () {

        if (d_Timer > mapRefreshRate)
        {
            //RefreshNodeList();
        }
        else
        {
            d_Timer += Time.deltaTime;
        }
	}

    public void FindPath(Vector3 dest)
    {
        // Reset variables
        OpenList.Clear();
        ClosedList.Clear();

        b_PathComplete = false;
        b_PathFound = false;

        int SizeX = theLevelManager.columns;
        int SizeY = theLevelManager.rows;
        for (int i = 0; i < SizeX; i++)
        {
            for (int j = 0; j < SizeY; j++)
            {
                NodeList[i][j].Reset();
            }
        }

        m_Destination = dest;
        Node CurrentNode = GetNode(transform.position);
        Node TargetNode = GetNode(m_Destination);
        List<Node> NeighbourList = new List<Node>();

        while (!b_PathFound)
        {
            //Debug.Log("Finding Path to " + m_Destination.ToString());

            // Add Current Node to closed list
            ClosedList.Add(CurrentNode);
            ClosedList[ClosedList.Count - 1].IsInClosedList = true;

            // Check if reached target
            if (CurrentNode.m_pos == TargetNode.m_pos)
            {
                b_PathFound = true;
                Debug.Log("Path Found");
            }

            // Get Neighbours of curr node, compute F-values and add to openlist
            int CurrentGridPosX = (int)CurrentNode.m_GridPos.x;
            int CurrentGridPosY = (int)CurrentNode.m_GridPos.y;

            int CheckX = CurrentGridPosX;
            int CheckY = CurrentGridPosY; 

            // Top
            if (CurrentGridPosY != SizeY - 1)
            {
                CheckY = CurrentGridPosY + 1;
             
                // Top Middle
                CheckX = CurrentGridPosX;

                if (ValidateNode(NodeList[CheckX][CheckY]))
                {
                    OpenList.Add(NodeList[CheckX][CheckY]);
                    NeighbourList.Add(NodeList[CheckX][CheckY]);
                }

                // Top Left
                if (CurrentGridPosX != 0)
                {
                    CheckX = CurrentGridPosX - 1;

                    if (ValidateNode(NodeList[CheckX][CheckY]))
                    {
                        OpenList.Add(NodeList[CheckX][CheckY]);
                        NeighbourList.Add(NodeList[CheckX][CheckY]);
                    }
                }

                // Top Right
                if (CurrentGridPosX != SizeX - 1)
                {
                    CheckX = CurrentGridPosX + 1;
   
                    if (ValidateNode(NodeList[CheckX][CheckY]))
                    {
                        OpenList.Add(NodeList[CheckX][CheckY]);
                        NeighbourList.Add(NodeList[CheckX][CheckY]);
                    }
                }
            }

            // Bottom
            if (CurrentGridPosY != 0)
            {
                CheckY = CurrentGridPosY - 1;

                // Bottom Middle
                CheckX = CurrentGridPosX;

                if (ValidateNode(NodeList[CheckX][CheckY]))
                {
                    OpenList.Add(NodeList[CheckX][CheckY]);
                    NeighbourList.Add(NodeList[CheckX][CheckY]);
                }

                // Bottom Left
                if (CurrentGridPosX != 0)
                {
                    CheckX = CurrentGridPosX - 1;

                    if (ValidateNode(NodeList[CheckX][CheckY]))
                    {
                        OpenList.Add(NodeList[CheckX][CheckY]);
                        NeighbourList.Add(NodeList[CheckX][CheckY]);
                    }
                }

                // Bottom Right
                if (CurrentGridPosX != SizeX - 1)
                {
                    CheckX = CurrentGridPosX + 1;

                    if (ValidateNode(NodeList[CheckX][CheckY]))
                    {
                        OpenList.Add(NodeList[CheckX][CheckY]);
                        NeighbourList.Add(NodeList[CheckX][CheckY]);
                    }
                }
            }

            CheckY = CurrentGridPosY;

            // Left
            if (CurrentGridPosX != 0)
            {
                CheckX = CurrentGridPosX - 1;

                if (ValidateNode(NodeList[CheckX][CheckY]))
                {
                    OpenList.Add(NodeList[CheckX][CheckY]);
                    NeighbourList.Add(NodeList[CheckX][CheckY]);
                }
            }

            // Right
            if (CurrentGridPosX != SizeX - 1)
            {
                CheckX = CurrentGridPosX + 1;

                if (ValidateNode(NodeList[CheckX][CheckY]))
                {
                    OpenList.Add(NodeList[CheckX][CheckY]);
                    NeighbourList.Add(NodeList[CheckX][CheckY]);
                }
            }

            // Set all neghbours parent to current node
            foreach (Node aNode in NeighbourList)
            {
                if (aNode.ParentNode == null)
                aNode.ParentNode = CurrentNode;
            }

            // Get neighbour with lowest F value ()
            Node TempLowest = GetLowestF(OpenList);
            OpenList.Remove(TempLowest);
            CurrentNode = TempLowest;

            NeighbourList.Clear();
        }

    }

    public void FollowPath()
    {
        if (!b_FollowPathCreated)
        {
            Node endNode = ClosedList[ClosedList.Count - 1];

            Path.Add(endNode);

            while (endNode.ParentNode != null)
            {
                endNode = endNode.ParentNode;
                Path.Add(endNode);
            }

            Path.Reverse();

            b_FollowPathCreated = true;
        }

        if (!b_PathComplete)
        {
            //Debug.Log("Following Path");

            // Check if reached point
            if (Vector3.Distance(transform.position, Path[i_CurrentIdx].m_pos) <= 0.001f) // Should change to a var
            {
                ++i_CurrentIdx;
            }

            // Check if complete
            if (i_CurrentIdx >= Path.Count)
            {
                b_PathComplete = true;
                b_PathFound = false;
                b_FollowPathCreated = false;
                Path.Clear();

                i_CurrentIdx = 0;

                Debug.Log("Path Complete");
                return;
            }

            // DEBUG
            for (int i = 0; i < Path.Count - 1; ++i)
            {
                Debug.DrawLine(Path[i].m_pos, Path[i + 1].m_pos, Color.blue);
            }

            GetComponent<BaseSM>().WalkTowardPoint(Path[i_CurrentIdx].m_pos);
        }
    }

    float GetManhattenDistance(Node aNode)
    {
        return (Mathf.Abs(m_Destination.x - aNode.m_pos.x) + Mathf.Abs(m_Destination.y - aNode.m_pos.y));
    }

    Node GetNode(Vector3 pos)
    {
        Vector2 check = theLevelManager.GetGridPos(pos);

        int SizeX = theLevelManager.columns;
        int SizeY = theLevelManager.rows;
        for (int i = 0; i < SizeX; ++i)
        {
            for (int j = 0; j < SizeY; ++j)
            {
                if (i == check.x && j == check.y)
                    return NodeList[i][j];
            }
        }

        return null;
    }

    bool ValidateNode(Node checkNode)
    {
        // Do various checks if node is valid
        if (checkNode == null)
        {
            return false;
        }

        if (checkNode.TileCost == -1)
        {
            return false;
        }

        if (checkNode.IsInClosedList)
        {
            return false;
        }

        return true;
    }

    Node GetLowestF(List<Node> checkList)
    {
        if (checkList.Count <= 0)
            return null;

        int LowestF_Value = 99999;
        int LowestF_Idx = 0;
        for (int i = 0; i < checkList.Count; ++i)
        {
            if (checkList[i].CalculateAccCost() + GetManhattenDistance(checkList[i]) < LowestF_Value)
            {
                LowestF_Value = checkList[i].AccCost + (int)GetManhattenDistance(checkList[i]);
                LowestF_Idx = i;
            }
        }
        return checkList[LowestF_Idx];
    }

    bool CheckIfInClosedList(Node checkNode)
    {
        for (int i = 0; i < ClosedList.Count; ++i)
        {
            if (ClosedList[i].Equals(checkNode))
            {
                return true;
            }
        }

        return false;
    }

    bool CheckIfInOpenList(Node checkNode)
    {
        for (int i = 0; i < OpenList.Count; ++i)
        {
            if (OpenList[i].Equals(checkNode))
            {
                return true;
            }
        }

        return false;
    }

    public bool GetPathFound()
    {
        return b_PathFound;
    }

    public bool GetPathComplete()
    {
        return b_PathComplete;
    }

    public Vector3 RandomPos(int range)
    {
        Node CurrentNode = GetNode(transform.position);

        while (true)
        {
            // Random a node
            int RandomX = (int)CurrentNode.m_GridPos.x + Random.Range(-range, range);
            int RandomY = (int)CurrentNode.m_GridPos.y + Random.Range(-range, range);

            RandomX = Mathf.Clamp(RandomX, 0, theLevelManager.columns);
            RandomY = Mathf.Clamp(RandomY, 0, theLevelManager.rows);

            Node RandomNode = NodeList[RandomX][RandomY];

            if (ValidateNode(RandomNode) && CurrentNode != RandomNode)
            {
                // Check if random node behind a wall 
                int diffX = (int)Mathf.Abs(RandomNode.m_GridPos.x - CurrentNode.m_GridPos.x);
                int diffY = (int)Mathf.Abs(RandomNode.m_GridPos.y - CurrentNode.m_GridPos.y);

                bool ValidNode = true;
                for (int i = diffX; i > 0; --i)
                {
                    for (int j = diffY; j > 0; --j)
                    {
                        int checkX = Mathf.Clamp(RandomX - diffX, 0, theLevelManager.columns);
                        int checkY = Mathf.Clamp(RandomY - diffY, 0, theLevelManager.rows);

                        if (!ValidateNode(NodeList[checkX][checkY]))
                        {
                            ValidNode = false;
                            break;
                        }
                    }

                    if (!ValidNode)
                        break;
                }

                if (ValidNode)
                    return RandomNode.m_pos;
            }
        }
    }
}
