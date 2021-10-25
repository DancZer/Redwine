using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block
{
    public bool IsAir {get;}

    public bool IsLiquid {get;}

    public bool IsSolid => !(IsAir || IsLiquid);

    public Block(bool air, bool liquid)
    {
        IsAir = air;
        IsLiquid = liquid;
    }

    public BlockState Default(){
        return new BlockState(this);
    }

    override public string ToString(){
        return $"IsAir: {IsAir}, IsLiquid:{IsLiquid}";
    }
}
