using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation.Samples;
using Klak.Ndi;
//https://gist.github.com/openroomxyz/b7221ed30a0a0e04c32ae6d5fa948ac9

public class GetRenderTexture : MonoBehaviour
{
    public NdiSender ndiSenderCamera;
    //public NdiSender ndiSenderDepth;
    public CpuImageSample cpuImage;
    public bool depthFlag;

    
    //public RenderTexture thisCameraTexture;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (depthFlag == true)
        {
            ndiSenderCamera.sourceTexture = cpuImage.m_DepthTexture;
        }
        else
        {
            ndiSenderCamera.sourceTexture = cpuImage.m_CameraTexture;
        }
        
        //ndiSenderDepth.sourceTexture = cpuImage.m_DepthTexture;
    }


    
}
