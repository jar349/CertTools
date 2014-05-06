using System;
using System.DirectoryServices.AccountManagement;
using System.IO;
using System.Security.Cryptography.X509Certificates;

namespace adcerts
{
    class Program
    {
        public static PrincipalContext ctx = new PrincipalContext(ContextType.Domain);


        public static int Main(string[] args)
        {

            if (args.Length < 1)
            {
                Program.PrintUsage();
                return 1;
            }

            string command = args[0];
            int returnCode = 1;

            try
            {
                switch (command)
                {
                    case "-list":
                        returnCode = Program.ListCertificates(args);
                        break;
                    case "-export":
                        returnCode = Program.GetAllCerts(args);
                        break;
                    case "-put":
                        returnCode = Program.PublishCert(args);
                        break;
                    case "-del":
                        returnCode = Program.RemoveCert(args);
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

        private static int ListCertificates(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Must specify the distinguished name of the user");
                return 1;
            }

            string userDN = args[1];
            UserPrincipal user = UserPrincipal.FindByIdentity(ctx, IdentityType.DistinguishedName, userDN);

            if (null == user)
            {
                Console.WriteLine("No user found with DN: " + userDN);
                return 1;
            }

            if (0 == user.Certificates.Count)
            {
                Console.WriteLine("Found user in AD, but the user has no certificates");
                return 0;
            }

            foreach (X509Certificate2 cert in user.Certificates)
            {
                Console.WriteLine();
                Console.WriteLine(cert.SerialNumber);
                Console.WriteLine("  Subject: " + cert.Subject);
                X509Extension certTemplate = Program.GetCertificateTemplateExtension(cert);
                if (null != certTemplate)
                {
                    Console.WriteLine("  Template: " + certTemplate.Format(false));
                }
            }

            return 0;
        }

        private static X509Extension GetCertificateTemplateExtension(X509Certificate2 cert)
        {
            X509Extension certTemplate = null;

            foreach (X509Extension ext in cert.Extensions)
            {
                if (string.Equals(ext.Oid.FriendlyName, "Certificate Template Information"))
                {
                    certTemplate = ext;
                    break;
                }
            }

            return certTemplate;
        }

        private static int RemoveCert(string[] args)
        {
            if (args.Length < 3)
            {
                Console.WriteLine("Must specify a serial number and the distinguished name of the user");
                return 1;
            }

            string userDN = args[1];
            string serialNumber = args[2];

            UserPrincipal user = UserPrincipal.FindByIdentity(ctx, IdentityType.DistinguishedName, userDN);

            if (null == user)
            {
                Console.WriteLine("No user found with DN: " + userDN);
                return 1;
            }

            if (0 == user.Certificates.Count)
            {
                Console.WriteLine("Found user in AD, but the user has no certificates");
                return 0;
            }

            X509Certificate2Collection certs = user.Certificates
                .Find(X509FindType.FindBySerialNumber, serialNumber, false);

            if (0 == certs.Count)
            {
                Console.WriteLine("User does not have a certificate with serial number: " + serialNumber);
                return 0;
            }

            foreach (X509Certificate2 cert in certs)
            {
                user.Certificates.Remove(cert);
                Console.WriteLine("Removing a cert with serial number: " + serialNumber);
            }

            user.Save();

            return 0;
        }

        private static int PublishCert(string[] args)
        {
            if (args.Length < 3)
            {
                Console.WriteLine("Must specify a cert and the distinguished name of the user");
                return 1;
            }

            string certFile = args[1];

            if (false == File.Exists(certFile))
            {
                Console.WriteLine("No such cert file: " + certFile);
                return 1;
            }

            X509Certificate2 cert = new X509Certificate2(File.ReadAllBytes(certFile));

            string userDN = args[2];
            UserPrincipal user = UserPrincipal.FindByIdentity(ctx, IdentityType.DistinguishedName, userDN);

            if (null == user)
            {
                Console.WriteLine("No user found with DN: " + userDN);
                return 1;
            }

            user.Certificates.Add(cert);
            user.Save();

            return 0;
        }

        private static int GetAllCerts(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Must specify the distinguished name of the user");
                return 1;
            }

            string userDN = args[1];
            UserPrincipal user = UserPrincipal.FindByIdentity(ctx, IdentityType.DistinguishedName, userDN);

            if (null == user)
            {
                Console.WriteLine("No user found with DN: " + userDN);
                return 1;
            }

            if (0 == user.Certificates.Count)
            {
                Console.WriteLine("Found user in AD, but the user has no certificates");
                return 0;
            }

            string serialNumber = null;
            if (3 == args.Length)
            {
                serialNumber = args[2];
            }

            foreach (X509Certificate2 cert in user.Certificates)
            {
                // if a serial number is specified and this cert has it, write it
                if ((null != serialNumber) && (string.Equals(serialNumber, cert.SerialNumber)))
                {
                    Program.WriteCertificate(cert);
                }
                else if (null == serialNumber) // if no serial number specified, write them all
                {
                    Program.WriteCertificate(cert);
                }
            }

            return 0;
        }

        public static void WriteCertificate(X509Certificate2 cert)
        {
            byte[] certBytes = cert.GetRawCertData();
            string fileName = string.Format("{0}.cer", cert.SerialNumber);
            File.WriteAllBytes(fileName, certBytes);
            Console.WriteLine("Wrote: " + fileName);
        }

        public static void PrintUsage()
        {
            Console.WriteLine("Usage: adcerts.exe <command> [options]");
            Console.WriteLine();
            Console.WriteLine("  Commands: ");
            Console.WriteLine("    -list     Lists all certs for a user by serial number");
            Console.WriteLine("    -export   Get certs for a user");
            Console.WriteLine("    -put      Publish a cert for a user");
            Console.WriteLine("    -del      Remove a cert from a user");
            Console.WriteLine();
            Console.WriteLine("  Examples: ");
            Console.WriteLine();
            Console.WriteLine("    List all certs (by serial number) for a user (specified by DN)");
            Console.WriteLine("      adcerts.exe -list \"CN=John A. Ruiz,CN=Users,DC=example,DC=com\"");
            Console.WriteLine();
            Console.WriteLine("    Export all certs for a user (specified by DN)");
            Console.WriteLine("      adcerts.exe -export \"CN=John A. Ruiz,CN=Users,DC=example,DC=com\"");
            Console.WriteLine();
            Console.WriteLine("    Export a cert (specified by serial number) for a user (specified by DN)");
            Console.WriteLine("      adcerts.exe -export \"CN=John A. Ruiz,CN=Users,DC=example,DC=com\" 62905F2500000000004D");
            Console.WriteLine();
            Console.WriteLine("    Publish a cert for user (specified by DN)");
            Console.WriteLine("      adcerts.exe -put newCert.cer \"CN=John A. Ruiz,CN=Users,DC=example,DC=com\"");
            Console.WriteLine();
            Console.WriteLine("    Remove the cert with the given serial number from a user (specified by DN)");
            Console.WriteLine("      adcerts.exe -del \"CN=John A. Ruiz,CN=Users,DC=example,DC=com\" 62905F2500000000004D");
        }
    }
}