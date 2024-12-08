using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Media;
using UnityEngine;
using UnityEngine.Video;

namespace COMMANDS
{
    public class CMD_Extension_Database_GraphicPanels : CMD_Database_Extension
    {
        private static string[] PARAM_PANEL = new string[] { "-p", "-panel" };
        private static string[] PARAM_LAYER = new string[] { "-l", "-layer" };
        private static string[] PARAM_MEDIA = new string[] { "-m", "-media" };
        private static string[] PARAM_SPEED = new string[] { "-spd", "-speed" };
        private static string[] PARAM_IMMEDIATE = new string[] { "-i", "-immediate" };
        private static string[] PARAM_BLENDTEXT = new string[] { "-b", "-blend" };
        private static string[] PARAM_USEVIDEOAUDIO = new string[] { "-aud", "-audio" };

        private const string HOME_DIRECTORY_SYMBOL = "~/";

        new public static void Extend(CommandDatabase database)
        {
            database.AddCommand("setlayermedia", new Func<string[], IEnumerator>(SetLayerMedia));
            database.AddCommand("clearlayermedia", new Func<string[], IEnumerator>(ClearLayerMedia));
        }

        private static IEnumerator SetLayerMedia(string[] data)
        {
            string panelName = "";
            string mediaName = "";
            string blendTexName = "";
            string pathToGraphic = "";

            int layer = 0;
            
            float transitionSpeed = 0;
            
            bool immediate = false;
            bool useAudio = false;

            UnityEngine.Object graphic = null;
            Texture blendTex = null;

            var parameters = ConvertDataToParameters(data);

            parameters.TryGetValue(PARAM_PANEL, out panelName);
            GraphicPanel panel = GraphicPanelManager.instance.GetPanel(panelName);
            if(panel == null)
            {
                Debug.LogError($"Unable to grab panel '{panelName}, Please check panelName or your command!");
                yield break;
            }

            parameters.TryGetValue(PARAM_LAYER, out layer, defaultValue: 0);

            parameters.TryGetValue(PARAM_MEDIA, out mediaName);

            parameters.TryGetValue(PARAM_IMMEDIATE, out immediate, defaultValue: false);

            if(!immediate)
                parameters.TryGetValue(PARAM_SPEED, out transitionSpeed, defaultValue: 1);

            parameters.TryGetValue(PARAM_BLENDTEXT, out blendTexName);

            parameters.TryGetValue(PARAM_USEVIDEOAUDIO, out useAudio, defaultValue: false);

            // LOGIC

            // Grabbing graphic
            pathToGraphic = FilePath.GetPathToResource(FilePath.resources_backgroundImages, mediaName);
            graphic = Resources.Load<Texture>(pathToGraphic);
            if(graphic == null)
            {
                pathToGraphic = FilePath.GetPathToResource(FilePath.resources_backgroundVideos, mediaName);
                graphic = Resources.Load<VideoClip>(pathToGraphic);
            }
            if(graphic == null)
            {
                Debug.LogError($"Could not find media file called '{mediaName}' in the Resources directories!");
                yield break;
            }

            // Get the blend texture
            if(!immediate && blendTexName != string.Empty)
                blendTex = Resources.Load<Texture>(FilePath.resources_blendTextures + blendTexName);

            // Get the layer
            GraphicLayer graphicLayer = panel.GetLayer(layer, createIfNotExist: true);

            // Set up the layer with the correct media
            if(graphic is Texture)
                yield return graphicLayer.SetTexture(graphic as Texture, transitionSpeed, blendTex, pathToGraphic, immediate);
            else
                yield return graphicLayer.SetVideo(graphic as VideoClip, transitionSpeed, useAudio, blendTex, pathToGraphic, immediate);

        }

        private static IEnumerator ClearLayerMedia(string[] data)
        {
            string panelName = "";
            string blendTexName = "";

            bool immediate = false;

            int layer = 0;

            float transitionSpeed = 0;

            Texture blendTex = null;

            var parameters = ConvertDataToParameters(data);

            parameters.TryGetValue(PARAM_PANEL, out panelName);
            GraphicPanel panel = GraphicPanelManager.instance.GetPanel(panelName);
            if (panel == null)
            {
                Debug.LogError($"Unable to grab panel '{panelName}, Please check panelName or your command!");
                yield break;
            }

            parameters.TryGetValue(PARAM_LAYER, out layer, defaultValue: -1);

            parameters.TryGetValue(PARAM_IMMEDIATE, out immediate, defaultValue: false);

            if (!immediate)
                parameters.TryGetValue(PARAM_SPEED, out transitionSpeed, defaultValue: 1);

            parameters.TryGetValue(PARAM_BLENDTEXT, out blendTexName);

            // Get the blend texture
            if (!immediate && blendTexName != string.Empty)
                blendTex = Resources.Load<Texture>(FilePath.resources_blendTextures + blendTexName);

            if (layer == -1)
                panel.Clear(transitionSpeed, blendTex, immediate);
            else
            {
                GraphicLayer gLayer = panel.GetLayer(layer);
                if(gLayer == null)
                {
                    Debug.LogError($"Could not clear layer [{layer}] on panel '{panel.panelName}'");
                    yield break;
                }

                gLayer.Clear(transitionSpeed, blendTex, immediate);
            }


        }

    }
}