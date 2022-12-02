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
    public MQTTTest mqtt;
    public NdiSender ndiSenderCamera;
    //public NdiSender ndiSenderDepth;
    public CpuImageSample cpuImage;
    public bool depthFlag;

    public int width=1920, heigh=1080;

    public Color[] depthPixels;
    //public List<float> depthValue;
    [Serializable]
    public class objectDetection
    {
        public string name;
        public int x;
        public int y;


    }
    [Serializable]
    public class objectToSend
    {
        public string name="bottle";
        public float x=0;
        public float y=0;
        public float d=0;
    }

    
    public objectToSend objectSend;


    void Start()
    {

        objectSend = new objectToSend();
    }

    // Update is called once per frame
    void Update()
    {

        if (cpuImage.m_DepthTexture != null)
            depthPixels = cpuImage.m_DepthTexture.GetPixels();

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

        if (mqtt.objectDetection != null && depthPixels.Length>0)
        {
            var myObject = JsonUtility.FromJson<objectDetection>(mqtt.objectDetection);
            if (myObject != null)
            {
                
                objectSend.x = Remap(myObject.x, 0, cpuImage.m_CameraTexture.width, 0, 1);
                objectSend.y = Remap(myObject.y, 0, cpuImage.m_CameraTexture.height, 0, 1);

                if(objectSend.x>0&& objectSend.y > 0)
                {
                    var depthIndex = ((int)(objectSend.y * cpuImage.m_DepthTexture.height) - 1) * cpuImage.m_DepthTexture.width + (int)(objectSend.x * cpuImage.m_DepthTexture.width);
                    //Debug.Log(depthIndex);
                    objectSend.d = depthPixels[depthIndex].r;
                }
                
            }
            
        }

    }


    float Remap(float from, float fromMin, float fromMax, float toMin, float toMax)
    {
        var fromAbs = from - fromMin;
        var fromMaxAbs = fromMax - fromMin;

        var normal = fromAbs / fromMaxAbs;

        var toMaxAbs = toMax - toMin;
        var toAbs = toMaxAbs * normal;

        var to = toAbs + toMin;

        return to;
    }



    



}


