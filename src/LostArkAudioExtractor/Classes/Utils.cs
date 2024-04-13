using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace LostArkAudioExtractor.Classes {

    internal class Utils {

        public static string Version {
            get => Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }
        public static string Root {
            get => Environment.CurrentDirectory;
        }
        public static string ErrorOutput {
            get => Path.Combine(Root, "error.txt");
        }

        public static string OutputMP3Files {
            get => Path.Combine(Root, "Wwise-Unpacker\\MP3");
        }
        public static string OutputOGGFiles {
            get => Path.Combine(Root, "Wwise-Unpacker\\OGG");
        }
        public static string OutputGameFiles {
            get => Path.Combine(Root, "Wwise-Unpacker\\Game Files");
        }
        public static string OutputDecodedFiles {
            get => Path.Combine(Root, "Wwise-Unpacker\\Tools\\Decoding");
        }
        public static string OutputDecryptedFiles {
            get => Path.Combine(Root, "LAU-Tool");
        }

        public static string ConverterLAU {
            get => Path.Combine(Root, "LAU-Tool\\LAUTool.exe");
        }
        public static string ConverterMP3 {
            get => Path.Combine(Root, "Wwise-Unpacker\\Unpack to MP3.bat");
        }
        public static string ConverterOGG {
            get => Path.Combine(Root, "Wwise-Unpacker\\Unpack to OGG.bat");
        }

        public static bool IsArg(string source) {
            return source.ToLower() == "m" ||
                   source.ToLower() == "o";
        }
        public static bool IsFile(string source) {
            try {
                var attributes = File.GetAttributes(source);
                if ((attributes & FileAttributes.Directory) == FileAttributes.Directory)
                    return false;
                return Path.GetExtension(source) == ".bnk";
            }
            catch {
                if (string.IsNullOrEmpty(Path.GetExtension(source)))
                    return false;
                return true;
            }
        }

        public static bool CleanOldFiles() {
            try {

                foreach (var file in Directory.GetFiles(OutputDecryptedFiles).Where(f => Path.GetFileName(f).ToLower() != "lautool.exe")) {
                    File.Delete(file);
                }

                if (Directory.Exists(OutputMP3Files) == false) {
                    Directory.CreateDirectory(OutputMP3Files);
                }
                else {
                    foreach (var file in Directory.GetFiles(OutputMP3Files))
                        File.Delete(file);
                }

                if (Directory.Exists(OutputOGGFiles) == false) {
                    Directory.CreateDirectory(OutputOGGFiles);
                }
                else {
                    foreach (var file in Directory.GetFiles(OutputOGGFiles))
                        File.Delete(file);
                }

                if (Directory.Exists(OutputGameFiles) == false) {
                    Directory.CreateDirectory(OutputGameFiles);
                }
                else {
                    foreach (var file in Directory.GetFiles(OutputGameFiles))
                        File.Delete(file);
                }

                if (Directory.Exists(OutputDecodedFiles) == false) {
                    Directory.CreateDirectory(OutputDecodedFiles);
                }
                else {
                    foreach (var file in Directory.GetFiles(OutputDecodedFiles))
                        File.Delete(file);
                }

                return true;

            }
            catch  {
                return false;
            }
        }

        public static bool VerifyToolsExistLAU() {
            try {

                return Directory.Exists(OutputDecryptedFiles) &&
                       File.Exists(Path.Combine(Root, "LAU-Tool\\LAUTool.exe"));
            }
            catch {
                return false;
            }
        }
        public static bool VerifyToolsExistWwise() {
            try {

                return Directory.Exists(Path.Combine(Root, "Wwise-Unpacker")) && 
                       File.Exists(Path.Combine(Root, "Wwise-Unpacker\\Tools\\bnkextr.exe")) &&
                       File.Exists(Path.Combine(Root, "Wwise-Unpacker\\Tools\\ffmpeg.exe")) &&
                       File.Exists(Path.Combine(Root, "Wwise-Unpacker\\Tools\\quickbms.exe")) &&
                       File.Exists(Path.Combine(Root, "Wwise-Unpacker\\Tools\\revorb.exe")) &&
                       File.Exists(Path.Combine(Root, "Wwise-Unpacker\\Tools\\ww2ogg.exe")) &&
                       File.Exists(Path.Combine(Root, "Wwise-Unpacker\\Tools\\packed_codebooks_aoTuV_603.bin")) &&
                       File.Exists(Path.Combine(Root, "Wwise-Unpacker\\Tools\\wavescan.bms")) &&
                       File.Exists(Path.Combine(Root, "Wwise-Unpacker\\Unpack to MP3.bat")) &&
                       File.Exists(Path.Combine(Root, "Wwise-Unpacker\\Unpack to OGG.bat"));

            }
            catch {
                return false;
            }
        }

        public static void SetInfo(string result, bool title = true) {
            Console.ForegroundColor = ConsoleColor.Green;
            if (title) {
                Console.WriteLine($"[Info]: {result}");
            }
            else
                Console.WriteLine($"{result}");
            Console.ForegroundColor = ConsoleColor.White;
        }
        public static void SetOK(string result) {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(result);
            Console.ForegroundColor = ConsoleColor.White;
        }
        public static void SetError(string result, bool title = true) {
            Console.ForegroundColor = ConsoleColor.Red;
            if (title) {
                Console.WriteLine($"[Error]: {result}");
            }
            else
                Console.WriteLine($"{result}");
            Console.ForegroundColor = ConsoleColor.White;
        }
        public static void SetWarning(string result, bool title = true) {
            Console.ForegroundColor = ConsoleColor.Yellow;
            if (title) {
                Console.WriteLine($"[Warning]: {result}");
            }
            else
                Console.WriteLine($"{result}");
            Console.ForegroundColor = ConsoleColor.White;
        }
        public static void SaveException(Exception result) {
            try {

                SetError(result.ToString());

                using (var stream = File.OpenWrite(ErrorOutput))
                using (var writer = new StreamWriter(stream)) {
                    writer.Write(result.ToString());
                }

            }
            catch (Exception ex) {
                SetError(ex.ToString());
            }
        }

    }

}