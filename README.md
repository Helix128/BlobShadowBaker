# Blob Shadow Baker
 Simple tool for baking blob shadow textures in Unity!

# (OLD) Demo GIFs
 [Demo GIF 1](https://gyazo.com/5ce594c8f29729b1841618a4561612d3)
 [Demo GIF 2](https://gyazo.com/25377563c485102c4fdbdfa44dd7f5a2)
 
# Usage
The tool is located at the editor tab Tools->Shadow Baker.
 
# Parameters:
 -Target
 
 The target Transform of the object that you want to create a shadow.
 
 -Texture Size
 
 Shadow texture resolution. Higher means more quality but a larger file size.
 
 -Orthographic Scale
 
 Scale of the camera used for rendering the shadow. 
 Increase it if the shadow does not fit the image.
 
 -Blur Intensity/Iterations
 
 Gaussian blur effect for the shadow texture. Increase the iteration count if your texture has banding.
 
 -Bake Children
 
 Enable/disable children gameobjects appearing in the shadow.
 
 # Output
 The shadow output will be generated at BlobShadowBaker/Output/(ObjectName+ObjectID).png
 
 # Extra
 This asset also includes a [shadow decal shader by Ronja Böhringer](https://github.com/ronja-tutorials/ShaderTutorials/blob/master/Assets/054_Unlit_Decals/UnlitDynamicDecal.shader) for the Built-in RP.


