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
        position = new Vector3(texture.cupObject.x*Screen.width, texture.cupObject.y*Screen.height, texture.cupObject.d);
        transform.position = Camera.main.ScreenToWorldPoint(position);
    }
}
