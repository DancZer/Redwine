using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

//This is more like the player area, not the whole world. functionality should be split
public class World
{
    private Dictionary<Vector3Int, Chunk> chunks = new Dictionary<Vector3Int, Chunk>();

    private WorldGenerator generator = new WorldGenerator();

    public void Init()
    {
        generator.Init(Config.RandomSeed);
    }

    public Vector3Int GetStartPos()
    {
        return new Vector3Int(0, Mathf.FloorToInt(generator.GetBaseLandHeight(0, 0))+2, 0);
    }

    public Chunk GetChunk(Vector3Int pos)
    {       
        var chunkPos = pos.ToChunkAligned();

        Chunk chunk;
        if(!chunks.TryGetValue(chunkPos, out chunk)){
            chunk = generator.Generate(chunkPos);
            chunks.Add(chunkPos, chunk);
        }
        return chunk;
    }
}
