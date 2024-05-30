using EasyUI.Progress;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using YoutubePlayer.Components;

public class VideoPlayerHandler : MonoBehaviour
{
   


    [SerializeField] VideoPlayer player;
    [SerializeField] InvidiousVideoPlayer invidiousVideoPlayer;
    private string url;
    // Start is called before the first frame update
    private void Awake()
    {
         url = Lesson.GetVideoUrl();
        
    }
    string getIdFromUrl(string url)
    {
        var queries = url.Split("?");
        var query = queries[1].Split("&");
        if(query.Length == 1)
        {
            var result = query[0].Split("=");
            return result[1];
        }
        return null;
    }
    void Start()
    {
        
        
        if (url != null)
        {
            var id = getIdFromUrl(url);
            if(id != null)
            {
                invidiousVideoPlayer.VideoId = id;
                Debug.Log("Video: " + id);

            }
        }
        Progress.Show("Đang tải video...", ProgressColor.Orange);
    }

    // Update is called once per frame
    void Update()
    {
        if (player.isPrepared)
        {
            Progress.Hide();
        }
        
    }
    private void OnDisable()
    {
        player.url= null;
        invidiousVideoPlayer.VideoId= null;
    }
}
