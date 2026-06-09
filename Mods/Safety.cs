using GorillaLocomotion;
using Qatual.Classes;
using Qatual.Notifications;
using System.Linq;
using UnityEngine;
using UnityEngine.XR;
using static Qatual.Classes.RigManager;
using static Qatual.Menu.Main;

namespace Qatual.Mods
{
    public class Safety
    {
        public static VRRig reportRig;
        public static void AntiReport(System.Action<VRRig, Vector3> onReport)
        {
            if (!NetworkSystem.Instance.InRoom) return;

            if (reportRig != null)
            {
                onReport?.Invoke(reportRig, reportRig.transform.position);
                reportRig = null;
                return;
            }

            foreach (GorillaPlayerScoreboardLine line in GorillaScoreboardTotalUpdater.allScoreboardLines)
            {
                if (line.linePlayer != NetworkSystem.Instance.LocalPlayer) continue;
                Transform report = line.reportButton.gameObject.transform;

                foreach (var vrrig in from vrrig in VRRigCache.ActiveRigs where !vrrig.isLocal let D1 = Vector3.Distance(vrrig.rightHandTransform.position, report.position) let D2 = Vector3.Distance(vrrig.leftHandTransform.position, report.position) where D1 < 0.35f || D2 < 0.35f select vrrig)
                    onReport?.Invoke(vrrig, report.transform.position);
            }
        }

        public static float antiReportDelay;
        public static void AntiReportDisconnect()
        {
            AntiReport((vrrig, position) =>
            {
                NetworkSystem.Instance.ReturnToSinglePlayer();

                if (!(Time.time > antiReportDelay)) return;
                antiReportDelay = Time.time + 1f;
                NotifiLib.SendNotification("<color=grey>[</color><color=purple>ANTI-REPORT</color><color=grey>]</color> " + GetPlayerFromVRRig(vrrig).NickName + " attempted to report you, you have been disconnected.");
            });
        }

        public static float antiReportReconnectDelay;
        public static void AntiReportReconnect()
        {
            AntiReport((vrrig, position) =>
            {
                if (!(Time.time > antiReportReconnectDelay)) return;
                antiReportReconnectDelay = Time.time + 1f;
                NotifiLib.SendNotification("<color=grey>[</color><color=purple>ANTI-REPORT</color><color=grey>]</color> " + GetPlayerFromVRRig(vrrig).NickName + " tried to report you, reconnecting...");
                RoomMods.Reconnect();
            });
        }

        public static float antiReportNotifyDelay;
        public static void AntiReportNotify()
        {
            AntiReport((vrrig, position) =>
            {
                if (!(Time.time > antiReportNotifyDelay)) return;
                antiReportNotifyDelay = Time.time + 1f;
                NotifiLib.SendNotification("<color=grey>[</color><color=purple>ANTI-REPORT</color><color=grey>]</color> " + GetPlayerFromVRRig(vrrig).NickName + " tried to report you.");
            });
        }

        public static void NoFinger()
        {
            ControllerInputPoller.instance.leftControllerGripFloat = 0f;
            ControllerInputPoller.instance.rightControllerGripFloat = 0f;
            ControllerInputPoller.instance.leftControllerIndexFloat = 0f;
            ControllerInputPoller.instance.rightControllerIndexFloat = 0f;
            ControllerInputPoller.instance.leftControllerPrimaryButton = false;
            ControllerInputPoller.instance.leftControllerSecondaryButton = false;
            ControllerInputPoller.instance.rightControllerPrimaryButton = false;
            ControllerInputPoller.instance.rightControllerSecondaryButton = false;
            ControllerInputPoller.instance.leftControllerPrimaryButtonTouch = false;
            ControllerInputPoller.instance.leftControllerSecondaryButtonTouch = false;
            ControllerInputPoller.instance.rightControllerPrimaryButtonTouch = false;
            ControllerInputPoller.instance.rightControllerSecondaryButtonTouch = false;
        }

        private static Vector3 fakePowerOffPos;
        private static Vector3 fakePowerOffVel;
        public static void FakePowerOff()
        {
            if (ControllerInputPoller.instance.leftControllerPrimaryButton)
            {
                if (fakePowerOffPos == Vector3.zero)
                {
                    fakePowerOffPos = GorillaTagger.Instance.rigidbody.transform.position;
                    fakePowerOffVel = GorillaTagger.Instance.rigidbody.linearVelocity;
                }
                VRRig.LocalRig.enabled = false;
                GorillaTagger.Instance.rigidbody.transform.position = fakePowerOffPos;
                GorillaTagger.Instance.rigidbody.linearVelocity = fakePowerOffVel;
            }
            else
            {
                fakePowerOffPos = Vector3.zero;
                VRRig.LocalRig.enabled = true;
            }
        }
    }
}
