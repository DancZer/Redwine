using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkRendererVoxelCube : MonoBehaviour
{
    private ChunkRendererInterface chunkRenderer;

    private Chunk chunk;

    void Start()
    {
        chunkRenderer = GetComponent<ChunkRendererInterface>();
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

        var mesh = BuildChunkMesh();
        
        GetComponent<MeshFilter>().mesh = mesh;
        GetComponent<MeshCollider>().sharedMesh = mesh;
    }
    
    private Mesh BuildChunkMesh()
    {
        var verts = new List<Vector3>();
        var uvs = new  List<Vector2>();

        //Debug.Log("BuildChunkMesh:"+chunk.BlockPos);
        var numFaces = 0;
        for(int x=-1; x<Chunk.Size; x++){
            for(int z=-1; z<Chunk.Size; z++){
                for(int y=-1; y<Chunk.Size; y++){
                    var blockPos = new Vector3Int(x, y, z);

                    var block = GetBlock(blockPos);
                    if(!block.IsSolid) continue;

                    //Above
                    block = GetBlock(blockPos.Above());
                    if(!block.IsSolid){
                        verts.Add(new Vector3(0, 1, 0)+blockPos);
                        verts.Add(new Vector3(0, 1, 1)+blockPos);
                        verts.Add(new Vector3(1, 1, 1)+blockPos);
                        verts.Add(new Vector3(1, 1, 0)+blockPos);
                        numFaces++;

                        uvs.AddRange(block.Top.GetUVs());
                    }
                    
                    //Below
                    block = GetBlock(blockPos.Below());
                    if(!block.IsSolid){
                        verts.Add(new Vector3(0, 0, 0)+blockPos);
                        verts.Add(new Vector3(1, 0, 0)+blockPos);
                        verts.Add(new Vector3(1, 0, 1)+blockPos);
                        verts.Add(new Vector3(0, 0, 1)+blockPos);
                        numFaces++;

                        uvs.AddRange(block.Bottom.GetUVs());
                    }

                    //South
                    block = GetBlock(blockPos.South());
                    if(!block.IsSolid){
                        verts.Add(new Vector3(0, 0, 0)+blockPos);
                        verts.Add(new Vector3(0, 1, 0)+blockPos);
                        verts.Add(new Vector3(1, 1, 0)+blockPos);
                        verts.Add(new Vector3(1, 0, 0)+blockPos);
                        numFaces++;

                        uvs.AddRange(block.Side.GetUVs());
                    }

                    //East
                    block = GetBlock(blockPos.East());
                    if(!block.IsSolid){
                        verts.Add(new Vector3(1, 0, 0)+blockPos);
                        verts.Add(new Vector3(1, 1, 0)+blockPos);
                        verts.Add(new Vector3(1, 1, 1)+blockPos);
                        verts.Add(new Vector3(1, 0, 1)+blockPos);
                        numFaces++;

                        uvs.AddRange(block.Side.GetUVs());
                    }

                    //North
                    block = GetBlock(blockPos.North());
                    if(!block.IsSolid){
                        verts.Add(new Vector3(1, 0, 1)+blockPos);
                        verts.Add(new Vector3(1, 1, 1)+blockPos);
                        verts.Add(new Vector3(0, 1, 1)+blockPos);
                        verts.Add(new Vector3(0, 0, 1)+blockPos);
                        numFaces++;

                        uvs.AddRange(block.Side.GetUVs());
                    }

                    //West
                    block = GetBlock(blockPos.West());
                    if(!block.IsSolid){
                        verts.Add(new Vector3(0, 0, 1)+blockPos);
                        verts.Add(new Vector3(0, 1, 1)+blockPos);
                        verts.Add(new Vector3(0, 1, 0)+blockPos);
                        verts.Add(new Vector3(0, 0, 0)+blockPos);
                        numFaces++;

                        uvs.AddRange(block.Side.GetUVs());
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

    private Block GetBlock(Vector3Int pos){
        var blockType = chunk.GetBlockState(pos);

        return Blocks.blocks[blockType];
    }
}
