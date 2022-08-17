# VFX Graph Helper
This repository contains a collection of custom tools that extends the VFX graph package.

###### Current support: Unity 2021.3.8f1 with Visual Effect Graph 12.1.7 (custom package with minimal modifications)

**Usage of the project**
* Clone the repository or download the zip to use this project locally.
* Load the project using Unity 2021.3.8f1 or later
* Each scene (located at Scenes folder) contains a different Visual Effect graph examples

# Examples
Below are shown the examples developed on this repository.

## Generate position and color textures from Point cache asset
Since the [Point Cache asset](https://docs.unity3d.com/Packages/com.unity.visualeffectgraph@12.1/manual/point-cache-in-vfx-graph.html) can't be exposed as a property yet ([Point Cache Overhaul](https://portal.productboard.com/unity/1-unity-platform-rendering-visual-effects/c/118-point-cache-overhaul)), it can be generated a couple of textures (position and color) from a PointCache asset. Then, those textures can be assigned to the position and color maps of a Visual Effect asset.

For doing the above, you can access to the menu: **Tools/Visual Effect Graph/Generate Point Cache textures**

In the popup window that appears (*PointCacheVFXHelperEditor*), you will be able to select the PointCache asset to which you want to generate their respective textures (position | color).

![Point cache tool](http://drive.google.com/uc?export=view&id=1RW9NnEAVTCbQGEYsGFl0MkUDhoCoibr7)

Once you obtain those textures, you must to modify your VFX graph from using a [Point Cache operator](https://docs.unity3d.com/Packages/com.unity.visualeffectgraph@12.1/manual/Operator-PointCache.html).

![VFX using a Point cache operator](http://drive.google.com/uc?export=view&id=1ncSLXcRxNNvXmtt4v-malglgYS0Dlu1I)

To use a couple of Textures 2D that can be exposed in the VFX blackboard. So, the **position** and **color** textures must be directly connected to the _Set Position from map_ and _Set Color from Map_ respectively.

![VFX using a couple of textures generated from a PointCache asset](http://drive.google.com/uc?export=view&id=1pGbmt8NDRgLMiEDhq8rpTdGle4A0RzZJ)

This is the result of using this tool with the MorphingFace VFX asset.

![Point cache result](http://drive.google.com/uc?export=view&id=1RDrYq7RwfitY_ry7lUrLIKxYAc4g16SL)


Files to take into account for achieving this:
 * C# file(s): `PointCacheVFXHelperEditor.cs` (located at: *Packages/com.v3gs.vfxgraphhelper/Editor*)
 * VFX(s): `MorphingFace_PointCacheTextures.vfx` (located at: *Assets/VFXs*)
 * Texture(s): `MaskFace_position.exr` and `MaskFace_color.exr` (located at: *Assets/Samples/MorphingFace/pCache*)

> **Warning**
> 
> By default, the _PointCacheAsset_ is inaccessible due to its protection level. Therefore, I had to embedded the `com.unity.visualeffectgraph@12.1.7` package into my Package folder.
> 
> Then, I modified the PointCacheAsset class (*Packages/com.unity.visualeffectgraph/Editor/Utilities/pCache/PointCacheAsset.cs*) to make it `public`. This way, I can access to this class from my custom package (VFX Graph helper).

