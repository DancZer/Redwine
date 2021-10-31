using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockState
{
    public Block Block {get;}
    public bool Enabled {get; set;}

    public BlockState(Block block)
    {
        Block = block;
        Enabled = true;
    }

    override public string ToString(){
        return Block.ToString();
    }
}
