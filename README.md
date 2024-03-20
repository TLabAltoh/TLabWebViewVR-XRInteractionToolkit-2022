The parameters described [here](https://github.com/TLabAltoh/TLabWebViewVR#:~:text=Open%20Assets/TLab/TLabWebViewVR/OculusIntegration,within%20Unity%20(default%20512%20*%20512)) are used for initialisation, changes to variables after initialisation are not reflected in the webview. Therefore, the GUI for editing these parameters is not implemented in the example scene.

To modify webview settings, change parameter in ``` TLabWebView.cs ``` from inspector with Unity Editor.

<details><summary>すごく長い文章とかプログラムとか</summary>
    
## 見出し1
- 本文

## 見出し2
- 本文
    
</details>

The ``` Destroy ``` method is used to terminate the WebView application running in the background. It is only called by ``` OnDestroy ``` and ``` OnApplicationQuit ```, so it is my mistake to declare the Destroy method as a public function.

If you want to destroy the WebView at runtime, you need to use [``` Object.Destroy ```](https://docs.unity3d.com/ja/2021.1/ScriptReference/Object.Destroy.html) to destroy the WebView prefab. And if you want to instantiate the WebView, you should call [``` Object.Instantiate ```](https://docs.unity3d.com/ja/2018.4/Manual/InstantiatingPrefabs.html) to create an instance of the WebView prefab.

# TLabWebViewVR-XRInteractionToolkit-2022

## Overview
- Sample project for using TLabWebView with Unity's VR templates
- This sample is the minimum configuration for using TLabWebView with the XR Interaction Toolkit.

## Document
[document is here](https://tlabgames.gitbook.io/tlabwebview/scripting-api)

## Note

<details><summary>please see here</summary>

### Android Custom Manifest Issue 1

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

### Android Custom Manifest Issue 2

When specifying OpenXR as the XR plugin provider, a part of the manifest is forcibly deleted and an error occurs in WebView. Therefore, it is recommended to specify Oculus as the plugin provider.

```xml
<!-- Error: net::ERR_CACHE_MISS -->
<uses-permission android:name="ANDROID.PERMISSION.INTERNET"/> <!-- Missing !! -->
```

### The policy has been changed to manage libraries in the repository as submodules.

- Commit ``` 84f7b5e ``` If you cloned the project before, please clone the repository again.
- Use ``` git submodule update --init ``` to adjust the commit of the submodule to the version recommended by the project.

</details>
    
## Getting Started

### Requirements
- Unity Editor: 2022.3.19f1

### Installing
Clone the repository with the following command
```
git clone https://github.com/TLabAltoh/TLabWebViewVR-XRInteractionToolkit-2022.git

cd TLabWebViewVR-XRInteractionToolkit-2022

git submodule update --init
```

### Set up
- Build Settings  

| Setting items | value |
| --- | --- |  
| platform | android |  

- Project Settings

| Setting items | value |
| --- | --- |  
| Color Space | Linear |  
| Graphics | OpenGLES3 |  
| Minimum API Level | 29 |  
| Target API Level | 32 |  

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

- XR Plug-in Management

| Setting items | value |
| --- | --- |  
| plugin provider | Oculus (not OpenXR) |  
 
## Issue

<details><summary>please see here</summary>

### After updating the repository, the built app crashes

The specific cause of this problem is still unknown. Please delete the build cache (``` root/Library/Bee ```) and try building again.
    
</details>
