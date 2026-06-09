using Qatual.Classes;
using Qatual.Mods;
using static Qatual.Menu.Main;
using static Qatual.Settings;

namespace Qatual.Menu
{
    public class Buttons
    {
        public static ButtonInfo[][] buttons = new ButtonInfo[][]
        {
            new ButtonInfo[] {
                new ButtonInfo { buttonText = "Join Discord", method =() => UnityEngine.Application.OpenURL("https://discord.gg/unblockings"), isTogglable = false, toolTip = "Opens the Discord server in your browser."},
                new ButtonInfo { buttonText = "Settings", method =() => currentCategory = 1, isTogglable = false, toolTip = "Opens the main settings page for the menu."},

                new ButtonInfo { buttonText = "Room Mods", method =() => currentCategory = 4, isTogglable = false, toolTip = "Opens the room mods tab."},
                new ButtonInfo { buttonText = "Movement Mods", method =() => currentCategory = 5, isTogglable = false, toolTip = "Opens the movement mods tab."},
                new ButtonInfo { buttonText = "Safety Mods", method =() => currentCategory = 6, isTogglable = false, toolTip = "Opens the safety mods tab."},
                new ButtonInfo { buttonText = "Advantages", method =() => currentCategory = 7, isTogglable = false, toolTip = "Opens the advantages mods tab."},
                new ButtonInfo { buttonText = "Detected Mods", method =() => currentCategory = 8, isTogglable = false, toolTip = "Opens the detected mods tab."},
                new ButtonInfo { buttonText = "Tag Mods", method =() => currentCategory = 9, isTogglable = false, toolTip = "Opens the tag mods tab."},
            },

            new ButtonInfo[] {
                new ButtonInfo { buttonText = "Return to Main", method =() => currentCategory = 0, isTogglable = false, toolTip = "Returns to the main page of the menu."},
                new ButtonInfo { buttonText = "Menu", method =() => currentCategory = 2, isTogglable = false, toolTip = "Opens the settings for the menu."},
                new ButtonInfo { buttonText = "Movement", method =() => currentCategory = 3, isTogglable = false, toolTip = "Opens the movement settings for the menu."},
            },

            new ButtonInfo[] {
                new ButtonInfo { buttonText = "Return to Settings", method =() => currentCategory = 1, isTogglable = false, toolTip = "Returns to the main settings page for the menu."},
                new ButtonInfo { buttonText = "Right Hand", enableMethod =() => rightHanded = true, disableMethod =() => rightHanded = false, toolTip = "Puts the menu on your right hand."},
                new ButtonInfo { buttonText = "Notifications", enableMethod =() => disableNotifications = false, disableMethod =() => disableNotifications = true, enabled = !disableNotifications, toolTip = "Toggles the notifications."},
                new ButtonInfo { buttonText = "FPS Counter", enableMethod =() => fpsCounter = true, disableMethod =() => fpsCounter = false, enabled = fpsCounter, toolTip = "Toggles the FPS counter."},
                new ButtonInfo { buttonText = "Disconnect Button", enableMethod =() => disconnectButton = true, disableMethod =() => disconnectButton = false, enabled = disconnectButton, toolTip = "Toggles the disconnect button."},
            },

            new ButtonInfo[] {
                new ButtonInfo { buttonText = "Return to Settings", method =() => currentCategory = 1, isTogglable = false, toolTip = "Returns to the main settings page for the menu."},

                new ButtonInfo { buttonText = "Change Fly Speed", overlapText = "Change Fly Speed [Normal]", method =() => Mods.Settings.Movement.ChangeFlySpeed(), isTogglable = false, toolTip = "Changes the speed of the fly mod."},
            },

            new ButtonInfo[] {
                new ButtonInfo { buttonText = "Return to Main", method =() => currentCategory = 0, isTogglable = false, toolTip = "Returns to the main page of the menu."},

                new ButtonInfo { buttonText = "Disconnect", method =() => NetworkSystem.Instance.ReturnToSinglePlayer(), isTogglable = false, toolTip = "Disconnects you from the room."},
                new ButtonInfo { buttonText = "Reconnect", method =() => RoomMods.Reconnect(), isTogglable = false, toolTip = "Disconnects and rejoins the same room."},
                new ButtonInfo { buttonText = "Join Random", method =() => RoomMods.JoinRandom(), isTogglable = false, toolTip = "Joins a random public room."},
            },

            new ButtonInfo[] {
                new ButtonInfo { buttonText = "Return to Main", method =() => currentCategory = 0, isTogglable = false, toolTip = "Returns to the main page of the menu."},

                new ButtonInfo { buttonText = "Platforms", method =() => Movement.Platforms(), toolTip = "Spawns platforms on your hands when pressing grip."},
                new ButtonInfo { buttonText = "Fly", method =() => Movement.Fly(), toolTip = "Sends you forward when holding A."},
                new ButtonInfo { buttonText = "Teleport Gun", method =() => Movement.TeleportGun(), toolTip = "Teleports you to wherever your pointer is when pressing trigger."},
                new ButtonInfo { buttonText = "WASD Fly", method = () => Movement.WASDFly(), toolTip = "WASD to fly, Space to go up, Ctrl to go down."},
                new ButtonInfo { buttonText = "Long Arms", enableMethod = () => Movement.EnableLongArms(), disableMethod = () => Movement.DisableLongArms(), toolTip = "you get really long arms idk???"},
                new ButtonInfo { buttonText = "Mosa Speed", enableMethod = () => Movement.EnableMosa(), disableMethod = () => Movement.DisableMosa(), toolTip = "Extreme jump speed boost."},
                new ButtonInfo { buttonText = "High Jump", enableMethod = () => Movement.EnableHighJump(), disableMethod = () => Movement.DisableHighJump(), toolTip = "Boosts jump multiplier for higher jumps."},
                new ButtonInfo { buttonText = "Low Gravity", method = () => Movement.LowGravity(), toolTip = "Reduces gravity so you float higher."},
                new ButtonInfo { buttonText = "Anti Gravity", method = () => Movement.AntiGravity(), toolTip = "Cancels gravity so you float freely."},
                new ButtonInfo { buttonText = "Speed Boost", method = () => Movement.SpeedBoost(), toolTip = "Multiplies your velocity each frame for increasing speed."},
                new ButtonInfo { buttonText = "Bhop Speed", method = () => Movement.BhopSpeed(), toolTip = "Adds horizontal force when moving for bunny-hop style speed."},
                new ButtonInfo { buttonText = "No Clip", enableMethod = () => Movement.EnableNoClip(), method = () => Movement.NoClipUpdate(), disableMethod = () => Movement.DisableNoClip(), toolTip = "Disables collision. Use WASD to fly through walls."},
                new ButtonInfo { buttonText = "Freeze", enableMethod = () => Movement.EnableFreeze(), method = () => Movement.FreezeUpdate(), disableMethod = () => Movement.DisableFreeze(), toolTip = "Freezes your position in place."},
            },

            new ButtonInfo[] {
                new ButtonInfo { buttonText = "Return to Main", method =() => currentCategory = 0, isTogglable = false, toolTip = "Returns to the main page of the menu."},

                new ButtonInfo { buttonText = "Anti Report [DC]", method =() => Safety.AntiReportDisconnect(), toolTip = "Disconnects you when someone tries to report you."},
                new ButtonInfo { buttonText = "Anti Report [RC]", method =() => Safety.AntiReportReconnect(), toolTip = "Reconnects you when someone tries to report you."},
                new ButtonInfo { buttonText = "Anti Report [Notify]", method =() => Safety.AntiReportNotify(), toolTip = "Notifies you when someone tries to report you."},
                new ButtonInfo { buttonText = "No Finger", method =() => Safety.NoFinger(), toolTip = "Hides your finger tracking / zeroes controller inputs."},
                new ButtonInfo { buttonText = "Fake Power Off", method =() => Safety.FakePowerOff(), toolTip = "Hold left primary to freeze your position as if powered off."},
            },
            new ButtonInfo[] {
                new ButtonInfo { buttonText = "Return to Main", method =() => currentCategory = 0, isTogglable = false, toolTip = "Returns to the main page of the menu."},

                new ButtonInfo { buttonText = "Long Arms", enableMethod = () => Movement.EnableLongArms(), disableMethod = () => Movement.DisableLongArms(), toolTip = "you get really long arms idk???"},
                new ButtonInfo { buttonText = "Mosa Speed", enableMethod = () => Movement.EnableMosa(), disableMethod = () => Movement.DisableMosa(), toolTip = "You get speed as if you are moza, which is fast"},
           },

            new ButtonInfo[] {
                new ButtonInfo { buttonText = "Return to Main", method =() => currentCategory = 0, isTogglable = false, toolTip = "Returns to the main page of the menu."},

                new ButtonInfo { buttonText = "Auto Master", method =() => Detected.AutoSetMasterClient(), isTogglable = false, toolTip = "Instantly sets you as master client."},
                new ButtonInfo { buttonText = "Set Master Gun", method =() => Detected.SetMasterClientGun(), toolTip = "Point and shoot to set someone as master client."},
                new ButtonInfo { buttonText = "Set Master All", method =() => Detected.SetMasterClientAll(), toolTip = "Sets every player as master client rapidly."},
                new ButtonInfo { buttonText = "Set Master Aura", method =() => Detected.SetMasterClientAura(), toolTip = "Sets nearby players as master client."},
                new ButtonInfo { buttonText = "Set Master Touch", method =() => Detected.SetMasterClientOnTouch(), toolTip = "Sets players you touch as master client."},
                new ButtonInfo { buttonText = "Crash Gun", method =() => Detected.CrashGun(), toolTip = "Point and lock onto a player to crash them."},
                new ButtonInfo { buttonText = "Crash All", method =() => Detected.CrashAll(), isTogglable = false, toolTip = "Crashes all players in the room."},
                new ButtonInfo { buttonText = "Crash Aura", method =() => Detected.CrashAura(), toolTip = "Crashes players near you."},
                new ButtonInfo { buttonText = "Crash On Touch", method =() => Detected.CrashOnTouch(), toolTip = "Crashes players you touch."},
                new ButtonInfo { buttonText = "Crash When Touched", method =() => Detected.CrashWhenTouched(), toolTip = "Crashes players who touch you."},
                new ButtonInfo { buttonText = "Ghost Gun", method =() => Detected.GhostGun(), toolTip = "Point and shoot to ghost a player for everyone else."},
                new ButtonInfo { buttonText = "Ghost All", method =() => Detected.GhostAll(), isTogglable = false, toolTip = "Ghosts all players for each other."},
                new ButtonInfo { buttonText = "Ghost Aura", method =() => Detected.GhostAura(), toolTip = "Ghosts players near you."},
                new ButtonInfo { buttonText = "Ghost On Touch", method =() => Detected.GhostOnTouch(), toolTip = "Ghosts players you touch."},
                new ButtonInfo { buttonText = "Unghost Gun", method =() => Detected.UnghostGun(), toolTip = "Point and shoot to unghost a player."},
                new ButtonInfo { buttonText = "Unghost All", method =() => Detected.UnghostAll(), isTogglable = false, toolTip = "Unghosts all previously ghosted players."},
                new ButtonInfo { buttonText = "Unghost Aura", method =() => Detected.UnghostAura(), toolTip = "Unghosts players near you."},
                new ButtonInfo { buttonText = "Unghost On Touch", method =() => Detected.UnghostOnTouch(), toolTip = "Unghosts players you touch."},
                new ButtonInfo { buttonText = "Lag Gun", method =() => Detected.LagGun(), toolTip = "Point and lock onto a player to lag them."},
                new ButtonInfo { buttonText = "Lag All", method =() => Detected.LagAll(), toolTip = "Lags all players in the room."},
                new ButtonInfo { buttonText = "Lag Aura", method =() => Detected.LagAura(), toolTip = "Lags players near you."},
                new ButtonInfo { buttonText = "Lag On Touch", method =() => Detected.LagOnTouch(), toolTip = "Lags players you touch."},
                new ButtonInfo { buttonText = "Mute Gun", method =() => Detected.MuteGun(), toolTip = "Point and lock onto a player to mute them."},
                new ButtonInfo { buttonText = "Mute All", method =() => Detected.MuteAll(), toolTip = "Mutes all players in the room."},
                new ButtonInfo { buttonText = "Mute Aura", method =() => Detected.MuteAura(), toolTip = "Mutes players near you."},
                new ButtonInfo { buttonText = "Mute On Touch", method =() => Detected.MuteOnTouch(), toolTip = "Mutes players you touch."},
                new ButtonInfo { buttonText = "Change Name Gun", method =() => Detected.ChangeNameGun(), toolTip = "Point and lock to rename a player to 'Qatual'."},
                new ButtonInfo { buttonText = "Change Name All", method =() => Detected.ChangeNameAll(), isTogglable = false, toolTip = "Renames all players to 'Qatual'."},
                new ButtonInfo { buttonText = "Change Name Aura", method =() => Detected.ChangeNameAura(), toolTip = "Renames players near you to 'Qatual'."},
                new ButtonInfo { buttonText = "Change Name Touch", method =() => Detected.ChangeNameOnTouch(), toolTip = "Renames players you touch to 'Qatual'."},
                new ButtonInfo { buttonText = "Ban Gun", method =() => Detected.BanGun(), toolTip = "Point and lock to ban a player."},
                new ButtonInfo { buttonText = "Ban All", method =() => Detected.BanAll(), isTogglable = false, toolTip = "Bans all players in the room."},
                new ButtonInfo { buttonText = "Ban Aura", method =() => Detected.BanAura(), toolTip = "Bans players near you."},
                new ButtonInfo { buttonText = "Ban On Touch", method =() => Detected.BanOnTouch(), toolTip = "Bans players you touch."},
                new ButtonInfo { buttonText = "Include GM Gun", method =() => Detected.GamemodeIncludeGun(), toolTip = "Point and shoot to include a player in gamemodes."},
                new ButtonInfo { buttonText = "Include GM All", method =() => Detected.GamemodeIncludeAll(), isTogglable = false, toolTip = "Includes all players in gamemodes."},
                new ButtonInfo { buttonText = "Include GM Aura", method =() => Detected.GamemodeIncludeAura(), toolTip = "Includes nearby players in gamemodes."},
                new ButtonInfo { buttonText = "Include GM Touch", method =() => Detected.GamemodeIncludeOnTouch(), toolTip = "Includes players you touch in gamemodes."},
                new ButtonInfo { buttonText = "Exclude GM Gun", method =() => Detected.GamemodeExcludeGun(), toolTip = "Point and shoot to exclude a player from gamemodes."},
                new ButtonInfo { buttonText = "Exclude GM All", method =() => Detected.GamemodeExcludeAll(), isTogglable = false, toolTip = "Excludes all players from gamemodes."},
                new ButtonInfo { buttonText = "Exclude GM Aura", method =() => Detected.GamemodeExcludeAura(), toolTip = "Excludes nearby players from gamemodes."},
                new ButtonInfo { buttonText = "Exclude GM Touch", method =() => Detected.GamemodeExcludeOnTouch(), toolTip = "Excludes players you touch from gamemodes."},
                new ButtonInfo { buttonText = "Break Gamemode", enableMethod =() => Detected.BreakGamemode(true), disableMethod =() => Detected.BreakGamemode(false), toolTip = "Breaks the gamemode for all players."},
            },

            new ButtonInfo[] {
                new ButtonInfo { buttonText = "Return to Main", method =() => currentCategory = 0, isTogglable = false, toolTip = "Returns to the main page of the menu."},

                new ButtonInfo { buttonText = "Tag Self", method =() => TagMods.TagSelf(), toolTip = "Tags yourself. Master: direct. Non-master: teleport to nearest infected."},
                new ButtonInfo { buttonText = "Untag Self", method =() => TagMods.UntagSelf(), isTogglable = false, toolTip = "Removes your tag. Master: direct. Non-master: reconnects."},
                new ButtonInfo { buttonText = "Tag All", method =() => TagMods.TagAll(), isTogglable = false, toolTip = "Tags every player in the room. Master client only."},
                new ButtonInfo { buttonText = "Untag All", method =() => TagMods.UntagAll(), isTogglable = false, toolTip = "Untags every player in the room. Master client only."},
                new ButtonInfo { buttonText = "Tag Gun", method =() => TagMods.TagGun(), toolTip = "Point and shoot to tag a player. Hold grip, aim, pull trigger."},
                new ButtonInfo { buttonText = "Untag Gun", method =() => TagMods.UntagGun(), toolTip = "Point and shoot to untag a player. Master client only."},
                new ButtonInfo { buttonText = "Tag Aura", method =() => TagMods.TagAura(), toolTip = "Tags players within 3 meters when you are infected."},
                new ButtonInfo { buttonText = "Anti Tag", method =() => TagMods.AntiTag(), toolTip = "Keeps you untagged. Master: removes infection. Non-master: reconnects."},
            },
        };
    }
}
