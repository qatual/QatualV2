using GorillaGameModes;
using GorillaLocomotion;
using GorillaNetworking;
using Photon.Pun;
using Qatual.Classes;
using System.Linq;
using UnityEngine;
using UnityEngine.XR;
using static Qatual.Menu.Main;

namespace Qatual.Mods
{
    public class TagMods
    {
        private static GorillaTagManager TagMgr => GorillaGameManager.instance as GorillaTagManager;

        private static bool IsTagged(VRRig rig)
        {
            var tm = TagMgr;
            if (tm == null) return false;
            var player = RigManager.GetPlayerFromVRRig(rig);
            return player != null && tm.currentInfected.Contains(NetPlayer.Get(player));
        }

        private static bool IsLocalTagged()
        {
            var tm = TagMgr;
            return tm != null && tm.currentInfected.Contains(NetworkSystem.Instance.LocalPlayer);
        }

        private static float reportDelay;
        private static void SendTagReport(VRRig target)
        {
            if (Time.time < reportDelay) return;
            reportDelay = Time.time + 0.1f;
            var player = RigManager.GetPlayerFromVRRig(target);
            if (player != null)
                GameMode.ReportTag(NetPlayer.Get(player));
        }

        public static void TagSelf()
        {
            if (IsLocalTagged()) { VRRig.LocalRig.enabled = true; return; }
            var tm = TagMgr;
            if (tm == null) return;

            if (PhotonNetwork.IsMasterClient)
            {
                tm.AddInfectedPlayer(NetworkSystem.Instance.LocalPlayer, false);
                VRRig.LocalRig.enabled = true;
                return;
            }

            var nearest = VRRigCache.ActiveRigs
                .Where(r => !r.isMyPlayer && IsTagged(r))
                .OrderBy(r => Vector3.Distance(r.transform.position, VRRig.LocalRig.transform.position))
                .FirstOrDefault();

            if (nearest == null) return;
            VRRig.LocalRig.enabled = false;
            VRRig.LocalRig.transform.position = nearest.rightHandTransform.position;
        }

        public static void UntagSelf()
        {
            var tm = TagMgr;
            if (tm == null) return;

            if (PhotonNetwork.IsMasterClient)
                tm.currentInfected.Remove(NetworkSystem.Instance.LocalPlayer);
            else
                RoomMods.Reconnect();

            GTPlayer.Instance.disableMovement = false;
            VRRig.LocalRig.enabled = true;
        }

        public static void TagAll()
        {
            if (!PhotonNetwork.IsMasterClient) return;
            var tm = TagMgr;
            if (tm == null) return;
            foreach (var player in PhotonNetwork.PlayerList)
                tm.AddInfectedPlayer(NetPlayer.Get(player), false);
        }

        public static void UntagAll()
        {
            if (!PhotonNetwork.IsMasterClient) return;
            var tm = TagMgr;
            tm?.currentInfected.Clear();
        }

        public static void TagGun()
        {
            if (ControllerInputPoller.instance.rightGrab)
            {
                var gunData = RenderGun();

                if (gunLocked && lockTarget != null)
                {
                    if (PhotonNetwork.IsMasterClient)
                        TagMgr?.AddInfectedPlayer(NetPlayer.Get(RigManager.GetPlayerFromVRRig(lockTarget)), false);
                    else if (IsLocalTagged())
                    {
                        VRRig.LocalRig.enabled = false;
                        VRRig.LocalRig.transform.position = lockTarget.transform.position - new Vector3(0f, 3f, 0f);
                        SendTagReport(lockTarget);
                    }
                }

                if (ControllerInputPoller.TriggerFloat(XRNode.RightHand) > 0.5f && gunData.Ray.collider != null)
                {
                    VRRig t = gunData.Ray.collider.GetComponentInParent<VRRig>();
                    if (t != null && !t.isMyPlayer && !IsTagged(t)) { gunLocked = true; lockTarget = t; }
                }
            }
            else if (gunLocked)
            {
                gunLocked = false;
                VRRig.LocalRig.enabled = true;
            }
        }

        public static void UntagGun()
        {
            if (!PhotonNetwork.IsMasterClient || !ControllerInputPoller.instance.rightGrab) return;
            var ray = RenderGun().Ray;
            if (ControllerInputPoller.TriggerFloat(XRNode.RightHand) > 0.5f && ray.collider != null)
            {
                VRRig t = ray.collider.GetComponentInParent<VRRig>();
                if (t == null || t.isMyPlayer) return;
                var tm = TagMgr;
                var player = RigManager.GetPlayerFromVRRig(t);
                if (tm != null && player != null)
                    tm.currentInfected.Remove(NetPlayer.Get(player));
            }
        }

        private static float tagAuraDelay;
        public static float tagAuraRange = 3f;

        public static void TagAura()
        {
            if (!IsLocalTagged() && !PhotonNetwork.IsMasterClient) return;
            if (Time.time < tagAuraDelay) return;
            tagAuraDelay = Time.time + 0.1f;

            foreach (VRRig rig in VRRigCache.ActiveRigs)
            {
                if (rig.isMyPlayer || IsTagged(rig)) continue;
                if (Vector3.Distance(rig.transform.position, VRRig.LocalRig.transform.position) > tagAuraRange) continue;

                if (PhotonNetwork.IsMasterClient)
                    TagMgr?.AddInfectedPlayer(NetPlayer.Get(RigManager.GetPlayerFromVRRig(rig)), false);
                else
                {
                    VRRig.LocalRig.enabled = false;
                    VRRig.LocalRig.transform.position = rig.transform.position;
                    SendTagReport(rig);
                }
            }
        }

        private static float antiTagDelay;

        public static void AntiTag()
        {
            if (!IsLocalTagged()) { VRRig.LocalRig.enabled = true; return; }
            var tm = TagMgr;
            if (tm == null) return;

            if (PhotonNetwork.IsMasterClient)
                tm.currentInfected.Remove(NetworkSystem.Instance.LocalPlayer);
            else if (Time.time > antiTagDelay)
            {
                antiTagDelay = Time.time + 3f;
                RoomMods.Reconnect();
            }
        }
    }
}
