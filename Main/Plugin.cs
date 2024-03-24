using BepInEx;
using System;
using UnityEngine;
using Utilla;

namespace VRThirdPerson
{
    [ModdedGamemode]
    [BepInDependency("org.legoandmars.gorillatag.utilla", "1.5.0")]
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    public class Plugin : BaseUnityPlugin
    {
        bool inRoom;

        void Awake()
        {
            Utilla.Events.GameInitialized += OnGameInitialized;
        }

        void OnEnable()
        {
            HarmonyPatches.ApplyHarmonyPatches();
        }

        void OnDisable()
        {
            HarmonyPatches.RemoveHarmonyPatches();
            tpCam.enabled = false;
        }

        void OnGameInitialized(object sender, EventArgs e)
        {
            gameObject.AddComponent<VRInput>();
            tpCam = new GameObject("VR Third Person Camera").AddComponent<Camera>();
            tpCamTransform = tpCam.transform;
            tpCam.enabled = false;
            tpCam.useOcclusionCulling = true;
            tpCam.nearClipPlane = 0.01f;
            VRTPConfig.RefreshSettings();
        }

        public static Transform tpCamTransform;
        Camera tpCam;
        public static float tpCamOffsetValue = 2f;
        public static bool inverseCameraRotation = true;
        public static bool cameraClipping = false;
        private bool modEnabled;
        void Update()
        {
            if (!tpCam)
                return;

            if (VRInput.leftHand.secondary.Pressed)
            {
                VRTPConfig.RefreshSettings();
                tpCam.enabled = !tpCam.enabled;
                GorillaTagger.Instance.thirdPersonCamera.gameObject.SetActive(!tpCam.enabled);
            }

            if (!tpCam.enabled)
                return;

            if (VRInput.leftHand.thumbstick.axisPosition != Vector2.zero)
            {
                tpCamTransform.transform.Rotate(new Vector3((inverseCameraRotation ? -1 : 1), 0, 0), (VRInput.leftHand.thumbstick.axisPosition.y * 180) * Time.deltaTime);
                tpCamTransform.transform.Rotate(new Vector3(0, (inverseCameraRotation ? -1 : 1), 0), -(VRInput.leftHand.thumbstick.axisPosition.x * 180) * Time.deltaTime, Space.World);
                //tpCamTransform.position = (tpCamTransform.position - GorillaLocomotion.Player.Instance.transform.position) + GorillaLocomotion.Player.Instance.transform.position;
            }

            tpCamTransform.position = GorillaLocomotion.Player.Instance.bodyCollider.transform.position;
            if (cameraClipping)
            {
                if (Physics.Raycast(GorillaLocomotion.Player.Instance.headCollider.transform.position, -tpCam.transform.forward, out var hitInfo, tpCamOffsetValue, GorillaLocomotion.Player.Instance.locomotionEnabledLayers))
                {
                    tpCamTransform.position = hitInfo.point;
                    return;
                }
            }
            tpCamTransform.position += -tpCamTransform.forward * tpCamOffsetValue;
        }


        [ModdedGamemodeJoin]
        public void OnJoin(string gamemode)
        {
            inRoom = true;
        }

        [ModdedGamemodeLeave]
        public void OnLeave(string gamemode)
        {
            inRoom = false;
        }
    }
}