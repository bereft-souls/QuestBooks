using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace QuestBooks.Systems
{
    internal class NativeFileDialogLoader : ModSystem
    {
        // We have embedded library files for various platforms for NativeFileDialogSharp.
        // https://github.com/milleniumbug/NativeFileDialogSharp
        //
        // By copying the embedded file to an external file, we can then load the bindings, enabling
        // us to use NativeFileDialog from within tModLoader.
        //
        // We use this for saving/loading user-made quest books.
        public override void Load()
        {
            if (!NativeLibrary.TryLoad("nfd", out _))
            {
                string platform = "win-x64";
                string file = "nfd.dll";

                if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    platform = "linux-x64";
                    file = "libnfd.so";
                }

                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    file = "libnfd.dylib";

                    if (RuntimeInformation.ProcessArchitecture == Architecture.Arm64)
                        platform = "osx-arm64";

                    else
                        platform = "osx-x64";
                }

                else if (RuntimeInformation.ProcessArchitecture == Architecture.X86)
                    platform = "win-x86";

                string embeddedFile = $"QuestBooks/lib/NativeFileDialog/{platform}/{file}";
                string outputDirectory = string.Join(Path.DirectorySeparatorChar, Main.SavePath, "QuestBooks", "NativeFileDialog", "0.6.0");
                string outputFile = $"{outputDirectory}{Path.DirectorySeparatorChar}{file}";

                Directory.CreateDirectory(outputDirectory);

                if (!File.Exists(outputFile))
                {
                    var bytes = ModContent.GetFileBytes(embeddedFile);
                    File.WriteAllBytes(outputFile, bytes);
                }

                NativeLibrary.Load(outputFile);
            }
        }
    }
}