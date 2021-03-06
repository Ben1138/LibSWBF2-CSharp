﻿using System;
using System.Security;
using System.IO;
using System.Collections.Generic;
using LibSWBF2.Types;
using LibSWBF2.MSH.Chunks;
using LibSWBF2.MSH.Types;
using LibSWBF2.Exceptions;

namespace LibSWBF2.MSH {
    public class MSH {
        private HEDR header;

        /// <summary>
        /// <para>Optional, Dispensable</para>
        /// <para>Selection Information for the Zero Editor.</para>
        /// <para>Returns NULL if no Information is specified!</para>
        /// </summary>
        public SINF SelectionInformation { get { return header.Mesh.SelectionInformation; } }

        /// <summary>
        /// <para>Dispensable</para>
        /// <para>Presumably just used to store the last Camera Position used by the modeller.</para>
        /// </summary>
        public CAMR Camera { get { return header.Mesh.Camera; } }

        /// <summary>
        /// All Materials this MSH contains
        /// </summary>
        public List<MATD> Materials { get { return header.Mesh.MaterialList.Materials; } }

        /// <summary>
        /// All Models this MSH contains
        /// </summary>
        public List<MODL> Models { get { return header.Mesh.Models; } }


        /// <summary>
        /// Does an integrity check to see if all values are properly set and in a valid range
        /// </summary>
        /// <returns>Contains all found errors</returns>
        public CheckResult CheckIntegrity() {
            CheckResult result = new CheckResult();

            //check for double uses of model indices
            for (int i = 0; i < Models.Count - 1; i++) {
                for (int j = i+1; j < Models.Count; j++) {
                    if (Models[i].Index == Models[j].Index) {
                        result.AddError(
                            string.Format(
                                "The Model Index {0} is used by both {1} and {2}",
                                Models[i].Index,
                                Models[i].Name,
                                Models[j].Name
                            )
                        );
                    }
                }
            }

            try {
                result = CheckResult.Merge(result, CheckResult.Merge(SelectionInformation));
                result = CheckResult.Merge(result, CheckResult.Merge(Materials.ToArray()));
                result = CheckResult.Merge(result, CheckResult.Merge(Models.ToArray()));
            }
            catch (ArgumentNullException ex) {
                result.AddError(ex.Message);
                return result;
            }
            catch (Exception ex) {
                result.AddError("An Unknown Error occured! " + ex.Message);
                return result;
            }

            return result;
        }


        /// <summary>
        /// Loads Mesh from File
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Path given is not valid</exception>
        /// <exception cref="EndOfDataException">Unexpected end of Data</exception>
        /// <exception cref="FileNotFoundException">File could not be found</exception>
        /// <exception cref="InsufficientPermissionsException">Insufficient Permissions</exception>
        /// <exception cref="IOException">Read / Write Error</exception>
        public static MSH LoadFromFile(string path) {
            MSH msh = new MSH();

            Log.Add("Open File " + path, LogType.Info);
            ChunkStream stream = null;
            
            try {
                stream = new ChunkStream(path, FileMode.Open, FileAccess.Read);
                msh.header = new HEDR(BaseChunk.FromData(stream, msh));

                Log.Add("Flushing raw Data", LogType.Info);
                msh.header.FlushData();
            }
            catch (ArgumentException ex) {
                Log.Add("Path given is not valid!", LogType.Error);
                throw new ArgumentException("Path given is not valid!", ex);
            }
            catch (NotSupportedException ex) {
                Log.Add("Path given is not valid!", LogType.Error);
                throw new ArgumentException("Path given is not valid!", ex);
            }
            catch (PathTooLongException ex) {
                Log.Add("Path given is not valid!", LogType.Error);
                throw new ArgumentException("Path given is not valid!", ex);
            }
            catch (UnauthorizedAccessException ex) {
                Log.Add("Insufficient Permissions!", LogType.Error);
                throw new InsufficientPermissionsException("Insufficient Permissions!", ex);
            }
            catch (SecurityException ex) {
                Log.Add("Insufficient Permissions!", LogType.Error);
                throw new InsufficientPermissionsException("Insufficient Permissions!", ex);
            }
            finally {
                if (stream != null)
                    stream.Close();
            }

            //Apply Parent Reference AFTER we loaded all Models
            Log.Add("Apply References", LogType.Info);
            foreach (MODL mdl in msh.Models) {
                mdl.ApplyReferences();
            }

            Log.Add("DONE!", LogType.Info);
            
            return msh;
        }

        public void WriteToFile(string path) {
            header.WriteData();

            FileStream stream;

            try {
                stream = new FileStream(path, FileMode.Create, FileAccess.Write);
            }
            catch (ArgumentException ex) {
                Log.Add("Path given is not valid!", LogType.Error);
                throw new ArgumentException("Path given is not valid!", ex);
            }
            catch (NotSupportedException ex) {
                Log.Add("Path given is not valid!", LogType.Error);
                throw new ArgumentException("Path given is not valid!", ex);
            }
            catch (PathTooLongException ex) {
                Log.Add("Path given is not valid!", LogType.Error);
                throw new ArgumentException("Path given is not valid!", ex);
            }
            catch (UnauthorizedAccessException ex) {
                Log.Add("Insufficient Permissions!", LogType.Error);
                throw new InsufficientPermissionsException("Insufficient Permissions!", ex);
            }
            catch (SecurityException ex) {
                Log.Add("Insufficient Permissions!", LogType.Error);
                throw new InsufficientPermissionsException("Insufficient Permissions!", ex);
            }

            stream.Write(header.Data, 0, header.Data.Length);
            stream.Close();
        }
    }
}