# TLabWebViewVR-XRInteractionToolkit-2022

## Overview
- Sample project for using TLabWebView with Unity's VR templates
- This sample is the minimum configuration for using TLabWebView with the XR Interaction Toolkit.

## Note
- Android Custom Manifest Issue 1
Unity 2021.1.* recommended adding the following to the manifest file
```xml
<!-- For Unity-WebView -->
<application android:allowBackup="true"/>
<application android:supportsRtl="true"/>
<application android:hardwareAccelerated="true"/>
<application android:usesCleartextTraffic="true"/>

<uses-permission android:name="android.permission.INTERNET" />
<uses-permission android:name="android.permission.ACCESS_NETWORK_STATE"/>
<uses-permission android:name="android.permission.CAMERA" />
<uses-permission android:name="android.permission.MICROPHONE" />
<uses-permission android:name="android.permission.MODIFY_AUDIO_SETTINGS" />
<uses-permission android:name="android.permission.RECORD_AUDIO" />

<uses-feature android:name="android.hardware.camera" />
<uses-feature android:name="android.hardware.microphone" />
<!-- For Unity-WebView -->
```
However, this is not recommended for Unity 2022.1.* and later. 
Adding this caused a problem with the manifest file not being properly configured at build time.
It is recommended not to add these items to Android Custom Manifest after Unity 2022.1.*. (Creating Custom Manifest itself is not a problem)

- Android Custom Manifest Issue 2
When specifying OpenXR as the XR plugin provider, a part of the manifest is forcibly deleted and an error occurs in WebView. Therefore, it is recommended to specify Oculus as the plugin provider.
```xml
<!-- Error: net::ERR_CACHE_MISS -->
<uses-permission android:name="ANDROID.PERMISSION.INTERNET"/> <!-- Missing !! -->
```

## Requirements
- Unity Editor: 2022.3.11f1

### Installing
Clone the repository with the following command
```
git clone . https://github.com/TLabAltoh/TLabWebViewVR-XRInteractionToolkit-2022.git
```

### Set up
- Change platform to Android from Build Settings  
- Add the following symbols to Project Settings --> Player --> Other Settings (to be used at build time)  
	```
	UNITYWEBVIEW_ANDROID_USES_CLEARTEXT_TRAFFIC
	```
	```
	UNITYWEBVIEW_ANDROID_ENABLE_CAMERA
	```
	```
	UNITYWEBVIEW_ANDROID_ENABLE_MICROPHONE
	```
	- Color Space: Linear
	- Graphics: OpenGLES3
	- Minimux API Level: 29 
	- Target API Level: 32
	- Set plugin provider to Oculus
- XR Plug-in Management --> Android, Set plugin provider to Oculus (not OpenXR)