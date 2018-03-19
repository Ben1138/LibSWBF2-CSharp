using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Security;
using LibSWBF2.Exceptions;

namespace LibSWBF2.WLD {
    public class WLD {
        public string LightFile = "";
        public TER Terrain { get; private set; } = null;
        public string SkyFile = "";
        public List<LYR> Layers { get; private set; } = new List<LYR>();


        /// <summary>
        /// Loads a .wld File
        /// </summary>
        /// <param name="path">The path to the .wld file</param>
        /// <returns>A new WLD instance</returns>
        /// <exception cref="ArgumentException">Path given is not valid</exception>
        /// <exception cref="EndOfDataException">Unexpected end of Data</exception>
        /// <exception cref="FileNotFoundException">File could not be found</exception>
        /// <exception cref="InsufficientPermissionsException">Insufficient Permissions</exception>
        /// <exception cref="IOException">Read / Write Error</exception>
        public static WLD LoadFromFile(string path) {
            FileInfo fileInfo = new FileInfo(path);

            StreamReader reader = null;
            string fileContent = "";

            try {
                reader = new StreamReader(path, Encoding.ASCII, false);
                fileContent = reader.ReadToEnd();

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
            finally {
                if (reader != null)
                    reader.Close();
            }

            WLD wld = new WLD();


            Match m = Regex.Match(fileContent, @"LightName\([""]([A-Za-z0-9_-]+\.[A-Za-z]+)[""]\)");
            if (m.Success && m.Groups.Count >= 2) {
                wld.LightFile = m.Groups[1].Value;
            }

            m = Regex.Match(fileContent, @"TerrainName\([""]([A-Za-z0-9_-]+\.[A-Za-z]+)[""]\)");
            if (m.Success && m.Groups.Count >= 2) {
                string terrainPath = fileInfo.DirectoryName + "\\" + m.Groups[1].Value;

                if (File.Exists(terrainPath)) {
                    wld.Terrain = TER.LoadFromFile(terrainPath);
                }
            }

            m = Regex.Match(fileContent, @"SkyName\([""]([A-Za-z0-9_-]+\.[A-Za-z]+)[""]\)");
            if (m.Success && m.Groups.Count >= 2) {
                wld.SkyFile = m.Groups[1].Value;
            }

            //Load [Base] Layer
            LYR Base = LYR.LoadFromString(fileContent);
            Base.Name = "[Base]";
            wld.Layers.Add(Base);

            string[] layerFiles = Directory.GetFiles(fileInfo.DirectoryName, "*.lyr", SearchOption.AllDirectories);

            //load all additional layers
            foreach (string file in layerFiles) {
                wld.Layers.Add(LYR.LoadFromFile(file));
            }

            return wld;
        }
    }
}
