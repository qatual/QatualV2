using GorillaLocomotion;
using Qatual.Classes;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using static Qatual.Menu.Main;

namespace Qatual.Mods
{
    public class Movement
    {
        public static void Fly()
        {
            if (ControllerInputPoller.instance.rightControllerPrimaryButton)
            {
                GTPlayer.Instance.transform.position += GorillaTagger.Instance.headCollider.transform.forward * Time.deltaTime * Settings.Movement.flySpeed;
                GorillaTagger.Instance.rigidbody.linearVelocity = Vector3.zero;
            }
        }

        public static GameObject platl;
        public static GameObject platr;

        public static void Platforms()
        {
            if (ControllerInputPoller.instance.leftGrab)
            {
                if (platl == null)
                {
                    platl = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    platl.transform.localScale = new Vector3(0.025f, 0.3f, 0.4f);
                    platl.transform.position = TrueLeftHand().position;
                    platl.transform.rotation = TrueLeftHand().rotation;

                    FixStickyColliders(platl);

                    ColorChanger colorChanger = platl.AddComponent<ColorChanger>();
                    colorChanger.colors = Qatual.Settings.backgroundColor;
                }
                else
                {
                    if (platl != null)
                    {
                        Object.Destroy(platl);
                        platl = null;
                    }
                }
            }

            if (ControllerInputPoller.instance.rightGrab)
            {
                if (platr == null)
                {
                    platr = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    platr.transform.localScale = new Vector3(0.025f, 0.3f, 0.4f);
                    platr.transform.position = TrueRightHand().position;
                    platr.transform.rotation = TrueRightHand().rotation;

                    FixStickyColliders(platr);

                    ColorChanger colorChanger = platr.AddComponent<ColorChanger>();
                    colorChanger.colors = Qatual.Settings.backgroundColor;
                }
                else
                {
                    if (platr != null)
                    {
                        Object.Destroy(platr);
                        platr = null;
                    }
                }
            }
        }

        private static Vector3 longArmsOriginalScale;

        public static void EnableLongArms()
        {
            GameObject target = VRRig.LocalRig.gameObject;
            longArmsOriginalScale = target.transform.localScale;
            target.transform.localScale = new Vector3(2.5f, 2.5f, 2.5f);
        }

        public static void DisableLongArms()
        {
            VRRig.LocalRig.transform.localScale = longArmsOriginalScale;
        }

        public static void WASDFly()
        {
            Vector3 forward = GorillaTagger.Instance.headCollider.transform.forward;
            Vector3 right = GorillaTagger.Instance.headCollider.transform.right;
            forward.y = 0;
            right.y = 0;
            forward.Normalize();
            right.Normalize();

            Vector3 move = Vector3.zero;

            if (Keyboard.current[Key.W].isPressed) move += forward;
            if (Keyboard.current[Key.S].isPressed) move -= forward;
            if (Keyboard.current[Key.A].isPressed) move -= right;
            if (Keyboard.current[Key.D].isPressed) move += right;
            if (Keyboard.current[Key.Space].isPressed) move += Vector3.up;
            if (Keyboard.current[Key.LeftCtrl].isPressed) move += Vector3.down;

            if (move != Vector3.zero)
            {
                GTPlayer.Instance.transform.position += move * Time.deltaTime * Settings.Movement.flySpeed;
                GorillaTagger.Instance.rigidbody.linearVelocity = Vector3.zero;
            }
        }

        private static float originalMaxJumpSpeed;
        private static float originalJumpMultiplier;

        public static void EnableMosa()
        {
            originalMaxJumpSpeed = GTPlayer.Instance.maxJumpSpeed;
            originalJumpMultiplier = GTPlayer.Instance.jumpMultiplier;
            GTPlayer.Instance.maxJumpSpeed = 999f;
            GTPlayer.Instance.jumpMultiplier = 1.2f;
        }

        public static void DisableMosa()
        {
            GTPlayer.Instance.maxJumpSpeed = originalMaxJumpSpeed;
            GTPlayer.Instance.jumpMultiplier = originalJumpMultiplier;
        }

