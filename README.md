# TLabWebViewVR-XRInteractionToolkit-2022

## Overview
This sample is the minimum configuration for using TLabWebView with the XR Interaction Toolkit.

## Document
[document is here](https://tlabgames.gitbook.io/tlabwebview/scripting-api)

## Note
<details><summary>please see here</summary>

### Issue 1 (Android Custom Manifest)

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

### Issue 2 (Android Custom Manifest)

When specifying OpenXR as the XR plugin provider, a part of the manifest is forcibly deleted and an error occurs in WebView. Therefore, it is recommended to specify Oculus as the plugin provider.

```xml
<!-- Error: net::ERR_CACHE_MISS -->
<uses-permission android:name="ANDROID.PERMISSION.INTERNET"/> <!-- Missing !! -->
```

### Module Management Policy Modified
The policy has been changed to manage libraries in the repository as submodules after commit ``` 4a7a833 ```. Please run ``` git submodule update --init ``` to adjust the commit of the submodule to the version recommended by the project.

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

### Set Up
- Build Settings

| Property      | Value   |
| ------------- | ------- |
| Platform      | Android |

- Project Settings

| Property          | Value     |
| ----------------- | --------- |
| Color Space       | Linear    |
| Minimum API Level | 29        |
| Target API Level  | 32        |

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

| Property        | Value               |
| --------------- | ------------------- |
| Plugin Provider | Oculus (not OpenXR) |
