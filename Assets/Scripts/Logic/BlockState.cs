using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockState
{
    public Block Block {get;}

    public BlockState(Block block)
    {
        Block = block;
    }

    override public string ToString(){
        return Block.ToString();
    }
}
