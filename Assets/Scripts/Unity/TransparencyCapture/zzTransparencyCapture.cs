﻿using UnityEngine;
using System.IO;

public class zzTransparencyCapture
{
    public static Texture2D Capture(Camera lCamera, Rect pRect, bool isTransparent)
    {
		RenderTexture renderTexture = new RenderTexture((int)pRect.width, (int)pRect.height, 32);
        Texture2D lOut;
        if (isTransparent) {
            var lPreClearFlags = lCamera.clearFlags;
            var lPreBackgroundColor = lCamera.backgroundColor;
            // Texture.ReadPixels reads from whatever texture is active. Ours needs to
            // be active. But let's remember the old one so we can restore it later.
            RenderTexture oldRenderTexture = RenderTexture.active;
            {
                lCamera.clearFlags = CameraClearFlags.Color;
                lCamera.targetTexture = renderTexture;
                RenderTexture.active = renderTexture;

                //make two captures with black and white background
                lCamera.backgroundColor = Color.black;
                lCamera.Render();
                var lBlackBackgroundCapture = CaptureView(pRect);

                lCamera.backgroundColor = Color.white;
                lCamera.Render();
                var lWhiteBackgroundCapture = CaptureView(pRect);

                for (int x = 0; x < lWhiteBackgroundCapture.width; ++x) {
                    for (int y = 0; y < lWhiteBackgroundCapture.height; ++y) {
                        Color lColorWhenBlack = lBlackBackgroundCapture.GetPixel(x, y);
                        Color lColorWhenWhite = lWhiteBackgroundCapture.GetPixel(x, y);
                        if (lColorWhenBlack != Color.clear) {
                            //set real color
                            lWhiteBackgroundCapture.SetPixel(x, y,
                                GetColor(lColorWhenBlack, lColorWhenWhite));
                        }
                    }
                }
                lWhiteBackgroundCapture.Apply();
                lOut = lWhiteBackgroundCapture;
                Object.DestroyImmediate(lBlackBackgroundCapture);
            }
            // Restore previous settings.
            Camera.main.targetTexture = null;
            RenderTexture.active = oldRenderTexture;
            lCamera.backgroundColor = lPreBackgroundColor;
            lCamera.clearFlags = lPreClearFlags;
        } else {
            var lPreClearFlags = lCamera.clearFlags;
            var lPreBackgroundColor = lCamera.backgroundColor;

            lCamera.clearFlags = CameraClearFlags.Color;
            lCamera.backgroundColor = new Color(lPreBackgroundColor.r, lPreBackgroundColor.g, lPreBackgroundColor.b, 1f);

            RenderTexture oldRenderTexture = RenderTexture.active;
            lCamera.targetTexture = renderTexture;
            RenderTexture.active = renderTexture;
            // Render
            lCamera.Render();
            lOut = CaptureView(pRect);
            // Restore previous settings.
            Camera.main.targetTexture = null;
            RenderTexture.active = oldRenderTexture;
            lCamera.backgroundColor = lPreBackgroundColor;
            lCamera.clearFlags = lPreClearFlags;
        }
        return lOut;
    }

    /// <summary>
    /// Capture a screenshot(not include GUI)
    /// </summary>
    /// <returns></returns>
    public static Texture2D CaptureScreenshot(int width, int height, bool isTransparent, Camera camera = null)
    {
        if (camera == null) camera = Camera.main;
        return Capture(camera, new Rect(0f, 0f, width, height), isTransparent);
    }

    /// <summary>
    /// Capture a screenshot(not include GUI) at path filename as a PNG file
    /// eg. zzTransparencyCapture.captureScreenshot("Screenshot.png")
    /// </summary>
    /// <param name="pFileName"></param>
    /// <returns></returns>
    public static void CaptureScreenshot(string pFileName, bool isTransparent)
    {
        var lScreenshot = CaptureScreenshot(Screen.width, Screen.height, isTransparent);
        try
        {
            using (var lFile = new FileStream(pFileName, FileMode.Create))
            {
                BinaryWriter lWriter = new BinaryWriter(lFile);
                lWriter.Write(lScreenshot.EncodeToPNG());
            }
        }
        finally
        {
            Object.DestroyImmediate(lScreenshot);
        }
    }

    //pColorWhenBlack!=Color.clear
    static Color GetColor(Color pColorWhenBlack,Color pColorWhenWhite)
    {
        float lAlpha = GetAlpha(pColorWhenBlack.r, pColorWhenWhite.r);
        return new Color(
            pColorWhenBlack.r / lAlpha,
            pColorWhenBlack.g / lAlpha,
            pColorWhenBlack.b / lAlpha,
            lAlpha);
    }


    //           Color*Alpha      Color   Color+(1-Color)*(1-Alpha)=1+Color*Alpha-Alpha
    //0----------ColorWhenZero----Color---ColorWhenOne------------1
    static float GetAlpha(float pColorWhenZero, float pColorWhenOne)
    {
        //pColorWhenOne-pColorWhenZero=1-Alpha
        return 1 + pColorWhenZero - pColorWhenOne;
    }

    static Texture2D CaptureView(Rect pRect)
    {
        Texture2D lOut = new Texture2D((int)pRect.width, (int)pRect.height, TextureFormat.ARGB32, false);
        lOut.ReadPixels(pRect, 0, 0, false);
        return lOut;
    }

}