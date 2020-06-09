# ShadowBaker
 Simple tool for baking blob shadow textures in Unity!
 
 [Demo GIF 1](https://gyazo.com/5ce594c8f29729b1841618a4561612d3)
 [Demo GIF 2](https://gyazo.com/25377563c485102c4fdbdfa44dd7f5a2)
 
# Usage

 Required setup: Create a new layer called "Shadowcaster".

 Open the Shadow Baker window from the Tools section of the top layout.
 
# Parameters:
 -Target
 
 The target Transform of the object that you want to create a shadow.
 
 -Texture Size
 
 Shadow texture resolution. Higher means more quality but a larger file size.
 
 -Orthographic Scale
 
 Scale of the camera used for rendering the shadow. 
 Increase it if the shadow appears black or decrease it if it appears too small or white.
 
 -Shadow Blur
 
 Gaussian blur effect for the shadow Texture
 
 -Bake Children
 
 Enable/disable children appearing in the shadow.
 
 # Output
 The shadow output will be generated at Assets/Shadows/(ObjectName)(ObjectID).png
 
 # Extra
 This asset also includes a shadow projector shader and a projector falloff texture.
 
 # Known Issues
 
 "An infinite import loop has been detected. The following Assets were imported multiple times, but no changes to them have been detected. Please check if any custom code is trying to import them" and "Could not create asset from Assets/Shadows/(your object).png: File could not be read"
 
 This issue only happens the first time you try to bake a shadow. If there is no shadow texture at the Shadows folder,try again and it will bake correctly.
 
 # Future features (work in progress)
 
 -Custom shadow direction
 
 # About
 
 Created by Helix a.k.a Diegoatari8 with Unity 2019

