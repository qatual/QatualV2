using BepInEx;
using UnityEngine;

namespace Qatual
{
    [System.ComponentModel.Description(PluginInfo.Description)]
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    public class HarmonyPatches : BaseUnityPlugin
    {
        private void Awake() =>
            GorillaTagger.OnPlayerSpawned(OnPlayerSpawned);

        public void OnPlayerSpawned() =>
            Patches.PatchHandler.PatchAll();

        private GUIStyle titleStyle;
        private GUIStyle subtitleStyle;

        private void InitStyles()
        {
            if (titleStyle != null) return;

            Font font = Font.CreateDynamicFontFromOSFont(new string[] { "Quicksand Bold", "Quicksand", "Arial Bold", "Arial" }, 18);

            titleStyle = new GUIStyle(GUI.skin.label)
            {
                font = font,
                fontStyle = FontStyle.Bold,
                fontSize = 18,
                alignment = TextAnchor.LowerRight,
                normal = { textColor = new Color(1f, 1f, 1f, 0.70f) }
            };

            subtitleStyle = new GUIStyle(GUI.skin.label)
            {
                font = font,
                fontStyle = FontStyle.Normal,
                fontSize = 13,
                alignment = TextAnchor.LowerRight,
                normal = { textColor = new Color(1f, 1f, 1f, 0.50f) }
            };
        }

        private void OnGUI()
        {
            InitStyles();

            const int pad = 14;
            const int w = 240;
            const int lineH = 22;

            GUI.Label(new Rect(Screen.width - w - pad, Screen.height - lineH * 2 - pad, w, lineH),
                $"Qatual V2.{PluginInfo.Version}", titleStyle);

            GUI.Label(new Rect(Screen.width - w - pad, Screen.height - lineH - pad, w, lineH),
                "discord.gg/unblockings", subtitleStyle);
        }
    }
}
