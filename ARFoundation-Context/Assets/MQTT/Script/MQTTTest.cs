using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using M2MqttUnity;
using System.Diagnostics;
using System.Runtime.InteropServices;


/// <summary>
/// Examples for the M2MQTT library (https://github.com/eclipse/paho.mqtt.m2mqtt),
/// </summary>
namespace M2MqttUnity.Examples
{
    /// <summary>
    /// Script for testing M2MQTT with a Unity UI
    /// </summary>
    public class MQTTTest : M2MqttUnityClient
    {
        public GetRenderTexture texture;

        [Tooltip("Set this to true to perform a testing cycle automatically on startup")]
        public bool autoTest = false;
       

        private List<string> eventMessages = new List<string>();
        private bool updateUI = false;



        public string cupDetection;
        public string leftHand;//10
        public string rightHand;//9
        public string nose;//0


        public void DepthPublish()
        {
            if (texture.cupObject != null&&client!=null)
            {

               client.Publish("AceLab/ObjectValue", System.Text.Encoding.UTF8.GetBytes(JsonUtility.ToJson(texture.cupObject)), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
               client.Publish("AceLab/Distance", System.Text.Encoding.UTF8.GetBytes(texture.dist.ToString()), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);

            }
                

        }

        public void SetBrokerAddress(string brokerAddress)
        {/*
            if (addressInputField && !updateUI)
            {
                this.brokerAddress = brokerAddress;
            }
            */
        }

        public void SetBrokerPort(string brokerPort)
        {/*
            if (portInputField && !updateUI)
            {
                int.TryParse(brokerPort, out this.brokerPort);
            }
            */
        }

        public void SetEncrypted(bool isEncrypted)
        {
            this.isEncrypted = isEncrypted;
        }

       
        protected override void OnConnecting()
        {
            base.OnConnecting();
            //SetUiMessage("Connecting to broker on " + brokerAddress + ":" + brokerPort.ToString() + "...\n");
        }

        protected override void OnConnected()
        {
            base.OnConnected();
            //SetUiMessage("Connected to broker on " + brokerAddress + "\n");

            if (autoTest)
            {
                DepthPublish();
            }
        }

        protected override void SubscribeTopics()
        {
            client.Subscribe(new string[] { "AceLab/objectDetection/cup" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
            // node AceLab/poseNet/0/0
            client.Subscribe(new string[] { "AceLab/poseNet/0/0" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });


        }

        protected override void UnsubscribeTopics()
        {
            //client.Unsubscribe(new string[] { "M2MQTT_Unity/test" });
        }

        protected override void OnConnectionFailed(string errorMessage)
        {
            //AddUiMessage("CONNECTION FAILED! " + errorMessage);
        }

        protected override void OnDisconnected()
        {
            //AddUiMessage("Disconnected.");
        }

        protected override void OnConnectionLost()
        {
            //AddUiMessage("CONNECTION LOST!");
        }
       
        protected override void Start()
        {
            Connect();
            //SetUiMessage("Ready.");
            //updateUI = true;
            base.Start();
        }

        protected override void DecodeMessage(string topic, byte[] message)
        {
            string msg = System.Text.Encoding.UTF8.GetString(message);
            
            StoreMessage(msg);
            //Data = JsonMapper.ToObject(msg);

            if (topic == "AceLab/objectDetection/cup")
            {
                
               cupDetection = msg;

            }
            //AceLab/poseNet/0/0
            if (topic == "AceLab/poseNet/0/0")
            {

                nose = msg;

            }

        }

        private void StoreMessage(string eventMsg)
        {
            eventMessages.Add(eventMsg);
        }

        private void ProcessMessage(string msg)
        {
            //AddUiMessage("Received: " + msg);
            print(msg);
        }

        protected override void Update()
        {
            base.Update(); // call ProcessMqttEvents()
            //DepthPublish();
            DepthPublish();


        }

        private void OnDestroy()
        {
            Disconnect();
        }

        private void OnValidate()
        {
            if (autoTest)
            {
                autoConnect = true;
            }
        }
    }
}