using System.Collections.Generic;
using UnityEngine;

public class EllersMazeGenerator : MonoBehaviour
{
    [Header("Maze Settings")]
    public int width = 10;
    public int height = 10;

    [Header("Prefabs")]
    //public GameObject floorPrefab;
    public GameObject wallPrefab;

    private class Cell
    {
        public int setID;
        public bool rightWall = true;
        public bool bottomWall = true;
    }

    private List<Cell> currentRow = new List<Cell>();
    private int nextSetID = 1;

    void Start()
    {
        GenerateMaze();
    }

    void GenerateMaze()
    {
        for (int y = 0; y < height; y++)
        {
            // Step 1: Initialize row
            currentRow.Clear();
            for (int x = 0; x < width; x++)
            {
                Cell cell = new Cell();
                if (y == 0 || x >= currentRow.Count)
                    cell.setID = nextSetID++; // new set
                else
                    cell.setID = currentRow[x].setID; // inherited from previous row
                currentRow.Add(cell);
            }

            // Step 2: Randomly join horizontal neighbors
            for (int x = 0; x < width - 1; x++)
            {
                Cell a = currentRow[x];
                Cell b = currentRow[x + 1];

                if (a.setID != b.setID && Random.value > 0.5f)
                {
                    // Merge sets
                    int oldSet = b.setID;
                    for (int i = 0; i < width; i++)
                        if (currentRow[i].setID == oldSet)
                            currentRow[i].setID = a.setID;

                    // Remove right wall
                    a.rightWall = false;
                }
            }

            // Step 3: Create vertical connections
            Dictionary<int, List<int>> sets = new Dictionary<int, List<int>>();
            for (int x = 0; x < width; x++)
            {
                int setID = currentRow[x].setID;
                if (!sets.ContainsKey(setID)) sets[setID] = new List<int>();
                sets[setID].Add(x);
            }

            foreach (var kvp in sets)
            {
                List<int> cells = kvp.Value;
                int numVerticals = Mathf.Max(1, Random.Range(1, cells.Count + 1)); // at least 1 vertical
                List<int> selected = new List<int>();
                while (selected.Count < numVerticals)
                {
                    int idx = cells[Random.Range(0, cells.Count)];
                    if (!selected.Contains(idx))
                        selected.Add(idx);
                }

                foreach (int x in cells)
                {
                    if (!selected.Contains(x))
                        currentRow[x].bottomWall = true;
                    else
                        currentRow[x].bottomWall = false;
                }
            }

            // Top and Bottom
            for (int x = 0; x < width; x++)
            {
                Instantiate(wallPrefab, new Vector3(x, 0.5f, 0), Quaternion.identity, transform).transform.localScale = new Vector3(1, 0.1f, 1);
                Instantiate(wallPrefab, new Vector3(x, -height + 0.5f, 0), Quaternion.identity, transform).transform.localScale = new Vector3(1, 0.1f, 1);
            }

            // Left and Right
            for (int z = 0; z < height; z++)
            {
                Instantiate(wallPrefab, new Vector3(-0.5f, -z, 0), Quaternion.identity, transform).transform.localScale = new Vector3(0.1f, 1, 1);
                Instantiate(wallPrefab, new Vector3(width - 0.5f, -z, 0), Quaternion.identity, transform).transform.localScale = new Vector3(0.1f, 1, 1);
            }


            // Inside the Instantiate loop
            for (int x = 0; x < width; x++)
            {
                Vector3 pos = new Vector3(x, -y, 0);

                // Right wall
                if (currentRow[x].rightWall)
                {
                    Vector3 wallPos = pos + new Vector3(0.5f, 0, 0);
                    GameObject rightWall = Instantiate(wallPrefab, wallPos, Quaternion.identity, transform);
                    rightWall.transform.localScale = new Vector3(0.1f, 1, 1); // vertical
                }

                // Bottom wall
                if (currentRow[x].bottomWall)
                {
                    Vector3 wallPos = pos + new Vector3(0, -0.5f, 0);
                    GameObject bottomWall = Instantiate(wallPrefab, wallPos, Quaternion.identity, transform);
                    bottomWall.transform.localScale = new Vector3(1, 0.1f, 1); // horizontal
                }
            }


            // Step 5: Prepare next row sets
            if (y < height - 1)
            {
                for (int x = 0; x < width; x++)
                {
                    if (currentRow[x].bottomWall)
                        currentRow[x].setID = nextSetID++; // new set if no vertical
                }
            }


        }
    }
}
