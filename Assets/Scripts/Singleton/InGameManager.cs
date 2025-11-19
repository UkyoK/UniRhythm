using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameManager : MonoBehaviour
{
    public static InGameManager Instance;

    public float EndTime { get; private set; }
    public void SetEndTime(float endTime)
    {
        EndTime = endTime;
    }

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

        EndTime = Time.time + 10000;
    }

    private void Start()
    {
        MySoundManager.Instance.PlayMusic();
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time > EndTime)
        {
            MySoundManager.Instance.MusicStop();
            // リザルトシーンに行く
            // 今は仮で初期シーンに
            SceneManager.LoadScene("ResultScene");
            Destroy(gameObject);
        }
    }

}
