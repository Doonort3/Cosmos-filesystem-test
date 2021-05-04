using System;
using System.Collections.Generic;
using System.Text;
using Sys = Cosmos.System;
using Cosmos.System.FileSystem;
using Cosmos.System.FileSystem.VFS;
using System.IO;

namespace DISKTEST {
    public class Kernel : Sys.Kernel
    {
        protected override void BeforeRun()
        {
            // init filesystem
            var fs = new CosmosVFS();
            VFSManager.RegisterVFS(fs);
            
        }

        protected override void Run()
        {
            // Main directory
            var CurentDirectory = "0:/";
            var input = Console.ReadLine();
            var directory_list = Sys.FileSystem.VFS.VFSManager.GetDirectoryListing(CurentDirectory);


                // Commands!

            // The help commanmd
            Console.WriteLine("\n\nWrite 'help' to output all commands.");

            if (input == "help" | input == "Help")
            {
                Console.WriteLine("\nAvailable commands:  \n" +
                "   disk info\n" +
                "   ls\n" +
                "   ls -all\n" +
                "   read file\n" +
                "   edit file\n" +
                "   create file\n" +
                "   clear file\n" +
                "   'clear' or 'cls'\n");
            }

            // The disk info command
            if (input == "disk info")
            {
                long available_space = Sys.FileSystem.VFS.VFSManager.GetAvailableFreeSpace("0:/");
                Console.WriteLine("Available Free Space: " + available_space + "\n");
                string fs_type = Sys.FileSystem.VFS.VFSManager.GetFileSystemType("0:/");
                Console.WriteLine("File System Type: " + fs_type);
                Console.WriteLine();
            }

            // The ls command
            if (input == "ls")
            {
                
                foreach (var directoryEntry in directory_list)
                {
                    Console.WriteLine(directoryEntry.mName);
                }
                Console.WriteLine();
            }

            if (input == "clear" | input == "cls")
            {
                Console.Clear();
            }

            // The 'ls -all' command
            if (input == "ls -all")
            {

                foreach (var directoryEntry in directory_list)
                {
                    var file_stream = directoryEntry.GetFileStream();
                    var entry_type = directoryEntry.mEntryType;
                    if (entry_type == Sys.FileSystem.Listing.DirectoryEntryTypeEnum.File)
                    {
                        Console.WriteLine();
                        byte[] content = new byte[file_stream.Length];
                        file_stream.Read(content, 0, (int)file_stream.Length);
                        Console.WriteLine("File name: " + directoryEntry.mName);
                        Console.WriteLine("File size: " + directoryEntry.mSize);
                        Console.WriteLine("Content: ");
                        foreach (char ch in content) 
                        { 
                            Console.Write(ch.ToString()); 
                        }
                        Console.WriteLine();
                    }
                }
            }

            // The 'read file' command
            if (input == "read file")
            {
                Console.WriteLine("Which file to read: ");
                string nameReadFile = Console.ReadLine();
                
                var hello_file = Sys.FileSystem.VFS.VFSManager.GetFile($@"0:\{nameReadFile}");
                var hello_file_stream = hello_file.GetFileStream();

                if (hello_file_stream.CanRead)
                {
                    byte[] text_to_read = new byte[hello_file_stream.Length];
                    hello_file_stream.Read(text_to_read, 0, (int)hello_file_stream.Length);
                    Console.WriteLine(Encoding.Default.GetString(text_to_read));

                    Console.WriteLine();
                }
            }

            // The 'edit file' command
            if (input == "edit file")
            {
                Console.WriteLine("Which file should I edit: ");
                string nameEditFile = Console.ReadLine();

                Console.WriteLine("Entry text: ");
                string textEditFile = Console.ReadLine();

                var hello_file = Sys.FileSystem.VFS.VFSManager.GetFile($@"0:\{nameEditFile}");
                var hello_file_stream = hello_file.GetFileStream();

                if (hello_file_stream.CanWrite)
                {
                    byte[] text_to_write = Encoding.ASCII.GetBytes(textEditFile);
                    hello_file_stream.Write(text_to_write, 0, text_to_write.Length);
                }
            }

            // The 'create file' command
            if (input == "create file")
            {
                Console.WriteLine("File name without an extension: ");
                string nameCreateFile = Console.ReadLine();

                Sys.FileSystem.VFS.VFSManager.CreateFile($@"0:\{nameCreateFile}.txt");

                Console.WriteLine();
            }

            // The 'clear file' command
            if (input == "clear file")
            {
                Console.WriteLine("Which file should I clear: ");
                string nameClearFile = Console.ReadLine();

                var hello_file = Sys.FileSystem.VFS.VFSManager.GetFile($@"0:\{nameClearFile}");
                var hello_file_stream = hello_file.GetFileStream();

                if (hello_file_stream.CanWrite)
                {
                    byte[] text_to_write = Encoding.ASCII.GetBytes("                                                                                                                                                              ");
                    hello_file_stream.Write(text_to_write, 0, text_to_write.Length);
                }
            }

            // The 'delete file' command
            if (input == "delete file")
            {
                Console.WriteLine("Which file should I delete: ");
                string nameDeleteFile = Console.ReadLine();

                File.Delete($@"0:\{nameDeleteFile}");
            }

            // The 'off' command(shut down)
            if (input == "off")
            {
                Cosmos.System.Power.Shutdown();
            }
        }
    }
}