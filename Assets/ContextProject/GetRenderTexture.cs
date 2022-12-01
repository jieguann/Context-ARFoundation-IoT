using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation.Samples;
using Klak.Ndi;
using M2MqttUnity.Examples;
using System;
//https://gist.github.com/openroomxyz/b7221ed30a0a0e04c32ae6d5fa948ac9

public class GetRenderTexture : MonoBehaviour
{
    public NdiSender ndiSenderCamera;
    //public NdiSender ndiSenderDepth;
    public CpuImageSample cpuImage;
    public bool depthFlag;

    public int width=1920, heigh=1080;

    public Color[] depthPixels;
    //public List<float> depthValue;

    [Serializable]
    public class depthClass
    {
        public List<float> depthValue=new List<float>();
    }
    public depthClass classVelue = new depthClass();
    void Start()
    {
        
        //classVelue.depthValue
        for(int i = 0; i < width * heigh; i++)
        {
            classVelue.depthValue.Add(0f);
            print(classVelue.depthValue);
        }
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
        if (cpuImage.m_DepthTexture != null)
        {
            depthPixels = cpuImage.m_DepthTexture.GetPixels();
            for (int i = 0; i < classVelue.depthValue.Count; i++)
            {
                classVelue.depthValue[i] = depthPixels[i].r;
            }


            Debug.Log(JsonUtility.ToJson(classVelue));
            //string jsonString = JsonUtility.ToJson(classVelue);
            
        }
           




    }

    



}
