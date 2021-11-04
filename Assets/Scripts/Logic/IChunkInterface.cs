using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IChunkInterface
{
    Vector3Int Size {get;}
    BlockType GetBlockType(Vector3Int pos);
    void SetBlockType(Vector3Int pos, BlockType type);
}
