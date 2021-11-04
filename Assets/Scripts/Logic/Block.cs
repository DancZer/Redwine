using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block
{
    public readonly bool IsAir;
    public readonly bool IsLiquid;
    public bool IsSolid => !(IsAir || IsLiquid);
    public readonly Tile Top;
    public readonly Tile Side;
    public readonly Tile Front;
    public readonly Tile Bottom;

    public Block(bool air, bool liquid, Tile tile)
    {
        IsAir = air;
        IsLiquid = liquid;
        
        Top = Side = Front = Bottom = tile;
    }
    public Block(bool air, bool liquid, Tile top, Tile side, Tile bottom)
    {
        IsAir = air;
        IsLiquid = liquid;

        Top = top;
        Side = Front = side;
        Bottom = bottom;
    }

    public Block(bool air, bool liquid, Tile top, Tile front, Tile side, Tile bottom)
    {
        IsAir = air;
        IsLiquid = liquid;

        Top = top;
        Front = front;
        Side = side;
        Bottom = bottom;
    }

    override public string ToString(){
        return $"IsAir:{IsAir}, IsLiquid:{IsLiquid}, Top:{Top}, Side:{Side}, Bottom:{Bottom}";
    }
}
