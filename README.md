# TLabWebViewVR-XRInteractionToolkit-2022

## Overview
- Sample project for using TLabWebView with Unity's VR templates
- This sample is the minimum configuration for using TLabWebView with the XR Interaction Toolkit.

## Note
- Unity versions of 2021.* recommended creating custom android manifests, but since 2022.1.* ``` <activity></activity> ``` is not automatically added, and this was causing build errors.
- When specifying OpenXR as the XR plugin provider, a part of the manifest is forcibly deleted and an error occurs in WebView. Therefore, it is recommended to specify Oculus as the plugin provider.
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