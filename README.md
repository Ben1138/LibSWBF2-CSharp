# LibSWBF2
C# Library for reading files from Star Wars Battlefront 2 (2005) Mod Tools
So far just reading/writing MSH Files is supported.

# MSH Files
1. Add using directive to MSH ```using LibSWBF2.MSH;``` 
2. Create a new instance of MSH ```MSH msh = MSH.LoadFromFile("C:\\myMesh.msh");``` 
3. Grab the information you want or modify the Mesh as you please
4. Save MSH File ```msh.WriteToFile("C:\\myModifiedMesh.msh");```

NDXL and NDXT Chunks are not supported. Polygon strips will be written into STRP Chunk
(Seems like Zero Editor doesn't like STRP)
