using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation.Samples;
using Klak.Ndi;
using M2MqttUnity.Examples;
using System;
using TMPro;
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
        public string name="";
        public float x=0;
        public float y=0;
        public float d=0;
    }

    
    public objectToSend cupObject;
    public objectToSend nose;
    public float dist;
    public TMP_Text distanceText;


    void Start()
    {

        cupObject = new objectToSend();
        nose = new objectToSend();
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
        if(depthPixels.Length > 0)
        {
            detection(mqtt.cupDetection, cupObject);
            detection(mqtt.nose, nose);
            dist = Vector3.Distance(
                new Vector3(cupObject.x,cupObject.y,cupObject.d),
                new Vector3(nose.x,nose.y,nose.d)
                );
            distanceText.text = "Distance: " + dist.ToString();

        }
        

    }

    void detection(string detection, objectToSend send)
    {
        if (detection != null)
        {
            var myObject = JsonUtility.FromJson<objectDetection>(detection);
            if (myObject != null)
            {

                send.x = Remap(myObject.x, 0, cpuImage.m_CameraTexture.width, 0, 1);
                send.y = Remap(myObject.y, 0, cpuImage.m_CameraTexture.height, 0, 1);

                if (send.x > 0 && send.y > 0)
                {
                    var depthIndex = ((int)(send.y * cpuImage.m_DepthTexture.height) - 1) * cpuImage.m_DepthTexture.width + (int)(send.x * cpuImage.m_DepthTexture.width);
                    //Debug.Log(depthIndex);
                    send.d = depthPixels[depthIndex].r;
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


