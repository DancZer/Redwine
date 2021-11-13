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

        if(InvisibleShadowCasterMaterial != null){
            meshRenderer.materials = new Material[]{
                AtlasMaterial, 
                InvisibleShadowCasterMaterial
            };
        }else{
            meshRenderer.materials = new Material[]{
                AtlasMaterial
            };
        }
        

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

            if(InvisibleShadowCasterMaterial != null){
                BuildInvisibleShadowCasterMesh(mesh);
            }

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
            var belowBuilder = new BlockFaceBuilder(new Vector3(1, 0, 0), new Vector3(0, 0, 1));
            var aboveBuilder = new BlockFaceBuilder(new Vector3(0, 0, 1), new Vector3(1, 0, 0));

            for(int z=0; z<chunkSize.z; z++){
                //Chunk Below
                var blockPos = new Vector3Int(x, 0, z);
                var mainBlock = GetBlock(blockPos);

                if(mainBlock.IsSolid) {
                    belowBuilder.ExtendTo(blockPos);
                }else{
                    belowBuilder.Close();
                }
                
                //Chunk Above
                blockPos = new Vector3Int(x, chunkSize.y-1, z);
                mainBlock = GetBlock(blockPos);

                if(mainBlock.IsSolid) {
                    aboveBuilder.ExtendTo(blockPos+ new Vector3(0,1,0));
                }else{
                    aboveBuilder.Close();
                }
            }

            belowBuilder.Close();
            aboveBuilder.Close();

            verts.AddRange(belowBuilder.Verts);
            numFaces += belowBuilder.NumFaces;

            verts.AddRange(aboveBuilder.Verts);
            numFaces += aboveBuilder.NumFaces;
        }

        for(int x=0; x<chunkSize.x; x++){
            var southBuilder = new BlockFaceBuilder(new Vector3(0, 1, 0), new Vector3(1, 0, 0));
            var northBuilder = new BlockFaceBuilder(new Vector3(1, 0, 0), new Vector3(0, 1, 0));

            for(int y=0; y<chunkSize.y; y++){
                //Chunk South
                var blockPos = new Vector3Int(x, y, 0);
                var mainBlock = GetBlock(blockPos);

                if(mainBlock.IsSolid) {
                    southBuilder.ExtendTo(blockPos);
                }else{
                    southBuilder.Close();
                }
                
                //Chunk North
                blockPos = new Vector3Int(x, y, chunkSize.z-1);
                mainBlock = GetBlock(blockPos);

                if(mainBlock.IsSolid) {
                    northBuilder.ExtendTo(blockPos + new Vector3(0,0,1));
                }else{
                    northBuilder.Close();
                }
            }

            southBuilder.Close();
            northBuilder.Close();

            verts.AddRange(southBuilder.Verts);
            numFaces += southBuilder.NumFaces;

            verts.AddRange(northBuilder.Verts);
            numFaces += northBuilder.NumFaces;
        }

        for(int z=0; z<chunkSize.z; z++){
            var westBuilder = new BlockFaceBuilder(new Vector3(0, 0, 1), new Vector3(0, 1, 0));
            var eastBuilder = new BlockFaceBuilder(new Vector3(0, 1, 0), new Vector3(0, 0, 1));

            for(int y=0; y<chunkSize.y; y++){
                //Chunk West
                var blockPos = new Vector3Int(0, y, z);
                var mainBlock = GetBlock(blockPos);

                if(mainBlock.IsSolid) {
                    westBuilder.ExtendTo(blockPos);
                }else{
                    westBuilder.Close();
                }
                
                //Chunk East
                blockPos = new Vector3Int(chunkSize.x-1, y, z);
                mainBlock = GetBlock(blockPos);

                if(mainBlock.IsSolid) {
                    eastBuilder.ExtendTo(blockPos+ new Vector3(1,0,0));
                }else{
                    eastBuilder.Close();
                }
            }

            westBuilder.Close();
            eastBuilder.Close();

            verts.AddRange(westBuilder.Verts);
            numFaces += westBuilder.NumFaces;

            verts.AddRange(eastBuilder.Verts);
            numFaces += eastBuilder.NumFaces;
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
            verts.Add(new Vector3(0, 1, 0)+blockPos);
            verts.Add(new Vector3(0, 1, 1)+blockPos);
            verts.Add(new Vector3(1, 1, 1)+blockPos);
            verts.Add(new Vector3(1, 1, 0)+blockPos);

            ++numFaces;

            uvs?.AddRange(mainBlock.Top.GetUVs());
        }

        return numFaces;
    }

    private int BuildBelowSide(Vector3Int blockPos, List<Vector3> verts, int numFaces, Block mainBlock = null, List<Vector2> uvs = null)
    {
        var block = GetBlock(blockPos.Below());
        if(!block.IsSolid){
            verts.Add(new Vector3(0, 0, 0)+blockPos);
            verts.Add(new Vector3(1, 0, 0)+blockPos);
            verts.Add(new Vector3(1, 0, 1)+blockPos);
            verts.Add(new Vector3(0, 0, 1)+blockPos);
            
            ++numFaces;

            uvs?.AddRange(mainBlock.Bottom.GetUVs());
        }

        return numFaces;
    }

    private int BuildSouthSide(Vector3Int blockPos, List<Vector3> verts, int numFaces, Block mainBlock = null, List<Vector2> uvs = null)
    {
        var block = GetBlock(blockPos.South());
        if(!block.IsSolid){
            verts.Add(new Vector3(0, 0, 0)+blockPos);
            verts.Add(new Vector3(0, 1, 0)+blockPos);
            verts.Add(new Vector3(1, 1, 0)+blockPos);
            verts.Add(new Vector3(1, 0, 0)+blockPos);
            
            ++numFaces;

            uvs?.AddRange(mainBlock.Front.GetUVs());
        }

        return numFaces;
    }

    private int BuildNorthSide(Vector3Int blockPos, List<Vector3> verts, int numFaces, Block mainBlock = null, List<Vector2> uvs = null)
    {
        var block = GetBlock(blockPos.North());
        if(!block.IsSolid){
            verts.Add(new Vector3(1, 0, 1)+blockPos);
            verts.Add(new Vector3(1, 1, 1)+blockPos);
            verts.Add(new Vector3(0, 1, 1)+blockPos);
            verts.Add(new Vector3(0, 0, 1)+blockPos);
            
            ++numFaces;
            
            uvs?.AddRange(mainBlock.Front.GetUVs());
        }

        return numFaces;
    }


    private int BuildEastSide(Vector3Int blockPos, List<Vector3> verts, int numFaces, Block mainBlock = null, List<Vector2> uvs = null)
    {
        var block = GetBlock(blockPos.East());
        if(!block.IsSolid){
            verts.Add(new Vector3(1, 0, 0)+blockPos);
            verts.Add(new Vector3(1, 1, 0)+blockPos);
            verts.Add(new Vector3(1, 1, 1)+blockPos);
            verts.Add(new Vector3(1, 0, 1)+blockPos);

            ++numFaces;

            uvs?.AddRange(mainBlock.Side.GetUVs());
        }

        return numFaces;
    }

    private int BuildWestSide(Vector3Int blockPos, List<Vector3> verts, int numFaces, Block mainBlock = null, List<Vector2> uvs = null)
    {
        var block = GetBlock(blockPos.West());
        if(!block.IsSolid){
            verts.Add(new Vector3(0, 0, 1)+blockPos);
            verts.Add(new Vector3(0, 1, 1)+blockPos);
            verts.Add(new Vector3(0, 1, 0)+blockPos);
            verts.Add(new Vector3(0, 0, 0)+blockPos);

            ++numFaces;

            uvs?.AddRange(mainBlock.Side.GetUVs());
        }

        return numFaces;
    }

    private Block GetBlock(Vector3Int pos){
        var blockType = _chunk.GetBlockType(pos);

        return Blocks.blocks[blockType];
    }
}
