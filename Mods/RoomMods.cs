using GorillaNetworking;
using System.Collections;
using UnityEngine;

namespace Qatual.Mods
{
    public class RoomMods
    {
        private static IEnumerator ReconnectCoroutine(string roomName)
        {
            NetworkSystem.Instance.ReturnToSinglePlayer();
            yield return new WaitUntil(() => !NetworkSystem.Instance.InRoom);
            yield return new WaitForSeconds(1f);
            PhotonNetworkController.Instance.AttemptToJoinSpecificRoom(roomName, JoinType.Solo);
        }

        public static void Reconnect()
        {
            if (!NetworkSystem.Instance.InRoom) return;
            string roomName = NetworkSystem.Instance.RoomName;
            GorillaTagger.Instance.StartCoroutine(ReconnectCoroutine(roomName));
        }

        private static IEnumerator JoinRandomCoroutine()
        {
            if (NetworkSystem.Instance.InRoom)
            {
                NetworkSystem.Instance.ReturnToSinglePlayer();
                yield return new WaitUntil(() => !NetworkSystem.Instance.InRoom);
                yield return new WaitForSeconds(1f);
            }
            GorillaNetworkJoinTrigger trigger = PhotonNetworkController.Instance.currentJoinTrigger
                ?? GorillaComputer.instance.GetJoinTriggerForZone("forest")
                ?? Object.FindAnyObjectByType<GorillaNetworkJoinTrigger>();
            if (trigger != null)
                PhotonNetworkController.Instance.AttemptToJoinPublicRoom(trigger, JoinType.Solo, null, false);
        }

        public static void JoinRandom()
        {
            GorillaTagger.Instance.StartCoroutine(JoinRandomCoroutine());
        }
    }
}
