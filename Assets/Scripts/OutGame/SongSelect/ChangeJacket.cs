using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeJacket : MonoBehaviour
{
    public static ChangeJacket Instance;

    List<Sprite> SpriteList;
    Image Jacket;
    int SongID;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        SpriteList = new List<Sprite>();
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < SongInfoLoader.Instance.SongInfoList.Count; i++)
        {
            string path = "Images/" + SongInfoLoader.Instance.SongInfoList[i].FolderName;
            Sprite sprite = Resources.Load<Sprite>(path);
            SpriteList.Add(sprite);
        }

        Jacket = GetComponent<Image>();
        SongID = SongSelect.Instance.TopSong + 1;
        if (SongID >= SongInfoLoader.Instance.SongInfoList.Count)
        {
            SongID = 0;
        }
        Jacket.sprite = SpriteList[SongID];
    }

    public void JacketChange(int id)
    {
        SongID = id + 1;

        if (SongID >= SongInfoLoader.Instance.SongInfoList.Count)
        {
            SongID = 0;
        }

        Jacket.sprite = SpriteList[SongID];
    }

}
