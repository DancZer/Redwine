using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldRenderer : MonoBehaviour
{
    private World world;

    private Mesh mesh;
    private List<Vector3> verts;
    private List<int> tris;
    private List<Vector2> uvs;
    private int numFaces;

    // Start is called before the first frame update
    void Start()
    {
        world = new World();
        world.Init();

        BuildMesh();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void BuildMesh(){
        mesh = new Mesh();

        verts = new List<Vector3>();
        tris = new List<int>();
        uvs = new List<Vector2>();

        numFaces = 0;

         for(int x=0;x<World.SIZE;x++){
            for(int z=0;z<World.SIZE;z++){
                for(int y=0;y<World.SIZE;y++){
                    BuildChunkMesh(world.Chunks[x,y,z]);
                }
            }
         }

        int tl = verts.Count - 4 * numFaces;
        for(int i = 0; i < numFaces; i++)
        {
            tris.AddRange(new int[] { tl + i * 4, tl + i * 4 + 1, tl + i * 4 + 2, tl + i * 4, tl + i * 4 + 2, tl + i * 4 + 3 });
            //uvs.AddRange(Block.blocks[BlockType.Grass].topPos.GetUVs());
        }


        mesh.vertices = verts.ToArray();
        mesh.triangles = tris.ToArray();
        mesh.uv = uvs.ToArray();

        mesh.RecalculateNormals();

        GetComponent<MeshFilter>().mesh = mesh;
        GetComponent<MeshCollider>().sharedMesh = mesh;
    }

    public void BuildChunkMesh(Chunk chunk){
        //Debug.Log("BuildChunkMesh:"+chunk.BlockPos);

        var chunkWorldPosStart = chunk.BlockPos;
        var chunkWorldPosEnd = chunk.BlockPos + Chunk.SIZE;

        for(int x=chunkWorldPosStart.X;x<chunkWorldPosEnd.X;x++){
            for(int z=chunkWorldPosStart.Z;z<chunkWorldPosEnd.Z;z++){
                for(int y=chunkWorldPosStart.Y;y<chunkWorldPosEnd.Y;y++){
                    BlockPos blockPos = new BlockPos(x, y, z);

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
    }
}