        private static float origHighJumpMultiplier;
        public static void EnableHighJump() { origHighJumpMultiplier = GTPlayer.Instance.jumpMultiplier; GTPlayer.Instance.jumpMultiplier = 4f; }
        public static void DisableHighJump() => GTPlayer.Instance.jumpMultiplier = origHighJumpMultiplier;
        public static void LowGravity() => GorillaTagger.Instance.rigidbody.AddForce(Vector3.up * 9.81f * 0.7f);
        public static void AntiGravity() => GorillaTagger.Instance.rigidbody.AddForce(Vector3.up * 9.81f * 1.05f);

        public static void SpeedBoost()
        {
            Vector3 vel = GorillaTagger.Instance.rigidbody.linearVelocity;
            if (vel.magnitude > 0.5f)
            {
                vel *= 1.08f;
                if (vel.magnitude > 25f) vel = vel.normalized * 25f;
                GorillaTagger.Instance.rigidbody.linearVelocity = vel;
            }
        }

        public static void BhopSpeed()
        {
            Rigidbody rb = GorillaTagger.Instance.rigidbody;
            if (!GTPlayer.Instance.isClimbing && rb.linearVelocity.magnitude > 0.1f)
            {
                Vector3 horizontal = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
                rb.AddForce(horizontal.normalized * 3f, ForceMode.Acceleration);
            }
        }

        public static void EnableNoClip() { GorillaTagger.Instance.rigidbody.useGravity = false; GorillaTagger.Instance.rigidbody.detectCollisions = false; }
        public static void NoClipUpdate()
        {
            GorillaTagger.Instance.rigidbody.linearVelocity = Vector3.zero;
            Vector3 forward = GorillaTagger.Instance.headCollider.transform.forward;
            Vector3 right = GorillaTagger.Instance.headCollider.transform.right;
            forward.Normalize(); right.Normalize();
            Vector3 move = Vector3.zero;
            if (Keyboard.current[Key.W].isPressed) move += forward;
            if (Keyboard.current[Key.S].isPressed) move -= forward;
            if (Keyboard.current[Key.A].isPressed) move -= right;
            if (Keyboard.current[Key.D].isPressed) move += right;
            if (Keyboard.current[Key.Space].isPressed) move += Vector3.up;
            if (Keyboard.current[Key.LeftCtrl].isPressed) move -= Vector3.up;
            if (move != Vector3.zero) GTPlayer.Instance.transform.position += move * Time.deltaTime * Settings.Movement.flySpeed;
        }
        public static void DisableNoClip() { GorillaTagger.Instance.rigidbody.useGravity = true; GorillaTagger.Instance.rigidbody.detectCollisions = true; GorillaTagger.Instance.rigidbody.linearVelocity = Vector3.zero; }

        private static Vector3 freezePosition;
        public static void EnableFreeze() { freezePosition = GTPlayer.Instance.transform.position; GTPlayer.Instance.disableMovement = true; }
        public static void FreezeUpdate() { GTPlayer.Instance.transform.position = freezePosition; GorillaTagger.Instance.rigidbody.linearVelocity = Vector3.zero; }
        public static void DisableFreeze() => GTPlayer.Instance.disableMovement = false;

        public static bool previousTeleportTrigger;
        public static void TeleportGun()
        {
            if (ControllerInputPoller.instance.rightGrab)
            {
                var GunData = RenderGun();
                GameObject NewPointer = GunData.NewPointer;

                if (ControllerInputPoller.TriggerFloat(XRNode.RightHand) > 0.5f && !previousTeleportTrigger)
                {
                    GTPlayer.Instance.TeleportTo(NewPointer.transform.position + Vector3.up, GTPlayer.Instance.transform.rotation);
                    GorillaTagger.Instance.rigidbody.linearVelocity = Vector3.zero;
                }

                previousTeleportTrigger = ControllerInputPoller.TriggerFloat(XRNode.RightHand) > 0.5f;
            }
        }
    }
}
