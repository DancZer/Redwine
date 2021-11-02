using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class WorldRenderer : MonoBehaviour
{
    private World world = new World();

    private Dictionary<Vector3Int, GameObject> chunkRenderers;

    public MonoBehaviour Player;
    public GameObject ChunkRendererPrefab;

    // Start is called before the first frame update
    void Start()
    {        
        Player.transform.position = new Vector3();

        chunkRenderers = new Dictionary<Vector3Int, GameObject>();

        world.Start();

        CreateRenderers();

        Render();
    }
    private void CreateRenderers()
    {
        for(int x=0;x<World.Size;x++){
            for(int z=0;z<World.Size;z++){
                for(int y=0;y<World.Size;y++){
                    var blockPos = new Vector3Int(x,y,z);

                    var chunkObject = Instantiate(ChunkRendererPrefab, new Vector3(), Quaternion.identity, transform);       
                    chunkObject.SetActive(false);
                    chunkRenderers.Add(blockPos, chunkObject);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        var playerPos = Vector3Int.FloorToInt(Player.transform.position);
        if(world.Update(playerPos)){
            world.LoadChunks();
            Render();
        }
    }

    private void Render()
    {
        var newChunkRenderers = new Dictionary<Vector3Int, GameObject>();
        var listMissingChunkObjects = new List<Vector3Int>();

        foreach (var key in world.ChunkKeys())
        {
            GameObject obj;
            if(chunkRenderers.TryGetValue(key, out obj)){
                var renderer = obj.GetComponent<ChunkRendererInterface>();

                if(renderer.chunk == null){
                    listMissingChunkObjects.Add(key);
                }else{
                    chunkRenderers.Remove(key);
                    newChunkRenderers.Add(key, obj);
                }
                
                //Debug.Log("WorldRenderer: Already exists:"+key);
            }else{
                listMissingChunkObjects.Add(key);
                //Debug.Log("WorldRenderer: Missing:"+key+" hash:"+key.GetHashCode());
            }
        }

        var notUsedObjects = chunkRenderers.Keys.ToList();

        //Debug.Log("WorldRenderer: Missing total:"+listMissingChunkObjects.Count);
        //Debug.Log("WorldRenderer: notUsedObjects total:"+notUsedObjects.Count);

        if(listMissingChunkObjects.Count != notUsedObjects.Count)
        {
            throw new UnityException("WorldRenderer is not initialized correctly. Missing is not matches the not used:"+listMissingChunkObjects.Count+","+notUsedObjects.Count);
        }

        foreach (var key in listMissingChunkObjects)
        {
            var notUsedKey = notUsedObjects.First();
            notUsedObjects.RemoveAt(0);
            
            //Debug.Log("WorldRenderer: notUsedKey:"+notUsedKey+" hash:"+notUsedKey.GetHashCode()+" key:"+key+" hash:"+key.GetHashCode());

            var obj = chunkRenderers[notUsedKey];
            chunkRenderers.Remove(notUsedKey);

            obj.SetActive(false);

            var renderer = obj.GetComponent<ChunkRendererInterface>();
            var chunk = world.GetChunk(key);

            //Debug.Log("WorldRenderer: chunk:"+chunk);

            renderer.chunk = chunk;
            renderer.shouldRender = true;

            obj.name = chunk.Name;
            obj.transform.position = chunk.Pos;
            obj.SetActive(true);
            
            newChunkRenderers.Add(key, obj);
        }

        if(chunkRenderers.Count != 0)
        {
            throw new UnityException("WorldRenderer is not initialized correctly. There are some renderers left:"+chunkRenderers.Count);
        }

        //Debug.Log("WorldRenderer: newChunkRenderers:"+newChunkRenderers.Count);

        chunkRenderers = newChunkRenderers;
    }

}
