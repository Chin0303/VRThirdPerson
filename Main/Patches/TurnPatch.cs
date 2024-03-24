using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace VRThirdPerson.Patches
{
    [HarmonyPatch(typeof(GorillaSnapTurn))]
    [HarmonyPatch("StartTurn", MethodType.Normal)]
    internal class TurnPatch
    {
        private static void Postfix(ref float amount)
        {
            //Debug.Log($"{1 * amount}");
            Plugin.tpCamTransform.transform.Rotate(new Vector3(0, 1, 0), 1 * amount, Space.World);
        }
    }
}
