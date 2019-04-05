# Blank Room 

Developers: Robert DiLorenzo, Shannon Cain, and Karl Lamoureux

This is  our senior thesis project Blank Room, a VR workspace built for android and the Google Daydream View headset. 

#### Table of Contents
1.[Project Overview](#project-overview)<br>
2.[Demo Setup](#demo-setup)<br>
3.[Unity Scene Overview](#scene-overview)<br>
4.[Naigation System](#navigation-system)<br>
5.[Writing System](#writing-system)<br>
6.[File I/O](#file-io)<br>
7.[References](#references)<br>

<a name="project-overview"></a>
#### Project Overview
In our app, the user is placed in a room with a virtual dry erase board that they can write on using the handheld controller. The wall's surface is saved to a file which can be opened and edited later on. This allows users to spread out their work and solve problems without needing a huge amount of space and money for expensive dry erase board installments. 

(INSTRUCTIONS FOR OUR PROFESSOR)
At the root directory we have this README file, a folder containing our Unity project, our Proposal doc, Design doc and Final paper. Within the Unity project, go into the Assets folder. All folders inside are self descriptive. You will find all of our code in the scripts folder, however it is important to note that many of the assets we used for this project are outside of this folder (for example our "levels" are in the scenes folder, our textures and materials are in the materials folder, etc.). In order to see how these files are entertwined, you will need to have Unity installed, and from there you will be able to open the project folder in the editor. You can still open our code files and image files like you would normally, but certain files like .meta files and prefab files are Unity specific files. 

<a name="demo-setup"></a>
#### Demo Setup
To run our demo, you need an android phone that is compatible with the Google Daydream View. Here are the list of phones that are compatible with the headset: [Headset Ready Phones](https://vr.google.com/daydream/smartphonevr/phones/). You can either download the APK directly from our repository onto your phone, or you can download it onto your computer, hook up your phone via USB cable and drag it into your phone's file directory. Simply tap the app after it is installed, put it in your headset and begin. It is important to note that our app is built specifically for the Daydream View, you will not be able to use the app with other headsets. 

<a name="scene-overview"></a>
#### Unity Scene Overview
Screenshots Pending!

For our scene Writing Test, our primary demo, we first have all of the required Google Daydream set up prefabs. Outside of this, we have the pen object attached as a child of the player, which needs to have its public variables set. We also have the primary canvas, initially hidden. This canvas contains all of the UI panels. In order for the UI to function, you need a GameObject in the scene with the Save Image script attached to it. This scrip will automatically populate itself with all UI elements. Lastly, we have our primary wall Plane (1), our render wall Plane, and second camera called Render camera. See the Writing Test scene in Unity to see all the specific placements of objects required for the project. 

<a name="navigation-system"></a>
#### Navigation System
The navigation system consisted of a few different components, some where so that the user could move freely and look all around others were for the remote controller in order to grab, release, move object, highlight object, and select. The gameObject component used was getComponent. This was important for using the remote in order to control the objects in the scene. It included the GvrControllerMain and GvrControllerPointer. This section also consisted of the functions in order for the user to navigate throughout the scenes.

<a name="writing-system"></a>
#### Writing System

<a name="file-io"></a>
#### File I/O
The image on the wall is saved as a png named "roomname"_1.png (with the name of the room the user writes) and the player position and rotation variables are saved in a text file called "roomname"_Player.txt. A UI can be accessed to: create a new room from scratch; save the current room; save a copy of the current room as a new room; load a saved room; delete a saved room; and clear the walls to start over.

<a name="references"></a>
#### References
