using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkRendererCube : MonoBehaviour
{
    public Material Material;

    private GameObject[,,] Cubes;

    void Start()
    {
        if(Cubes == null){
            Cubes = new GameObject[Chunk.SIZE, Chunk.SIZE, Chunk.SIZE];
            for(int x=0;x<Chunk.SIZE;x++){
                for(int z=0;z<Chunk.SIZE;z++){
                    for(int y=0;y<Chunk.SIZE;y++){
                        var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        cube.SetActive(false);
                        cube.transform.SetParent(this.transform);
                        cube.transform.localScale = new Vector3(1,1,1);
                        cube.transform.localPosition = new Vector3(x+0.5f,y+0.5f,z+0.5f);
                        
                        var renderer = cube.GetComponent<MeshRenderer>();
                        renderer.material = Material;
                        Cubes[x,y,z] = cube;
                    }
                }
            }
        }

        Render();
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnEnable() {
        if(Cubes != null){
            Render();
        }
    }

    private void Render(){
        var chunkRenderer = GetComponent<ChunkRendererInterface>();
        
        var chunk = chunkRenderer.Chunk;

        for(int x=0;x<Chunk.SIZE;x++){
            for(int z=0;z<Chunk.SIZE;z++){
                for(int y=0;y<Chunk.SIZE;y++){
                    var cube = Cubes[x,y,z];
                    cube.SetActive(false);

                    var blockPos = chunk.Pos+new BlockPos(x,y,z);

                    var blockState = ShouldRenderCubeAt(blockPos);
                    if(blockState == null) continue;
                    
                    var blockMat = blockState.Block.Material;
                    var renderer = cube.GetComponent<MeshRenderer>();
                    renderer.material.SetTextureOffset("_MainTex", blockMat.GetOffset());
                    renderer.material.SetTextureScale("_MainTex", blockMat.GetScale());
                    cube.SetActive(true);
                }
            }
        }
        
    }

    private BlockState ShouldRenderCubeAt(BlockPos pos){
        var blockState = World.Instance().GetBlockStates(pos);

        if(!blockState.Enabled ||
            blockState.Block.IsAir || 
            IsEnabledSolid(World.Instance().GetBlockStates(pos.Below())) &&
            IsEnabledSolid(World.Instance().GetBlockStates(pos.Above())) &&
            IsEnabledSolid(World.Instance().GetBlockStates(pos.North())) &&
            IsEnabledSolid(World.Instance().GetBlockStates(pos.South())) &&
            IsEnabledSolid(World.Instance().GetBlockStates(pos.West())) &&
            IsEnabledSolid(World.Instance().GetBlockStates(pos.East()))) return null; 

        return blockState;
    }

    private bool IsEnabledSolid(BlockState blockState){
        return blockState.Enabled && blockState.Block.IsSolid;
    }
}
