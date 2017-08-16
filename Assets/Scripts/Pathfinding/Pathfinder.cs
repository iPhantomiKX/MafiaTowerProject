using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour {

    public LevelManager theLevelManager;
    public float mapRefreshRate;
	public float reachedNodeDist = 0.001f;
	public bool WalkDiagonal = true;

	Node CurrentNode;
    Vector2 CurrentVec2Pos;

    Vector3 m_Destination;
    bool b_PathFound;
    bool b_PathComplete;
    bool b_FollowPathCreated;
    bool b_ContinueNextFrame;

    List<List<Node>> NodeList;
    List<Node> OpenList;
    List<Node> ClosedList;
    List<Node> Path;

    double d_Timer;
    int i_CurrentIdx;

	float cooldownTime;

    // Debug
    int BreakInfiniteLoopAmount = 500;

	// Use this for initialization
	void Start () {

        b_PathFound = false;
        b_PathComplete = false;
        b_FollowPathCreated = false;
        b_ContinueNextFrame = false;

        NodeList = new List<List<Node>>();
        OpenList = new List<Node>();
        ClosedList = new List<Node>();
        Path = new List<Node>();

        d_Timer = 99999.0;
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

        if (theLevelManager == null)
        {
            theLevelManager = GameObject.FindObjectOfType<LevelManager>().GetComponent<LevelManager>();
        }

        CurrentVec2Pos = theLevelManager.GetGridPos(transform.position);
    }
	
	// Update is called once per frame
	void Update () {

        if (d_Timer > mapRefreshRate)
        {
            //theLevelManager.MirgratePos(CurrentVec2Pos, theLevelManager.GetGridPos(transform.position));
			CurrentVec2Pos = theLevelManager.GetGridPos (transform.position);
            d_Timer = 0.0;
        }
        else
        {
            d_Timer += Time.deltaTime;
        }

		if (cooldownTime > 0) 
		{
			cooldownTime -= Time.deltaTime;
		}
	}

    public void FindPath(Vector3 dest)
    {
		if (cooldownTime > 0)
			return;

        int SizeX = theLevelManager.columns;
        int SizeY = theLevelManager.rows;

        if (!b_ContinueNextFrame)
        {
            // Reset variables
            b_PathComplete = false;
            b_PathFound = false;

            RefreshNodeList();

            m_Destination = dest;
			CurrentNode = GetNode(transform.position);
        }
				
        Node TargetNode = GetNode(m_Destination);
        List<Node> NeighbourList = new List<Node>();

        bool BreakLoop = false;
        int LoopCount = 0;

        while (!b_PathFound && !BreakLoop)
        {
            //Debug.Log("Finding Path to " + m_Destination.ToString());

            BreakLoop = LoopCount >= BreakInfiniteLoopAmount;
            if (BreakLoop)
            {
                b_ContinueNextFrame = true;
                //Debug.Log(" ------------------------------------------------------------");
                //Debug.Log(" ----- Pathfind took too long. Loops: " + LoopCount );
                //Debug.Log(" ----- ClosedList Size: " + ClosedList.Count);
                //Debug.Log(" ----- Destination: " + m_Destination.ToString());
                //Debug.Log(" ----- Current Position: " + CurrentVec2Pos.ToString());
                //Debug.Log(" ------------------------------------------------------------");
                //Debug.Log("Current Node: " + CurrentNode.TileCost);

                return;
            }

            // Add Current Node to closed list
			if (ValidateNode (CurrentNode)) {
				CurrentNode.IsInOpenList = false;
				CurrentNode.IsInClosedList = true;
				ClosedList.Add (CurrentNode);
			}

            // Check if reached target
			if (ClosedList.Contains(TargetNode))
            {
                b_PathFound = true;
                b_ContinueNextFrame = false;

                //Debug.Log("Path Found. Loops: " + LoopCount);
            }

            // Get Neighbours of curr node, compute F-values and add to openlist
            int CurrentGridPosX = (int)CurrentVec2Pos.x;
            int CurrentGridPosY = (int)CurrentVec2Pos.y;

            if (CurrentNode != null)
            {
                CurrentGridPosX = (int)CurrentNode.m_GridPos.x;
                CurrentGridPosY = (int)CurrentNode.m_GridPos.y;
            }

            int CheckX = CurrentGridPosX;
            int CheckY = CurrentGridPosY; 

            // Top
            if (CurrentGridPosY != SizeY - 1)
            {
                CheckY = CurrentGridPosY + 1;

                // Top Middle
                CheckX = CurrentGridPosX;

                if (ValidateNode(NodeList[CheckX][CheckY], true))
                {
                    NodeList[CheckX][CheckY].IsInOpenList = true;

                    OpenList.Add(NodeList[CheckX][CheckY]);
                    NeighbourList.Add(NodeList[CheckX][CheckY]);
                }

                if (WalkDiagonal)
                {
                    // Top Left
                    if (CurrentGridPosX != 0)
                    {
                        CheckX = CurrentGridPosX - 1;

                        if (ValidateNode(NodeList[CheckX][CheckY], true))
                        {
                            NodeList[CheckX][CheckY].IsInOpenList = true;

                            OpenList.Add(NodeList[CheckX][CheckY]);
                            NeighbourList.Add(NodeList[CheckX][CheckY]);
                        }
                    }

                    // Top Right
                    if (CurrentGridPosX != SizeX - 1)
                    {
                        CheckX = CurrentGridPosX + 1;

                        if (ValidateNode(NodeList[CheckX][CheckY], true))
                        {
                            NodeList[CheckX][CheckY].IsInOpenList = true;

                            OpenList.Add(NodeList[CheckX][CheckY]);
                            NeighbourList.Add(NodeList[CheckX][CheckY]);
                        }
                    }
                }
            }

            // Bottom
            if (CurrentGridPosY != 0)
            {
                CheckY = CurrentGridPosY - 1;

                // Bottom Middle
                CheckX = CurrentGridPosX;

				if (ValidateNode(NodeList[CheckX][CheckY], true))
                {
					NodeList [CheckX] [CheckY].IsInOpenList = true;
                    
					OpenList.Add(NodeList[CheckX][CheckY]);
                    NeighbourList.Add(NodeList[CheckX][CheckY]);
                }

                if (WalkDiagonal)
                {
                    // Bottom Left
                    if (CurrentGridPosX != 0)
                    {
                        CheckX = CurrentGridPosX - 1;

                        if (ValidateNode(NodeList[CheckX][CheckY], true))
                        {
                            NodeList[CheckX][CheckY].IsInOpenList = true;

                            OpenList.Add(NodeList[CheckX][CheckY]);
                            NeighbourList.Add(NodeList[CheckX][CheckY]);
                        }
                    }

                    // Bottom Right
                    if (CurrentGridPosX != SizeX - 1)
                    {
                        CheckX = CurrentGridPosX + 1;

                        if (ValidateNode(NodeList[CheckX][CheckY], true))
                        {
                            NodeList[CheckX][CheckY].IsInOpenList = true;

                            OpenList.Add(NodeList[CheckX][CheckY]);
                            NeighbourList.Add(NodeList[CheckX][CheckY]);
                        }
                    }
                }
            }

            CheckY = CurrentGridPosY;

            // Left
            if (CurrentGridPosX != 0)
            {
                CheckX = CurrentGridPosX - 1;

				if (ValidateNode(NodeList[CheckX][CheckY], true))
                {
					NodeList [CheckX] [CheckY].IsInOpenList = true;
                    
					OpenList.Add(NodeList[CheckX][CheckY]);
                    NeighbourList.Add(NodeList[CheckX][CheckY]);
                }
            }

            // Right
            if (CurrentGridPosX != SizeX - 1)
            {
                CheckX = CurrentGridPosX + 1;

				if (ValidateNode(NodeList[CheckX][CheckY], true))
                {
					NodeList [CheckX] [CheckY].IsInOpenList = true;
                   
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

			if (NeighbourList.Count > 0)
			{

			}

			// Get neighbour with lowest F value ()
			Node TempLowest = GetLowestF (OpenList);
			OpenList.Remove (TempLowest);
			CurrentNode = TempLowest;

			NeighbourList.Clear ();
            
			LoopCount++;
        }

    }

    public Vector3 FollowPath()
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
            //Debug.Log(Vector2.Distance(transform.position, Path[i_CurrentIdx].m_pos));
            if (Vector2.Distance(transform.position, Path[i_CurrentIdx].m_pos) <= reachedNodeDist)
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

                //Debug.Log("Path Complete");
                return new Vector3();
            }

            // DEBUG
            for (int i = 0; i < Path.Count - 1; ++i)
            {
                Debug.DrawLine(Path[i].m_pos, Path[i + 1].m_pos, Color.blue);
            }

            if (GetComponent<BaseSM>())
                GetComponent<BaseSM>().WalkTowardPoint(Path[i_CurrentIdx].m_pos);
            else
                return (Path[i_CurrentIdx].m_pos - transform.position).normalized;
        }

        return new Vector3();
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

	bool ValidateNode(Node checkNode, bool CheckForOpenList = false)
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

		if (CheckForOpenList)  
		{
			if (checkNode.IsInOpenList) {
				return false;
			}
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

    void RefreshNodeList()
    {
        NodeList.Clear();
		OpenList.Clear ();
		ClosedList.Clear ();
		Path.Clear ();

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

    public bool GetPathFound()
    {
        return b_PathFound;
    }

    public bool GetPathComplete()
    {
        return b_PathComplete;
    }

	public void SetCooldown()
	{
        cooldownTime = 2.0f + Random.Range(0, 2); // change to var
	}

	public float GetCooldown()
	{
		return cooldownTime;
	}

    public Vector3 GetVec2Pos()
    {
        return new Vector3(CurrentVec2Pos.x, CurrentVec2Pos.x, transform.position.z);
    }

	public void Reset()
	{
		b_PathFound = false;
		b_PathComplete = false;
		b_FollowPathCreated = false;
        b_ContinueNextFrame = false;

		i_CurrentIdx = 0;

		RefreshNodeList ();
	}

    public Vector3 RandomPos(int range, Vector3 startPos)
    {
        Node CurrentNode = GetNode(startPos);
        int maxRetries = 5;

        while (maxRetries > 0)
        {
            // Random a node
            int RandomX = (int)CurrentNode.m_GridPos.x + Random.Range(-range, range);
            int RandomY = (int)CurrentNode.m_GridPos.y + Random.Range(-range, range);

            RandomX = Mathf.Clamp(RandomX, 0, theLevelManager.columns);
            RandomY = Mathf.Clamp(RandomY, 0, theLevelManager.rows);

            Node RandomNode = NodeList[RandomX][RandomY];
            RandomNode.Reset();
            //Debug.Log("Random Node Found: " + theLevelManager.GetTileType(RandomX, RandomY));

            if (ValidateNode(RandomNode) && CurrentNode != RandomNode)
            {
                // Check if random node behind a wall 
                int diffX = (int)Mathf.Abs(RandomNode.m_GridPos.x - CurrentNode.m_GridPos.x);
                int diffY = (int)Mathf.Abs(RandomNode.m_GridPos.y - CurrentNode.m_GridPos.y);

                bool ValidNode = true;
                //for (int i = diffX; i >= 0; --i)
                //{
                //    for (int j = diffY; j >= 0; --j)
                //    {
                //        int checkX = Mathf.Clamp(RandomX - diffX, 0, theLevelManager.columns);
                //        int checkY = Mathf.Clamp(RandomY - diffY, 0, theLevelManager.rows);
                //        NodeList[checkX][checkY].Reset();

                //        if (!ValidateNode(NodeList[checkX][checkY]))
                //        {
                //            ValidNode = false;
                //            break;
                //        }
                //    }

                //    if (!ValidNode)
                //        break;
                //}

                if (ValidNode)
                    return RandomNode.m_pos;
            }

            --maxRetries;
        }

        return transform.position;
    }

	public bool ValidPos(Vector3 pos){
		Node checkNode = GetNode(pos);
		return ValidateNode (checkNode);

	}

	void OnDrawGizmos()
	{
        //foreach (Node aNode in ClosedList)
        //{
        //    Gizmos.color = Color.red;
        //    Gizmos.DrawSphere(aNode.m_pos, 0.075f);
        //}

        //foreach (Node aNode in OpenList) 
        //{
        //    Gizmos.color = Color.blue;
        //    Gizmos.DrawSphere (aNode.m_pos, 0.05f);
        //}

        //if (CurrentNode != null)
        //{
        //    Gizmos.color = Color.yellow;
        //    Gizmos.DrawSphere(CurrentNode.m_pos, 0.1f);
        //}

        //if (!b_PathComplete && b_PathFound)
        //{
        //    if (Path[i_CurrentIdx] != null)
        //    {
        //        Gizmos.color = Color.gray;
        //        Gizmos.DrawSphere(Path[i_CurrentIdx].m_pos, 0.1f);
        //    }
        //}
	}
}
