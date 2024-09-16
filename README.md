# TLabWebViewVR-XRInteractionToolkit-2022

## Overview
This sample is the minimum configuration for using TLabWebView with the XR Interaction Toolkit. This includes searchbar example.

## Document
[document is here](https://tlabgames.gitbook.io/tlabwebview/scripting-api)
    
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

| Property | Value   |
| -------- | ------- |
| Platform | Android |

- Project Settings

| Property          | Value  |
| ----------------- | ------ |
| Color Space       | Linear |
| Minimum API Level | 29     |
| Target API Level  | 32     |

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
