using ExitGames.Client.Photon;
using GorillaNetworking;
using Photon.Pun;
using Photon.Realtime;
using Qatual.Classes;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR;
using static Qatual.Menu.Main;

namespace Qatual.Mods
{
    public class Detected
    {
        public static float masterDelay;
        public static float muteDelay;
        public static float customPropertyDelay;
        public static Dictionary<VRRig, int> viewIdArchive = new Dictionary<VRRig, int>();
        public static string changeName = "Qatual";

        private static void DestroyRig(VRRig rig, Hashtable hashtable = null, RaiseEventOptions opts = null, int viewID = -1)
        {
            PhotonView view = RigManager.GetPhotonViewFromVRRig(rig);
            if (view == null) return;
            hashtable ??= new Hashtable { { 0, viewID == -1 ? view.ViewID : viewID } };
            opts ??= new RaiseEventOptions { TargetActors = new[] { view.Owner.ActorNumber } };
            PhotonNetwork.NetworkingClient.OpRaiseEvent(204, hashtable, opts, SendOptions.SendReliable);
        }

        private static void DestroyPlayer(Player player)
        {
            var hashtable = new Hashtable { { 0, player.ActorNumber } };
            var opts = new RaiseEventOptions { TargetActors = new[] { player.ActorNumber } };
            PhotonNetwork.NetworkingClient.OpRaiseEvent(207, hashtable, opts, SendOptions.SendReliable);
        }


        public static void AutoSetMasterClient()
        {
            if (!PhotonNetwork.InRoom) return;
            if (!PhotonNetwork.IsMasterClient)
                PhotonNetwork.SetMasterClient(PhotonNetwork.LocalPlayer);
        }

        public static void SetMasterClientGun()
        {
            if (!ControllerInputPoller.instance.rightGrab) return;
            var ray = RenderGun().Ray;
            if (ControllerInputPoller.TriggerFloat(XRNode.RightHand) > 0.5f && ray.collider != null && Time.time > masterDelay)
            {
                VRRig target = ray.collider.GetComponentInParent<VRRig>();
                if (target != null && !target.isMyPlayer)
                {
                    Player p = RigManager.GetPlayerFromVRRig(target);
                    if (p != null) PhotonNetwork.SetMasterClient(p);
                    masterDelay = Time.time + 0.02f;
                }
            }
        }

        public static void SetMasterClientAll()
        {
            if (!PhotonNetwork.InRoom || Time.time <= masterDelay) return;
            foreach (VRRig rig in VRRigCache.ActiveRigs)
            {
                if (!rig.isMyPlayer)
                {
                    Player p = RigManager.GetPlayerFromVRRig(rig);
                    if (p != null) PhotonNetwork.SetMasterClient(p);
                }
            }
            masterDelay = Time.time + 0.02f;
        }

        public static void SetMasterClientAura()
        {
            if (!PhotonNetwork.InRoom || Time.time <= masterDelay) return;
            foreach (VRRig rig in VRRigCache.ActiveRigs)
            {
                if (!rig.isMyPlayer && Vector3.Distance(rig.transform.position, VRRig.LocalRig.transform.position) < 4f)
                {
                    Player p = RigManager.GetPlayerFromVRRig(rig);
                    if (p != null) PhotonNetwork.SetMasterClient(p);
                    masterDelay = Time.time + 0.02f;
                }
            }
        }

        public static void SetMasterClientOnTouch()
        {
            if (!PhotonNetwork.InRoom || Time.time <= masterDelay) return;
            foreach (VRRig rig in VRRigCache.ActiveRigs)
            {
                if (!rig.isMyPlayer && IsNearHands(rig, 0.35f))
                {
                    Player p = RigManager.GetPlayerFromVRRig(rig);
                    if (p != null) PhotonNetwork.SetMasterClient(p);
                    masterDelay = Time.time + 0.02f;
                }
            }
        }


