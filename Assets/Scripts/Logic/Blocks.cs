using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blocks
{
    public static Block Air = new Block(true, false, null);
    public static Block Dirt = new Block(false, false, BlockMaterials.Dirt);
    public static Block Water = new Block(false, true, BlockMaterials.Water1);
}
