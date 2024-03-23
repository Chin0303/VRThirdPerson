using UnityEngine.XR;
using UnityEngine;
using System;
using Valve.VR;


public class VRControllerInput
{
    private bool IsLeftHand;
    public VRControllerInput(bool IsLeftHand) => this.IsLeftHand = IsLeftHand;

    public InputState trigger = new InputState();
    public InputState grip = new InputState();
    public InputState primary = new InputState();
    public InputState secondary = new InputState();
    public InputAxis thumbstick = new InputAxis();

    private float triggerValue, gripValue, primaryValue, secondaryValue, thumbstickValue;
    private Vector2 thumbstickAxisValue;

    public void UpdateValues()
    {
        switch (VRInput.OnSteam)
        {
            case true:
                triggerValue = IsLeftHand ? ControllerInputPoller.instance.leftControllerIndexFloat : ControllerInputPoller.instance.rightControllerIndexFloat;
                gripValue = IsLeftHand ? ControllerInputPoller.instance.leftControllerGripFloat : ControllerInputPoller.instance.rightControllerGripFloat;
                primaryValue = IsLeftHand ? ControllerInputPoller.instance.leftControllerPrimaryButton == true ? 1 : 0 : ControllerInputPoller.instance.rightControllerPrimaryButton == true ? 1 : 0;
                secondaryValue = IsLeftHand ? ControllerInputPoller.instance.leftControllerSecondaryButton == true ? 1 : 0 : ControllerInputPoller.instance.rightControllerSecondaryButton == true ? 1 : 0;
                thumbstickValue = IsLeftHand ? SteamVR_Actions.gorillaTag_LeftJoystickClick.state ? 1 : 0 : SteamVR_Actions.gorillaTag_RightJoystickClick.state ? 1 : 0;
                thumbstickAxisValue = IsLeftHand ? SteamVR_Actions.gorillaTag_LeftJoystick2DAxis.axis : SteamVR_Actions.gorillaTag_RightJoystick2DAxis.axis;
                break;
            case false:
                var device = InputDevices.GetDeviceAtXRNode(IsLeftHand ? XRNode.LeftHand : XRNode.RightHand);
                triggerValue = IsLeftHand ? ControllerInputPoller.instance.leftControllerIndexFloat : ControllerInputPoller.instance.rightControllerIndexFloat;
                gripValue = IsLeftHand ? ControllerInputPoller.instance.leftControllerGripFloat : ControllerInputPoller.instance.rightControllerGripFloat;
                primaryValue = IsLeftHand ? ControllerInputPoller.instance.leftControllerPrimaryButton == true ? 1 : 0 : ControllerInputPoller.instance.rightControllerPrimaryButton == true ? 1 : 0;
                secondaryValue = IsLeftHand ? ControllerInputPoller.instance.leftControllerSecondaryButton == true ? 1 : 0 : ControllerInputPoller.instance.rightControllerSecondaryButton == true ? 1 : 0;
                device.TryGetFeatureValue(CommonUsages.primary2DAxis, out thumbstickAxisValue);
                device.TryGetFeatureValue(CommonUsages.primary2DAxisClick, out var stickValue);
                thumbstickValue = stickValue ? 0 : 1;
                /*var device = InputDevices.GetDeviceAtXRNode(IsLeftHand ? XRNode.LeftHand : XRNode.RightHand);
                triggerValue = IsLeftHand ? ControllerInputPoller.instance.leftControllerIndexFloat : ControllerInputPoller.instance.rightControllerIndexFloat;
                gripValue = IsLeftHand ? ControllerInputPoller.instance.leftControllerGripFloat : ControllerInputPoller.instance.rightControllerGripFloat;
                primaryValue = IsLeftHand ? ControllerInputPoller.instance.leftControllerPrimaryButton == true ? 1 : 0 : ControllerInputPoller.instance.rightControllerPrimaryButton == true ? 1 : 0;
                secondaryValue = IsLeftHand ? ControllerInputPoller.instance.leftControllerPrimaryButton == true ? 1 : 0 : ControllerInputPoller.instance.rightControllerSecondaryButton == true ? 1 : 0;
                thumbstickValue = IsLeftHand ? SteamVR_Actions.gorillaTag_LeftJoystickClick.state == true ? 1 : 0 : SteamVR_Actions.gorillaTag_RightGripClick.state == true ? 1 : 0;
                thumbstickAxisValue = IsLeftHand ? SteamVR_Actions.gorillaTag_LeftJoystick2DAxis.axis : SteamVR_Actions.gorillaTag_RightJoystick2DAxis.axis;
                device.TryGetFeatureValue(CommonUsages.primary2DAxis, out thumbstickAxisValue);
                device.TryGetFeatureValue(CommonUsages.primary2DAxisClick, out var stickValue);
                thumbstickValue = stickValue == true ? 1 : 0;*/
                break;
        }
    }
    public void UpdateInput()
    {
        UpdateValues();
        trigger.UpdateInput(triggerValue);
        grip.UpdateInput(gripValue);
        primary.UpdateInput(primaryValue);
        secondary.UpdateInput(secondaryValue);
        thumbstick.UpdateInput(thumbstickValue, thumbstickAxisValue);
    }
}