        public static void CrashGun()
        {
            if (ControllerInputPoller.instance.rightGrab)
            {
                var ray = RenderGun().Ray;
                if (gunLocked && lockTarget != null)
                {
                    Player p = RigManager.GetPlayerFromVRRig(lockTarget);
                    if (p != null) { PhotonNetwork.SetMasterClient(p); PhotonNetwork.SetMasterClient(PhotonNetwork.LocalPlayer); }
                }
                if (ControllerInputPoller.TriggerFloat(XRNode.RightHand) > 0.5f && ray.collider != null)
                {
                    VRRig t = ray.collider.GetComponentInParent<VRRig>();
                    if (t != null && !t.isMyPlayer) { gunLocked = true; lockTarget = t; }
                }
            }
            else if (gunLocked) gunLocked = false;
        }

        public static void CrashAll()
        {
            if (!PhotonNetwork.InRoom) return;
            foreach (VRRig rig in VRRigCache.ActiveRigs)
            {
                if (!rig.isMyPlayer)
                {
                    Player p = RigManager.GetPlayerFromVRRig(rig);
                    if (p != null) { PhotonNetwork.SetMasterClient(p); PhotonNetwork.SetMasterClient(PhotonNetwork.LocalPlayer); }
                }
            }
        }

        public static void CrashAura()
        {
            if (!PhotonNetwork.InRoom) return;
            foreach (VRRig rig in VRRigCache.ActiveRigs)
            {
                if (!rig.isMyPlayer && Vector3.Distance(rig.transform.position, VRRig.LocalRig.transform.position) < 4f)
                {
                    Player p = RigManager.GetPlayerFromVRRig(rig);
                    if (p != null) { PhotonNetwork.SetMasterClient(p); PhotonNetwork.SetMasterClient(PhotonNetwork.LocalPlayer); }
                }
            }
        }

        public static void CrashOnTouch()
        {
            if (!PhotonNetwork.InRoom) return;
            foreach (VRRig rig in VRRigCache.ActiveRigs)
            {
                if (!rig.isMyPlayer && IsNearHands(rig, 0.35f))
                {
                    Player p = RigManager.GetPlayerFromVRRig(rig);
                    if (p != null) { PhotonNetwork.SetMasterClient(p); PhotonNetwork.SetMasterClient(PhotonNetwork.LocalPlayer); }
                }
            }
        }

        public static void CrashWhenTouched()
        {
            if (!PhotonNetwork.InRoom) return;
            foreach (VRRig rig in VRRigCache.ActiveRigs)
            {
                if (!rig.isMyPlayer && !rig.isOfflineVRRig &&
                    (Vector3.Distance(rig.rightHandTransform.position, VRRig.LocalRig.transform.position) <= 0.5f ||
                     Vector3.Distance(rig.leftHandTransform.position, VRRig.LocalRig.transform.position) <= 0.5f ||
                     Vector3.Distance(rig.transform.position, VRRig.LocalRig.transform.position) <= 0.5f))
                {
                    Player p = RigManager.GetPlayerFromVRRig(rig);
                    if (p != null) { PhotonNetwork.SetMasterClient(p); PhotonNetwork.SetMasterClient(PhotonNetwork.LocalPlayer); }
                }
            }
        }


        public static void GhostGun()
        {
            if (!ControllerInputPoller.instance.rightGrab) return;
            var ray = RenderGun().Ray;
            if (ControllerInputPoller.TriggerFloat(XRNode.RightHand) > 0.5f && ray.collider != null)
            {
                VRRig t = ray.collider.GetComponentInParent<VRRig>();
                if (t != null && !t.isMyPlayer)
                {
                    PhotonView view = RigManager.GetPhotonViewFromVRRig(t);
                    if (view != null)
                    {
                        viewIdArchive[t] = view.ViewID;
                        DestroyRig(t, new Hashtable { { 0, view.ViewID } },
                            new RaiseEventOptions { TargetActors = PhotonNetwork.PlayerList.Where(p => p != view.Owner).Select(p => p.ActorNumber).ToArray() });
                    }
                }
            }
        }

        public static void GhostAll()
        {
            foreach (VRRig rig in VRRigCache.ActiveRigs)
            {
                try
                {
                    if (!rig.isMyPlayer)
                    {
                        PhotonView view = RigManager.GetPhotonViewFromVRRig(rig);
                        if (view != null)
                        {
                            viewIdArchive[rig] = view.ViewID;
                            DestroyRig(rig, new Hashtable { { 0, view.ViewID } },
                                new RaiseEventOptions { TargetActors = PhotonNetwork.PlayerList.Where(p => p != view.Owner).Select(p => p.ActorNumber).ToArray() });
                        }
                    }
                }
                catch { }
            }
        }

