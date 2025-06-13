using System.Reflection;
using System.Collections.Generic;
namespace TheSushiRoles
{
    // Class to preload all audio/sound effects that are contained in the embedded resources.
    // The effects are made available through the soundEffects Dict / the get and the play methods.
    public static class SoundEffectsManager
        
    {
        private static Dictionary<string, AudioClip> soundEffects = new();

        public static void Load()
        {
            soundEffects = new Dictionary<string, AudioClip>();
            Assembly assembly = Assembly.GetExecutingAssembly();
            string[] resourceNames = assembly.GetManifestResourceNames();
            foreach (string resourceName in resourceNames)
            {
                if (resourceName.Contains("TheSushiRoles.Resources.SoundEffects.") && resourceName.Contains(".raw"))
                {
                    soundEffects.Add(resourceName, Utils.LoadAudioClipFromResources(resourceName));
                }
            }
        }

        public static AudioClip Get(string path)
        {
            // Convenience: As as SoundEffects are stored in the same folder, allow using just the name as well
            if (!path.Contains(".")) path = "TheSushiRoles.Resources.SoundEffects." + path + ".raw";
            AudioClip returnValue;
            return soundEffects.TryGetValue(path, out returnValue) ? returnValue : null;
        }


        public static void Play(string path, float volume=0.8f, bool loop = false)
        {
            if (!MapOptions.enableSoundEffects) return;
            AudioClip clipToPlay = Get(path);
            Stop(path);
            if (Constants.ShouldPlaySfx() && clipToPlay != null)
            {
                AudioSource source = SoundManagerInstance().PlaySound(clipToPlay, false, volume);
                source.loop = loop;
            }
        }

        public static void Stop(string path) 
        {
            var soundToStop = Get(path);
            if (soundToStop != null)
                if (Constants.ShouldPlaySfx()) SoundManagerInstance().StopSound(soundToStop);
        }

        public static void StopAll() 
        {
            if (soundEffects == null) return;
            foreach (var path in soundEffects.Keys) Stop(path);        
        }
    }
}