using System;
using System.Collections.Generic;
using System.Text;
using LibSWBF2.MSH.Chunks;

namespace LibSWBF2.MSH.Types {
    public class CheckResult {
        /// <summary>
        /// Returns TRUE if the Integrity Check was successfull
        /// </summary>
        public bool IsValid { get { return isValid; } }
        private bool isValid = true;

        /// <summary>
        /// Error Messages, if the Integrity Check was unsuccessful
        /// </summary>
        public string[] Messages { get { return messages.ToArray(); } }
        private List<string> messages = new List<string>();


        /// <summary>
        /// Add an Error message. The Check Result automatically becomes invalid!
        /// </summary>
        /// <param name="message"></param>
        public void AddError(string message) {
            isValid = false;
            messages.Add(message);
        }

        /// <summary>
        /// <para>Merges two or more Results into one. Usefull to collect all chunk errors</para>
        /// </summary>
        /// <param name="results">The results to merge</param>
        /// <returns>The merged Result Information</returns>
        /// <exception cref="ArgumentNullException">if less than one result or NULL is provided</exception>
        public static CheckResult Merge(params CheckResult[] results) {
            if (results == null || results.Length == 0)
                throw new ArgumentNullException("No results given to merge!");

            if (results.Length == 1)
                return results[0];

            CheckResult result = new CheckResult();

            foreach (CheckResult res in results) {
                if (res != null) {
                    //if we encounter just one invalid case, the whole merged result stays invalid!
                    if (!res.isValid)
                        result.isValid = false;

                    //just merge non-empty messages
                    foreach (string msg in res.messages) {
                        if (!string.IsNullOrEmpty(msg))
                            result.messages.Add(msg);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// <para>Does an Integrity Check for all given chunks and merges the results</para>
        /// </summary>
        /// <param name="chunks">The chunks of which the Check Integrity results will be merged</param>
        /// <returns>The merged Result Information</returns>
        /// <exception cref="ArgumentNullException">if less than one result or NULL is provided</exception>
        public static CheckResult Merge(params BaseChunk[] chunks) {
            if (chunks == null || chunks.Length == 0)
                throw new ArgumentNullException("No chunks given to merge!");

            if (chunks.Length == 1)
                return chunks[0].CheckIntegrity();


            List<CheckResult> results = new List<CheckResult>();

            foreach (BaseChunk chunk in chunks) {
                if (chunk != null)
                    results.Add(chunk.CheckIntegrity());
            }

            return Merge(results.ToArray());
        }
    }
}
