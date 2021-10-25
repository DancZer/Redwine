using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blocks
{
    public static Block Air = new Block(true, false);
    public static Block Dirt = new Block(false, false);
    public static Block Water = new Block(false, true);
}
