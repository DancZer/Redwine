using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldRenderer : MonoBehaviour
{
    private World world;

    private GameObject[,,] ChunkRenderers;

    public MonoBehaviour Player;
    public GameObject ChunkRendererPrefab;

    private BlockPos PrevPlayerPos = new BlockPos();

    // Start is called before the first frame update
    void Start()
    {        
        Player.transform.position = new Vector3();

        ChunkRenderers = new GameObject[World.SIZE, World.SIZE, World.SIZE];

        CreateRenderers();

        world = World.Instance();
        world.Init();

        Render();
    }
    private void CreateRenderers()
    {
        for(int x=0;x<World.SIZE;x++){
            for(int z=0;z<World.SIZE;z++){
                for(int y=0;y<World.SIZE;y++){
                    var chunkObject = Instantiate(ChunkRendererPrefab, new Vector3(), Quaternion.identity, transform);       
                    chunkObject.SetActive(false);
                    ChunkRenderers[x,y,z] = chunkObject;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        bool shouldRender = false;

        var playerPos = new BlockPos(Vector3Int.FloorToInt(Player.transform.position));
        if(world.Update(playerPos)){
            world.LoadChunks();
            shouldRender = true;
        }

        //SKIP block rendering where the player is
        if(PrevPlayerPos != playerPos){
            var playerBlockState = world.GetBlockStates(PrevPlayerPos);
            playerBlockState.Enabled = true;

            playerBlockState = world.GetBlockStates(playerPos);
            playerBlockState.Enabled = false;

            PrevPlayerPos = playerPos;
            
            shouldRender = true;
        }

        if(shouldRender){
            Render();
        }
    }

    private void Render()
    {
        for(int x=0;x<World.SIZE;x++){
            for(int z=0;z<World.SIZE;z++){
                for(int y=0;y<World.SIZE;y++){
                    var obj = ChunkRenderers[x,y,z];
                    obj.SetActive(false);

                    var renderer = obj.GetComponent<ChunkRendererInterface>();
                    renderer.World = world;
                    renderer.Chunk = world.Chunks[x,y,z];

                    obj.name = renderer.Chunk.Name;
                    obj.transform.position = renderer.Chunk.Pos.Vect;
                    obj.SetActive(true);
                }
            }
        }
    }

}