        public static void GhostAura()
        {
            if (!PhotonNetwork.InRoom) return;
            foreach (VRRig rig in VRRigCache.ActiveRigs.ToList())
            {
                if (!rig.isMyPlayer && Vector3.Distance(rig.transform.position, VRRig.LocalRig.transform.position) < 4f)
                {
                    PhotonView view = RigManager.GetPhotonViewFromVRRig(rig);
                    if (view != null)
                    {
                        viewIdArchive[rig] = view.ViewID;
                        DestroyRig(rig, new Hashtable { { 0, view.ViewID } },
                            new RaiseEventOptions { TargetActors = PhotonNetwork.PlayerList.Where(p => p != view.Owner).Select(p => p.ActorNumber).ToArray() });
                    }
                }
            }
        }

        public static void GhostOnTouch()
        {
            if (!PhotonNetwork.InRoom) return;
            foreach (VRRig rig in VRRigCache.ActiveRigs)
            {
                if (!rig.isMyPlayer && IsNearHands(rig, 0.35f))
                {
                    PhotonView view = RigManager.GetPhotonViewFromVRRig(rig);
                    if (view != null)
                    {
                        viewIdArchive[rig] = view.ViewID;
                        DestroyRig(rig, new Hashtable { { 0, view.ViewID } },
                            new RaiseEventOptions { TargetActors = PhotonNetwork.PlayerList.Where(p => p != view.Owner).Select(p => p.ActorNumber).ToArray() });
                    }
                }
            }
        }

        public static void UnghostGun()
        {
            if (!ControllerInputPoller.instance.rightGrab) return;
            var ray = RenderGun().Ray;
            if (ControllerInputPoller.TriggerFloat(XRNode.RightHand) > 0.5f && ray.collider != null)
            {
                VRRig t = ray.collider.GetComponentInParent<VRRig>();
                if (t != null && !t.isMyPlayer && viewIdArchive.TryGetValue(t, out int viewID))
                    DestroyRig(t, null, null, viewID);
            }
        }

        public static void UnghostAll()
        {
            foreach (VRRig rig in VRRigCache.ActiveRigs)
            {
                if (viewIdArchive.TryGetValue(rig, out int viewID))
                    DestroyRig(rig, null, null, viewID);
            }
        }

        public static void UnghostAura()
        {
            if (!PhotonNetwork.InRoom) return;
            foreach (VRRig rig in VRRigCache.ActiveRigs)
            {
                if (!rig.isMyPlayer && Vector3.Distance(rig.transform.position, VRRig.LocalRig.transform.position) < 4f &&
                    viewIdArchive.TryGetValue(rig, out int viewID))
                    DestroyRig(rig, null, null, viewID);
            }
        }

        public static void UnghostOnTouch()
        {
            if (!PhotonNetwork.InRoom) return;
            foreach (VRRig rig in VRRigCache.ActiveRigs)
            {
                if (!rig.isMyPlayer && IsNearHands(rig, 0.35f) && viewIdArchive.TryGetValue(rig, out int viewID))
                    DestroyRig(rig, null, null, viewID);
            }
        }


        public static void LagGun()
        {
            if (ControllerInputPoller.instance.rightGrab)
            {
                var ray = RenderGun().Ray;
                if (gunLocked && lockTarget != null) { Player p = RigManager.GetPlayerFromVRRig(lockTarget); if (p != null) DestroyPlayer(p); }
                if (ControllerInputPoller.TriggerFloat(XRNode.RightHand) > 0.5f && ray.collider != null)
                {
                    VRRig t = ray.collider.GetComponentInParent<VRRig>();
                    if (t != null && !t.isMyPlayer) { gunLocked = true; lockTarget = t; }
                }
            }
            else if (gunLocked) gunLocked = false;
        }

