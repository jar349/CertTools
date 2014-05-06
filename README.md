CertTools
=========

Utility programs for managing certs in Microsoft Active Directory

### certhex
Converts between Hex strings (as found in AD) and DER-formatted certs

>Usage: certhex.exe &lt;command&gt; &lt;sourceFile&gt; &lt;destFile&gt;
>
>&nbsp;&nbsp;Commands:
>
> - h2c&nbsp;&nbsp;&nbsp;&nbsp;Hex String to Certificate
> - c2h&nbsp;&nbsp;&nbsp;&nbsp;Certificate to Hex String
>
>&nbsp;&nbsp;Example:
>
>&nbsp;&nbsp;&nbsp;&nbsp;certhex.exe -h2c myHex.txt myCert.cer

### adcerts
Lists, Adds, Exports, and Removes certs to and from Active Directory

>Usage: adcerts.exe &lt;command&gt; [options]
>
>&nbsp;&nbsp;Commands:
>
> - list&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Lists all certs for a user by serial number
> - export&nbsp;&nbsp;&nbsp;Get certs for a user
> - put&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Publish a cert for a user
> - del&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Remove a cert from a user
>
>&nbsp;&nbsp;Examples:
>
>&nbsp;&nbsp;&nbsp;&nbsp;List all certs (by serial number) for a user (specified by DN)
>
>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;adcerts.exe -list "CN=John A. Ruiz,CN=Users,DC=example,DC=com"
>
>
>&nbsp;&nbsp;&nbsp;&nbsp;Export all certs for a user (specified by DN)
>
>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;adcerts.exe -export "CN=John A. Ruiz,CN=Users,DC=example,DC=com"
>
>
>&nbsp;&nbsp;&nbsp;&nbsp;Export a cert (specified by serial number) for a user (specified by DN)
>
>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;adcerts.exe -export "CN=John A. Ruiz,CN=Users,DC=example,DC=com" 62905F2500000000004D
>
>
>&nbsp;&nbsp;&nbsp;&nbsp;Publish a cert for user (specified by DN)
>
>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;adcerts.exe -put newCert.cer "CN=John A. Ruiz,CN=Users,DC=example,DC=com"
>
>
>&nbsp;&nbsp;&nbsp;&nbsp;Remove the cert with the given serial number from a user (specified by DN)
>
>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;adcerts.exe -del "CN=John A. Ruiz,CN=Users,DC=example,DC=com" 62905F2500000000004D