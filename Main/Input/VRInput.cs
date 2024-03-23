using GorillaNetworking;
using HarmonyLib;
using Photon.Pun;
using Photon.Realtime;
using Steamworks;
using System;
using System.Collections.Generic;
using UnityEngine;

public class VRInput : MonoBehaviour
{
    public static VRControllerInput leftHand, rightHand;
    public static bool OnSteam = false;
    void Awake()
    {
        string platform = (string)typeof(PlayFabAuthenticator).GetField("platform", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).GetValue(PlayFabAuthenticator.instance);
        if (platform.ToUpper() == "STEAM")
        {
            OnSteam = true;
        }

        leftHand = new VRControllerInput(true);
        rightHand = new VRControllerInput(false);
    }
    public void Update() => UpdateInput();

    public void UpdateInput()
    {
        leftHand?.UpdateInput();
        rightHand?.UpdateInput();
    }
}