        public static void LagAll()
        {
            foreach (VRRig rig in VRRigCache.ActiveRigs)
            {
                if (!rig.isMyPlayer) { Player p = RigManager.GetPlayerFromVRRig(rig); if (p != null) DestroyPlayer(p); }
            }
        }

        public static void LagAura()
        {
            if (!PhotonNetwork.InRoom) return;
            foreach (VRRig rig in VRRigCache.ActiveRigs)
            {
                if (!rig.isMyPlayer && Vector3.Distance(rig.transform.position, VRRig.LocalRig.transform.position) < 4f)
                { Player p = RigManager.GetPlayerFromVRRig(rig); if (p != null) DestroyPlayer(p); }
            }
        }

        public static void LagOnTouch()
        {
            if (!PhotonNetwork.InRoom) return;
            foreach (VRRig rig in VRRigCache.ActiveRigs)
            {
                if (!rig.isMyPlayer && IsNearHands(rig, 0.35f)) { Player p = RigManager.GetPlayerFromVRRig(rig); if (p != null) DestroyPlayer(p); }
            }
        }


        public static void MuteGun()
        {
            if (ControllerInputPoller.instance.rightGrab)
            {
                var ray = RenderGun().Ray;
                if (gunLocked && lockTarget != null && Time.time > muteDelay)
                { Player p = RigManager.GetPlayerFromVRRig(lockTarget); if (p != null) DestroyPlayer(p); muteDelay = Time.time + 0.15f; }
                if (ControllerInputPoller.TriggerFloat(XRNode.RightHand) > 0.5f && ray.collider != null)
                {
                    VRRig t = ray.collider.GetComponentInParent<VRRig>();
                    if (t != null && !t.isMyPlayer) { gunLocked = true; lockTarget = t; }
                }
            }
            else if (gunLocked) gunLocked = false;
        }

        public static void MuteAll()
        {
            if (Time.time <= muteDelay) return;
            foreach (VRRig rig in VRRigCache.ActiveRigs)
            {
                if (!rig.isMyPlayer) { Player p = RigManager.GetPlayerFromVRRig(rig); if (p != null) DestroyPlayer(p); }
            }
            muteDelay = Time.time + 0.15f;
        }

        public static void MuteAura()
        {
            if (!PhotonNetwork.InRoom || Time.time <= muteDelay) return;
            foreach (VRRig rig in VRRigCache.ActiveRigs)
            {
                if (!rig.isMyPlayer && Vector3.Distance(rig.transform.position, VRRig.LocalRig.transform.position) < 4f)
                { Player p = RigManager.GetPlayerFromVRRig(rig); if (p != null) DestroyPlayer(p); muteDelay = Time.time + 0.15f; }
            }
        }

        public static void MuteOnTouch()
        {
            if (!PhotonNetwork.InRoom || Time.time <= muteDelay) return;
            foreach (VRRig rig in VRRigCache.ActiveRigs)
            {
                if (!rig.isMyPlayer && IsNearHands(rig, 0.35f))
                { Player p = RigManager.GetPlayerFromVRRig(rig); if (p != null) DestroyPlayer(p); muteDelay = Time.time + 0.15f; }
            }
        }


        public static void ChangeNameGun()
        {
            if (ControllerInputPoller.instance.rightGrab)
            {
                var ray = RenderGun().Ray;
                if (gunLocked && lockTarget != null)
                {
                    Player p = RigManager.GetPlayerFromVRRig(lockTarget);
                    if (p != null) PhotonNetwork.CurrentRoom.LoadBalancingClient.OpSetPropertiesOfActor(p.ActorNumber, new Hashtable { [byte.MaxValue] = changeName });
                }
                if (ControllerInputPoller.TriggerFloat(XRNode.RightHand) > 0.5f && ray.collider != null)
                {
                    VRRig t = ray.collider.GetComponentInParent<VRRig>();
                    if (t != null && !t.isMyPlayer) { gunLocked = true; lockTarget = t; }
                }
            }
            else if (gunLocked) gunLocked = false;
        }

        public static void ChangeNameAll()
        {
            foreach (Player player in PhotonNetwork.PlayerListOthers)
                PhotonNetwork.CurrentRoom.LoadBalancingClient.OpSetPropertiesOfActor(player.ActorNumber, new Hashtable { [byte.MaxValue] = changeName });
        }

