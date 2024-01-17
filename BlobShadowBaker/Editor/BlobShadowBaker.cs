using UnityEditor;
using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;


public class BlobShadowBakerUtils
{
 
    public static async void BakeShadow(int ObjectLayer, Transform Target, int textureSize, float OrthographicSize, float Blur, int Iter, bool bakeChildren)
    {  
        if (Target != null)
        {  
            Camera ShadowCam = new GameObject("tempShadowCam").AddComponent<Camera>();
            RenderTexture rt = new RenderTexture(textureSize, textureSize, 24, RenderTextureFormat.ARGBFloat);

            ShadowCam.orthographic = true;
            ShadowCam.orthographicSize = OrthographicSize;
            ShadowCam.backgroundColor = new Color(0,0,0,0);
            ShadowCam.clearFlags = CameraClearFlags.SolidColor;

              
            Vector3 OriginalGOEuler = Target.eulerAngles;
            Target.eulerAngles = Vector3.Scale(Target.eulerAngles, new Vector3(1, 0, 1));
            List<int> layers = new List<int>();
            int baseLayer = Target.gameObject.layer;
            ShadowCam.cullingMask = 1 << 30;
            Target.gameObject.layer = 30;
            Renderer targetRenderer = Target.GetComponent<Renderer>();
            Material baseMaterial = new Material(targetRenderer.sharedMaterial);
            List<Material[]> materials = new List<Material[]>();
            Material shadowMaterial;
            bool isURP;
            if (QualitySettings.renderPipeline != null) {
                if (QualitySettings.renderPipeline.GetType().Name.Contains("Universal"))
                {
                    isURP = true;
                }
                else
                {
                    isURP = false;
                }
            }
            else
            {
                isURP = false;
            }

            if (isURP)
            {
                shadowMaterial = new Material(Shader.Find("Shader Graphs/BSBObjectURP"));
            }
            else
            {
                shadowMaterial = new Material(Shader.Find("Hidden/BSBObject"));
            }
          
        
            if (bakeChildren)
            {
                foreach (Renderer mesh in Target.GetComponentsInChildren<Renderer>())
                {
                    materials.Add(mesh.sharedMaterials);
                    layers.Add(mesh.gameObject.layer);
                    Material[] tempMaterials = new Material[mesh.sharedMaterials.Length];
                    for (int i = 0; i < mesh.sharedMaterials.Length; i++)
                    {
                        tempMaterials[i] = shadowMaterial;

                    }
                    mesh.sharedMaterials = tempMaterials;
                    mesh.gameObject.layer = 30;
                }
            }
            targetRenderer.sharedMaterial = shadowMaterial;
            rt.Create();
            ShadowCam.targetTexture = rt;

            ShadowCam.transform.forward = -Vector3.up;
            ShadowCam.transform.position = Target.position + Vector3.up * (25 + (10 * OrthographicSize));

            ShadowCam.Render();
            Material blurMat = new(Shader.Find("Hidden/BSBBlur"));
            blurMat.SetFloat("_Blur", Blur);
            blurMat.SetInt("_Iter", Iter);
            RenderTexture rtemp = new RenderTexture(rt);
            rtemp.Create();
            if (Blur > 0)
            {
                Graphics.Blit(rt, rtemp, blurMat);
            }
            else
            {
                Graphics.Blit(rt, rtemp);
            }
            Target.eulerAngles = OriginalGOEuler;
          
         
            int index = 0;
            targetRenderer.sharedMaterial = baseMaterial;
            if (bakeChildren)
            {
                foreach (Renderer mesh in Target.GetComponentsInChildren<Renderer>())
                {
                    mesh.sharedMaterials = materials[index];
                    mesh.gameObject.layer = layers[index];
                    index++;
                }
            }
             SaveTexture(rtemp, Target, textureSize);
      
            ShadowCam.targetTexture = null;

            rtemp.Release();
            rt.Release();
            GameObject.DestroyImmediate(ShadowCam.gameObject);
            Target.gameObject.layer = baseLayer;
            await Resources.UnloadUnusedAssets();
          


        }
        else if(Target==null)
        {
            Debug.LogWarning("There is no Target object selected!!");
        }
    }
    public static async void SaveTexture(RenderTexture rt, Transform Target, int textureSize)
    {
        byte[] bytes = ToTexture2D(rt, textureSize).EncodeToPNG();
        string directoryPath = $"Assets/BlobShadowBaker/Output/"+Target.name+Target.gameObject.GetHashCode();

        if (Directory.Exists(directoryPath))
        {
            await File.WriteAllBytesAsync(directoryPath + "/" + Target.name + ".png", bytes);
            await Task.Delay(1);
            AssetDatabase.Refresh();
            TextureImporter importer = (TextureImporter)TextureImporter.GetAtPath(directoryPath + "/" + Target.name + ".png");

            importer.isReadable = true;

            TextureImporterSettings importerSettings = new TextureImporterSettings();
            importer.ReadTextureSettings(importerSettings);

            importer.SetTextureSettings(importerSettings);

            importer.maxTextureSize = textureSize; // or whatever
            importer.alphaIsTransparency = true;
            importer.textureCompression = TextureImporterCompression.Uncompressed;
            importer.alphaSource = TextureImporterAlphaSource.FromInput;

            EditorUtility.SetDirty(importer);
            importer.SaveAndReimport();
            AssetDatabase.Refresh();
            EditorGUIUtility.PingObject(AssetDatabase.LoadAssetAtPath(directoryPath + "/" + Target.name + ".png", typeof(Texture2D)));


        }
        else
        {
            Directory.CreateDirectory(directoryPath);
            await Task.Delay(1);
            await File.WriteAllBytesAsync(directoryPath + "/" + Target.name + ".png", bytes);
            await Task.Delay(1);
            AssetDatabase.Refresh();
            TextureImporter importer = (TextureImporter)TextureImporter.GetAtPath(directoryPath + "/" + Target.name + ".png");

            importer.isReadable = true;

            TextureImporterSettings importerSettings = new TextureImporterSettings();
            importer.ReadTextureSettings(importerSettings);

            importer.SetTextureSettings(importerSettings);

            importer.maxTextureSize = textureSize; // or whatever
            importer.alphaIsTransparency = true;
            importer.textureCompression = TextureImporterCompression.Compressed;
            importer.alphaSource = TextureImporterAlphaSource.FromInput;

            EditorUtility.SetDirty(importer);
            importer.SaveAndReimport();
            AssetDatabase.Refresh();
            EditorGUIUtility.PingObject(AssetDatabase.LoadAssetAtPath(directoryPath + "/" + Target.name + ".png", typeof(Texture2D)));


        }
    }

