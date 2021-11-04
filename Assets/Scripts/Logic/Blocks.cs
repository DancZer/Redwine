using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blocks
{
    public static Dictionary<BlockType, Block> blocks = new Dictionary<BlockType, Block>()
    {
        {BlockType.Air, new Block(true, false, null)},
        {BlockType.Dirt, new Block(false, false, new Tile(2,15,"Dirt"))},
        {BlockType.Cloud, new Block(false, false, new Tile(2,15,"Dirt"))},
        {BlockType.DirtGrass, new Block(false, false, new Tile(12,3,"Grass"), new Tile(3,15,"DirtGrassSide"), new Tile(2,15,"Dirt"))},
        {BlockType.Water, new Block(false, true, new Tile(14,3,"Water"))},
    };
}
public enum BlockType
{
    Air, Cloud, Dirt, DirtGrass, Water
}
