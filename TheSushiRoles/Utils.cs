using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;
using System.Linq;
using Hazel;
using Reactor.Utilities.Extensions;
using System.Text;
using System.Collections;
using BepInEx.Unity.IL2CPP.Utils;
using Reactor.Networking;
using Reactor.Networking.Extensions;

namespace TheSushiRoles 
{
    public static class Utils
    {
        public static Dictionary<string, Sprite> CachedSprites = new();
        public static Sprite LoadSprite(string path, float pixelsPerUnit, bool cache=true) 
        {
            try
            {
                if (cache && CachedSprites.TryGetValue(path + pixelsPerUnit, out var sprite)) return sprite;
                Texture2D texture = LoadTextureFromResources(path);
                sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), pixelsPerUnit);
                if (cache) sprite.hideFlags |= HideFlags.HideAndDontSave | HideFlags.DontSaveInEditor;
                if (!cache) return sprite;
                return CachedSprites[path + pixelsPerUnit] = sprite;
            } 
            catch 
            {
                System.Console.WriteLine("Error loading sprite from path: " + path);
            }
            return null;
        }
        public static AudioClip LoadAudioClipFromResources(string path, string clipName = "UNNAMED_TSR_AUDIO_CLIP") 
        {
            // must be "raw (headerless) 2-channel signed 32 bit pcm (le)" (can e.g. use Audacity® to export)
            try {
                Assembly assembly = Assembly.GetExecutingAssembly();
                Stream stream = assembly.GetManifestResourceStream(path);
                var byteAudio = new byte[stream.Length];
                _ = stream.Read(byteAudio, 0, (int)stream.Length);
                float[] samples = new float[byteAudio.Length / 4]; // 4 bytes per sample
                int offset;
                for (int i = 0; i < samples.Length; i++)
                {
                    offset = i * 4;
                    samples[i] = (float)BitConverter.ToInt32(byteAudio, offset) / Int32.MaxValue;
                }
                int channels = 2;
                int sampleRate = 48000;
                AudioClip audioClip = AudioClip.Create(clipName, samples.Length / 2, channels, sampleRate, false);
                audioClip.hideFlags |= HideFlags.HideAndDontSave | HideFlags.DontSaveInEditor;
                audioClip.SetData(samples, 0);
                return audioClip;
            }
            catch
            {
                System.Console.WriteLine("Error loading AudioClip from resources: " + path);
            }
            return null;
        }

        public static void BecomeImpostor(PlayerControl player)
        {
            SendRPC(CustomRPC.BecomeImpostor, player);
            RPCProcedure.BecomeImpostor(player);
        }
        public static void BecomeCrewmate(PlayerControl player)
        {
            SendRPC(CustomRPC.BecomeCrewmate, player);
            RPCProcedure.BecomeCrewmate(player);
        }

        public static unsafe Texture2D LoadTextureFromResources(string path)
        {
            try
            {
                Texture2D texture = new Texture2D(2, 2, TextureFormat.ARGB32, true);
                Assembly assembly = Assembly.GetExecutingAssembly();
                Stream stream = assembly.GetManifestResourceStream(path);
                var length = stream.Length;
                var byteTexture = new Il2CppStructArray<byte>(length);
                stream.Read(new Span<byte>(IntPtr.Add(byteTexture.Pointer, IntPtr.Size * 4).ToPointer(), (int)length));
                ImageConversion.LoadImage(texture, byteTexture, false);
                return texture;
            }
            catch
            {
                System.Console.WriteLine("Error loading texture from resources: " + path);
            }
            return null;
        }

        public static Texture2D LoadTextureFromDisk(string path) 
        {
            try 
            {
                if (File.Exists(path))     
                {
                    Texture2D texture = new Texture2D(2, 2, TextureFormat.ARGB32, true);
                    var byteTexture = Il2CppSystem.IO.File.ReadAllBytes(path);
                    ImageConversion.LoadImage(texture, byteTexture, false);
                    return texture;
                }
            } 
            catch 
            {
                TheSushiRolesPlugin.Logger.LogError("Error loading texture from disk: " + path);
            }
            return null;
        }

        public static string ReadTextFromResources(string path) 
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            Stream stream = assembly.GetManifestResourceStream(path);
            StreamReader textStreamReader = new StreamReader(stream);
            return textStreamReader.ReadToEnd();
        }
        public static void EndGame(GameOverReason reason = GameOverReason.ImpostorsByVote, bool showAds = false)
        {
            GameManager.Instance.RpcEndGame(reason, showAds);
        }
        public static PlayerControl PlayerById(byte id)
        {
            foreach (PlayerControl player in PlayerControl.AllPlayerControls)
                if (player.PlayerId == id)
                    return player;
            return null;
        }
        
        public static Dictionary<byte, PlayerControl> AllPlayersById()
        {
            Dictionary<byte, PlayerControl> res = new Dictionary<byte, PlayerControl>();
            foreach (PlayerControl player in PlayerControl.AllPlayerControls)
                res.Add(player.PlayerId, player);
            return res;
        }

        public static bool TwoPlayersAlive() => PlayerControl.AllPlayerControls.ToArray().Count(x => !x.Data.IsDead) == 2;

        public static void HandlePoisonedOnBodyReport() 
        {
            if (Viper.Player == null) return;
            
            // Murder the poisoned player and reset poisoned (regardless whether the kill was successful or not)
            CheckMurderAttemptAndKill(Viper.Player, Viper.poisoned, true, false);
            SendRPC(CustomRPC.ViperSetPoisoned, byte.MaxValue, byte.MaxValue);
            RPCProcedure.ViperSetPoisoned(byte.MaxValue, byte.MaxValue);
        }

        public static void RefreshRoleDescription(this PlayerControl player) 
        {
            if (player == null || player.myTasks == null) return;

            // Retrieve role and modifier information
            var roleInfos = RoleInfo.GetRoleInfoForPlayer(player);
            var modifierInfos = ModifierInfo.GetModifierInfoForPlayer(player);
            var abilityInfos = AbilityInfo.GetAbilityInfoForPlayer(player);

            // Prepare task texts for roles and modifiers
            var roleTaskTexts = roleInfos.Select(GetRoleString).ToList();
            var modifierTaskTexts = modifierInfos.Select(GetModifierString).ToList();
            var abilityTaskTexts = abilityInfos.Select(GetAbilityString).ToList();

            // Collect tasks to remove
            var tasksToRemove = new List<PlayerTask>();
            foreach (var task in player.myTasks.GetFastEnumerator())
            {
                if (task.TryCast<ImportantTextTask>() is not ImportantTextTask textTask) continue;

                if (roleTaskTexts.Contains(textTask.Text))
                {
                    roleTaskTexts.Remove(textTask.Text); // Task already exists for this role
                }
                else if (modifierTaskTexts.Contains(textTask.Text))
                {
                    modifierTaskTexts.Remove(textTask.Text); // Task already exists for this modifier
                }
                else if (abilityTaskTexts.Contains(textTask.Text))
                {
                    abilityTaskTexts.Remove(textTask.Text); // Task already exists for this ability
                }
                else
                {
                    tasksToRemove.Add(task); // Task is outdated and should be removed
                }
            }

            // Remove outdated tasks
            foreach (var task in tasksToRemove)
            {
                task.OnRemove();
                player.myTasks.Remove(task);
                UnityEngine.Object.Destroy(task.gameObject);
            }

            // Add new tasks for modifiers
            for (int i = modifierTaskTexts.Count - 1; i >= 0; i--)
            {
                AddImportantTextTask(player, modifierTaskTexts[i], isModifier: true, isAbility: false);
            }

            // Add new tasks for abilities
            for (int i = abilityTaskTexts.Count - 1; i >= 0; i--)
            {
                AddImportantTextTask(player, abilityTaskTexts[i], isModifier: false, isAbility: true);
            }

            // Add new tasks for roles second
            for (int i = roleTaskTexts.Count - 1; i >= 0; i--)
            {
                AddImportantTextTask(player, roleTaskTexts[i], isModifier: false, isAbility: false);
            }
        }

        private static void AddImportantTextTask(PlayerControl player, string text, bool isModifier = false, bool isAbility = false)
        {
            string taskName = isAbility ? "AbilityTask" : (isModifier ? "ModifierTask" : "RoleTask");
            var task = new GameObject(taskName).AddComponent<ImportantTextTask>();
            task.transform.SetParent(player.transform, false);
            task.Text = text;
            player.myTasks.Insert(0, task);
        }

        internal static string GetRoleString(RoleInfo roleInfo)
        {
            if (roleInfo.Name == "Jackal") 
            {
                var getRecruitText = Jackal.canCreateRecruit ? " and create a Recruit" : "";
                return ColorString(roleInfo.Color, $"Role: <b>{roleInfo.Name}</b>\nKill everyone{getRecruitText}\nAlignment: <b>{roleInfo.AlignmentText()}</b>");  
            }
            return ColorString(roleInfo.Color, $"Role: <b>{roleInfo.Name}</b>\n{roleInfo.ShortDescription} \nAlignment: <b>{roleInfo.AlignmentText()}</b>");
        }
        internal static string GetModifierString(ModifierInfo modInfo)
        {
            if (modInfo.Name == "Drunk") 
            {
                return ColorString(modInfo.Color, $"Modifier: <b>{modInfo.Name}</b>\n{modInfo.ShortDescription} ({Drunk.meetings})");
            }
            return ColorString(modInfo.Color, $"Modifier: <b>{modInfo.Name}</b>\n{modInfo.ShortDescription}");
        }
        internal static string GetAbilityString(AbilityInfo abInfo)
        {
            return ColorString(abInfo.Color, $"Ability: <b>{abInfo.Name}</b>\n{abInfo.Description}");
        }

        public static string PreviousEndGameSummary = "";
        public static bool IsD(byte playerId) 
        {
            return playerId % 2 == 0;
        }

        public static bool IsLighterColor(PlayerControl target) 
        {
            return IsD(target.PlayerId);
        }
        public static Il2CppSystem.Collections.Generic.List<PlayerControl> GetClosestPlayers(Vector2 truePosition, float radius)
        {
            var playerControlList = new Il2CppSystem.Collections.Generic.List<PlayerControl>();
            var allPlayers = GameData.Instance.AllPlayers;
            var lightRadius = radius * ShipStatus.Instance.MaxLightRadius;

            for (var index = 0; index < allPlayers.Count; ++index)
            {
                var playerInfo = allPlayers[index];

                if (!playerInfo.Disconnected)
                {
                    var vector2 = new Vector2(playerInfo.Object.GetTruePosition().x - truePosition.x, playerInfo.Object.GetTruePosition().y - truePosition.y);
                    var magnitude = vector2.magnitude;

                    if (magnitude <= lightRadius)
                    {
                        var playerControl = playerInfo.Object;
                        playerControlList.Add(playerControl);
                    }
                }
            }

            return playerControlList;
        }

        public static bool IsCustomServer() 
        {
            if (FastDestroyableSingleton<ServerManager>.Instance == null) return false;
            StringNames n = FastDestroyableSingleton<ServerManager>.Instance.CurrentRegion.TranslateName;
            return n != StringNames.ServerNA && n != StringNames.ServerEU && n != StringNames.ServerAS;
        }

        public static bool HasFakeTasks(this PlayerControl player) 
        {
            return player.IsPassiveNeutral()|| player.IsNeutralKiller() && player != Agent.Player;
        }
        

        public static bool ShouldShowGhostInfo() 
        {
            return PlayerControl.LocalPlayer != null && PlayerControl.LocalPlayer.Data.IsDead && MapOptions.ghostsSeeInformation || AmongUsClient.Instance.GameState == InnerNet.InnerNetClient.GameStates.Ended;
        }

        public static void ClearAllTasks(this PlayerControl player) 
        {
            if (player == null) return;
            foreach (var playerTask in player.myTasks.GetFastEnumerator())
            {
                playerTask.OnRemove();
                UnityEngine.Object.Destroy(playerTask.gameObject);
            }
            player.myTasks.Clear();
            
            if (player.Data != null && player.Data.Tasks != null)
                player.Data.Tasks.Clear();
        }

        public static void MurderPlayer(this PlayerControl player, PlayerControl target)
        {
            player.MurderPlayer(target, MurderResultFlags.Succeeded);
        }

        public static void RpcRepairSystem(this ShipStatus shipStatus, SystemTypes systemType, byte amount)
        {
            shipStatus.RpcUpdateSystem(systemType, amount);
        }

        public static bool IsMira() 
        {
            return GameOptionsManager.Instance.CurrentGameOptions.MapId == 1;
        }
        public static bool IsAirship() 
        {
            return GameOptionsManager.Instance.CurrentGameOptions.MapId == 4;
        }
        public static bool IsSkeld() 
        {
            return GameOptionsManager.Instance.CurrentGameOptions.MapId == 0;
        }
        public static bool IsPolus() 
        {
            return GameOptionsManager.Instance.CurrentGameOptions.MapId == 2;
        }
        public static bool IsFungle() 
        {
            return GameOptionsManager.Instance.CurrentGameOptions.MapId == 5;
        }

        public static bool MushroomSabotageActive() 
        {
            return PlayerControl.LocalPlayer.myTasks.ToArray().Any((x) => x.TaskType == TaskTypes.MushroomMixupSabotage);
        }

        // Class from StellarRoles
        [HarmonyPatch(typeof(ShipStatus), nameof(ShipStatus.StartMeeting))]
        class ShipStatusStartMeetingPatch
        {
            static void Prefix()
            {
                if (FastDestroyableSingleton<HudManager>.Instance.FullScreen == null) return;
                FastDestroyableSingleton<HudManager>.Instance.FullScreen.gameObject.SetActive(true);
                FastDestroyableSingleton<HudManager>.Instance.FullScreen.enabled = true;
                var color = Color.black;

                FastDestroyableSingleton<HudManager>.Instance.StartCoroutine(Effects.Lerp(0.8f, new Action<float>((p) =>
                {
                    FastDestroyableSingleton<HudManager>.Instance.FullScreen.color = new Color(color.r, color.g, color.b, Mathf.Clamp01(1 - p));
                    if (p == 1)
                    {
                        FastDestroyableSingleton<HudManager>.Instance.FullScreen.enabled = false;
                    }
                })));
            }
        }
        public static void ShowTextToast(string text, float delay = 1.25f)
        {
            HudManager.Instance.StartCoroutine(CoTextToast(text, delay));
        }
        public static void SendRPC(params object[] data)
        {
            if (data[0] is not CustomRPC) throw new ArgumentException($"first parameter must be a {typeof(CustomRPC).FullName}");

            var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                        (byte)(CustomRPC)data[0], SendOption.Reliable, -1);

            if (data.Length == 1)
            {
                AmongUsClient.Instance.FinishRpcImmediately(writer);
                return;
            }

            foreach (var item in data[1..])
            {

                if (item is bool boolean)
                {
                    writer.Write(boolean);
                }
                else if (item is int integer)
                {
                    writer.Write(integer);
                }
                else if (item is uint uinteger)
                {
                    writer.Write(uinteger);
                }
                else if (item is float Float)
                {
                    writer.Write(Float);
                }
                else if (item is byte Byte)
                {
                    writer.Write(Byte);
                }
                else if (item is sbyte sByte)
                {
                    writer.Write(sByte);
                }
                else if (item is Vector2 vector)
                {
                    writer.Write(vector);
                }
                else if (item is Vector3 vector3)
                {
                    writer.Write(vector3);
                }
                else if (item is string str)
                {
                    writer.Write(str);
                }
                else if (item is byte[] array)
                {
                    writer.WriteBytesAndSize(array);
                }
                else
                {
                    TheSushiRolesPlugin.Logger.LogMessage($"unknown data type entered for rpc write: item - {nameof(item)}, {item.GetType().FullName}, rpc - {data[0]}");
                }
            }
            AmongUsClient.Instance.FinishRpcImmediately(writer);
        }
        public static void Revive(PlayerControl player)
        {
            if (Viper.poisoned == player) Viper.poisoned = null;

            player.Revive();
            MapOptions.RevivedPlayers.Add(player.PlayerId);

            // Clear text before updating it
            ClearAllRoleTexts();
            PlayerControlFixedUpdatePatch.UpdatePlayerInfoText(player);

            var body = UnityEngine.Object.FindObjectsOfType<DeadBody>()
                .FirstOrDefault(b => b.ParentId == player.PlayerId);
            var position = body.TruePosition;

            player.NetTransform.SnapTo(new Vector2(position.x, position.y + 0.3636f));

            if (SubmergedCompatibility.IsSubmerged && PlayerControl.LocalPlayer.PlayerId == player.PlayerId)
            {
                SubmergedCompatibility.ChangeFloor(player.transform.position.y > -7);
            }

            if (body != null) UnityEngine.Object.Destroy(body.gameObject);
        }
        public static IEnumerator CoTextToast(string text, float delay)
        {
            GameObject taskOverlay = UnityEngine.Object.Instantiate(HudManager.Instance.TaskCompleteOverlay.gameObject, HudManager.Instance.transform);
            taskOverlay.SetActive(true);
            taskOverlay.GetComponentInChildren<TextTranslatorTMP>().DestroyImmediate();
            taskOverlay.GetComponentInChildren<TMPro.TextMeshPro>().text = text;
            
            yield return Effects.Slide2D(taskOverlay.transform, new Vector2(0f, -8f), Vector2.zero, 0.25f);
            
            for (float time = 0f; time < delay; time += Time.deltaTime)
            {
                yield return null;
            }
            
            yield return Effects.Slide2D(taskOverlay.transform, Vector2.zero, new Vector2(0f, 8f), 0.25f);
            
            taskOverlay.SetActive(true);
            taskOverlay.Destroy();
        }

        public static void SetSemiTransparent(this PoolablePlayer player, bool value, float alpha=0.25f) 
        {
            alpha = value ? alpha : 1f;
            foreach (SpriteRenderer r in player.gameObject.GetComponentsInChildren<SpriteRenderer>())
                r.color = new Color(r.color.r, r.color.g, r.color.b, alpha);
            player.cosmetics.nameText.color = new Color(player.cosmetics.nameText.color.r, player.cosmetics.nameText.color.g, player.cosmetics.nameText.color.b, alpha);
        }

        public static string GetString(this TranslationController t, StringNames key, params Il2CppSystem.Object[] parts)
        {
            return t.GetString(key, parts);
        }

        public static string ColorString(Color c, string s) 
        {
            return string.Format("<color=#{0:X2}{1:X2}{2:X2}{3:X2}>{4}</color>", ToByte(c.r), ToByte(c.g), ToByte(c.b), ToByte(c.a), s);
        }

        public static int lineCount(string text)
        {
            return text.Count(c => c == '\n');
        }

        private static byte ToByte(float f)
        {
            f = Mathf.Clamp01(f);
            return (byte)(f * 255);
        }

        public static KeyValuePair<byte, int> MaxPair(this Dictionary<byte, int> self, out bool tie)
        {
            tie = true;
            KeyValuePair<byte, int> result = new KeyValuePair<byte, int>(byte.MaxValue, int.MinValue);
            foreach (KeyValuePair<byte, int> keyValuePair in self)
            {
                if (keyValuePair.Value > result.Value)
                {
                    result = keyValuePair;
                    tie = false;
                }
                else if (keyValuePair.Value == result.Value)
                {
                    tie = true;
                }
            }
            return result;
        }
        public static void ClearAllRoleTexts()
        {
            foreach (var text in RoleInfo.RoleTexts.Values)
            {
                if (text != null)
                    UnityEngine.Object.Destroy(text.gameObject);
            }
            RoleInfo.RoleTexts.Clear();
        }
        public static IList CreateList(Type myType)
        {
            Type genericListType = typeof(List<>).MakeGenericType(myType);
            return (IList)Activator.CreateInstance(genericListType);
        }
        public static bool HidePlayerName(PlayerControl source, PlayerControl target)
        {
            if (Painter.PaintTimer > 0f || MushroomSabotageActive()) return true; // No names are visible
            if (Patches.SurveillanceMinigamePatch.nightVisionIsActive) return true;
            else if (Assassin.isInvisble && Assassin.Player == target) return true;
            else if (Wraith.IsVanished && Wraith.Player == target) return true;
            else if (!MapOptions.hidePlayerNames) return false; // All names are visible
            else if (source == null || target == null) return true;
            else if (source == target) return false; // Player sees his own name
            else if (source.Data.Role.IsImpostor && (target.Data.Role.IsImpostor || target == Spy.Player)) return false; // Members of team Impostors see the names of Impostors/Spies
            else if ((source == Lovers.Lover1 || source == Lovers.Lover2) && (target == Lovers.Lover1 || target == Lovers.Lover2)) return false; // Members of team Lovers see the names of each other
            else if ((source == Jackal.Player || source == Recruit.Player) && (target == Jackal.Player || target == Recruit.Player)) return false; // Members of team Jackal see the names of each other
            return true;
        }

        public static void SetDefaultLook(this PlayerControl target, bool enforceNightVisionUpdate = true) 
        {
            if (MushroomSabotageActive()) 
            {
                var instance = ShipStatus.Instance.CastFast<FungleShipStatus>().specialSabotage;
                MushroomMixupSabotageSystem.CondensedOutfit condensedOutfit = instance.currentMixups[target.PlayerId];
                NetworkedPlayerInfo.PlayerOutfit playerOutfit = instance.ConvertToPlayerOutfit(condensedOutfit);
                target.MixUpOutfit(playerOutfit);
            }
            else
                target.SetLook(target.Data.PlayerName, target.Data.DefaultOutfit.ColorId, target.Data.DefaultOutfit.HatId, target.Data.DefaultOutfit.VisorId, target.Data.DefaultOutfit.SkinId, target.Data.DefaultOutfit.PetId, enforceNightVisionUpdate);
        }

        public static void SetLook(this PlayerControl target, String playerName, int colorId, string hatId, string visorId, string skinId, string petId, bool enforceNightVisionUpdate = true) 
        {
            target.RawSetColor(colorId);
            target.RawSetVisor(visorId, colorId);
            target.RawSetHat(hatId, colorId);
            target.RawSetName(HidePlayerName(PlayerControl.LocalPlayer, target) ? "" : playerName);


            SkinViewData nextSkin = null;
            try { nextSkin = ShipStatus.Instance.CosmeticsCache.GetSkin(skinId); } catch { return; };
            
            PlayerPhysics playerPhysics = target.MyPhysics;
            AnimationClip clip = null;
            var spriteAnim = playerPhysics.myPlayer.cosmetics.skin.animator;
            var currentPhysicsAnim = playerPhysics.Animations.Animator.GetCurrentAnimation();


            if (currentPhysicsAnim == playerPhysics.Animations.group.RunAnim) clip = nextSkin.RunAnim;
            else if (currentPhysicsAnim == playerPhysics.Animations.group.SpawnAnim) clip = nextSkin.SpawnAnim;
            else if (currentPhysicsAnim == playerPhysics.Animations.group.EnterVentAnim) clip = nextSkin.EnterVentAnim;
            else if (currentPhysicsAnim == playerPhysics.Animations.group.ExitVentAnim) clip = nextSkin.ExitVentAnim;
            else if (currentPhysicsAnim == playerPhysics.Animations.group.IdleAnim) clip = nextSkin.IdleAnim;
            else clip = nextSkin.IdleAnim;
            float progress = playerPhysics.Animations.Animator.m_animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
            playerPhysics.myPlayer.cosmetics.skin.skin = nextSkin;
            playerPhysics.myPlayer.cosmetics.skin.UpdateMaterial();

            spriteAnim.Play(clip, 1f);
            spriteAnim.m_animator.Play("a", 0, progress % 1);
            spriteAnim.m_animator.Update(0f);

            target.RawSetPet(petId, colorId);

            if (enforceNightVisionUpdate) Patches.SurveillanceMinigamePatch.EnforceNightVision(target);
            Chameleon.Update();  // so that morphling and camo wont make the chameleons visible
        }
        public static string AlignmentText(this RoleInfo role)
        {
            if (role == null) return "";
            var name = "";

            if (role.Alignment == RoleAlignment.CrewInvest) name = "<color=#8BFDFDFF>Crew</color> (<color=#1D7CF2FF>Investigative</color>)";
            else if (role.Alignment == RoleAlignment.CrewPower) name = "<color=#8BFDFDFF>Crew</color> (<color=#1D7CF2FF>Power</color>)";
            else if (role.Alignment == RoleAlignment.CrewProtect) name = "<color=#8BFDFDFF>Crew</color> (<color=#1D7CF2FF>Protective</color>)";
            else if (role.Alignment == RoleAlignment.CrewSupport) name = "<color=#8BFDFDFF>Crew</color> (<color=#1D7CF2FF>Support</color>)";
            else if (role.Alignment == RoleAlignment.CrewSpecial) name = "<color=#8BFDFDFF>Crew</color> (<color=#1D7CF2FF>Special</color>)";
            else if (role.Alignment == RoleAlignment.NeutralBenign) name = "<color=#4C544E>Neutral</color> (<color=#1D7CF2FF>Benign</color>)";
            else if (role.Alignment == RoleAlignment.NeutralEvil) name = "<color=#4C544E>Neutral</color> (<color=#1D7CF2FF>Evil</color>)";
            else if (role.Alignment == RoleAlignment.NeutralKilling) name = "<color=#4C544E>Neutral</color> (<color=#1D7CF2FF>Killing</color>)";
            else if (role.Alignment == RoleAlignment.ImpConcealing) name = "<color=#FF0000FF>Imp</color> (<color=#1D7CF2FF>Concealing</color>)";
            else if (role.Alignment == RoleAlignment.ImpPower) name = "<color=#FF0000FF>Imp</color> (<color=#1D7CF2FF>Power</color>)";
            else if (role.Alignment == RoleAlignment.ImpSpecial) name = "<color=#FF0000FF>Imp</color> (<color=#1D7CF2FF>Special</color>)";
            else if (role.Alignment == RoleAlignment.ImpSupport) name = "<color=#FF0000FF>Imp</color> (<color=#1D7CF2FF>Support</color>)";

            return name;
        }
        public static void TrackPlayer(PlayerControl target, Arrow arrow, Color color)
        {
            if (target == null) return;
            
            DeadBody body = null;
            if (target.Data.IsDead)
            {
                body = UnityEngine.Object.FindObjectsOfType<DeadBody>().FirstOrDefault(b => b.ParentId == target.PlayerId);
            }
            if (body != null)
                target.transform.position = body.transform.position;

            arrow.Update(target.transform.position, color);
            arrow.arrow.SetActive(!target.Data.IsDead || body != null);
        }

        public static void ShowFlash(Color color, float Duration = 1f, string message = "") 
        {
            if (Grenadier.Active || FastDestroyableSingleton<HudManager>.Instance == null || FastDestroyableSingleton<HudManager>.Instance.FullScreen == null) return;
            FastDestroyableSingleton<HudManager>.Instance.FullScreen.gameObject.SetActive(true);
            FastDestroyableSingleton<HudManager>.Instance.FullScreen.enabled = true;
            // Message Text
            TMPro.TextMeshPro messageText = GameObject.Instantiate(FastDestroyableSingleton<HudManager>.Instance.KillButton.cooldownTimerText, FastDestroyableSingleton<HudManager>.Instance.transform);
            messageText.text = message;
            messageText.enableWordWrapping = false;
            messageText.transform.localScale = Vector3.one * 0.5f;
            messageText.transform.localPosition += new Vector3(0f, 2f, -69f);
            messageText.gameObject.SetActive(true);
            FastDestroyableSingleton<HudManager>.Instance.StartCoroutine(Effects.Lerp(Duration, new Action<float>((p) => {
                var renderer = FastDestroyableSingleton<HudManager>.Instance.FullScreen;

                if (p < 0.5)
                {
                    if (renderer != null)
                        renderer.color = new Color(color.r, color.g, color.b, Mathf.Clamp01(p * 2 * 0.75f));
                }
                else 
                {
                    if (renderer != null)
                        renderer.color = new Color(color.r, color.g, color.b, Mathf.Clamp01((1 - p) * 2 * 0.75f));
                }
                if (p == 1f && renderer != null) renderer.enabled = false;
                if (p == 1f) messageText.gameObject.Destroy();
            })));
        }
        public static bool HasPrimaryButton()
        {
            foreach (var button in CustomButton.buttons)
            {
                if (button != null && button.PositionOffset == CustomButton.ButtonPositions.upperRowRight)
                    return true;
            }
            return false;
        }
        public static bool IsVenter(this PlayerControl player)
        {
            bool isVenter = false;
            if (Engineer.Player != null && Engineer.Player == player)
                isVenter = true;
            else if (Wraith.Player != null && Wraith.Player == player)
                isVenter = false;
            else if (Jackal.canUseVents && Jackal.Player != null && Jackal.Player == player)
                isVenter = true;
            else if (VengefulRomantic.CanUseVents && VengefulRomantic.Player != null && VengefulRomantic.Player == player)
                isVenter = true;
            else if (Pestilence.CanUseVents && Pestilence.Player != null && Pestilence.Player == player)
                isVenter = true;
            else if (Werewolf.CanUseVents && Werewolf.Player != null && Werewolf.Player == player)
                isVenter = true;
            else if (Hitman.CanUseVents && Hitman.Player != null && Hitman.Player == player)
                isVenter = true;
            else if (Agent.CanUseVents && Agent.Player != null && Agent.Player == player)
                isVenter = true;
            else if (Jester.CanUseVents && Jester.Player != null && Jester.Player == player)
                isVenter = true;
            else if (Juggernaut.CanUseVents && Juggernaut.Player != null && Juggernaut.Player == player)
                isVenter = true;
            else if (Spy.canEnterVents && Spy.Player != null && Spy.Player == player)
                isVenter = true;
            else if (Predator.CanUseVents && Predator.Player != null && Predator.Player == player)
                isVenter = true;
            else if (Glitch.canEnterVents && Glitch.Player != null && Glitch.Player == player)
                isVenter = true;
            else if (Scavenger.canUseVents && Scavenger.Player != null && Scavenger.Player == player)
                isVenter = true;
            else if (player.Data?.Role != null && player.Data.Role.CanVent)
            {
                isVenter = true;
            }
            return isVenter;
        }

        public static bool CheckLucky(PlayerControl target, bool breakShield, bool showShield, bool additionalCondition = true)
        {
            if (target != null && Lucky.Player != null && Lucky.Player == target && !Lucky.ProtectionBroken && additionalCondition) {
                if (breakShield) 
                {
                    SendRPC(CustomRPC.BreakArmor);
                    RPCProcedure.BreakArmor();
                }
                if (showShield) 
                {
                    target.ShowFailedMurder();
                }
                return true;
            }
            return false;
        }

        public static MurderAttemptResult CheckMurderAttempt(PlayerControl killer, PlayerControl target, bool blockRewind = false, bool ignoreBlank = false, bool ignoreIfKillerIsDead = false, bool ignoreMedic = false) 
        {
            var targetRole = RoleInfo.GetRoleInfoForPlayer(target).FirstOrDefault();
            // Modified vanilla checks
            if (AmongUsClient.Instance.IsGameOver) return MurderAttemptResult.SuppressKill;
            if (killer == null || killer.Data == null || (killer.Data.IsDead && !ignoreIfKillerIsDead) || killer.Data.Disconnected) return MurderAttemptResult.SuppressKill; // Allow non Impostor kills compared to vanilla code
            if (target == null || target.Data == null || target.Data.IsDead || target.Data.Disconnected) return MurderAttemptResult.SuppressKill; // Allow killing players in vents compared to vanilla code
            if (GameOptionsManager.Instance.currentGameOptions.GameMode == GameModes.HideNSeek) return MurderAttemptResult.PerformKill;

            // Handle first kill attempt
            if (MapOptions.ShieldFirstKill && MapOptions.FirstPlayerKilled == target) return MurderAttemptResult.SuppressKill;

            // Handle blank shot
            if (!ignoreBlank && Survivor.blankedList.Any(x => x.PlayerId == killer.PlayerId)) 
            {
                SendRPC(CustomRPC.SetBlanked, killer.PlayerId, (byte)0);
                RPCProcedure.SetBlanked(killer.PlayerId, 0);

                return MurderAttemptResult.BlankKill;
            }

            // Block impostor Shielded kill
            if (!ignoreMedic && Medic.Shielded != null && Medic.Shielded == target) 
            {
                SendRPC(CustomRPC.ShieldedMurderAttempt);
                RPCProcedure.ShieldedMurderAttempt();
                SoundEffectsManager.Play("fail");
                return MurderAttemptResult.SuppressKill;
            }

            if (Monarch.Player != null && Monarch.Player == target && Monarch.KnightedPlayers.Contains(killer))
            {
                SoundEffectsManager.Play("fail");
                return MurderAttemptResult.SuppressKill;
            }

            // Murder whoever tries to kill the fortified player.
            if (Crusader.FortifiedPlayer != null && Crusader.FortifiedPlayer == target && Crusader.Player != null && !Crusader.Player.Data.IsDead)
            {
                SendRPC(CustomRPC.FortifiedMurderAttempt);
                RPCProcedure.FortifiedMurderAttempt();
                SoundEffectsManager.Play("fail");
                return MurderAttemptResult.MirrorKill;
            }

            // Block impostor not fully grown mini kill
            else if (Mini.Player != null && target == Mini.Player && !Mini.IsGrownUp)
            {
                return MurderAttemptResult.SuppressKill;
            }

            //Veteran with alert active
            else if (Veteran.Player != null && Veteran.AlertActive && Veteran.Player == target)
            {
                if (killer == Pestilence.Player)
                {
                    // Veteran dies to Pestilence
                    return MurderAttemptResult.PerformKill;
                }

                if (Medic.Shielded != null && Medic.Shielded == target)
                {
                    SendRPC(CustomRPC.ShieldedMurderAttempt, target.PlayerId);
                    RPCProcedure.ShieldedMurderAttempt();
                }
                else if (Crusader.FortifiedPlayer != null && Crusader.FortifiedPlayer == target)
                {
                    SendRPC(CustomRPC.FortifiedMurderAttempt, target.PlayerId);
                    RPCProcedure.FortifiedMurderAttempt();
                }
                return MurderAttemptResult.MirrorKill;
            }

            // Pestilence murder attempt
            else if (Pestilence.Player != null && Pestilence.Player == target)
            {
                if (Medic.Shielded != null && Medic.Shielded == target)
                {
                    SendRPC(CustomRPC.ShieldedMurderAttempt, target.PlayerId);
                    RPCProcedure.ShieldedMurderAttempt();
                }
                else if (Crusader.FortifiedPlayer != null && Crusader.FortifiedPlayer == target)
                {
                    SendRPC(CustomRPC.FortifiedMurderAttempt, target.PlayerId);
                    RPCProcedure.FortifiedMurderAttempt();
                }
                return MurderAttemptResult.MirrorKill;
            }

            // Block Lucky with armor kill
            else if (CheckLucky(target, true, killer == PlayerControl.LocalPlayer, Sheriff.Player == null || killer.PlayerId != Sheriff.Player.PlayerId || !target.IsCrew() && Sheriff.canKillNeutrals || IsKiller(target)))
            {
                return MurderAttemptResult.BlankKill;
            }
            
            if (TransportationToolPatches.IsUsingTransportation(target) && !blockRewind && killer == Viper.Player) 
            {
                return MurderAttemptResult.DelayViperKill;
            }
            else if (TransportationToolPatches.IsUsingTransportation(target)) return MurderAttemptResult.SuppressKill;
            return MurderAttemptResult.PerformKill;
        }
        public static void MurderPlayer(PlayerControl killer, PlayerControl target, bool showAnimation)
        {
            SendRPC(CustomRPC.UncheckedMurderPlayer, killer.PlayerId, target.PlayerId, showAnimation ? Byte.MaxValue : 0);
            RPCProcedure.UncheckedMurderPlayer(killer.PlayerId, target.PlayerId, showAnimation ? Byte.MaxValue : (byte)0);
        }
        public static MurderAttemptResult CheckMurderAttemptAndKill(PlayerControl killer, PlayerControl target, bool isMeetingStart = false, bool showAnimation = true, bool ignoreBlank = false, bool ignoreIfKillerIsDead = false)  
        {
            // The local player checks for the validity of the kill and performs it afterwards (different to vanilla, where the host performs all the checks)
            // The kill attempt will be shared using a custom RPC, hence combining modded and unmodded versions is impossible
            MurderAttemptResult murder = CheckMurderAttempt(killer, target, isMeetingStart, ignoreBlank, ignoreIfKillerIsDead);

            if (murder == MurderAttemptResult.PerformKill)
            {
                MurderPlayer(killer, target, showAnimation);
            }
            else if (murder == MurderAttemptResult.MirrorKill)
            {
                MurderPlayer(target, killer, showAnimation);
            }
            else if (murder == MurderAttemptResult.DelayViperKill)
            {
                HudManager.Instance.StartCoroutine(Effects.Lerp(10f, new Action<float>((p) =>
                {

                    if (!TransportationToolPatches.IsUsingTransportation(target) && Viper.poisoned != null)
                    {
                        SendRPC(CustomRPC.ViperSetPoisoned, byte.MaxValue, byte.MaxValue);
                        RPCProcedure.ViperSetPoisoned(byte.MaxValue, byte.MaxValue);
                        MurderPlayer(killer, target, showAnimation);
                    }
                })));
            }
            return murder;            
        }
    
        public static void ShareGameVersion() 
        {
            MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.VersionHandshake, Hazel.SendOption.Reliable, -1);
            writer.Write((byte)TheSushiRolesPlugin.Version.Major);
            writer.Write((byte)TheSushiRolesPlugin.Version.Minor);
            writer.Write((byte)TheSushiRolesPlugin.Version.Build);
            writer.Write(AmongUsClient.Instance.AmHost ? Patches.GameStartManagerPatch.timer : -1f);
            writer.WritePacked(AmongUsClient.Instance.ClientId);
            writer.Write((byte)(TheSushiRolesPlugin.Version.Revision < 0 ? 0xFF : TheSushiRolesPlugin.Version.Revision));
            writer.Write(Assembly.GetExecutingAssembly().ManifestModule.ModuleVersionId.ToByteArray());
            AmongUsClient.Instance.FinishRpcImmediately(writer);
            RPCProcedure.VersionHandshake(TheSushiRolesPlugin.Version.Major, TheSushiRolesPlugin.Version.Minor, TheSushiRolesPlugin.Version.Build, TheSushiRolesPlugin.Version.Revision, Assembly.GetExecutingAssembly().ManifestModule.ModuleVersionId, AmongUsClient.Instance.ClientId);
        }

        public static void Shuffle<T>(this List<T> list)
        {
            for (var i = list.Count - 1; i > 0; --i)
            {
                var j = UnityEngine.Random.Range(0, i + 1);
                (list[i], list[j]) = (list[j], list[i]);
            }
        }

        public static bool IsWinner(this string playerName)
        {
            var flag = false;
            var winners = EndGameResult.CachedWinners;

            foreach (var win in winners)
            {
                if (win.PlayerName == playerName)
                {
                    flag = true;
                    break;
                }
            }

            return flag;
        }
        public static bool IsJackal(PlayerControl player)
        {
            return player != null && Recruit.Player != null && player.PlayerId == Recruit.Player.PlayerId ||
            player != null && Jackal.Player != null && player.PlayerId == Jackal.Player.PlayerId;
        }

        public static bool IsNeutral(this PlayerControl player)
        {
            RoleInfo roleInfo = RoleInfo.GetRoleInfoForPlayer(player).FirstOrDefault();
            if (roleInfo != null)
                return roleInfo.FactionId == Faction.Neutrals;
            return false;
        }
        public static bool IsPassiveNeutral(this PlayerControl player) 
        {
            RoleInfo roleInfo = RoleInfo.GetRoleInfoForPlayer(player).FirstOrDefault();
            if (roleInfo != null)
                return roleInfo.Alignment == RoleAlignment.NeutralBenign || roleInfo.Alignment == RoleAlignment.NeutralEvil;
            return false;
        }
        public static bool IsNeutralKiller(this PlayerControl player) 
        {
            RoleInfo roleInfo = RoleInfo.GetRoleInfoForPlayer(player).FirstOrDefault();
            if (roleInfo != null)
                return roleInfo.Alignment == RoleAlignment.NeutralKilling;
            return false;
        }
        public static bool IsCrew(this PlayerControl player) 
        {
            RoleInfo roleInfo = RoleInfo.GetRoleInfoForPlayer(player).FirstOrDefault();
            if (roleInfo != null)
                return roleInfo.FactionId == Faction.Crewmates;
            return false;
        }
        public static PlayerControl CheckForLandlordTargets(PlayerControl original)
        {
            if (original == null) return null;

            if (Landlord.FirstTarget != null && Landlord.SecondTarget != null)
            {
                bool firstDead = Landlord.FirstTarget.Data?.IsDead ?? true;
                bool secondDead = Landlord.SecondTarget.Data?.IsDead ?? true;

                if (original == Landlord.FirstTarget)
                {
                    return secondDead ? Landlord.Player : Landlord.SecondTarget;
                }

                if (original == Landlord.SecondTarget)
                {
                    return firstDead ? Landlord.Player : Landlord.FirstTarget;
                }
            }
            return original;
        }
        public static bool In(this RoleId id, params RoleId[] list) => list.Contains(id);

        public static bool IsImpostor(this PlayerControl player)
        {
            RoleInfo roleInfo = RoleInfo.GetRoleInfoForPlayer(player).FirstOrDefault();
            if (roleInfo != null)
                return roleInfo.FactionId == Faction.Impostors;
            return false;
        }
        public static bool CheckVeteranPestilenceKill(this PlayerControl target)
        {
            bool CanKill = Veteran.Player == target && Veteran.AlertActive;
            if (CanKill)
            {
                SendRPC(CustomRPC.VeteranAlertKill, PlayerControl.LocalPlayer.PlayerId);
                RPCProcedure.VeteranAlertKill(PlayerControl.LocalPlayer.PlayerId);
            }
            bool CanPestiKill = Pestilence.Player == target;
            if (CanPestiKill)
            {
                SendRPC(CustomRPC.PestilenceKill, PlayerControl.LocalPlayer.PlayerId);
                RPCProcedure.PestilenceKill(PlayerControl.LocalPlayer.PlayerId);
            }
            bool CouldKill = CanKill || CanPestiKill;
            return CouldKill;
        }

        public static bool CheckFortifiedPlayer(this PlayerControl target)
        {            
            return Crusader.FortifiedPlayer == target && !Crusader.Player.Data.IsDead;
        }

        public static bool IsKiller(this PlayerControl player) 
        {
            return player.Data.Role.IsImpostor || player.IsNeutralKiller();
        }
        public static bool IsProssecutorTarget(this PlayerControl player)
        {
            return Prosecutor.target != null 
            && player.PlayerId == Prosecutor.target.PlayerId 
            && Prosecutor.Player != null;
        }
        public static bool IsBeloved(this PlayerControl player)
        {
            return Romantic.beloved != null 
            && player.PlayerId == Romantic.beloved.PlayerId 
            && Romantic.Player != null || VengefulRomantic.Lover != null 
            && player.PlayerId == VengefulRomantic.Lover.PlayerId 
            && VengefulRomantic.Player != null;
        }
        public static bool IsShielded(this PlayerControl player)
        {
            return Medic.Shielded != null
            && player.PlayerId == Medic.Shielded.PlayerId 
            && Medic.Player != null && !Medic.Player.Data.IsDead;
        }
        public static bool IsLawyerClient(this PlayerControl player)
        {
            return Lawyer.target != null && player.PlayerId == Lawyer.target.PlayerId && Lawyer.Player != null;
        }
        public static void ShowRoleInfo()
        {
            var role = RoleInfo.GetRoleInfoForPlayer(PlayerControl.LocalPlayer);
			foreach (RoleInfo roleInfo in role)
            {
                if (role == null) return;
                
                var stringb = new StringBuilder();
                stringb.Append(ColorString(roleInfo.Color, $"{roleInfo.Name} Description:\n"));
                stringb.Append(ColorString(roleInfo.Color, $"{roleInfo.RoleDescription}\n\n"));
                
                FastDestroyableSingleton<HudManager>.Instance.ShowPopUp(stringb.ToString());
                SoundEffectsManager.Play("knockKnock");
            }
        }
        public static bool zoomOutStatus = false;
        public static void ToggleZoom(bool reset=false) 
        {
            float orthographicSize = reset || zoomOutStatus ? 3f : 12f;

            zoomOutStatus = !zoomOutStatus && !reset;
            Camera.main.orthographicSize = orthographicSize;
            foreach (var cam in Camera.allCameras) {
                if (cam != null && cam.gameObject.name == "UI Camera") cam.orthographicSize = orthographicSize;  // The UI is scaled too, else we cant click the buttons. Downside: map is super small.
            }

            var tzGO = GameObject.Find("TOGGLEZOOMBUTTON");
            if (tzGO != null) 
            {
                var rend = tzGO.transform.Find("Inactive").GetComponent<SpriteRenderer>();
                var rendActive = tzGO.transform.Find("Active").GetComponent<SpriteRenderer>();
                rend.sprite = zoomOutStatus ? LoadSprite("TheSushiRoles.Resources.Plus_Button.png", 100f) : LoadSprite("TheSushiRoles.Resources.Minus_Button.png", 100f);
                rendActive.sprite = zoomOutStatus ? LoadSprite("TheSushiRoles.Resources.Plus_ButtonActive.png", 100f) : LoadSprite("TheSushiRoles.Resources.Minus_ButtonActive.png", 100f);
                tzGO.transform.localScale = new Vector3(1.2f, 1.2f, 1f) * (zoomOutStatus ? 4 : 1);
            }

            ResolutionManager.ResolutionChanged.Invoke((float)Screen.width / Screen.height, Screen.width, Screen.height, Screen.fullScreen); // This will move button positions to the correct position.
        }

        private static long GetBuiltInTicks()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var builtin = assembly.GetType("Builtin");
            if (builtin == null) return 0;
            var field = builtin.GetField("CompileTime");
            if (field == null) return 0;
            var value = field.GetValue(null);
            if (value == null) return 0;
            return (long)value;
        }

        public static bool HasImpVision(NetworkedPlayerInfo player) 
        {
            return player.Role.IsImpostor
                || Jackal.Player != null && Jackal.Player.PlayerId == player.PlayerId
                || (Recruit.Player != null && Recruit.Player.PlayerId == player.PlayerId)
                || (Glitch.Player != null && Glitch.Player.PlayerId == player.PlayerId)
                || (Werewolf.Player != null && Werewolf.Player.PlayerId == player.PlayerId)
                || (Juggernaut.Player != null && Juggernaut.Player.PlayerId == player.PlayerId)
                || (Hitman.Player != null && Hitman.Player.PlayerId == player.PlayerId)
                || (Pestilence.Player != null && Pestilence.Player.PlayerId == player.PlayerId)
                || (VengefulRomantic.Player != null && VengefulRomantic.Player.PlayerId == player.PlayerId)
                || (Predator.Player != null && Predator.HasImpostorVision && Predator.Player.PlayerId == player.PlayerId)
                || (Spy.Player != null && Spy.Player.PlayerId == player.PlayerId && Spy.hasImpostorVision)
                || (Jester.Player != null && Jester.Player.PlayerId == player.PlayerId && Jester.hasImpostorVision);
        }
    }
}