    public static Texture2D ToTexture2D(RenderTexture rTex, int textureSize)
    {
        Texture2D tex = new Texture2D(textureSize, textureSize, TextureFormat.RGBAFloat, false);
        RenderTexture.active = rTex;
    
        tex.ReadPixels(new Rect(0, 0, rTex.width, rTex.height), 0, 0);
        tex.Apply();
        tex.wrapMode = TextureWrapMode.Clamp;
        RenderTexture.active = null;
        return tex;
    }
}

public class BlobShadowBaker : EditorWindow
{
    
    public enum TextureSize
    {
        x64 = 64,
        x128 = 128,
        x256 = 256,
        x512 = 512,
        x1024 = 1024,
        x2048 = 2048

    }
    public LayerMask ObjectLayer;
    public Transform Target;
    public TextureSize textureSize = TextureSize.x1024;
    public float OrthographicSize = 1;
    [Range(0,2.0f)]
    public float BlurIntensity = 0.5f;
    [Range(8,128)]
    public int BlurIterations = 32;
    public bool includeChildren;



    [MenuItem("Tools/Shadow Baker")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        BlobShadowBaker window = (BlobShadowBaker)EditorWindow.GetWindow(typeof(BlobShadowBaker));
        window.Show();
    }

    void OnGUI()
    {
      
        Target = EditorGUILayout.ObjectField("Target", Target, typeof(Transform), true) as Transform;
        //  EditorGUILayout.LabelField("Object Layer");
        //ObjectLayer = EditorGUILayout.LayerField(ObjectLayer);
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("Texture Size");
        textureSize = (TextureSize)EditorGUILayout.EnumPopup(textureSize);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("Camera Scale");
        OrthographicSize = EditorGUILayout.FloatField(OrthographicSize);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("Include Children");
        includeChildren = EditorGUILayout.Toggle(includeChildren);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.LabelField("Blur",EditorStyles.boldLabel);
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("Intensity");
        BlurIntensity = EditorGUILayout.Slider(BlurIntensity,0,2.0f);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("Iterations");
        BlurIterations = EditorGUILayout.IntSlider(BlurIterations,8,128);
        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button("Bake"))
        {
            BlobShadowBakerUtils.BakeShadow(ObjectLayer, Target, (int)textureSize, OrthographicSize, BlurIntensity, BlurIterations, includeChildren);
        }
    }
}