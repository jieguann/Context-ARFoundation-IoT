# Context-ARFoundation-IoT

## It is a part of the main project
https://github.com/jieguann/XRI-Agent-Avatar-Design-Prototype-ACELab


## Description
This prototype demenstrate how to detecte a cup close to the nose of the user through detection of object (Cup) and the position of nose through PoseNet, plus the depth map from Lidar with IOS from ARFoundation.

## Framework for NDI and ML5 connection

![DepthObjectDetectionFramework](https://user-images.githubusercontent.com/60665347/205207386-f29af7ac-51d3-428c-a336-3f1c24521db4.png)



## Phone Application Setup
* Change resolution to 1920 x 1080 at the left botton.
* Set iphone to auto rotate in the setting, and turn it to landscape mode.

## Webcame ML5.js Object Detection and PoseNet
Send all the name and positino of objects and PoseNet point through MQTT.

PoseNet Key point https://github.com/tensorflow/tfjs-models/tree/master/pose-detection
