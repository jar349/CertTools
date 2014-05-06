CertTools
=========

Utility programs for managing certs in Microsoft Active Directory

### certhex
Converts between Hex strings (as found in AD) and DER-formatted certs

>Usage: certhex.exe &lt;command&gt; &lt;sourceFile&gt; &lt;destFile&gt;
>
>  Commands:
>
>    -h2c    Hex String to Certificate
>
>    -c2h    Certificate to Hex String
>
>
>  Example:
>
>    certhex.exe -h2c myHex.txt myCert.cer

### adcerts
Lists, Adds, Exports, and Removes certs to and from Active Directory

>Usage: adcerts.exe &lt;command&gt; [options]
>
>  Commands:
>
>    -list     Lists all certs for a user by serial number
>
>    -export   Get certs for a user
>
>    -put      Publish a cert for a user
>
>    -del      Remove a cert from a user
>
>
>  Examples:
>
>    List all certs (by serial number) for a user (specified by DN)
>
>      adcerts.exe -list "CN=John A. Ruiz,CN=Users,DC=example,DC=com"
>
>
>    Export all certs for a user (specified by DN)
>
>      adcerts.exe -export "CN=John A. Ruiz,CN=Users,DC=example,DC=com"
>
>
>    Export a cert (specified by serial number) for a user (specified by DN)
>
>      adcerts.exe -export "CN=John A. Ruiz,CN=Users,DC=example,DC=com" 62905F2500000000004D
>
>
>    Publish a cert for user (specified by DN)
>
>      adcerts.exe -put newCert.cer "CN=John A. Ruiz,CN=Users,DC=example,DC=com"
>
>
>    Remove the cert with the given serial number from a user (specified by DN)
>
>      adcerts.exe -del "CN=John A. Ruiz,CN=Users,DC=example,DC=com" 62905F2500000000004D