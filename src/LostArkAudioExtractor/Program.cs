using LostArkAudioExtractor.Classes;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace LostArkAudioExtractor {

    internal class Program {

        static void Main(string[] args) {

            try {

                Console.Clear();
                Console.WriteLine(@"--------------------------------------------------------------------------------------------------");
                Console.WriteLine(@"           ___                       ___                   ___           ___                      ");
                Console.WriteLine(@"          /\  \          ___        /\__\      ___        /\__\         /\  \          ___        ");
                Console.WriteLine(@"         /::\  \        /\  \      /:/  /     /\  \      /::|  |       /::\  \        /\  \       ");
                Console.WriteLine(@"        /:/\:\  \       \:\  \    /:/  /      \:\  \    /:|:|  |      /:/\:\  \       \:\  \      ");
                Console.WriteLine(@"       /::\~\:\__\      /::\__\  /:/  /       /::\__\  /:/|:|__|__   /::\~\:\__\      /::\__\     ");
                Console.WriteLine(@"      /:/\:\ \:|__|  __/:/\/__/ /:/__/     __/:/\/__/ /:/ |::::\__\ /:/\:\ \:|__|  __/:/\/__/     ");
                Console.WriteLine(@"      \:\~\:\/:/  / /\/:/  /    \:\  \    /\/:/  /    \/__/~~/:/  / \:\~\:\/:/  / /\/:/  /        ");
                Console.WriteLine(@"       \:\ \::/  /  \::/__/      \:\  \   \::/__/           /:/  /   \:\ \::/  /  \::/__/         ");
                Console.WriteLine(@"        \:\/:/  /    \:\__\       \:\  \   \:\__\          /:/  /     \:\/:/  /    \:\__\         ");
                Console.WriteLine(@"         \::/__/      \/__/        \:\__\   \/__/         /:/  /       \::/__/      \/__/         ");
                Console.WriteLine(@"          --                        \/__/                 \/__/         --                        ");
                Console.WriteLine(@"                                                                                                  ");
                Console.WriteLine(@"                              -- An audio extractor for Lost Ark --                               ");
                Console.WriteLine(@"                                                                                                  ");
                Console.WriteLine(@"--------------------------------------------------------------------------------------------------");
                Console.WriteLine($"> TheDeadNorth | v{Utils.Version}");
                Console.WriteLine($"> GitHub | https://github.com/Twigzie/Fantality-LostArkAudioExtractor");
                Console.WriteLine($"> Updates | https://github.com/Twigzie/Fantality-LostArkAudioExtractor/releases");
                Console.WriteLine($"> LAUTool | (c) 2021 / Ekey (h4x0r) / v0.3 | https://github.com/Ekey");
                Console.WriteLine($"> Wwise-Unpacker | https://github.com/Vextil/Wwise-Unpacker");
                Console.WriteLine(@"--------------------------------------------------------------------------------------------------");
                Console.WriteLine();

//#if DEBUG

//                args = new string[] {
//                    @"J:\GitHub\Fantality-LostArkAudioExtractor\files\test.bnk"
//                };

//#endif

                if (args.Length <= 0) {

                    Console.WriteLine("[Usage]");
                    Console.WriteLine();
                    Console.WriteLine("\t1) LostArkAudioExtractor.exe [ m | o ] [ source_file ]");
                    Console.WriteLine();
                    Console.WriteLine("\t\t[ m ]: The specified file will be converted to MP3.");
                    Console.WriteLine("\t\t[ o ]: The specified file will be converted to OGG.");
                    Console.WriteLine();
                    Console.WriteLine("\t\t[ source_file ]: The source file to decrypt");
                    Console.WriteLine();
                    Console.WriteLine("\tNOTE: If the source file contains multiple audios files, they will also be converted to the argument that was specified.");
                    Console.WriteLine();

                }
                else {

                    if (Utils.IsFile(args[0])) {

                        Utils.SetOK("[Method]");

                        SELECTION:
                        Console.WriteLine();
                        Console.WriteLine($"\t> Source: {args[0]}");
                        Console.WriteLine();
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine($"\t> Please specify the type of method you want to use");
                        Console.WriteLine($"\t1) MP3");
                        Console.WriteLine($"\t2) OGG");
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine();

                        var type = Console.ReadLine();
                        if (type == "1" || type == "2") {
                            switch (type) {
                                case "1": ProcessFileAsType(ExportType.MP3, args[0]); break;
                                case "2": ProcessFileAsType(ExportType.OGG, args[0]); break;
                            }
                        }
                        else {
                            goto SELECTION;
                        }

                    }
                    else {

                        if (Utils.IsArg(args[0]) == false && Utils.IsFile(args[0]) == false) {

                            Utils.SetError("Invalid argument specified!");
                            Utils.SetWarning($"\t> Argument: '{args[0]}'", false);
                            Utils.SetWarning($"\t> Error: The specified argument is not valid", false);

                            return;

                        }
                        else {

                            if (Utils.IsFile(args[1]) == false) {

                                Utils.SetError("Invalid argument value specified!");
                                Utils.SetWarning($"\t> Argument: '{args[0]}'", false);
                                Utils.SetWarning($"\t> Value: '{args[1]}'", false);
                                Utils.SetWarning($"\t> Error: The specified value is not a file.", false);

                                return;

                            }

                            ProcessFile(args[0], args[1]);

                        }

                    }

                }

            }
            catch (Exception ex) {
                Utils.SaveException(ex);
            }
            finally {

                Console.WriteLine();
                Console.WriteLine("Done!");
                Console.WriteLine();
                Console.ReadLine();

            }

        }

        public static void ProcessFile(string type, string source) {
            try {

                var export = type.ToExportType();
                switch (export) {
                    case ExportType.MP3:
                    case ExportType.OGG:
                        ProcessFileAsType(export, source);
                        break;
                    case ExportType.Unknown:
                    default:

                        Utils.SetError("Invalid type specified!");
                        Utils.SetWarning($"\t> Type: '{export}'", false);
                        Utils.SetWarning($"\t> Error: The specified type is not valid for this process.", false);

                        break;
                }

            }
            catch (Exception ex) {
                throw ex;
            }
        }
        private static void ProcessFileAsType(ExportType type, string source) {
            try {

                Utils.SetOK("[Processing]");
                Console.WriteLine();
                Console.WriteLine($"\t> Type: {type}");
                Console.WriteLine($"\t> Source: {source}");
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write($"\t> [Verifying]...");
                Console.ForegroundColor = ConsoleColor.White;

                if (Utils.VerifyToolsExistLAU() == false ||
                    Utils.VerifyToolsExistWwise() == false) {

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("Failed!");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine();

                    throw new Exception("Unable to locate required files for exporting the specified file!");

                }
                else {

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("Pass!");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write($"\t> [Removing existing files]...");
                    Console.ForegroundColor = ConsoleColor.White;

                    if (Utils.CleanOldFiles()) {

                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write("Pass!");
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine();

                    }
                    else {

                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("Failed!");
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine();

                        if (MessageBox.Show("Failed to delete existing files from an previous session!\r\r" +
                                            "You can continue, but might run into errors if the files already exist.\r\r" +
                                            "Continue?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Error, MessageBoxDefaultButton.Button2) == DialogResult.No) {

                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine($"Process canceled by the user.");
                            Console.ForegroundColor = ConsoleColor.White;

                            return;

                        }

                    }

                    var sourceName = Path.GetFileNameWithoutExtension(source);
                    var targetName = sourceName.Replace(sourceName, $"{sourceName}_decrypted.bnk");
                    var targetFile = Path.Combine(Utils.OutputGameFiles, targetName);

                    using (var process_lau = new Process()) {

                        Console.WriteLine();
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write($"\t> [Attempting to launch LAUTool]...");
                        Console.ForegroundColor = ConsoleColor.White;

                        process_lau.StartInfo = new ProcessStartInfo() {
                            WorkingDirectory = Path.GetDirectoryName(Utils.ConverterLAU),
                            FileName = Utils.ConverterLAU,
                            Arguments = $"-d \"{source}\" \"{targetFile}\""
                        };
                        process_lau.EnableRaisingEvents = true;
                        process_lau.Exited += (sender_lau, event_lau) => {

                            using (var process_wwise = new Process()) {

                                Console.WriteLine();
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                Console.Write($"\t> [Attempting to launch Wwise-Unpacker]...");
                                Console.ForegroundColor = ConsoleColor.White;

                                process_wwise.StartInfo = new ProcessStartInfo() {
                                    WorkingDirectory = Path.GetDirectoryName(Utils.ConverterMP3),
                                    FileName = Utils.ConverterMP3,
                                };
                                process_wwise.EnableRaisingEvents = true;
                                process_wwise.Exited += (sender_wwise, event_wwise) => {

                                    switch (type) {
                                        case ExportType.MP3:

                                            //HACK!! LAUTool wont work when using the -d argument so im forced to use -e which which to versions of the output file. Just delete the version of each file that contains the original filename.. gross
                                            foreach (var file in Directory.GetFiles(Utils.OutputMP3Files).Where(f => f.Contains(Path.GetFileNameWithoutExtension(targetFile))))
                                                File.Delete(file);

                                            Process.Start(Utils.OutputMP3Files);

                                            break;
                                        case ExportType.OGG:

                                            //HACK!! LAUTool wont work when using the -d argument so im forced to use -e which which to versions of the output file. Just delete the version of each file that contains the original filename.. gross
                                            foreach (var file in Directory.GetFiles(Utils.OutputOGGFiles).Where(f => f.Contains(Path.GetFileNameWithoutExtension(targetFile))))
                                                File.Delete(file);

                                            Process.Start(Utils.OutputOGGFiles);

                                            break;
                                    }

                                };

                                if (process_wwise.Start()) {

                                    Console.ForegroundColor = ConsoleColor.Green;
                                    Console.Write("Launched!");
                                    Console.ForegroundColor = ConsoleColor.White;
                                    Console.WriteLine();
                                    Console.WriteLine();
                                    Console.ForegroundColor = ConsoleColor.Magenta;
                                    Console.WriteLine("Please wait for the process to exit before closing this application! It may take some time depending on the file you've selected!");
                                    Console.ForegroundColor = ConsoleColor.White;

                                    while (process_wwise.HasExited == false) {

                                        Console.Write(".");
                                        Thread.Sleep(1000);

                                    }

                                }
                                else {

                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.Write("Failed!");
                                    Console.ForegroundColor = ConsoleColor.White;

                                }

                            }

                        };

                        if (process_lau.Start()) {

                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write("Launched!");
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.WriteLine();
                            Console.WriteLine();
                            Console.ForegroundColor = ConsoleColor.Magenta;
                            Console.WriteLine("Please wait for the process to exit before closing this application! It may take some time depending on the files you've specified!");
                            Console.ForegroundColor = ConsoleColor.White;

                            process_lau.WaitForExit();

                        }
                        else {

                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write("Failed!");
                            Console.ForegroundColor = ConsoleColor.White;

                        }

                    }

                }

            }
            catch (Exception ex) {
                throw ex;
            }
        }

    }

}