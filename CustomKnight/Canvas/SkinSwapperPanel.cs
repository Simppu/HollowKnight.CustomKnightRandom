﻿using System;
using System.Collections;
using System.IO;
using ModCommon;
using ModCommon.Util;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace CustomKnight.Canvas
{
    public class SkinSwapperPanel
    {
        public static CanvasPanel Panel;

        private static float y;

        public static void BuildMenu(GameObject canvas)
        {
            Log("Building Skin Swapper Panel");
            y = 30.0f;

            Panel = new CanvasPanel(
                canvas,
                GUIController.Instance.images["Panel_BG"],
                new Vector2(0, y), 
                Vector2.zero,
                new Rect(0, 0, GUIController.Instance.images["Panel_BG"].width, 60)
            );

            float textHeight = 90;
            Panel.AddText(
                "Change Skin Text",
                "Change Skin",
                new Vector2(0, y),
                new Vector2(GUIController.Instance.images["Panel_BG"].width, textHeight), 
                GUIController.Instance.trajanNormal,
                24,
                FontStyle.Bold,
                TextAnchor.MiddleCenter
            );
            y += textHeight;

            GC.Collect();

            foreach (string path in Directory.GetDirectories(CustomKnight.DATA_DIR))
            {
                string directoryName = new DirectoryInfo(path).Name;
                
                Texture2D tex = null;
                
                int imageHeight = 128;
                int imageWidth = 300;
                
                if (File.Exists((CustomKnight.DATA_DIR + "/" + directoryName + "/Icon.png").Replace("\\", "/")))
                {
                    byte[] iconBytes = File.ReadAllBytes((CustomKnight.DATA_DIR + "/" + directoryName + "/Icon.png").Replace("\\", "/"));
                    Texture2D icon = new Texture2D(1, 1);
                    bool isLoaded = icon.LoadImage(iconBytes);
                    if (isLoaded)
                    {
                        tex = Resize(icon, imageWidth, imageHeight);
                    }
                }
                else if (File.Exists((CustomKnight.DATA_DIR + "/" + directoryName + "/icon.png").Replace("\\", "/")))
                {
                    byte[] iconBytes = File.ReadAllBytes((CustomKnight.DATA_DIR + "/" + directoryName + "/icon.png").Replace("\\", "/"));
                    Texture2D icon = new Texture2D(1, 1);
                    bool isLoaded = icon.LoadImage(iconBytes);
                    if (isLoaded)
                    {
                        tex = Resize(icon, imageWidth, imageHeight);
                    }
                }
                else if (File.Exists((CustomKnight.DATA_DIR + "/" + directoryName + "/" + CustomKnight.KNIGHT_PNG).Replace("\\", "/")))
                {
                    byte[] knightBytes = File.ReadAllBytes((CustomKnight.DATA_DIR + "/" + directoryName + "/" + CustomKnight.KNIGHT_PNG).Replace("\\", "/"));
                    Texture2D knightTex = new Texture2D(1, 1);
                    bool isLoaded = knightTex.LoadImage(knightBytes);

                    if (isLoaded)
                    {
                        Color[] colors = knightTex.GetPixels(2890, 2523, imageWidth, imageHeight);
                        tex = new Texture2D(imageWidth, imageHeight);
                        tex.SetPixels(colors);
                        tex.Apply();
                    }
                }
                else
                {
                    tex = new Texture2D(imageWidth, imageHeight);
                }

                Panel.AddButton(
                    directoryName,
                    tex,
                    new Vector2(0, y),
                    Vector2.zero,
                    ChangeSkin,
                    new Rect(0, y, imageWidth, imageHeight),
                    GUIController.Instance.trajanNormal,
                    directoryName,
                    24
                );
                y += imageHeight;
                
                GC.Collect();
            }

            Vector2 newPanelSize = new Vector2(GUIController.Instance.images["Panel_BG"].width, y);
            Panel.ResizeBG(newPanelSize);
        }

        private static void ChangeSkin(string buttonName)
        {
            CustomKnight.SKIN_FOLDER = buttonName;
            HeroController.instance.GetComponent<SpriteFlash>().flashFocusHeal();
            Panel.SetActive(false, true);
            CustomKnight.Instance.Initialize(null);
            Panel.SetActive(true, false);
        }
        
        public static void Update()
        {
            if (Panel == null)
            {
                return;
            }

            if (GameManager.instance.IsGamePaused())
            {
                if (!Panel.active)
                {
                    Panel.SetActive(true, false);    
                }
            }
            else
            {
                if (Panel.active)
                {
                    Panel.SetActive(false, true);   
                }
            }
        }

        
        // Taken from https://stackoverflow.com/questions/56949217/how-to-resize-a-texture2d-using-height-and-width
        private static Texture2D Resize(Texture2D texture2D ,int targetX,int targetY)
        {
            RenderTexture rt=new RenderTexture(targetX, targetY,24);
            RenderTexture.active = rt;
            Graphics.Blit(texture2D,rt);
            Texture2D result=new Texture2D(targetX,targetY);
            result.ReadPixels(new Rect(0,0,targetX,targetY),0,0);
            result.Apply();
            return result;
        }
        
        private static void Log(object message) => Modding.Logger.Log("[Skin Swapper Panel] " + message);
    }
}