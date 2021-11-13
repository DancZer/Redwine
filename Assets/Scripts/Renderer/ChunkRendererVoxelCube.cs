using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkRendererVoxelCube : MonoBehaviour
{
    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;
    private MeshCollider meshCollider;

    public Material AtlasMaterial;
    public Material InvisibleShadowCasterMaterial;

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

        meshRenderer.materials = new Material[]{
            AtlasMaterial, 
            InvisibleShadowCasterMaterial
        };

        UpdateColor();
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
            var mesh = new Mesh();
            mesh.subMeshCount = 2;

            BuildAtlasMesh(mesh);
            BuildInvisibleShadowCasterMesh(mesh); //In some cases few triangles are not rendered

            mesh.Optimize();
            mesh.RecalculateNormals();
        
            meshFilter.mesh = mesh;
            meshCollider.sharedMesh = mesh;

            lastRenderedTime = _chunk.LastChangedTime;
            meshRenderer.enabled = meshCollider.enabled = true;
        }

        UpdateColor();
    }

    private void UpdateColor(){
        var color = meshRenderer.material.color;
        color.a = _chunk == null ? 0 : _chunk.Opacity;

        meshRenderer.material.SetColor("_Color", color);
    }
    
    private void BuildAtlasMesh(Mesh mesh)
    {
        var verts = new List<Vector3>();
        var uvs = new  List<Vector2>();

        mesh.GetVertices(verts);
        mesh.GetUVs(0, uvs);

        var numFaces = 0;
        var chunkSize = _chunk.Size;
        for(int x=0; x<chunkSize.x; x++){
            for(int z=0; z<chunkSize.z; z++){
                for(int y=0; y<chunkSize.y; y++){
                    var blockPos = new Vector3Int(x, y, z);

                    var mainBlock = GetBlock(blockPos);
                    if(!mainBlock.IsSolid) continue;

                    numFaces = BuildAboveSide(blockPos, verts, numFaces, mainBlock, uvs);
                    numFaces = BuildBelowSide(blockPos, verts, numFaces, mainBlock, uvs);
                    numFaces = BuildSouthSide(blockPos, verts, numFaces, mainBlock, uvs);
                    numFaces = BuildNorthSide(blockPos, verts, numFaces, mainBlock, uvs);
                    numFaces = BuildEastSide(blockPos, verts, numFaces, mainBlock, uvs);
                    numFaces = BuildWestSide(blockPos, verts, numFaces, mainBlock, uvs);
                }
            }
        }

        var tris = new List<int>();
        int tl = verts.Count - 4 * numFaces;
        for(int i = 0; i < numFaces; i++)
        {
            tris.AddRange(new int[] { tl + i * 4, tl + i * 4 + 1, tl + i * 4 + 2, tl + i * 4, tl + i * 4 + 2, tl + i * 4 + 3 });
        }
        
        mesh.SetVertices(verts);
        mesh.SetTriangles(tris, 0);
        mesh.SetUVs(0, uvs);
    }

    private void BuildInvisibleShadowCasterMesh(Mesh mesh)
    {
        var verts = new List<Vector3>();

        mesh.GetVertices(verts);

        var numFaces = 0;
        var chunkSize = _chunk.Size;

        for(int x=0; x<chunkSize.x; x++){
            for(int z=0; z<chunkSize.z; z++){
                //Chunk Bottom
                var blockPos = new Vector3Int(x, 0, z);

                var mainBlock = GetBlock(blockPos);
                if(!mainBlock.IsSolid) continue;

                numFaces = AddBelowSide(blockPos, verts, numFaces);
                
                //Chunk Top
                blockPos = new Vector3Int(x, chunkSize.y-1, z);

                mainBlock = GetBlock(blockPos);
                if(!mainBlock.IsSolid) continue;

                numFaces = AddAboveSide(blockPos, verts, numFaces);
            }
        }

        for(int x=0; x<chunkSize.x; x++){
            for(int y=0; y<chunkSize.y; y++){
                //Chunk South
                var blockPos = new Vector3Int(x, y, 0);

                var mainBlock = GetBlock(blockPos);
                if(!mainBlock.IsSolid) continue;

                numFaces = AddSouthSide(blockPos, verts, numFaces);

                //Chunk North
                blockPos = new Vector3Int(x, y, chunkSize.z-1);

                mainBlock = GetBlock(blockPos);
                if(!mainBlock.IsSolid) continue;

                numFaces = AddNorthSide(blockPos, verts, numFaces);
            }
        }

        for(int z=0; z<chunkSize.z; z++){
            for(int y=0; y<chunkSize.y; y++){
                //Chunk West
                var blockPos = new Vector3Int(0, y, z);

                var mainBlock = GetBlock(blockPos);
                if(!mainBlock.IsSolid) continue;

                numFaces = AddWestSide(blockPos, verts, numFaces);

                //Chunk East
                blockPos = new Vector3Int(chunkSize.x-1, y, z);

                mainBlock = GetBlock(blockPos);
                if(!mainBlock.IsSolid) continue;

                numFaces = AddEastSide(blockPos, verts, numFaces);
            }
        }

        var tris = new List<int>();
        int tl = verts.Count - 4 * numFaces;
        for(int i = 0; i < numFaces; i++)
        {
            tris.AddRange(new int[] { tl + i * 4, tl + i * 4 + 1, tl + i * 4 + 2, tl + i * 4, tl + i * 4 + 2, tl + i * 4 + 3 });
        }
        
        mesh.SetVertices(verts);
        mesh.SetTriangles(tris, 1);
    }

    private int BuildAboveSide(Vector3Int blockPos, List<Vector3> verts, int numFaces, Block mainBlock = null, List<Vector2> uvs = null)
    {
        var block = GetBlock(blockPos.Above());
        if(!block.IsSolid){
            numFaces = AddAboveSide(blockPos, verts, numFaces);

            uvs?.AddRange(mainBlock.Top.GetUVs());
        }

        return numFaces;
    }

    private int AddAboveSide(Vector3Int blockPos, List<Vector3> verts, int numFaces){
        verts.Add(new Vector3(0, 1, 0)+blockPos);
        verts.Add(new Vector3(0, 1, 1)+blockPos);
        verts.Add(new Vector3(1, 1, 1)+blockPos);
        verts.Add(new Vector3(1, 1, 0)+blockPos);

        return ++numFaces;
    }

    private int BuildBelowSide(Vector3Int blockPos, List<Vector3> verts, int numFaces, Block mainBlock = null, List<Vector2> uvs = null)
    {
        var block = GetBlock(blockPos.Below());
        if(!block.IsSolid){
            numFaces = AddBelowSide(blockPos, verts, numFaces);

            uvs?.AddRange(mainBlock.Bottom.GetUVs());
        }

        return numFaces;
    }
    private int AddBelowSide(Vector3Int blockPos, List<Vector3> verts, int numFaces){
        verts.Add(new Vector3(0, 0, 0)+blockPos);
        verts.Add(new Vector3(1, 0, 0)+blockPos);
        verts.Add(new Vector3(1, 0, 1)+blockPos);
        verts.Add(new Vector3(0, 0, 1)+blockPos);
        
        return ++numFaces;
    }

    private int BuildSouthSide(Vector3Int blockPos, List<Vector3> verts, int numFaces, Block mainBlock = null, List<Vector2> uvs = null)
    {
        var block = GetBlock(blockPos.South());
        if(!block.IsSolid){
            numFaces = AddSouthSide(blockPos, verts, numFaces);

            uvs?.AddRange(mainBlock.Front.GetUVs());
        }

        return numFaces;
    }

    private int AddSouthSide(Vector3Int blockPos, List<Vector3> verts, int numFaces){
        verts.Add(new Vector3(0, 0, 0)+blockPos);
        verts.Add(new Vector3(0, 1, 0)+blockPos);
        verts.Add(new Vector3(1, 1, 0)+blockPos);
        verts.Add(new Vector3(1, 0, 0)+blockPos);
        
        return ++numFaces;
    }

    private int BuildNorthSide(Vector3Int blockPos, List<Vector3> verts, int numFaces, Block mainBlock = null, List<Vector2> uvs = null)
    {
        var block = GetBlock(blockPos.North());
        if(!block.IsSolid){
            numFaces = AddNorthSide(blockPos, verts, numFaces);
            
            uvs?.AddRange(mainBlock.Front.GetUVs());
        }

        return numFaces;
    }

    private int AddNorthSide(Vector3Int blockPos, List<Vector3> verts, int numFaces){
        verts.Add(new Vector3(1, 0, 1)+blockPos);
        verts.Add(new Vector3(1, 1, 1)+blockPos);
        verts.Add(new Vector3(0, 1, 1)+blockPos);
        verts.Add(new Vector3(0, 0, 1)+blockPos);
        
        return ++numFaces;
    }

    private int BuildEastSide(Vector3Int blockPos, List<Vector3> verts, int numFaces, Block mainBlock = null, List<Vector2> uvs = null)
    {
        var block = GetBlock(blockPos.East());
        if(!block.IsSolid){
            numFaces = AddEastSide(blockPos, verts, numFaces);
            
            uvs?.AddRange(mainBlock.Side.GetUVs());
        }

        return numFaces;
    }
    private int AddEastSide(Vector3Int blockPos, List<Vector3> verts, int numFaces){
        verts.Add(new Vector3(1, 0, 0)+blockPos);
        verts.Add(new Vector3(1, 1, 0)+blockPos);
        verts.Add(new Vector3(1, 1, 1)+blockPos);
        verts.Add(new Vector3(1, 0, 1)+blockPos);
        
        return ++numFaces;
    }

    private int BuildWestSide(Vector3Int blockPos, List<Vector3> verts, int numFaces, Block mainBlock = null, List<Vector2> uvs = null)
    {
        var block = GetBlock(blockPos.West());
        if(!block.IsSolid){
            numFaces = AddWestSide(blockPos, verts, numFaces);

            uvs?.AddRange(mainBlock.Side.GetUVs());
        }

        return numFaces;
    }
    private int AddWestSide(Vector3Int blockPos, List<Vector3> verts, int numFaces){
        verts.Add(new Vector3(0, 0, 1)+blockPos);
        verts.Add(new Vector3(0, 1, 1)+blockPos);
        verts.Add(new Vector3(0, 1, 0)+blockPos);
        verts.Add(new Vector3(0, 0, 0)+blockPos);
        
        return ++numFaces;
    }

    private Block GetBlock(Vector3Int pos){
        var blockType = _chunk.GetBlockType(pos);

        return Blocks.blocks[blockType];
    }
}
