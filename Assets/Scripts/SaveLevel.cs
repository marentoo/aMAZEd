/*public static class SaveLevel
{
    public static SaveData CreateSaveData(Maze maze, Player player)
    {
        SaveData saveData = new SaveData
        {
            mazeWidth = maze.Width,
            mazeHeight = maze.Height,
            cells = new List<CellData>(),
            playerData = new PlayerData
            {
                positionX = player.transform.position.x,
                positionY = player.transform.position.y,
                positionZ = player.transform.position.z
            }
            // Populate other elements like zombies, keys...
        };

        foreach (var cell in maze.Cells)
        {
            saveData.cells.Add(new CellData
            {
                x = cell.X,
                y = cell.Y,
                isWall = cell.IsWall
                // Populate other cell properties
            });
        }

        return saveData;
    }
}
*/