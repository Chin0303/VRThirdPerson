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
        public static Transform tpCamTransform;
        public static Transform camParentTransform;
        public static float tpCamOffsetValue = 2f;
        public static bool inverseCameraRotation = true;
        public static bool cameraClipping = false;
        //public static float YOffset;
        bool modEnabled;
        bool inRoom;
        Camera tpCam;

        void Awake()
        {
            Utilla.Events.GameInitialized += OnGameInitialized;
        }

        void OnEnable()
        {
            HarmonyPatches.ApplyHarmonyPatches();
            if (inRoom)
            {
                modEnabled = true;
            }
        }

        void OnDisable()
        {
            HarmonyPatches.RemoveHarmonyPatches();
            modEnabled = false;
            tpCam.enabled = false;
        }

        void OnGameInitialized(object sender, EventArgs e)
        {
            gameObject.AddComponent<VRInput>();

            camParentTransform = new GameObject("VR Third Person Camera").transform;
            tpCam = new GameObject("Camera").AddComponent<Camera>();
            tpCamTransform = tpCam.transform;
            tpCamTransform.SetParent(camParentTransform, false);
            tpCam.enabled = false;
            tpCam.useOcclusionCulling = true;
            tpCam.nearClipPlane = 0.01f;
            VRTPConfig.RefreshSettings();
        }

        void Update()
        {
            if (!modEnabled || tpCam == null)
                return;

            if (VRInput.rightHand.thumbstick.Pressed)
            {
                VRTPConfig.RefreshSettings();
                tpCam.enabled = !tpCam.enabled;
                GorillaTagger.Instance.thirdPersonCamera.gameObject.SetActive(!tpCam.enabled);
            }

            if (!tpCam.enabled)
                return;

            if (VRInput.leftHand.thumbstick.axisPosition != Vector2.zero)
            {
                camParentTransform.transform.Rotate(new Vector3((inverseCameraRotation ? -1 : 1), 0, 0), (VRInput.leftHand.thumbstick.axisPosition.y * 180) * Time.deltaTime);
                camParentTransform.transform.Rotate(new Vector3(0, (inverseCameraRotation ? -1 : 1), 0), -(VRInput.leftHand.thumbstick.axisPosition.x * 180) * Time.deltaTime, Space.World);
                //tpCamTransform.transform.Rotate(new Vector3(0, 1, 0), -(VRInput.leftHand.thumbstick.axisPosition.x * 180) * Time.deltaTime, Space.Self);
                //tpCamTransform.position = (tpCamTransform.position - GorillaLocomotion.Player.Instance.transform.position) + GorillaLocomotion.Player.Instance.transform.position;
            }

            camParentTransform.position = GorillaLocomotion.Player.Instance.bodyCollider.transform.position;
            tpCamTransform.rotation = Quaternion.Euler(GorillaLocomotion.Player.Instance.headCollider.transform.rotation.eulerAngles + new Vector3(0, camParentTransform.rotation.y, 0));
            if (cameraClipping)
            {
                if (Physics.Raycast(GorillaLocomotion.Player.Instance.headCollider.transform.position, -camParentTransform.transform.forward, out var hitInfo, tpCamOffsetValue, GorillaLocomotion.Player.Instance.locomotionEnabledLayers))
                {
                    camParentTransform.position = hitInfo.point;
                    return;
                }
            }
            camParentTransform.position += -camParentTransform.forward * tpCamOffsetValue;
        }


        [ModdedGamemodeJoin]
        public void OnJoin(string gamemode)
        {
            modEnabled = true;
            inRoom = true;
        }

        [ModdedGamemodeLeave]
        public void OnLeave(string gamemode)
        {
            tpCam.enabled = false;
            modEnabled = false;
            inRoom = false;
        }
    }
}

/*using BepInEx;
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
        public static Transform tpCamTransform;
        public static float tpCamOffsetValue = 2f;
        public static bool inverseCameraRotation = true;
        public static bool cameraClipping = false;
        bool modEnabled;
        bool inRoom;
        Camera tpCam;

        void Awake()
        {
            Utilla.Events.GameInitialized += OnGameInitialized;
        }

        void OnEnable()
        {
            HarmonyPatches.ApplyHarmonyPatches();
            if (inRoom)
            {
                modEnabled = true;
            }
        }

        void OnDisable()
        {
            HarmonyPatches.RemoveHarmonyPatches();
            modEnabled = false;
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

        void Update()
        {
            if (!modEnabled || tpCam == null)
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
            modEnabled = true;
            inRoom = true;
        }

        [ModdedGamemodeLeave]
        public void OnLeave(string gamemode)
        {
            tpCam.enabled = false;
            modEnabled = false;
            inRoom = false;
        }
    }
}*/