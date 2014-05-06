using System;
using System.IO;

namespace certhex
{
    class Program
    {
        static int Main(string[] args)
        {
            if (args.Length != 3)
            {
                Program.PrintUsage();
                return 1;
            }

            string command = args[0];
            string sourceFile = args[1];
            string destFile = args[2];
            int returnCode = 1;

            try
            {
                switch (command)
                {
                    case "-h2c":
                        returnCode = Program.HexToCertificate(sourceFile, destFile);
                        break;
                    case "-c2h":
                        returnCode = Program.CertificateToHex(sourceFile, destFile);
                        break;
                    default:
                        Console.WriteLine();
                        Console.WriteLine("  Invalid command: '" + command + "'");
                        Console.WriteLine();
                        Program.PrintUsage();
                        returnCode = 1;
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return 1;
            }

            Console.WriteLine();
            return returnCode;
        }

        private static int CertificateToHex(string sourceFile, string destFile)
        {
            if (false == File.Exists(sourceFile))
            {
                Console.WriteLine("The file " + sourceFile + " does not exist!");
                return 1;
            }
            else
            {
                byte[] certBytes = File.ReadAllBytes(sourceFile);
                string hexString = Hex.BytesToHexString(certBytes);
                File.WriteAllText(destFile, hexString);

                Console.WriteLine("Wrote hex to: " + destFile);
                return 0;
            }
        }

        private static int HexToCertificate(string sourceFile, string destFile)
        {
            if (false == File.Exists(sourceFile))
            {
                Console.WriteLine("The file " + sourceFile + " does not exist!");
                return 1;
            }
            else
            {
                string hexStringWithSpaces = File.ReadAllText(sourceFile);
                string hexString = hexStringWithSpaces.Replace(" ", string.Empty);

                byte[] cerBytes = Hex.HexStringToByteArray(hexString);
                File.WriteAllBytes(destFile, cerBytes);

                Console.WriteLine("Wrote cert to: " + destFile);
                return 0;
            }
        }

        public static void PrintUsage()
        {
            Console.WriteLine("Usage: certhex.exe <command> <sourceFile> <destFile>");
            Console.WriteLine();
            Console.WriteLine("  Commands: ");
            Console.WriteLine("    -h2c    Hex String to Certificate");
            Console.WriteLine("    -c2h    Certificate to Hex String");
            Console.WriteLine();
            Console.WriteLine("  Example: ");
            Console.WriteLine("    certhex.exe -h2c myHex.txt myCert.cer");
        }
    }
}
