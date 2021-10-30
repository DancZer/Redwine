using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block
{
    public bool IsAir {get;}

    public bool IsLiquid {get;}

    public bool IsSolid => !(IsAir || IsLiquid);

    public BlockMaterial Material {get;}

    public Block(bool air, bool liquid, BlockMaterial material)
    {
        IsAir = air;
        IsLiquid = liquid;
        Material = material;
    }

    public BlockState Default(){
        return new BlockState(this);
    }

    override public string ToString(){
        return $"IsAir: {IsAir}, IsLiquid:{IsLiquid}";
    }
}
