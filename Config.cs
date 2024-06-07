using System.IO;
using BepInEx;
using BepInEx.Configuration;
using UnityEngine;
using VRThirdPerson;

static internal class VRTPConfig
{
    public static ConfigFile config;

    static VRTPConfig()
    {
        config = new ConfigFile(Path.Combine(Paths.ConfigPath, "VRThirdPerson.cfg"), true);
    }
    public static void RefreshSettings() 
    {
        config.Reload();
        Plugin.tpCamOffsetValue = Mathf.Clamp(config.Bind("VR Third Person", "Third Person Camera Offset", 2).Value, 0.5f, 10000);
        Plugin.inverseCameraRotation = config.Bind("VR Third Person", "Inverted Camera Rotation", true).Value;
        Plugin.cameraClipping = config.Bind("VR Third Person", "Camera Clipping", false).Value;
    }
}


