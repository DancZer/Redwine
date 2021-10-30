using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoxelCubeRenderer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Render();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void Render(){
        var chunkRenderer = GetComponent<ChunkRendererInterface>();
        var mesh = BuildChunkMesh(chunkRenderer.Chunk, chunkRenderer.World);
        
        GetComponent<MeshFilter>().mesh = mesh;
        GetComponent<MeshCollider>().sharedMesh = mesh;
    }
    
    private Mesh BuildChunkMesh(Chunk chunk, World world)
    {
        var verts = new List<Vector3>();
        var uvs = new  List<Vector2>();

        //Debug.Log("BuildChunkMesh:"+chunk.BlockPos);
        var numFaces = 0;
        for(int x=0;x<Chunk.SIZE;x++){
            for(int z=0;z<Chunk.SIZE;z++){
                for(int y=0;y<Chunk.SIZE;y++){
                    BlockPos blockPos = chunk.Pos+new BlockPos(x, y, z);

                    BlockState blockState = world.GetBlockStates(blockPos);
                    if(!blockState.Block.IsSolid) continue;

                    //Above
                    if(!world.GetBlockStates(blockPos.Above()).Block.IsSolid){
                        verts.Add(new Vector3(0, 1, 0)+blockPos);
                        verts.Add(new Vector3(0, 1, 1)+blockPos);
                        verts.Add(new Vector3(1, 1, 1)+blockPos);
                        verts.Add(new Vector3(1, 1, 0)+blockPos);
                        numFaces++;

                        uvs.AddRange(blockState.Block.Material.GetUVs());
                    }
                    
                    //Below
                    if(!world.GetBlockStates(blockPos.Below()).Block.IsSolid){
                        verts.Add(new Vector3(0, 0, 0)+blockPos);
                        verts.Add(new Vector3(1, 0, 0)+blockPos);
                        verts.Add(new Vector3(1, 0, 1)+blockPos);
                        verts.Add(new Vector3(0, 0, 1)+blockPos);
                        numFaces++;

                        uvs.AddRange(blockState.Block.Material.GetUVs());
                    }

                    //South
                    if(!world.GetBlockStates(blockPos.South()).Block.IsSolid){
                        verts.Add(new Vector3(0, 0, 0)+blockPos);
                        verts.Add(new Vector3(0, 1, 0)+blockPos);
                        verts.Add(new Vector3(1, 1, 0)+blockPos);
                        verts.Add(new Vector3(1, 0, 0)+blockPos);
                        numFaces++;

                        uvs.AddRange(blockState.Block.Material.GetUVs());
                    }

                    //East
                    if(!world.GetBlockStates(blockPos.East()).Block.IsSolid){
                        verts.Add(new Vector3(1, 0, 0)+blockPos);
                        verts.Add(new Vector3(1, 1, 0)+blockPos);
                        verts.Add(new Vector3(1, 1, 1)+blockPos);
                        verts.Add(new Vector3(1, 0, 1)+blockPos);
                        numFaces++;

                        uvs.AddRange(blockState.Block.Material.GetUVs());
                    }

                    //North
                    if(!world.GetBlockStates(blockPos.North()).Block.IsSolid){
                        verts.Add(new Vector3(1, 0, 1)+blockPos);
                        verts.Add(new Vector3(1, 1, 1)+blockPos);
                        verts.Add(new Vector3(0, 1, 1)+blockPos);
                        verts.Add(new Vector3(0, 0, 1)+blockPos);
                        numFaces++;

                        uvs.AddRange(blockState.Block.Material.GetUVs());
                    }

                    //West
                    if(!world.GetBlockStates(blockPos.West()).Block.IsSolid){
                        verts.Add(new Vector3(0, 0, 1)+blockPos);
                        verts.Add(new Vector3(0, 1, 1)+blockPos);
                        verts.Add(new Vector3(0, 1, 0)+blockPos);
                        verts.Add(new Vector3(0, 0, 0)+blockPos);
                        numFaces++;

                        uvs.AddRange(blockState.Block.Material.GetUVs());
                    }
                }
            }
        }

        var tris = new List<int>();
        int tl = verts.Count - 4 * numFaces;
        for(int i = 0; i < numFaces; i++)
        {
            tris.AddRange(new int[] { tl + i * 4, tl + i * 4 + 1, tl + i * 4 + 2, tl + i * 4, tl + i * 4 + 2, tl + i * 4 + 3 });
            //uvs.AddRange(Block.blocks[BlockType.Grass].topPos.GetUVs());
        }

        var mesh = new Mesh();
        mesh.vertices = verts.ToArray();
        mesh.triangles = tris.ToArray();
        mesh.uv = uvs.ToArray();

        mesh.RecalculateNormals();

        return mesh;
    }
}
