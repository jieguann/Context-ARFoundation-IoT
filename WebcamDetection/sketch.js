// ml5.js: Object Detection with COCO-SSD (Webcam)
// The Coding Train / Daniel Shiffman
// https://thecodingtrain.com/learning/ml5/1.3-object-detection.html
// https://youtu.be/QEzRxnuaZCk

// p5.js Web Editor - Image: https://editor.p5js.org/codingtrain/sketches/ZNQQx2n5o
// p5.js Web Editor - Webcam: https://editor.p5js.org/codingtrain/sketches/VIYRpcME3
// p5.js Web Editor - Webcam Persistence: https://editor.p5js.org/codingtrain/sketches/Vt9xeTxWJ

// let img;
let video;
//object detection
let detector;
let detections = [];
let object;

//pose detection
let poseNet;
let poses = [];

const widthC = 1920;
const heightC = 1080;

//Json Object

let handLeft = {name:"left hand", x:0,y:0};
let handRight = {name:"right hand",x:0,y:0};
let head = {name:"right hand",x:0,y:0};


//MQTT
const mqttTitle = "AceLab"


function preload() {
  // img = loadImage('dog_cat.jpg');
  detector = ml5.objectDetector('cocossd');
}

function gotDetections(error, results) {
  if (error) {
    console.error(error);
  }
  detections = results;
  detector.detect(video, gotDetections);
}

function setup() {
  //put canvas in div html
  var canvas = createCanvas(widthC, heightC);
  canvas.parent('canvasForHTML');

  video = createCapture(VIDEO);
  video.size(width, height);
  // Hide the video element, and just show the canvas
  video.hide();

  //Object Detection 
  detector.detect(video, gotDetections);

  //pose net
  // Create a new poseNet method with a single detection
  poseNet = ml5.poseNet(video);
  // This sets up an event that fills the global variable "poses"
  // with an array every time new poses are detected
  poseNet.on("pose", function(results) {
    poses = results;
  });


  
}


function draw() {
  //set up vide
  image(video, 0, 0, width, height);
  //draw object
  drawDetection();
  //draw post
  drawKeypoints();
  drawSkeleton();
  //canvase
  stroke("red");
  noFill();
  //rect(widthC/4,20, widthC/2, heightC-20);

}

//object function
function drawDetection(){
  
  
  for (let i = 0; i < detections.length; i++) {
  // if(detections.length >0){
    object = detections[i];


    


      stroke(0, 255, 0);
      strokeWeight(4);
      noFill();
      rect(object.x, object.y, object.width, object.height);
      //cupObject.x = object.x+object.width/2;
      //cupObject.y = object.y+object.height/2;
      //cupObject.name = "cup";
      noStroke();
      fill(255);
      textSize(24);
      text(object.label, object.x + 10, object.y + 24);
      
      text(object.label, object.x + 10, object.y + 24);
      
       
      sendPositionMQTT(object.label,object.x,object.y);
      
    //client.publish(mqttTitle + '/cup', JSON.stringify(cupObject), { qos: 0, retain: false });

}
}



// A function to draw ellipses over the detected keypoints
function drawKeypoints() {
  // Loop through all the poses detected
  for (let i = 0; i < poses.length; i += 1) {
    // For each pose detected, loop through all the keypoints
    const pose = poses[i].pose;
    for (let j = 0; j < pose.keypoints.length; j += 1) {
      // A keypoint is an object describing a body part (like rightArm or leftShoulder)
      const keypoint = pose.keypoints[j];
      // Only draw an ellipse is the pose probability is bigger than 0.2
      if (keypoint.score > 0.2) {
        fill(255, 0, 0);
        noStroke();
        ellipse(keypoint.position.x, keypoint.position.y, 30, 30);
        sendPositionMQTT("poseNet/"+i.toString() + "/" + j.toString(),keypoint.position.x, keypoint.position.y)
      }
    }
  }
}

//publish to mqtt functino
function sendPositionMQTT(name,x,y){
  var detectedObject = {name:"",x:0,y:0};
  detectedObject.name = name;
  detectedObject.x = x;
  detectedObject.y = y;
  client.publish(mqttTitle + "/"+name, JSON.stringify(detectedObject), { qos: 0, retain: false });
}




function drawSkeleton() {
  // Loop through all the skeletons detected
  for (let i = 0; i < poses.length; i += 1) {
    const skeleton = poses[i].skeleton;
    // For every skeleton, loop through all body connections
    for (let j = 0; j < skeleton.length; j += 1) {
      const partA = skeleton[j][0];
      const partB = skeleton[j][1];
      stroke(255, 0, 0);
      line(partA.position.x, partA.position.y, partB.position.x, partB.position.y);
    }
  }
}









//Code for MQTT
const clientId = 'mqttjs_' + Math.random().toString(16).substr(2, 8)

const host = 'wss://mqtt.eclipseprojects.io:443/mqtt'
//const host = 'ws://134.122.33.147:9001/mqtt'

console.log('Connecting mqtt client')
const client = mqtt.connect(host)

client.on('error', (err) => {
console.log('Connection error: ', err)
client.end()
})

client.on('reconnect', () => {
console.log('Reconnecting...')
})

client.on('message', (topic, message, packet) => {
  console.log('Received Message: ' + message.toString() + '\nOn topic: ' + topic)
})




