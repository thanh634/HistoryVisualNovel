using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace COMMANDS
{
    public class CMD_Extension_Database_Audio : CMD_Database_Extension
    {
        private static string[] PARAM_SFX = new string[] { "-s", "-sfx" };
        private static string[] PARAM_VOLUME = new string[] { "-v", "-vol", "-volume" };
        private static string[] PARAM_PITCH = new string[] { "-p", "-pitch" };
        private static string[] PARAM_LOOP = new string[] { "-l", "-loop" };

        private static string[] PARAM_CHANNEL = new string[] { "-c", "-channel" };
        //private static string[] PARAM_IMMEDIATE = new string[] { "-i", "-immediate" };
        private static string[] PARAM_START_VOLUME = new string[] { "-sv", "-startvolume" };
        private static string[] PARAM_SONG = new string[] { "-s", "-song" };
        private static string[] PARAM_AMBIENCE = new string[] { "-a", "-ambience" };

        new public static void Extend(CommandDatabase database)
        {

            database.AddCommand("entercombat", new Action(EnterCombat));
            database.AddCommand("loadscene3", new Action(LoadScene3));
            database.AddCommand("loadscene4", new Action(LoadScene4));
            database.AddCommand("loadscene5", new Action(LoadScene5));
            database.AddCommand("loadend", new Action(LoadEnd));

            database.AddCommand("playsfx", new Action<string[]>(PlaySFX));
            database.AddCommand("stopsfx", new Action<string>(StopSFX));

            database.AddCommand("playvoice", new Action<string[]>(PlayVoice));
            database.AddCommand("stopvoice", new Action<string>(StopSFX));

            database.AddCommand("playsong", new Action<string[]>(PlaySong));
            database.AddCommand("stopsong", new Action<string>(StopSong));

            database.AddCommand("playambience", new Action<string[]>(PlayAmbience));
            database.AddCommand("stopambience", new Action<string>(StopAmbience));
        }

        private static void EnterCombat()
        {
            Debug.Log("Entering Combat");
            Loader.Load(Loader.Scene.SetupScene);
        }

        private static void LoadScene3()
        {
            Debug.Log("Loading Scene 3");
            Loader.Load(Loader.Scene.VisualNovel3);
        }

        private static void LoadScene4()
        {
            Debug.Log("Loading Scene 4");
            Loader.Load(Loader.Scene.VisualNovel4);
        }

        private static void LoadScene5()
        {
            Debug.Log("Loading Scene 5");
            Loader.Load(Loader.Scene.VisualNovel5);
        }

        private static void LoadEnd()
        {
            Debug.Log("Loading End Scene");
            Loader.Load(Loader.Scene.Ending);
        }
        
        private static void PlaySFX(string[] data)
        {
            string filePath;
            float volume, pitch;
            bool loop;

            var parameters = ConvertDataToParameters(data);

            parameters.TryGetValue(PARAM_SFX, out filePath);
            parameters.TryGetValue(PARAM_VOLUME, out volume, defaultValue: 1f);
            parameters.TryGetValue(PARAM_PITCH, out pitch, defaultValue: 1f);
            parameters.TryGetValue(PARAM_LOOP, out loop, defaultValue: false);

            AudioClip sound = Resources.Load<AudioClip>(FilePath.GetPathToResource(FilePath.resources_sfx,filePath));

            if (sound == null)
            {
                Debug.Log($"Was not able to load sfx '{filePath}'");
                return;
            }

            AudioManager.instance.PlaySoundEffect(sound, volume: volume, pitch: pitch, loop: loop);
        }
        private static void StopSFX(string data)
        {
            AudioManager.instance.StopSoundEffect(data);
        }
        private static void PlayVoice(string[] data)
        {
            string filePath;
            float volume, pitch;
            bool loop;

            var parameters = ConvertDataToParameters(data);

            parameters.TryGetValue(PARAM_SFX, out filePath);
            parameters.TryGetValue(PARAM_VOLUME, out volume, defaultValue: 1f);
            parameters.TryGetValue(PARAM_PITCH, out pitch, defaultValue: 1f);
            parameters.TryGetValue(PARAM_LOOP, out loop, defaultValue: false);

            AudioClip sound = Resources.Load<AudioClip>(FilePath.GetPathToResource(FilePath.resources_voices, filePath));

            if (sound == null)
            {
                Debug.Log($"Was not able to load voice '{filePath}'");
                return;
            }


            AudioManager.instance.PlayVoice(sound, volume: volume, pitch: pitch, loop: loop);
        }
        private static void PlaySong(string[] data)
        {
            string filepath;
            int channel;

            var parameters = ConvertDataToParameters(data);

            parameters.TryGetValue(PARAM_SONG, out filepath);
            filepath = FilePath.GetPathToResource(FilePath.resources_music,filepath);

            parameters.TryGetValue(PARAM_CHANNEL, out channel, defaultValue: 1);

            PlayTrack(filepath, channel, parameters);
        }

        private static void PlayAmbience(string[] data)
        {
            string filepath;
            int channel;

            var parameters = ConvertDataToParameters(data);

            parameters.TryGetValue(PARAM_AMBIENCE, out filepath);
            filepath = FilePath.GetPathToResource(FilePath.resources_ambience, filepath);

            parameters.TryGetValue(PARAM_CHANNEL, out channel, defaultValue: 0);

            PlayTrack(filepath, channel, parameters);
        }

        private static void PlayTrack(string filePath, int channel, CommandParameters parameters)
        {
            bool loop;
            float volumeCap;
            float startVolume;
            float pitch;

            parameters.TryGetValue(PARAM_VOLUME, out volumeCap, defaultValue: 1f);

            parameters.TryGetValue(PARAM_START_VOLUME, out startVolume, defaultValue: 0f);

            parameters.TryGetValue(PARAM_PITCH, out pitch, defaultValue: 1f);

            parameters.TryGetValue(PARAM_LOOP, out loop, defaultValue: true);

            AudioClip sound = Resources.Load<AudioClip>(filePath);

            if (sound == null)
            {
                Debug.Log($"Was not able to load track '{filePath}'");
                return;
            }

            AudioManager.instance.PlayTrack(sound, channel, loop,  startVolume, volumeCap, pitch, filePath);

        }

        private static void StopSong(string data)
        {
            if (data == string.Empty)
                StopTrack("1");
            else
                StopTrack(data);
        }

        private static void StopAmbience(string data)
        {
            if (data == string.Empty) 
                StopTrack("0");
            else
                StopTrack(data);
        }

        private static void StopTrack(string data)
        {
            if (int.TryParse(data, out int channel))
                AudioManager.instance.StopTrack(channel);
            else
                AudioManager.instance.StopTrack(data);
        }

    }
}