        public static void ChangeNameAura()
        {
            if (!PhotonNetwork.InRoom) return;
            foreach (VRRig rig in VRRigCache.ActiveRigs)
            {
                if (!rig.isMyPlayer && Vector3.Distance(rig.transform.position, VRRig.LocalRig.transform.position) < 4f)
                {
                    Player p = RigManager.GetPlayerFromVRRig(rig);
                    if (p != null) PhotonNetwork.CurrentRoom.LoadBalancingClient.OpSetPropertiesOfActor(p.ActorNumber, new Hashtable { [byte.MaxValue] = changeName });
                }
            }
        }

        public static void ChangeNameOnTouch()
        {
            if (!PhotonNetwork.InRoom) return;
            foreach (VRRig rig in VRRigCache.ActiveRigs)
            {
                if (!rig.isMyPlayer && IsNearHands(rig, 0.35f))
                {
                    Player p = RigManager.GetPlayerFromVRRig(rig);
                    if (p != null) PhotonNetwork.CurrentRoom.LoadBalancingClient.OpSetPropertiesOfActor(p.ActorNumber, new Hashtable { [byte.MaxValue] = changeName });
                }
            }
        }


        private static string BannedName() =>
            GorillaComputer.instance.anywhereTwoWeek[Random.Range(0, GorillaComputer.instance.anywhereTwoWeek.Length)];

        public static void BanGun()
        {
            if (ControllerInputPoller.instance.rightGrab)
            {
                var ray = RenderGun().Ray;
                if (gunLocked && lockTarget != null)
                {
                    Player p = RigManager.GetPlayerFromVRRig(lockTarget);
                    if (p != null) { PhotonNetwork.CurrentRoom.LoadBalancingClient.OpSetPropertiesOfActor(p.ActorNumber, new Hashtable { [byte.MaxValue] = BannedName() }); MonkeAgent.instance.SendReport("hacking", p.UserId, p.NickName); }
                }
                if (ControllerInputPoller.TriggerFloat(XRNode.RightHand) > 0.5f && ray.collider != null)
                {
                    VRRig t = ray.collider.GetComponentInParent<VRRig>();
                    if (t != null && !t.isMyPlayer) { gunLocked = true; lockTarget = t; }
                }
            }
            else if (gunLocked) gunLocked = false;
        }

        public static void BanAll()
        {
            foreach (Player player in PhotonNetwork.PlayerListOthers)
            { PhotonNetwork.CurrentRoom.LoadBalancingClient.OpSetPropertiesOfActor(player.ActorNumber, new Hashtable { [byte.MaxValue] = BannedName() }); MonkeAgent.instance.SendReport("hacking", player.UserId, player.NickName); }
        }

        public static void BanAura()
        {
            if (!PhotonNetwork.InRoom) return;
            foreach (VRRig rig in VRRigCache.ActiveRigs)
            {
                if (!rig.isMyPlayer && Vector3.Distance(rig.transform.position, VRRig.LocalRig.transform.position) < 4f)
                {
                    Player p = RigManager.GetPlayerFromVRRig(rig);
                    if (p != null) { PhotonNetwork.CurrentRoom.LoadBalancingClient.OpSetPropertiesOfActor(p.ActorNumber, new Hashtable { [byte.MaxValue] = BannedName() }); MonkeAgent.instance.SendReport("hacking", p.UserId, p.NickName); }
                }
            }
        }

        public static void BanOnTouch()
        {
            if (!PhotonNetwork.InRoom) return;
            foreach (VRRig rig in VRRigCache.ActiveRigs)
            {
                if (!rig.isMyPlayer && IsNearHands(rig, 0.35f))
                {
                    Player p = RigManager.GetPlayerFromVRRig(rig);
                    if (p != null) { PhotonNetwork.CurrentRoom.LoadBalancingClient.OpSetPropertiesOfActor(p.ActorNumber, new Hashtable { [byte.MaxValue] = BannedName() }); MonkeAgent.instance.SendReport("hacking", p.UserId, p.NickName); }
                }
            }
        }


