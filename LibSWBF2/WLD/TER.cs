using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Security;
using LibSWBF2.Types;
using LibSWBF2.Exceptions;
using LibSWBF2.WLD.Types;

namespace LibSWBF2.WLD {
    public class TER {
        /// <summary>
        /// Gets the Version of the Terrain (SWBF1 or SWBF2)
        /// </summary>
        public TerrainVersion Version { get; private set; } = TerrainVersion.Unknown;

        /// <summary>
        /// <para>Terrain extent in comparison to the Map Center. E.g. for a 256x256 Terrain, these value are typically -128, -128, 128, 128</para>
        /// <para>This does also function to clip the Terrain Data</para>
        /// </summary>
        public TerrainExtent Extent { get; private set; } = new TerrainExtent();

        /// <summary>
        /// Grid Size of the Terrain, e.g. 256 for a 256x256 Map
        /// </summary>
        public int GridSize { get; private set; }

        /// <summary>
        /// Terrain Texture Layers. A Terrain always has 16 Layers.
        /// </summary>
        public TerrainLayer[] Layers { get; private set; } = new TerrainLayer[16];

        /// <summary>
        /// Multiplier for the Terrain Heights
        /// </summary>
        public float HeightMultiplier { get; private set; }

        /// <summary>
        /// Scale of the Grid (distance between Points)
        /// </summary>
        public float GridScale { get; private set; }

        /// <summary>
        /// Storing the Terrain Height of each Point (Raw without Height Multiplier)
        /// </summary>
        public short[,] RawHeights { get; private set; } = new short[0,0];


        /// <summary>
        /// Loads Terrain from File
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Path given is not valid</exception>
        /// <exception cref="EndOfDataException">Unexpected end of Data</exception>
        /// <exception cref="FileNotFoundException">File could not be found</exception>
        /// <exception cref="InsufficientPermissionsException">Insufficient Permissions</exception>
        /// <exception cref="IOException">Read / Write Error</exception>
        public static TER LoadFromFile(string path) {
            ChunkStream stream = null;

            try {
                stream = new ChunkStream(path, FileMode.Open, FileAccess.Read);
            } catch (ArgumentException ex) {
                Log.Add("Path given is not valid!", LogType.Error);
                throw new ArgumentException("Path given is not valid!", ex);
            } catch (NotSupportedException ex) {
                Log.Add("Path given is not valid!", LogType.Error);
                throw new ArgumentException("Path given is not valid!", ex);
            } catch (PathTooLongException ex) {
                Log.Add("Path given is not valid!", LogType.Error);
                throw new ArgumentException("Path given is not valid!", ex);
            } catch (UnauthorizedAccessException ex) {
                Log.Add("Insufficient Permissions!", LogType.Error);
                throw new InsufficientPermissionsException("Insufficient Permissions!", ex);
            } catch (SecurityException ex) {
                Log.Add("Insufficient Permissions!", LogType.Error);
                throw new InsufficientPermissionsException("Insufficient Permissions!", ex);
            }

            TER terrain = new TER();

            if (!stream.ReadString(4).Equals("TERR"))
                throw new InvalidChunkException("Not a valid Terrain File: " + path);
           
            switch (stream.ReadInt32()) {
                case 21:
                    terrain.Version = TerrainVersion.SWBF1;
                    break;
                case 22:
                    terrain.Version = TerrainVersion.SWBF2;
                    break;
                default:
                    Log.Add("Unknown Terrain Version found!", LogType.Warning);
                    terrain.Version = TerrainVersion.Unknown;
                    break;
            }

            //Terrain Extent (From Position, To Position);
            terrain.Extent.FromX = stream.ReadInt16();
            terrain.Extent.FromY = stream.ReadInt16();
            terrain.Extent.ToX = stream.ReadInt16();
            terrain.Extent.ToY = stream.ReadInt16();

            //Int32, unknown purpose
            stream.SkipBytes(4);

            //16 floats follow, describing the Tile Range of each Texture Layer
            for (int i = 0; i < terrain.Layers.Length; i++) {
                //tile range is stored as reciprocal (1/X), e.g. 0.03125 for actually 32
                //to get the true tile size, we have ge the reciprocal again
                // (1 / 32) = 0.03125     (1 / 0.03125) = 32
                terrain.Layers[i].TileRange = 1 / stream.ReadFloat();
            }

            //16 bytes follow, describing mapping type for each layer
            for (int i = 0; i < terrain.Layers.Length; i++) {
                terrain.Layers[i].MappingType = stream.ReadByte();
            }

            //unknown purpose of bytes
            stream.SkipBytes(64);

            terrain.HeightMultiplier = stream.ReadFloat();
            terrain.GridScale = stream.ReadFloat();

            //unknown int32
            stream.SkipBytes(4);

            //Full Map Size (e.g. 256 for a 256x256 Map)
            terrain.GridSize = stream.ReadInt32();

            //unknown int32
            stream.SkipBytes(4);

            //SWBF2 Versions have a byte exactly on this point. purpose unknown
            if (terrain.Version == TerrainVersion.SWBF2)
                stream.SkipBytes(1);

            //16 Texture Layers follow (total length of 1024)
            //char[32]	Diffuse texture name.
            //char[32]  Detail texture name.
            for (int i = 0; i < terrain.Layers.Length; i++) {
                terrain.Layers[i].DiffuseTexture = stream.ReadString(32).Trim();
                terrain.Layers[i].DetailTexture = stream.ReadString(32).Trim();
            }

            //skip water layers
            stream.SkipBytes(1088);

            //unknown bytes
            stream.SkipBytes(524);

            //Terrain Height Data
            Log.Add("Size: " + terrain.GridSize, LogType.Info);

            terrain.RawHeights = new short[terrain.GridSize, terrain.GridSize];

            for (int x = 0; x < terrain.GridSize; x++) {
                for (int y = 0; y < terrain.GridSize; y++) {
                    terrain.RawHeights[x, y] = stream.ReadInt16();
                }
            }

            stream.Close();

            return terrain;
        }

        /// <summary>
        /// Get Terrain Height at specific Point. X or Y values out of Range will be clamped!
        /// </summary>
        public float GetHeight(int x, int y) {
            x = Math.Clamp(x, 0, RawHeights.GetLength(0) - 1);
            y = Math.Clamp(y, 0, RawHeights.GetLength(1) - 1);

            return RawHeights[x, y] * HeightMultiplier;
        }
    }
}
