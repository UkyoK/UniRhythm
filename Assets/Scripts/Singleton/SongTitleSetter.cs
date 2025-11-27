using TMPro;
using UnityEngine;

public class SongTitleSetter : MonoBehaviour
{
    public static SongTitleSetter Instance;

    [SerializeField]
    GameObject TitleObject;
    TextMeshProUGUI Title;

    [SerializeField]
    GameObject ArtistObject;
    TextMeshProUGUI Artist;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        Title = TitleObject.GetComponent<TextMeshProUGUI>();
        Artist = ArtistObject.GetComponent<TextMeshProUGUI>();
    }

    void Start()
    {
        Title.text = SettingManager.Instance.Title;
        Artist.text = SettingManager.Instance.ArtistName;
    }
}
