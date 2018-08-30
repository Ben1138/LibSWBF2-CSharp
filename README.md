# LibSWBF2
C# Library for reading files from Star Wars Battlefront 2 (2005) Mod Tools.<br />
In the long term, this should be replaced by the [C++ Implementation](https://github.com/Ben1138/LibSWBF2)<br />

# MSH Files
1. Add using directive ```using LibSWBF2.MSH;```
2. Create a new instance ```MSH msh = MSH.LoadFromFile("C:\\myMesh.msh");```
3. Grab the information you want or modify the Mesh as you please
4. Save MSH File ```msh.WriteToFile("C:\\myModifiedMesh.msh");```

What's supported:
- Selection Information (Animation Begin/End, Framerate)
- Camera Information (Last Camera used by the modeller)
- Materials (add, delete, modify)
- Models (add, delete, modify, recognition for collision and terraincut)
- Mesh Segments (apply different Materials to different Segments)
- Vertices (add, delete, modify Position, Normal, UV)
- Polygons (add, delete, modify)

NDXL and NDXT Chunks are not supported. Polygon strips will be written into STRP Chunk<br />
(Zero Editor might not be able to open the mesh)

# Loading Worlds
1. Add using directive ```using LibSWBF2.WLD;```
2. Create a new instance ```Wld world = WLD.LoadFromFile("C:\\world1.wld");```
3. Grab the information you want

What's supported:
- World Layers (Auto recognition and reading)
- Object Placement Information (Name, Mesh File, Position and Rotation)
- Terrain (Height data)
