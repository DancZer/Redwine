using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Tiles
{
    public static Tile Dirt = new Tile(2,15,"Dirt");
    public static Tile Cloud = new Tile(0,0,"Cloud");
    public static Tile Grass =  new Tile(12,3,"Grass");
    public static Tile GrassSide =  new Tile(3,15,"GrassSide");
    public static Tile Water =  new Tile(14,3,"Water");
    public static Tile Stone =  new Tile(1,15,"Stone");
    public static Tile TreeTrunk =  new Tile(4,14,"TreeTrunk");
    public static Tile TreeTrunkTop =  new Tile(5,14,"TreeTrunkTop");
    public static Tile TreeCrown =  new Tile(4,12,"TreeCrown");
}
