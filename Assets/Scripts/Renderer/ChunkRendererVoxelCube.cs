using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkRendererVoxelCube : MonoBehaviour
{
    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;
    private MeshCollider meshCollider;

    private float lastRenderedTime;
    private IChunkInterface _chunk;
    public IChunkInterface Chunk { set{
        _chunk = value;
        lastRenderedTime = 0;
    }}

    void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();
        meshCollider = GetComponent<MeshCollider>();
                
        var color = meshRenderer.material.color;
        color.a = 0;

        meshRenderer.material.SetColor("_Color", color);
    }

    private void OnDisable() {
        if(meshRenderer != null){
            meshRenderer.enabled = false;
        }
        if(meshCollider != null){
            meshCollider.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(lastRenderedTime == 0 || lastRenderedTime != _chunk.LastChangedTime){
            var mesh = BuildChunkMesh();
        
            meshFilter.mesh = mesh;
            meshCollider.sharedMesh = mesh;

            lastRenderedTime = _chunk.LastChangedTime;
            meshRenderer.enabled = meshCollider.enabled = true;
        }

        var color = meshRenderer.material.color;
        color.a = _chunk.Opacity;

        meshRenderer.material.SetColor("_Color", color);
    }
    
    private Mesh BuildChunkMesh()
    {
        var verts = new List<Vector3>();
        var uvs = new  List<Vector2>();

        //Debug.Log("BuildChunkMesh:"+lastRenderedTime+", "+_chunk.LastChangedTime);
        var numFaces = 0;
        var chunkSize = _chunk.Size;
        for(int x=0; x<chunkSize.x; x++){
            for(int z=0; z<chunkSize.z; z++){
                for(int y=0; y<chunkSize.y; y++){
                    var blockPos = new Vector3Int(x, y, z);

                    var mainBlock = GetBlock(blockPos);
                    if(!mainBlock.IsSolid) continue;

                    //Above
                    var block = GetBlock(blockPos.Above());
                    if(!block.IsSolid){
                        verts.Add(new Vector3(0, 1, 0)+blockPos);
                        verts.Add(new Vector3(0, 1, 1)+blockPos);
                        verts.Add(new Vector3(1, 1, 1)+blockPos);
                        verts.Add(new Vector3(1, 1, 0)+blockPos);
                        numFaces++;

                        uvs.AddRange(mainBlock.Top.GetUVs());
                    }
                    
                    //Below
                    block = GetBlock(blockPos.Below());
                    if(!block.IsSolid){
                        verts.Add(new Vector3(0, 0, 0)+blockPos);
                        verts.Add(new Vector3(1, 0, 0)+blockPos);
                        verts.Add(new Vector3(1, 0, 1)+blockPos);
                        verts.Add(new Vector3(0, 0, 1)+blockPos);
                        numFaces++;

                        uvs.AddRange(mainBlock.Bottom.GetUVs());
                    }

                    //South
                    block = GetBlock(blockPos.South());
                    if(!block.IsSolid){
                        verts.Add(new Vector3(0, 0, 0)+blockPos);
                        verts.Add(new Vector3(0, 1, 0)+blockPos);
                        verts.Add(new Vector3(1, 1, 0)+blockPos);
                        verts.Add(new Vector3(1, 0, 0)+blockPos);
                        numFaces++;

                        uvs.AddRange(mainBlock.Front.GetUVs());
                    }

                    //East
                    block = GetBlock(blockPos.East());
                    if(!block.IsSolid){
                        verts.Add(new Vector3(1, 0, 0)+blockPos);
                        verts.Add(new Vector3(1, 1, 0)+blockPos);
                        verts.Add(new Vector3(1, 1, 1)+blockPos);
                        verts.Add(new Vector3(1, 0, 1)+blockPos);
                        numFaces++;

                        uvs.AddRange(mainBlock.Side.GetUVs());
                    }

                    //North
                    block = GetBlock(blockPos.North());
                    if(!block.IsSolid){
                        verts.Add(new Vector3(1, 0, 1)+blockPos);
                        verts.Add(new Vector3(1, 1, 1)+blockPos);
                        verts.Add(new Vector3(0, 1, 1)+blockPos);
                        verts.Add(new Vector3(0, 0, 1)+blockPos);
                        numFaces++;

                        uvs.AddRange(mainBlock.Front.GetUVs());
                    }

                    //West
                    block = GetBlock(blockPos.West());
                    if(!block.IsSolid){
                        verts.Add(new Vector3(0, 0, 1)+blockPos);
                        verts.Add(new Vector3(0, 1, 1)+blockPos);
                        verts.Add(new Vector3(0, 1, 0)+blockPos);
                        verts.Add(new Vector3(0, 0, 0)+blockPos);
                        numFaces++;

                        uvs.AddRange(mainBlock.Side.GetUVs());
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
        var blockType = _chunk.GetBlockType(pos);

        return Blocks.blocks[blockType];
    }
}
