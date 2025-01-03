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
    [Range(0.35f, 0.5f)] public float threshold; // Threshold to define playable areas
    [Range(0, 10000)] public int levelSeed = 0; // Seed for the noise

    private int lastSeed = -1;      // Keeps track of the last used seed

    void Start()
    {
        RandomizeParameters(); // Randomize on start
        GenerateChessboard();
    }

    void OnValidate()
    {
        if (Application.isPlaying && levelSeed != lastSeed)
        {
            lastSeed = levelSeed;
            RandomizeParameters();
            GenerateChessboard();
        }
    }

    void RandomizeParameters()
    {
        // Randomize noiseScale and threshold
        noiseScale = Random.Range(0.35f, 0.9f);
        threshold = Random.Range(0.35f, 0.5f);

        Debug.Log($"Randomized parameters: Noise Scale = {noiseScale}, Threshold = {threshold}, Seed = {levelSeed}");
    }

    void GenerateChessboard()
    {
        // Clear any existing tiles
        tilemap.ClearAllTiles();

        // Set the random seed for consistency
        Random.InitState(levelSeed);

        // Loop through the grid
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                // Generate Perlin noise value with the level seed
                float noiseValue = Mathf.PerlinNoise(
                    (x + levelSeed) * noiseScale,
                    (y + levelSeed) * noiseScale
                );

                // Determine if this tile is within the playable area
                if (noiseValue < threshold)
                {
                    // Skip this tile if it's not part of the playable area
                    continue;
                }

                // Calculate chessboard alternating pattern
                bool isBlackTile = (x + y) % 2 == 0;

                // Choose the tile
                Tile tileToPlace = isBlackTile ? blackTile : whiteTile;

                // Place the tile
                Vector3Int tilePos = new Vector3Int(x, y, 0);
                tilemap.SetTile(tilePos, tileToPlace);
            }
        }
    }
}