        public static void GamemodeIncludeGun()
        {
            if (!ControllerInputPoller.instance.rightGrab) return;
            var ray = RenderGun().Ray;
            if (ControllerInputPoller.TriggerFloat(XRNode.RightHand) > 0.5f && ray.collider != null && Time.time > customPropertyDelay)
            {
                VRRig t = ray.collider.GetComponentInParent<VRRig>();
                if (t != null && !t.isMyPlayer)
                {
                    customPropertyDelay = Time.time + 0.25f;
                    Player p = RigManager.GetPlayerFromVRRig(t);
                    if (p != null) p.SetCustomProperties(new Hashtable { { "didTutorial", true } });
                }
            }
        }

        public static void GamemodeIncludeAll()
        {
            foreach (Player player in PhotonNetwork.PlayerList)
                player.SetCustomProperties(new Hashtable { { "didTutorial", true } });
        }

        public static void GamemodeIncludeAura()
        {
            if (!PhotonNetwork.InRoom) return;
            foreach (VRRig rig in VRRigCache.ActiveRigs)
            {
                if (!rig.isMyPlayer && Vector3.Distance(rig.transform.position, VRRig.LocalRig.transform.position) < 4f)
                { Player p = RigManager.GetPlayerFromVRRig(rig); if (p != null) p.SetCustomProperties(new Hashtable { { "didTutorial", true } }); }
            }
        }

        public static void GamemodeIncludeOnTouch()
        {
            if (!PhotonNetwork.InRoom) return;
            foreach (VRRig rig in VRRigCache.ActiveRigs)
            {
                if (!rig.isMyPlayer && IsNearHands(rig, 0.35f))
                { Player p = RigManager.GetPlayerFromVRRig(rig); if (p != null) p.SetCustomProperties(new Hashtable { { "didTutorial", true } }); }
            }
        }

        public static void GamemodeExcludeGun()
        {
            if (!ControllerInputPoller.instance.rightGrab) return;
            var ray = RenderGun().Ray;
            if (ControllerInputPoller.TriggerFloat(XRNode.RightHand) > 0.5f && ray.collider != null && Time.time > customPropertyDelay)
            {
                VRRig t = ray.collider.GetComponentInParent<VRRig>();
                if (t != null && !t.isMyPlayer)
                {
                    customPropertyDelay = Time.time + 0.25f;
                    Player p = RigManager.GetPlayerFromVRRig(t);
                    if (p != null) p.SetCustomProperties(new Hashtable { { "didTutorial", false } });
                }
            }
        }

        public static void GamemodeExcludeAll()
        {
            foreach (Player player in PhotonNetwork.PlayerList)
                player.SetCustomProperties(new Hashtable { { "didTutorial", false } });
        }

        public static void GamemodeExcludeAura()
        {
            if (!PhotonNetwork.InRoom) return;
            foreach (VRRig rig in VRRigCache.ActiveRigs)
            {
                if (!rig.isMyPlayer && Vector3.Distance(rig.transform.position, VRRig.LocalRig.transform.position) < 4f)
                { Player p = RigManager.GetPlayerFromVRRig(rig); if (p != null) p.SetCustomProperties(new Hashtable { { "didTutorial", false } }); }
            }
        }

        public static void GamemodeExcludeOnTouch()
        {
            if (!PhotonNetwork.InRoom) return;
            foreach (VRRig rig in VRRigCache.ActiveRigs)
            {
                if (!rig.isMyPlayer && IsNearHands(rig, 0.35f))
                { Player p = RigManager.GetPlayerFromVRRig(rig); if (p != null) p.SetCustomProperties(new Hashtable { { "didTutorial", false } }); }
            }
        }

        public static void BreakGamemode(bool breaking)
        {
            foreach (Player player in PhotonNetwork.PlayerList)
                player.SetCustomProperties(new Hashtable { { "didTutorial", !breaking } });
        }


        private static bool IsNearHands(VRRig rig, float dist) =>
            Vector3.Distance(rig.transform.position, VRRig.LocalRig.rightHandTransform.position) <= dist ||
            Vector3.Distance(rig.transform.position, VRRig.LocalRig.leftHandTransform.position) <= dist;
    }
}
