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
        }

        void OnGameInitialized(object sender, EventArgs e)
        {
            gameObject.AddComponent<VRInput>();
            tpCam = new GameObject("VR Third Person Camera").AddComponent<Camera>();
            tpCamTransform = tpCam.transform;
            tpCam.enabled = false;
            tpCam.useOcclusionCulling = true;
        }

        public static Transform tpCamTransform;
        Camera tpCam;
        float tpCamOffsetValue = 2.5f;
        void Update()
        {
            if (!tpCam)
                return;

            tpCamTransform.position = GorillaLocomotion.Player.Instance.headCollider.transform.position;
            if (Physics.Raycast(GorillaLocomotion.Player.Instance.headCollider.transform.position, -tpCam.transform.forward, out var hitInfo, tpCamOffsetValue, GorillaLocomotion.Player.Instance.locomotionEnabledLayers))
            {
                tpCamTransform.position = hitInfo.point;
            }
            else
            {
                tpCamTransform.position += -tpCamTransform.forward * tpCamOffsetValue;
            }

            if (VRInput.leftHand.secondary.Pressed)
            {
                tpCam.enabled = !tpCam.enabled;
            }
            if (tpCam.enabled && VRInput.leftHand.thumbstick.axisPosition != Vector2.zero)
            {
                tpCamTransform.transform.Rotate(new Vector3(1, 0, 0), (VRInput.leftHand.thumbstick.axisPosition.y * 180) * Time.deltaTime);
                tpCamTransform.transform.Rotate(new Vector3(0, 1, 0), -(VRInput.leftHand.thumbstick.axisPosition.x * 180) * Time.deltaTime, Space.World);
                //tpCamTransform.position = (tpCamTransform.position - GorillaLocomotion.Player.Instance.transform.position) + GorillaLocomotion.Player.Instance.transform.position;
            }
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
