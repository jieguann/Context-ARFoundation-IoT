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
        [Tooltip("Set this to true to perform a testing cycle automatically on startup")]
        public bool autoTest = false;
       

        private List<string> eventMessages = new List<string>();
        private bool updateUI = false;
        

         
        public float seconds = 0f;
        public float minutes = 0f;
        //public float totalSeconds = 0f;
        public bool cellPhone = false;
        public bool teddyBear = false;
        public bool wineGlass = false;


        public void TestPublish()
        {
            client.Publish("M2MQTT_Unity/test", System.Text.Encoding.UTF8.GetBytes("Test message"), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
            print("Test message published");
            //AddUiMessage("Test message published.");
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
                TestPublish();
            }
        }

        protected override void SubscribeTopics()
        {
            client.Subscribe(new string[] { "jieThesis/Oculus/seconds" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
            client.Subscribe(new string[] { "jieThesis/Oculus/minutes" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
            client.Subscribe(new string[] { "jieThesis/Oculus/cellPhone" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
            client.Subscribe(new string[] { "jieThesis/Oculus/teddyBear" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
            client.Subscribe(new string[] { "jieThesis/Oculus/wineGlass" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });

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

            if (topic == "jieThesis/Oculus/seconds")
            {
                //print("1: " + Single.Parse(msg));
                //print(msg.GetType());
                seconds = Single.Parse(msg);
                //print("seconds: "+ seconds);
            }

            if (topic == "jieThesis/Oculus/minutes")
            {
                //print("1: " + Single.Parse(msg));
                //print(msg.GetType());
                minutes = Single.Parse(msg);
                //print("minutes: " + minutes);
            }

            if (topic == "jieThesis/Oculus/cellPhone")
            {
                //print("1: " + Single.Parse(msg));
                //print(msg.GetType());
                cellPhone = bool.Parse(msg);
                //print("cell phone: " + cellPhone);
            }

            if (topic == "jieThesis/Oculus/teddyBear")
            {
                //print("1: " + Single.Parse(msg));
                //print(msg.GetType());
                teddyBear = bool.Parse(msg);
                //print("cell phone: " + cellPhone);
            }
            if (topic == "jieThesis/Oculus/wineGlass")
            {
                //print("1: " + Single.Parse(msg));
                //print(msg.GetType());
                wineGlass = bool.Parse(msg);
                //print("cell phone: " + cellPhone);
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