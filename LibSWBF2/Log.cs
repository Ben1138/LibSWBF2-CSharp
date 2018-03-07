using System;
using System.Collections.Generic;
using System.Text;

namespace LibSWBF2 {
    public enum LogType {
        Info = 0,
        Warning = 1,
        Error = 2
    }

    public class Log {
        private struct LogEntry {
            public string message;
            public LogType level;

            public LogEntry(string message, LogType level) {
                this.message = message;
                this.level = level;
            }
        }

        private static List<LogEntry> logEntrys = new List<LogEntry>();
        private static int lastIndex = 0;

        public static void Add(string message, LogType level) {
            if (string.IsNullOrEmpty(message))
                return;

            logEntrys.Add(new LogEntry(message, level));
        }

        public static string[] GetAllLines(LogType level) {
            List<string> resLines = new List<string>();

            for (int i = 0; i < logEntrys.Count; i++) {
                if (logEntrys[i].level >= level) {
                    resLines.Add(logEntrys[i].message);
                }
            }

            return resLines.ToArray();
        }

        public static string[] GetLastLines(LogType level) {
            List<string> resLines = new List<string>();

            for (int i = lastIndex; i < logEntrys.Count; i++) {
                if (logEntrys[i].level >= level) {
                    resLines.Add(logEntrys[i].message);
                }
            }

            //save position for next request
            lastIndex = logEntrys.Count - 1;

            return resLines.ToArray();
        }
    }
}
