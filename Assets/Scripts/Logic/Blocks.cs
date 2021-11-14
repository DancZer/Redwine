using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blocks
{
    public static Dictionary<BlockType, Block> blocks = new Dictionary<BlockType, Block>()
    {
        {BlockType.Air, new Block(true, false, null)},
        {BlockType.Dirt, new Block(false, false, Tiles.Dirt)},
        {BlockType.Cloud, new Block(false, false, Tiles.Cloud)},
        {BlockType.DirtGrass, new Block(false, false, Tiles.Grass, Tiles.GrassSide, Tiles.Dirt)},
        {BlockType.TreeTrunk, new Block(false, false, Tiles.TreeTrunkTop, Tiles.TreeTrunk, Tiles.TreeTrunkTop)},
        {BlockType.TreeCrown, new Block(false, false, Tiles.TreeCrown)},
        {BlockType.Water, new Block(false, true, Tiles.Water)},
        {BlockType.Stone, new Block(false, false, Tiles.Stone)},
    };
}
public enum BlockType
{
    Air, Cloud, Dirt, DirtGrass, Water, Stone, TreeTrunk, TreeCrown
}
