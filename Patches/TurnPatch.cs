using HarmonyLib;
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
            Plugin.camParentTransform.transform.Rotate(new Vector3(0, 1, 0), 1 * amount, Space.World);
        }
    }
}
