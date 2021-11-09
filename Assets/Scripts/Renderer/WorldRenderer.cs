using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class WorldRenderer : MonoBehaviour
{
    private readonly Dictionary<Vector3Int, VoxelRenderer> activeChunkRenderers = new Dictionary<Vector3Int, VoxelRenderer>();
    private readonly Queue<VoxelRenderer> notActiveChunkRenderers = new Queue<VoxelRenderer>();
    private readonly List<Vector3Int> renderChunkPosQueue = new List<Vector3Int>();
    private Vector3Int centerChunkPos;
    public Transform PlayerPos;

    // Start is called before the first frame update
    void Start()
    {
        World.Init();

        var startPos = World.GetStartPos() + new Vector3(0.5f, 0, 0.5f);
        
        //Debug.Log("Start Player startPos:"+startPos);

        centerChunkPos = Vector3Int.CeilToInt(startPos).ToChunkAligned();
        //Debug.Log("Start centerChunkPos:"+centerChunkPos);

        PreLoadPlayerLevel();

        PlayerPos.position = startPos;

        RenderChunks();
    }

    private void PreLoadPlayerLevel(){
        var boundary = GetViewDistanceBoundary();
        var from = boundary[0];
        var to = boundary[1];

        for(int y=from.y; y<to.y; y+=Config.ChunkSize){
            for(int x=from.x; x<to.x; x+=Config.ChunkSize){
                for(int z=from.z; z<to.z; z+=Config.ChunkSize){
                    var pos = new Vector3Int(x,y,z).ToChunkAligned();

                    LoadAndRenderNewChunk(pos);  
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        var playerPos = Vector3Int.FloorToInt(PlayerPos.position);

        if(UpdateCenterPos(playerPos)){
            //Debug.Log("CenterChunkPos changed:"+playerPos);
            RenderChunks();
        }
    }

    public bool UpdateCenterPos(Vector3Int playerPos)
    {
        var chunkPos = playerPos.ToChunkAligned();

        if(Vector3.Distance(chunkPos, centerChunkPos) >= Config.ChunkSize){
            centerChunkPos = chunkPos;

            return true;
        }

        return false;
    }

    private Vector3Int[] GetViewDistanceBoundary(){
        return new Vector3Int[]{
            centerChunkPos - new Vector3Int(Config.ViewDistanceBlockCount, Config.ViewDistanceBlockCount, Config.ViewDistanceBlockCount),
            centerChunkPos + new Vector3Int(Config.ViewDistanceBlockCount, Config.ViewDistanceBlockCount, Config.ViewDistanceBlockCount) + new Vector3Int(Config.ChunkSize, Config.ChunkSize, Config.ChunkSize)
        };
    }

    private void RenderChunks()
    {
        StopCoroutine(DelayBuildChunks());

        var notUsedChunkRenderersKey = new HashSet<Vector3Int>(activeChunkRenderers.Keys);
        
        renderChunkPosQueue.Clear();

        var boundary = GetViewDistanceBoundary();
        var from = boundary[0];
        var to = boundary[1];

        for(int y=from.y; y<to.y; y+=Config.ChunkSize){
            for(int x=from.x; x<to.x; x+=Config.ChunkSize){
                for(int z=from.z; z<to.z; z+=Config.ChunkSize){
                    var pos = new Vector3Int(x,y,z).ToChunkAligned();
                    
                    //already rendered
                    if(activeChunkRenderers.ContainsKey(pos)){
                        notUsedChunkRenderersKey.Remove(pos);
                    }else{
                        renderChunkPosQueue.Add(pos);
                    }
                }
            }
        }

        //deactivate not used objects
        foreach (var pos in notUsedChunkRenderersKey)
        {
            var obj = activeChunkRenderers[pos];

            activeChunkRenderers.Remove(pos);
            notActiveChunkRenderers.Enqueue(obj);
        }

        if(renderChunkPosQueue.Contains(centerChunkPos)){
            LoadAndRenderNewChunk(centerChunkPos);
        }

        StartCoroutine(DelayBuildChunks());
    }

    public IEnumerator DelayBuildChunks()
    {
        while(renderChunkPosQueue.Count > 0)
        {
            var pos = renderChunkPosQueue.OrderBy(p => Vector3Int.Distance(p, centerChunkPos)).First();
            renderChunkPosQueue.Remove(pos);
            LoadAndRenderNewChunk(pos);

            yield return new WaitForSeconds(.05f);
        }
    }

    private void LoadAndRenderNewChunk(Vector3Int pos)
    {
        if(activeChunkRenderers.ContainsKey(pos)){
            return;
        }

        VoxelRenderer renderer;
        if(notActiveChunkRenderers.Count == 0){
            renderer = new VoxelRenderer();       
        }else{
            renderer = notActiveChunkRenderers.Dequeue();
        }

        var chunk = World.GetChunk(pos);
        renderer.Chunk = chunk;

        activeChunkRenderers.Add(chunk.Pos, renderer);

        UpdateCombinedMesh();
    }

    private void UpdateCombinedMesh(){
        
        CombineInstance[] combine = new CombineInstance[activeChunkRenderers.Count];

        Debug.Log("UpdateCombinedMesh:"+combine.Length);

        var i=0;
        foreach (var pos in activeChunkRenderers.Keys)
        {
            var obj = activeChunkRenderers[pos];

            combine[i].mesh = obj.BuildMesh();

            var tran = new Matrix4x4();
            tran.SetTRS(pos, transform.rotation, transform.lossyScale);
            combine[i].transform = tran;

            ++i;
        }

        var mesh = new Mesh();
        mesh.CombineMeshes(combine);

        GetComponent<MeshFilter>().mesh = mesh;
        GetComponent<MeshCollider>().sharedMesh = mesh;
    }
}