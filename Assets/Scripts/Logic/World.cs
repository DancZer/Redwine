using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

//This is more like the player area, not the whole world. functionality should be split
public static class World
{
    private static Dictionary<Vector3Int, Chunk> chunks = new Dictionary<Vector3Int, Chunk>();

    private static WorldGenerator generator = new WorldGenerator();

    public static void Init()
    {
        generator.Init(Config.RandomSeed);
    }

    public static Vector3Int GetStartPos()
    {
        var startPos = new Vector3Int(0,0,0);
        startPos.y = generator.GetBaseLandHeight(startPos)+2;

        return startPos;
    }

    public static Chunk GetChunk(Vector3Int pos)
    {       
        var chunkPos = pos.ToChunkAligned();

        Chunk chunk;
        if(!chunks.TryGetValue(chunkPos, out chunk)){
            chunk = generator.Generate(chunkPos);
            chunks.Add(chunkPos, chunk);
        }
        return chunk;
    }

    public static BlockType GetBlockType(Vector3Int pos)
    {
        var chunk = GetChunk(pos);

        var relPos = pos-chunk.Pos;

        return chunk.GetBlockType(relPos);
    }
    public static void SetBlockType(Vector3Int pos, BlockType blockType)
    {
        var chunk = GetChunk(pos);
        var relPos = pos-chunk.Pos;

        chunk.SetBlockType(relPos, blockType);

        if(relPos.x == 0)
        {
            var westChunk = GetChunk(chunk.Pos.WestChunk());
            var westRelPos = pos-westChunk.Pos;
            westChunk.SetBlockType(westRelPos, blockType);
        }

        if(relPos.x == Config.ChunkSize-1)
        {
            var westChunk = GetChunk(chunk.Pos.EastChunk());
            var westRelPos = pos-westChunk.Pos;
            westChunk.SetBlockType(westRelPos, blockType);
        }

        if(relPos.y == 0)
        {
            var westChunk = GetChunk(chunk.Pos.BelowChunk());
            var westRelPos = pos-westChunk.Pos;
            westChunk.SetBlockType(westRelPos, blockType);
        }

        if(relPos.y == Config.ChunkSize-1)
        {
            var westChunk = GetChunk(chunk.Pos.AboveChunk());
            var westRelPos = pos-westChunk.Pos;
            westChunk.SetBlockType(westRelPos, blockType);
        }

        if(relPos.z == 0)
        {
            var westChunk = GetChunk(chunk.Pos.SouthChunk());
            var westRelPos = pos-westChunk.Pos;
            westChunk.SetBlockType(westRelPos, blockType);
        }

        if(relPos.z == Config.ChunkSize-1)
        {
            var westChunk = GetChunk(chunk.Pos.NorthChunk());
            var westRelPos = pos-westChunk.Pos;
            westChunk.SetBlockType(westRelPos, blockType);
        }
    }
}
