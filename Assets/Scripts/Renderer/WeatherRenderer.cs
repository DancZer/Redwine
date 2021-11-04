using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class WeatherRenderer : MonoBehaviour
{
    private Weather weather = new Weather();
    private readonly Dictionary<Cloud, GameObject> activeCloudRenderers = new Dictionary<Cloud, GameObject>();
    private readonly Queue<GameObject> notActiveCloudRenderers = new Queue<GameObject>();
    private readonly List<Cloud> renderCloudQueue = new List<Cloud>();

    public Transform PlayerPos;
    public GameObject ChunkRendererPrefab;

    // Start is called before the first frame update
    void Start()
    {
        weather.Init();
    }

    // Update is called once per frame
    void Update()
    {
        weather.Update(PlayerPos.position, Config.CloudViewDistanceBlockCount);

        RenderClouds();
    }

    private void RenderClouds()
    {
        StopCoroutine(DelayBuildClouds());

        var notUsedCloudRenderersKey = new HashSet<Cloud>(activeCloudRenderers.Keys);
        
        renderCloudQueue.Clear();

        foreach (var cloud in weather.GetVisibleClouds(PlayerPos.position, Config.CloudViewDistanceBlockCount))
        {
            GameObject obj;
            if(activeCloudRenderers.TryGetValue(cloud, out obj)){
                notUsedCloudRenderersKey.Remove(cloud);
                obj.transform.position = cloud.Pos;
            }else{
                renderCloudQueue.Add(cloud);
            }
        }
       
        //deactivate not used objects
        foreach (var cloud in notUsedCloudRenderersKey)
        {
            var obj = activeCloudRenderers[cloud];
            obj.SetActive(false);

            activeCloudRenderers.Remove(cloud);
            notActiveCloudRenderers.Enqueue(obj);
        }

        StartCoroutine(DelayBuildClouds());
    }

    public IEnumerator DelayBuildClouds()
    {
        while(renderCloudQueue.Count > 0)
        {
            var cloud = renderCloudQueue.OrderBy(p => Vector3.Distance(p.Pos, PlayerPos.position)).First();

            renderCloudQueue.Remove(cloud);
            LoadAndRenderNewCloud(cloud);

            yield return new WaitForSeconds(.05f);
        }
    }

    private void LoadAndRenderNewCloud(Cloud cloud)
    {
        if(activeCloudRenderers.ContainsKey(cloud)){
            return;
        }

        GameObject obj;
        if(notActiveCloudRenderers.Count == 0){
            obj = Instantiate(ChunkRendererPrefab, new Vector3(), Quaternion.identity, transform);       
            obj.SetActive(false);
        }else{
            obj = notActiveCloudRenderers.Dequeue();
        }

        var renderer = obj.GetComponent<ChunkRendererInterface>();
        renderer.chunk = cloud;
        renderer.shouldRender = true;

        obj.name = cloud.Name;
        obj.transform.position = cloud.Pos;
        obj.SetActive(true);

        activeCloudRenderers.Add(cloud, obj);
    }
}
