using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block
{
    public bool IsAir {get;}
    public bool IsLiquid {get;}
    public bool IsSolid => !(IsAir || IsLiquid);
    public Tile Top;
    public Tile Side;
    public Tile Bottom;

    public Block(bool air, bool liquid, Tile tile)
    {
        IsAir = air;
        IsLiquid = liquid;
        Top = Side = Bottom = tile;
    }

    override public string ToString(){
        return $"IsAir:{IsAir}, IsLiquid:{IsLiquid}, Top:{Top}, Side:{Side}, Bottom:{Bottom}";
    }
}
