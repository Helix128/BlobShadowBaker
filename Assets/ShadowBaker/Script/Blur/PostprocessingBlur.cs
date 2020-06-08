using UnityEngine;
[ExecuteInEditMode]
//behaviour which should lie on the same gameobject as the main camera
public class PostprocessingBlur : MonoBehaviour
{
    //material that's applied when doing postprocessing
    [SerializeField]
    private Material postprocessMaterial;
    public float blurSize;
    private void OnEnable()
    {
        postprocessMaterial = new Material(Shader.Find("Hidden/Blur"));
  
    }
    //method which is automatically called by unity after the camera is done rendering
    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        postprocessMaterial.SetFloat("_BlurSize", blurSize/25);
   
        //draws the pixels from the source texture to the destination texture
        var temporaryTexture = RenderTexture.GetTemporary(source.width, source.height);
        Graphics.Blit(source, temporaryTexture, postprocessMaterial, 0);
        Graphics.Blit(temporaryTexture, destination, postprocessMaterial, 1);
        RenderTexture.ReleaseTemporary(temporaryTexture);
    }
  
}