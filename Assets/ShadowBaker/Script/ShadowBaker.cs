using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class ShadowBaker : MonoBehaviour
{
  

    public static void BakeShadow(int ObjectLayer,Transform Target,int textureSize,float OrthographicSize,float Blur)
    {
        if (Target != null)
        {
            Camera ShadowCam = new GameObject("tempShadowCam").AddComponent<Camera>();
            RenderTexture rt = new RenderTexture(textureSize, textureSize, 24, RenderTextureFormat.ARGB32);

            ShadowCam.orthographic = true;
            ShadowCam.orthographicSize = OrthographicSize;
            Vector3 OriginalGOEuler = Target.eulerAngles;
            Target.eulerAngles = Vector3.Scale(Target.eulerAngles, new Vector3(1, 0, 1));
            ShadowCam.gameObject.AddComponent<RenderDepth>().depthLevel = 3;
            ShadowCam.gameObject.AddComponent<PostprocessingBlur>().blurSize = Blur;
            int GOLayer = Target.gameObject.layer;
            ShadowCam.cullingMask = 3 << 4;
            Target.gameObject.layer = 4;




            rt.Create();
            ShadowCam.targetTexture = rt;

            ShadowCam.transform.forward = -Vector3.up;
            ShadowCam.transform.position = Target.position + Vector3.up * 25;

            ShadowCam.Render();



            SaveTexture(rt, Target, textureSize);

            ShadowCam.targetTexture = null;
            DestroyImmediate(ShadowCam.gameObject);
            Target.gameObject.layer = GOLayer;
            Target.eulerAngles = OriginalGOEuler;
        }
        else
        {
            Debug.LogWarning("There is no Target object selected!!");
        }
    }
    public static void SaveTexture(RenderTexture rt,Transform Target,int textureSize)
    {
        byte[] bytes = toTexture2D(rt,textureSize).EncodeToPNG();
        string directoryPath = Application.dataPath+ "/Shadows";
       
        if (Directory.Exists(directoryPath))
        {
            File.WriteAllBytes(directoryPath + "/" + Target.name + Target.GetInstanceID()  + ".png", bytes);
            AssetDatabase.Refresh();
        
        }
        else
        {
            Directory.CreateDirectory(directoryPath);
            File.Create(directoryPath+"/"+Target.name+".png");
            File.WriteAllBytes(directoryPath + "/" + Target.name + Target.GetInstanceID() + ".png",bytes);
           AssetDatabase.Refresh();

        }
    }
 
   public static Texture2D toTexture2D(RenderTexture rTex,int textureSize)
    {
        Texture2D tex = new Texture2D(textureSize, textureSize, TextureFormat.RGB24, false);
        RenderTexture.active = rTex;
        tex.ReadPixels(new Rect(0, 0, rTex.width, rTex.height), 0, 0);
        tex.Apply();
        tex.wrapMode = TextureWrapMode.Clamp;
      
        return tex;
    }
}
