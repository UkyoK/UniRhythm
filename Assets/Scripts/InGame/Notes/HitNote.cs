using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitNote : MonoBehaviour
{
    public float JustTime { get; private set; }
    float NoteSpeed;

    public void Initialize(float lanePos, float justTime, float noteSpeed)
    {
        transform.position = new Vector3(lanePos, justTime, -0.55f);
        JustTime = justTime;
        NoteSpeed = noteSpeed;

        // 親オブジェクトのスケールによる影響を排除
        transform.localScale = new Vector3(
                transform.localScale.x / transform.parent.lossyScale.x,
                transform.localScale.y / transform.parent.lossyScale.y,
                transform.localScale.z / transform.parent.lossyScale.z);
    }

    // Update is called once per frame
    void Update()
    {
        float posY = NoteSpeed * (JustTime - Time.time);
        transform.position = new Vector3(transform.position.x, posY, transform.position.z);
    }
}
