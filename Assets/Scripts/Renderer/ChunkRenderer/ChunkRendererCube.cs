using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkRendererCube : MonoBehaviour
{
    public Material Material;

    private ChunkRendererInterface chunkRenderer;

    private Chunk chunk;

    private GameObject[,,] cubes;

    void Start()
    {
        chunkRenderer = GetComponent<ChunkRendererInterface>();

        cubes = new GameObject[Config.CHUNK_SIZE, Config.CHUNK_SIZE, Config.CHUNK_SIZE];
        for(int x=0;x<Config.CHUNK_SIZE;x++){
            for(int z=0;z<Config.CHUNK_SIZE;z++){
                for(int y=0;y<Config.CHUNK_SIZE;y++){
                    var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    cube.SetActive(false);
                    cube.transform.SetParent(this.transform);
                    cube.transform.localScale = new Vector3(1,1,1);
                    cube.transform.localPosition = new Vector3(x+0.5f,y+0.5f,z+0.5f);
                    
                    var renderer = cube.GetComponent<MeshRenderer>();
                    renderer.material = Material;
                    cubes[x,y,z] = cube;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(chunkRenderer.shouldRender){
            Render();
            chunkRenderer.shouldRender = false;
        }
    }

    private void Render(){
        chunk = chunkRenderer.chunk;

        //Debug.Log("Render:"+chunk);

        for(int x=0;x<Config.CHUNK_SIZE;x++){
            for(int z=0;z<Config.CHUNK_SIZE;z++){
                for(int y=0;y<Config.CHUNK_SIZE;y++){
                    var cube = cubes[x,y,z];
                    cube.SetActive(false);

                    var blockPos = new BlockPos(x,y,z);

                    var block = ShouldRenderCubeAt(blockPos);
                    //Debug.Log("block:"+block+" "+blockPos);
                    if(block == null) continue;
                    
                    var blockMat = block.Side;
                    var renderer = cube.GetComponent<MeshRenderer>();
                    renderer.material.SetTextureOffset("_MainTex", blockMat.GetOffset());
                    renderer.material.SetTextureScale("_MainTex", blockMat.GetScale());
                    cube.SetActive(true);
                }
            }
        }
    }
    
    private Block ShouldRenderCubeAt(BlockPos pos){
        var block = GetBlock(pos);

        if(block.IsAir || 
            IsEnabledSolid(GetBlock(pos.Below())) &&
            IsEnabledSolid(GetBlock(pos.Above())) &&
            IsEnabledSolid(GetBlock(pos.North())) &&
            IsEnabledSolid(GetBlock(pos.South())) &&
            IsEnabledSolid(GetBlock(pos.West())) &&
            IsEnabledSolid(GetBlock(pos.East()))) return null; 

        return block;
    }

    private Block GetBlock(BlockPos pos){
        var blockType = chunk.GetBlockState(pos);

        return Blocks.blocks[blockType];
    }

    private bool IsEnabledSolid(Block block){
        return block.IsSolid;
    }
}
