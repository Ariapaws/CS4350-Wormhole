using UnityEngine;
using System.Collections.Generic;

public class MazeRoom : ScriptableObject
{

    public int settingsIndex;

    public MazeRoomSettings settings;

    public List<MazeCell> cells = new List<MazeCell>();

    public void Add(MazeCell cell)
    {
        cell.room = this;
        cells.Add(cell);
    }

    public MazeCell getRandomCell()
    {
        int cellNum = cells.Count;
        int randCellIndex = Random.Range(1, cellNum + 1) - 1;
        return cells[randCellIndex];
    }

    public void Assimilate(MazeRoom room)
    {
        for (int i = 0; i < room.cells.Count; i++)
        {
            Add(room.cells[i]);
        }
    }
}