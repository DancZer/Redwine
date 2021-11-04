using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class WorldRenderer : MonoBehaviour
{
    private const int ViewDistanceBlockCount = Config.ViewDistanceChunkCount*Config.ChunkSize;
    private readonly World world = new World();
    private readonly Dictionary<Vector3Int, GameObject> activeChunkRenderers = new Dictionary<Vector3Int, GameObject>();
    private readonly Queue<GameObject> notActiveChunkRenderers = new Queue<GameObject>();
    private readonly List<Vector3Int> renderChunkPosQueue = new List<Vector3Int>();
    private Vector3Int centerChunkPos;

    public Component Player;
    public GameObject ChunkRendererPrefab;


    // Start is called before the first frame update
    void Start()
    {        
        world.Init();

        var startPos = world.GetStartPos() + new Vector3(0.5f, 0, 0.5f);
        
        //Debug.Log("Start Player startPos:"+startPos);

        centerChunkPos = Vector3Int.CeilToInt(startPos).ToChunkAligned();
        //Debug.Log("Start centerChunkPos:"+centerChunkPos);

        PreLoadPlayerLevel();

        Player.transform.position = startPos;

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
        var playerPos = Vector3Int.FloorToInt(Player.transform.position);

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
            centerChunkPos - new Vector3Int(ViewDistanceBlockCount, ViewDistanceBlockCount, ViewDistanceBlockCount),
            centerChunkPos + new Vector3Int(ViewDistanceBlockCount, ViewDistanceBlockCount, ViewDistanceBlockCount) + new Vector3Int(Config.ChunkSize, Config.ChunkSize, Config.ChunkSize)
        };
    }

    private void RenderChunks()
    {
        StopCoroutine(DelayBuildChunks());

        //Debug.Log("RenderChunks start!");

        var notUsedChunkRenderersKey = new HashSet<Vector3Int>(activeChunkRenderers.Keys);
        
        renderChunkPosQueue.Clear();

        var boundary = GetViewDistanceBoundary();
        var from = boundary[0];
        var to = boundary[1];

        foreach (var pos in notUsedChunkRenderersKey)
        {
            //Debug.Log("activeChunkRenderers:"+pos);
        }
        
        for(int y=from.y; y<to.y; y+=Config.ChunkSize){
            for(int x=from.x; x<to.x; x+=Config.ChunkSize){
                for(int z=from.z; z<to.z; z+=Config.ChunkSize){
                    var pos = new Vector3Int(x,y,z).ToChunkAligned();
                    
                    //already rendered
                    if(activeChunkRenderers.ContainsKey(pos)){
                        //Debug.Log("RenderChunk:"+pos+" visible!");
                        notUsedChunkRenderersKey.Remove(pos);
                    }else{
                        //Debug.Log("RenderChunk:"+pos+" queue!");
                        renderChunkPosQueue.Add(pos);
                    }
                }
            }
        }

        //deactivate not used objects
        foreach (var pos in notUsedChunkRenderersKey)
        {
            var obj = activeChunkRenderers[pos];
            obj.SetActive(false);

            activeChunkRenderers.Remove(pos);
            notActiveChunkRenderers.Enqueue(obj);
        }

        if(renderChunkPosQueue.Contains(centerChunkPos)){
            LoadAndRenderNewChunk(centerChunkPos);
        }

        //Debug.Log("renderChunkPosQueue:"+renderChunkPosQueue.Count);
        //Debug.Log("activeChunkRenderers:"+activeChunkRenderers.Count);
        //Debug.Log("notActiveChunkRenderers:"+notActiveChunkRenderers.Count);
        
        //Debug.Log("RenderChunks end! Starting Coroutine!");

        StartCoroutine(DelayBuildChunks());
    }

    public IEnumerator DelayBuildChunks()
    {
        //Debug.Log("DelayBuildChunks start!");

        while(renderChunkPosQueue.Count > 0)
        {
            var pos = renderChunkPosQueue.OrderBy(p => Vector3Int.Distance(p, centerChunkPos)).First();
            Debug.Log("DelayBuildChunks:"+pos);
            renderChunkPosQueue.Remove(pos);
            LoadAndRenderNewChunk(pos);

            //Debug.Log("renderChunkPosQueue:"+renderChunkPosQueue.Count);
            //Debug.Log("activeChunkRenderers:"+activeChunkRenderers.Count);
            //Debug.Log("notActiveChunkRenderers:"+notActiveChunkRenderers.Count);

            yield return new WaitForSeconds(.05f);
        }
        //Debug.Log("DelayBuildChunks end!");
    }

    private void LoadAndRenderNewChunk(Vector3Int pos)
    {
        //Debug.Log("LoadAndRenderNewChunk:"+pos);

        if(activeChunkRenderers.ContainsKey(pos)){
            //Debug.Log("Already Active:"+pos);
            return;
        }

        GameObject obj;
        if(notActiveChunkRenderers.Count == 0){
            obj = Instantiate(ChunkRendererPrefab, new Vector3(), Quaternion.identity, transform);       
            obj.SetActive(false);
            //Debug.Log("Create New:"+pos);
        }else{
            obj = notActiveChunkRenderers.Dequeue();
            //Debug.Log($"Re Use({pos}):"+obj.name);
        }

        var renderer = obj.GetComponent<ChunkRendererInterface>();
        var chunk = world.GetChunk(pos);

        renderer.chunk = chunk;
        renderer.shouldRender = true;

        obj.name = chunk.Name;
        obj.transform.position = chunk.Pos;
        obj.SetActive(true);

        activeChunkRenderers.Add(chunk.Pos, obj);
    }
}