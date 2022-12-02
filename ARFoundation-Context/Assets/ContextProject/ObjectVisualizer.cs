using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectVisualizer : MonoBehaviour
{
    public GetRenderTexture texture;
    private Vector3 position;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        position = new Vector3(texture.objectSend.x*Screen.width, texture.objectSend.y*Screen.height, texture.objectSend.d);
        transform.position = Camera.main.ScreenToWorldPoint(position);
    }
}
