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

    public int width=1280, heigh=720;
    //public Texture2D cameraTexture;
    //public Texture2D DepthTexture;
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
            /*
            if (cpuImage.m_DepthTexture != null)
                cpuImage.m_DepthTexture = ScaleTexture(cpuImage.m_DepthTexture, width, heigh);
            */
            ndiSenderCamera.sourceTexture = cpuImage.m_DepthTexture;
        }
        else
        {
            /*
            if (cpuImage.m_CameraTexture != null)
                cpuImage.m_CameraTexture = ScaleTexture(cpuImage.m_CameraTexture, 1920, 1080);
            */
            ndiSenderCamera.sourceTexture = cpuImage.m_CameraTexture;
        }
        
        //ndiSenderDepth.sourceTexture = cpuImage.m_DepthTexture;
    }

    private Texture2D ScaleTexture(Texture2D source, int targetWidth, int targetHeight)
    {
        Texture2D result = new Texture2D(targetWidth, targetHeight, source.format, false);
        float incX = (1.0f / (float)targetWidth);
        float incY = (1.0f / (float)targetHeight);
        for (int i = 0; i < result.height; ++i)
        {
            for (int j = 0; j < result.width; ++j)
            {
                Color newColor = source.GetPixelBilinear((float)j / (float)result.width, (float)i / (float)result.height);
                result.SetPixel(j, i, newColor);
            }
        }
        result.Apply();
        return result;
    }



}
