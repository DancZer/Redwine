using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : IChunkInterface
{
    private const int InnerSize = Config.ChunkSize+2;
    private BlockType[,,] blockStates;
    public string Name {get;}
    public Vector3Int Pos {get;}
    public Vector3Int Size {
        get
        {
            return new Vector3Int(Config.ChunkSize, Config.ChunkSize, Config.ChunkSize);
        }
    }
    public float Opacity {get;} = 1;
    
    public Chunk(Vector3Int pos)
    {
        Pos = pos;
        Name=$"Chunk_{pos.x}_{pos.y}_{pos.z}";
        blockStates = new BlockType[InnerSize, InnerSize, InnerSize];
    }

    public BlockType GetBlockType(Vector3Int pos)
    {
        return blockStates[pos.x+1, pos.y+1, pos.z+1];
    }

    public void SetBlockType(Vector3Int pos, BlockType type)
    {
        blockStates[pos.x+1, pos.y+1, pos.z+1] = type;
    }

    public override string ToString()
    {
        return Name;
    }
}
