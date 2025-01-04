using UnityEngine;
using UnityEngine.Tilemaps;

public class ProceduralChessboard : MonoBehaviour
{
    public Tilemap tilemap;         // The chessboard tilemap
    public Tile blackTile;          // Black tile
    public Tile whiteTile;          // White tile

    public int width = 20;          // Width of the grid
    public int height = 20;         // Height of the grid
    [Range(0.35f, 0.9f)] public float noiseScale; // Scale of the Perlin noise
    [Range(0.35f, 0.5f)] public float threshold; // Threshold to define the board's shape
    [Range(0, 10000)] public int levelSeed = 0;  // Seed for the noise

    private int lastSeed = -1;      // Tracks the last used seed

    void Start()
    {
        GenerateChessboard();
    }

    void OnValidate()
    {
        if (Application.isPlaying && levelSeed != lastSeed)
        {
            lastSeed = levelSeed;
            GenerateChessboard();
        }
    }

    void GenerateChessboard()
    {
        // Clear the previous chessboard
        tilemap.ClearAllTiles();

        // Set random seed for Perlin noise
        Random.InitState(levelSeed);

        // Loop through the grid dimensions
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                // Generate a Perlin noise value for this position
                float noiseValue = Mathf.PerlinNoise(
                    (x + levelSeed) * noiseScale,
                    (y + levelSeed) * noiseScale
                );

                // Check if the noise value exceeds the threshold
                if (noiseValue < threshold)
                {
                    continue; // Skip this tile, making it part of the "empty" area
                }

                // Determine whether to place a black or white tile
                bool isBlackTile = (x + y) % 2 == 0;

                // Select the tile to place
                Tile tileToPlace = isBlackTile ? blackTile : whiteTile;

                // Place the tile on the tilemap
                Vector3Int tilePos = new Vector3Int(x, y, 0);
                tilemap.SetTile(tilePos, tileToPlace);
            }
        }
    }
}
