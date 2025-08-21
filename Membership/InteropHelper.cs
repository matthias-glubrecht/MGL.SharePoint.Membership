using System;
using System.ComponentModel;
using System.Collections.Specialized;
using System.Runtime.InteropServices;
using System.Text;
using System.Security.Principal;
using System.DirectoryServices;

namespace MGL.SharePoint.Membership
{
    public enum DomainControllerAddressType
    {
        InetAddress = InteropHelper.DS_INET_ADDRESS,
        NetBIOSAddress = InteropHelper.DS_NETBIOS_ADDRESS
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    internal struct DOMAIN_CONTROLLER_INFO
    {
        public string DomainControllerName;
        public string DomainControllerAddress;
        public int DomainControllerAddressType;
        public Guid DomainGuid;
        public string DomainName;
        public string DnsForestName;
        public uint Flags;
        public string DcSiteName;
        public string ClientSiteName;
    }

    /// <summary>
    /// This class encapsulates a DOMAIN_CONTROLLER_INFO struct.
    /// </summary>
    public class DomainControllerInfo
    {
        private DOMAIN_CONTROLLER_INFO _dci;
        internal DomainControllerInfo(DOMAIN_CONTROLLER_INFO dci)
        {
            _dci = dci;
        }

        /// <summary>
        /// A string that specifies the computer name of the discovered domain controller.
        /// The returned computer name is prefixed with "\\". The DNS-style name, for example, "\\phoenix.fabrikam.com", 
        /// is returned, if available. If the DNS-style name is not available, the flat-style name (for example, "\\phoenix") 
        /// is returned. This example would apply if the domain is a Windows NT� 4.0 domain or if the domain 
        /// does not support the IP family of protocols.
        /// </summary>
        [
        Description(@"A string that specifies the computer name of the discovered domain controller.
The returned computer name is prefixed with ""\\"". The DNS-style name, for example, ""\\phoenix.fabrikam.com"", 
is returned, if available. If the DNS-style name is not available, the flat-style name (for example, ""\\phoenix"") 
is returned. This example would apply if the domain is a Windows NT� 4.0 domain or if the domain 
does not support the IP family of protocols.")
        ]
        public string DomainControllerName
        {
            get
            {
                return _dci.DomainControllerName;
            }
        }

        /// <summary>
        /// A String that specifies the address of the discovered domain controller. 
        /// The address is prefixed with "\\". This string is one of the types defined 
        /// by the DomainControllerAddressType member.
        /// </summary>
        [
        Description(@"A String that specifies the address of the discovered domain controller. 
The address is prefixed with ""\\"". This string is one of the types defined 
by the DomainControllerAddressType member.")
        ]
        public string DomainControllerAddress
        {
            get
            {
                return _dci.DomainControllerAddress;
            }
        }

        /// <summary>
        /// Indicates the type of string that is contained in the DomainControllerAddress member.
        /// </summary>
        [Description(@"Indicates the type of string that is contained in the DomainControllerAddress member.")]
        public DomainControllerAddressType DomainControllerAddressType
        {
            get
            {
                return (DomainControllerAddressType)_dci.DomainControllerAddressType;
            }
        }

        /// <summary>
        /// The GUID of the domain. This member is zero if the domain controller does not have a Domain GUID; for example, the domain controller is not a Windows� 2000 domain controller
        /// </summary>
        [
        Description(@"The GUID of the domain. This member is zero if the domain controller does not have a Domain GUID; for example, the domain controller is not a Windows� 2000 domain controller")
        ]
        public Guid DomainGuid
        {
            get
            {
                return _dci.DomainGuid;
            }
        }

        /// <summary>
        /// String that specifies the name of the domain. The DNS-style name, for example, "fabrikam.com", is returned if available. Otherwise, the flat-style name, for example, "fabrikam", is returned. This name may be different than the requested domain name if the domain has been renamed. 
        /// </summary>
        [
        Description(@"String that specifies the name of the domain. The DNS-style name, for example, ""fabrikam.com"", is returned if available. Otherwise, the flat-style name, for example, ""fabrikam"", is returned. This name may be different than the requested domain name if the domain has been renamed.")
        ]
        public string DomainName
        {
            get
            {
                return _dci.DomainName;
            }
        }

        /// <summary>
        /// String that specifies the name of the domain at the root of the DS tree. 
        /// The DNS-style name, for example, "fabrikam.com", is returned if available. 
        /// Otherwise, the flat-style name, for example, "fabrikam" is returned.
        /// </summary>
        [
        Description(@"String that specifies the name of the domain at the root of the DS tree. The DNS-style name, for example, ""fabrikam.com"", is returned if available. Otherwise, the flat-style name, for example, ""fabrikam"" is returned.")
        ]
        public string DnsForestName
        {
            get
            {
                return _dci.DnsForestName;
            }
        }

        /// <summary>
        /// Contains a set of flags that describe the domain controller. 
        /// These properties are all accessible via bool members of this class.
        /// </summary>
        [
        Description(@"Contains a set of flags that describe the domain controller. These properties are all accessible via bool members of this class.")
        ]
        public uint Flags
        {
            get
            {
                return _dci.Flags;
            }
        }

        /// <summary>
        /// String that specifies the name of the site where the domain controller is located. 
        /// This member may be null if the domain controller is not in a site; for example, the domain controller is a Windows NT� 4.0 domain controller. 
        /// </summary>
        [
        Description(@"String that specifies the name of the site where the domain controller is located. This member may be null if the domain controller is not in a site; for example, the domain controller is a Windows NT� 4.0 domain controller.")
        ]
        public string DcSiteName
        {
            get
            {
                return _dci.DcSiteName;
            }
        }

        /// <summary>
        /// String that specifies the name of the site that the computer belongs to. The computer is specified in the ComputerName parameter passed to DsGetDcName. This member may be null if the site that contains the computer cannot be found; for example, if the DS administrator has not associated the subnet that the computer is in with a valid site. 
        /// </summary>
        [
        Description(@"String that specifies the name of the site that the computer belongs to. The computer is specified in the ComputerName parameter passed to DsGetDcName. This member may be null if the site that contains the computer cannot be found; for example, if the DS administrator has not associated the subnet that the computer is in with a valid site. ")
        ]
        public string ClientSiteName
        {
            get
            {
                return _dci.ClientSiteName;
            }
        }

        /// <summary>
        /// Query if the DomainControllerName member is in DNS format.
        /// </summary>
        [
        Description(@"Query if the DomainControllerName member is in DNS format.")
        ]
        public bool DomainControllerNameIsDnsFormat
        {
            get
            {
                return (_dci.Flags & InteropHelper.DS_DNS_CONTROLLER_FLAG) != 0;
            }
        }

        /// <summary>
        /// Query if the DomainName member is in DNS format.
        /// </summary>
        [
        Description(@"Query if the DomainName member is in DNS format.")
        ]
        public bool DomainNameIsDnsFormat
        {
            get
            {
                return (_dci.Flags & InteropHelper.DS_DNS_DOMAIN_FLAG) != 0;
            }
        }

        ///
        /// Query if the DnsForestName member is in DNS format.
        /// 
        [
        Description(@"Query if the DnsForestName member is in DNS format.")
        ]
        public bool DnsForestNameIsDnsFormat
        {
            get
            {
                return (_dci.Flags & InteropHelper.DS_DNS_FOREST_FLAG) != 0;
            }
        }

        /// <summary>
        /// The domain controller is a directory service server for the domain.
        /// </summary>
        [
        Description(@"The domain controller is a directory service server for the domain.")
        ]
        public bool DomainControllerIsDsServer
        {
            get
            {
                return (_dci.Flags & InteropHelper.DS_DS_FLAG) != 0;
            }
        }

        /// <summary>
        /// The domain controller is a global catalog server for the forest specified by DnsForestName.
        /// </summary>
        [
        Description(@"The domain controller is a global catalog server for the forest specified by DnsForestName.")
        ]
        public bool DomainControllerIsGcServer
        {
            get
            {
                return (_dci.Flags & InteropHelper.DS_GC_FLAG) != 0;
            }
        }

        /// <summary>
        /// The domain controller is a Kerberos Key Distribution Center for the domain.
        /// </summary>
        [
        Description(@"The domain controller is a Kerberos Key Distribution Center for the domain.")
        ]
        public bool DomainControllerIsKerberosKeyDistributionCenter
        {
            get
            {
                return (_dci.Flags & InteropHelper.DS_KDC_FLAG) != 0;
            }
        }

        /// <summary>
        /// The domain controller is the primary domain controller of the domain.
        /// </summary>
        [
        Description(@"The domain controller is the primary domain controller of the domain.")
        ]
        public bool DomainControllerIsPDC
        {
            get
            {
                return (_dci.Flags & InteropHelper.DS_PDC_FLAG) != 0;
            }
        }

        /// <summary>
        /// The domain controller is running the Windows Time Service for the domain.
        /// </summary>
        [
        Description(@"The domain controller is running the Windows Time Service for the domain.")
        ]
        public bool DomainControllerRunsTimeService
        {
            get
            {
                return (_dci.Flags & InteropHelper.DS_TIMESERV_FLAG) != 0;
            }
        }

        /// <summary>
        /// The domain controller hosts a writable directory service (or SAM).
        /// </summary>
        [
        Description(@"The domain controller hosts a writable directory service (or SAM).")
        ]
        public bool DomainControllerIsWritable
        {
            get
            {
                return (_dci.Flags & InteropHelper.DS_WRITABLE_FLAG) != 0;
            }
        }
    }

    /// <summary>
    /// Wrapper functions for Win32 calls
    /// </summary>
    public class Interop
    {
        internal Interop() { }

        /// <summary>
        /// Get Information about the domain controller of a given domain.
        /// </summary>
        /// <param name="netBiosDomainName"></param>
        /// <returns></returns>
        public static DomainControllerInfo GetDomainControllerInfo(string netBiosDomainName)
        {
            int returnValue = InteropHelper.DsGetDcName(null, netBiosDomainName, null, null, InteropHelper.DS_RETURN_DNS_NAME, out IntPtr pDCI);
            if (returnValue != InteropHelper.NERR_SUCCESS)
            {
                throw InteropHelper.GetLastError(returnValue);
            }

            DOMAIN_CONTROLLER_INFO dci = (DOMAIN_CONTROLLER_INFO)Marshal.PtrToStructure(pDCI, typeof(DOMAIN_CONTROLLER_INFO));
            InteropHelper.NetApiBufferFree(pDCI);

            return new DomainControllerInfo(dci);
        }

        /// <summary>
        /// Returns the fastest answering DC for the specified domain
        /// </summary>
        /// <param name="domain">Name of the desired domain</param>
        /// <returns>Name of the DC</returns>
        public static string GetDC(string domain)
        {
            // get the fastest answering DC
            if (0 != InteropHelper.NetGetDCName(null, domain, out IntPtr bufPtr))
            {
                // error -> unknown domain?
                return null;
            }
            // convert buffer to string
            string dc = Marshal.PtrToStringAuto(bufPtr);

            // free buffer
            InteropHelper.NetApiBufferFree(bufPtr);

            // and finally return the DC's name
            return dc.Replace("\\", "");
        }
    }

    /// <summary>
    /// Helper class for wrapping Win32 functionality (Interop).
    /// </summary>
    internal class InteropHelper
    {
        #region Constants & Enumerations
        private const string Advapi32 = "Advapi32.dll";
        private const string Netapi32 = "netapi32.dll";
        private const string Kernel32 = "kernel32.dll";
        public const int NERR_SUCCESS = 0;
        public const int ERROR_INSUFFICIENT_BUFFER = 122;
        public const int ERROR_NONE_MAPPED = 1332;
        public const int TOKEN_GROUPS = 2;

        private enum Format_Message_Flags : uint
        {
            FORMAT_MESSAGE_ALLOCATE_BUFFER = 0x0100,
            FORMAT_MESSAGE_IGNORE_INSERTS = 0x0200,
            FORMAT_MESSAGE_FROM_STRING = 0x0400,
            FORMAT_MESSAGE_FROM_HMODULE = 0x0800,
            FORMAT_MESSAGE_FROM_SYSTEM = 0x1000,
            FORMAT_MESSAGE_ARGUMENT_ARRAY = 0x2000
        }
        #endregion

        #region Flags to be passed to DsGetDcName()
        internal const uint DS_FORCE_REDISCOVERY = 0x00000001;

        internal const uint DS_DIRECTORY_SERVICE_REQUIRED = 0x00000010;
        internal const uint DS_DIRECTORY_SERVICE_PREFERRED = 0x00000020;
        internal const uint DS_GC_SERVER_REQUIRED = 0x00000040;
        internal const uint DS_PDC_REQUIRED = 0x00000080;
        internal const uint DS_BACKGROUND_ONLY = 0x00000100;
        internal const uint DS_IP_REQUIRED = 0x00000200;
        internal const uint DS_KDC_REQUIRED = 0x00000400;
        internal const uint DS_TIMESERV_REQUIRED = 0x00000800;
        internal const uint DS_WRITABLE_REQUIRED = 0x00001000;
        internal const uint DS_GOOD_TIMESERV_PREFERRED = 0x00002000;
        internal const uint DS_AVOID_SELF = 0x00004000;
        internal const uint DS_ONLY_LDAP_NEEDED = 0x00008000;

        internal const uint DS_IS_FLAT_NAME = 0x00010000;
        internal const uint DS_IS_DNS_NAME = 0x00020000;

        internal const uint DS_RETURN_DNS_NAME = 0x40000000;
        internal const uint DS_RETURN_FLAT_NAME = 0x80000000;
        #endregion

        #region Values for DomainControllerAddressType returned by DsGetDcName()
        internal const int DS_INET_ADDRESS = 1;
        internal const int DS_NETBIOS_ADDRESS = 2;
        #endregion

        #region Values for Flags returned by DsGetDcName()
        internal const uint DS_PDC_FLAG = 0x00000001;    // DC is PDC of Domain
        internal const uint DS_GC_FLAG = 0x00000004;    // DC is a GC of forest
        internal const uint DS_LDAP_FLAG = 0x00000008;    // Server supports an LDAP server
        internal const uint DS_DS_FLAG = 0x00000010;    // DC supports a DS and is a Domain Controller
        internal const uint DS_KDC_FLAG = 0x00000020;    // DC is running KDC service
        internal const uint DS_TIMESERV_FLAG = 0x00000040;    // DC is running time service
        internal const uint DS_CLOSEST_FLAG = 0x00000080;    // DC is in closest site to client
        internal const uint DS_WRITABLE_FLAG = 0x00000100;    // DC has a writable DS
        internal const uint DS_GOOD_TIMESERV_FLAG = 0x00000200;    // DC is running time service (and has clock hardware)
        internal const uint DS_NDNC_FLAG = 0x00000400;    // DomainName is non-domain NC serviced by the LDAP server
        internal const uint DS_PING_FLAGS = 0x0000FFFF;    // Flags returned on ping

        internal const uint DS_DNS_CONTROLLER_FLAG = 0x20000000;    // DomainControllerName is a DNS name
        internal const uint DS_DNS_DOMAIN_FLAG = 0x40000000;    // DomainName is a DNS name
        internal const uint DS_DNS_FOREST_FLAG = 0x80000000;    // DnsForestName is a DNS name
        #endregion

        #region DLLImports (Win32 API)
        [DllImport(Advapi32, CallingConvention = CallingConvention.Winapi, SetLastError = true, CharSet = CharSet.Auto)]
        internal static extern bool LogonUser(string userName,
            string domain,
            string password,
            int logonType,
            int logonProvider,
            ref IntPtr phToken);

        [DllImport(Kernel32, CallingConvention = CallingConvention.Winapi, SetLastError = true, CharSet = CharSet.Auto)]
        internal static extern bool CloseHandle(IntPtr handle);

        [DllImport(Advapi32, CallingConvention = CallingConvention.Winapi, SetLastError = true)]
        internal static extern Int32 GetTokenInformation(IntPtr TokenHandle,
            int TokenInformationClass,
            IntPtr TokenInformation,
            int TokenInformationLength,
            out int ReturnLength);

        [DllImport(Advapi32, CallingConvention = CallingConvention.Winapi, SetLastError = true, CharSet = CharSet.Auto)]
        internal static extern Int32 LookupAccountName(string lpSystemName,
            string lpAccountName,
            IntPtr pSID,
            ref UInt32 cbSid,
            [Out] char[] DomainName,
            ref UInt32 cbDomainName,
            out uint peUse
            );

        [DllImport(Advapi32, CallingConvention = CallingConvention.Winapi, SetLastError = true, CharSet = CharSet.Auto)]
        internal static extern Int32 LookupAccountSid(string lpSystemName,
            IntPtr pSid,
            [Out] char[] Name,
            ref int cchName,
            [Out] char[] ReferencedDomainName,
            ref int cchReferencedDomainName,
            out uint peUse);

        [DllImport(Advapi32, CallingConvention = CallingConvention.Winapi, SetLastError = true, CharSet = CharSet.Auto)]
        internal static extern Int32 ConvertSidToStringSid(IntPtr pSID,
            out IntPtr StringSid);

        [DllImport(Advapi32, CallingConvention = CallingConvention.Winapi, SetLastError = true, CharSet = CharSet.Auto)]
        internal static extern Int32 ConvertStringSidToSid(string StringSid,
            out IntPtr pSID);

        [DllImport(Kernel32, SetLastError = true)]
        internal static extern Int32 FormatMessageA(uint dwFlags,
            ref object lpSource,
            int dwMessageId,
            int dwLanguageId,
            StringBuilder lpBuffer,
            int nSize,
            ref int Arguments);

        [DllImport(Netapi32, CallingConvention = CallingConvention.Winapi, SetLastError = true, CharSet = CharSet.Auto)]
        internal static extern void NetApiBufferFree(IntPtr hMem);

        [DllImport(Kernel32, CallingConvention = CallingConvention.Winapi, SetLastError = true, CharSet = CharSet.Auto)]
        internal static extern void LocalFree(IntPtr hMem);

        [DllImport(Netapi32)]
        internal static extern int NetGetDCName([MarshalAs(UnmanagedType.LPWStr)] string Servername,
            [MarshalAs(UnmanagedType.LPWStr)] string domainName,
            out IntPtr bufptr);

        [DllImport(Netapi32, CallingConvention = CallingConvention.Winapi, SetLastError = true, CharSet = CharSet.Auto)]
        internal static extern int DsGetDcName(string ComputerName,
            string DomainName,
            [In] GuidAsClass DomainGuid,
            string SiteName,
            uint Flags,
            out IntPtr pDCI);
        #endregion

        #region Functions (Static)

        /// <summary>
        /// Returns an Exception containing the message from the last Win32 error code
        /// </summary>
        /// <returns>System error message</returns>
        public static Exception ExceptionFromLastWin32Error()
        {
            return GetLastError(Marshal.GetLastWin32Error());
        }

        /// <summary>
        /// Resolves an error code (int) into the corresponding
        /// system error message.
        /// </summary>
        /// <param name="errorCode">System error code</param>
        /// <returns>System error message</returns>
        public static Exception GetLastError(int errorCode)
        {
            const int MAX_MESSAGE_LENGTH = 512;

            if (errorCode != NERR_SUCCESS)
            {
                StringBuilder message = new StringBuilder(MAX_MESSAGE_LENGTH);
                int result;
                object source = null;
                int args = 0;

                result = FormatMessageA(
                    (int)Format_Message_Flags.FORMAT_MESSAGE_FROM_SYSTEM,
                    ref source,
                    errorCode,
                    0,
                    message,
                    message.Capacity,
                    ref args);

                if (result != 0)
                {
                    return new Exception(message.ToString());
                }
                else
                {
                    return new Exception("Unknown Error: " + errorCode.ToString());
                }
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Resolves a windows account or group into the SID
        /// </summary>
        /// <param name="userName">Windows account in 'Domain\Alias' format</param>
        /// <param name="machineName">DomainController to use for the lookup</param>
        /// <returns>The users or groups SID in SDDL-format</returns>
        public static string LookupSid(string userName, string machineName)
        {
            uint cbSidSize = 0;
            uint cbDName = 0;
            int rc;

            // Call 'LookupAccountName' with a zero buffer to get required buffer size
            rc = LookupAccountName(machineName, userName, IntPtr.Zero, ref cbSidSize, null, ref cbDName, out _);
            if (rc == 0 && Marshal.GetLastWin32Error() != ERROR_INSUFFICIENT_BUFFER)
            {
                // Normally, we should get a 'ERROR_INSUFFICIENT_BUFFER' here...
                throw new Exception("Unable to lookup account '" + userName + "'", GetLastError(Marshal.GetLastWin32Error()));
            }

            char[] dname = new char[cbDName];
            IntPtr pSid = Marshal.AllocHGlobal((int)cbSidSize);

            // Call 'LookupAccountName' the second time with the required buffer
            rc = LookupAccountName(machineName, userName, pSid, ref cbSidSize, dname, ref cbDName, out _);
            if (rc == 0)
            {
                // All lookup errors should already be thrown above!
                // free our allocated buffer
                Marshal.FreeHGlobal(pSid);
                // throw new exception
                throw new Exception("Unable to lookup account '" + userName + "'", GetLastError(Marshal.GetLastWin32Error()));
            }

            // convert binary SID into SDDL format (S-...)
            string stringSID = string.Empty;
            rc = ConvertSidToStringSid(pSid, out IntPtr pStringSID);
            if (rc == 0)
            {
                // free our allocated buffer
                Marshal.FreeHGlobal(pSid);
                // throw new exception
                throw new Exception("Error converting SID into SDDL", GetLastError(Marshal.GetLastWin32Error()));
            }

            try
            {
                stringSID = Marshal.PtrToStringAuto(pStringSID);
            }
            finally
            {
                // free our allocated buffers
                Marshal.FreeHGlobal(pSid);
                LocalFree(pStringSID);
            }

            // return SDDL-SID
            return stringSID;
        }

        /// <summary>
        /// Resolves a SID (SDDL-format) into the windows account. For unknown SIDs
        /// the function will return the original SID ("S-1-...").
        /// </summary>
        /// <param name="SidString">SID in SDDL-format</param>
        /// <param name="machineName">DomainController to user for lookups</param>
        /// <returns>Windows account or group in 'Domain\Alias' format</returns>
        public static string LookupAccount(string sidString, string machineName)
        {
            if (sidString != null)
            {
                int cchDomainLength = 0;
                int cchAccountLength = 0;
                int rc;

                // convert SDDL-SID into binary format
                rc = ConvertStringSidToSid(sidString, out IntPtr psidPtr);
                if (rc == 0)
                {
                    throw new Exception("Unable to convert SDDL SID '" + sidString + "'", GetLastError(Marshal.GetLastWin32Error()));
                }

                // Call 'LookupAccountSid' without string buffers to get string lengths
                _ = LookupAccountSid(
                    machineName,
                    psidPtr,
                    null,
                    ref cchAccountLength,
                    null,
                    ref cchDomainLength,
                    out _);
                // this call must fail, so check the return code
                rc = Marshal.GetLastWin32Error();
                if (rc == ERROR_NONE_MAPPED)
                {
                    //throw new Exception("Unknown SID");
                    return sidString;
                }
                else if (rc != ERROR_INSUFFICIENT_BUFFER)
                {
                    // Normally, we should get a 'ERROR_INSUFFICIENT_BUFFER' here...
                    throw new Exception("Unable to resolve SID", GetLastError(rc));
                }

                // Call 'LookupAccountSid' again to get account and domain
                char[] domain = new char[cchDomainLength];
                char[] account = new char[cchAccountLength];
                rc = LookupAccountSid(
                    machineName,
                    psidPtr,
                    account,
                    ref cchAccountLength,
                    domain,
                    ref cchDomainLength,
                    out _);

                if (rc == 0)
                {
                    // All lookup errors should already be thrown above!
                    // free our allocated buffer
                    LocalFree(psidPtr);
                    // throw new exception
                    throw new Exception("Unable to resolve SID", GetLastError(Marshal.GetLastWin32Error()));
                }


                // free allocated SID buffer
                LocalFree(psidPtr);

                // copy character from array into string
                string domainName = new string(domain, 0, (int)cchDomainLength);
                string accountName = new string(account, 0, (int)cchAccountLength);

                // check return format
                if (cchDomainLength == 0)
                {
                    // wellknown group without domain
                    return accountName;
                }
                else
                {
                    // return "Domain\Alias"
                    return string.Concat(domainName, "\\", accountName);
                }
            }
            else
                return null;
        }


        /// <summary>
        /// Returns the TokenGroups as SDDL-SIDs ("S-1-...")
        /// </summary>
        /// <param name="tokenHandle"></param>
        /// <returns>Array of SDDL-SIDs</returns>
        public static StringCollection GetTokenGroups(IntPtr tokenHandle)
        {
            StringCollection groupSIDs = new System.Collections.Specialized.StringCollection();

            // call 'GetTokenInformation' without buffer to get required size
            int rc = GetTokenInformation(tokenHandle, TOKEN_GROUPS, IntPtr.Zero, 0, out int length);
            if (rc == 0 && Marshal.GetLastWin32Error() != ERROR_INSUFFICIENT_BUFFER)
            {
                // Normally, we should get a 'ERROR_INSUFFICIENT_BUFFER' here...
                throw new Exception("Unable to read groups from token", GetLastError(Marshal.GetLastWin32Error()));
            }

            // Allocate required buffer and call "GetTokenInformation" again
            IntPtr tokenGroups = Marshal.AllocHGlobal(length);
            rc = GetTokenInformation(tokenHandle, TOKEN_GROUPS, tokenGroups, length, out _);
            if (rc == 0)
            {
                // free our allocated buffer
                Marshal.FreeHGlobal(tokenGroups);

                // Error reading the TokenGroups from the token
                throw new Exception("Unable to read groups from token", GetLastError(Marshal.GetLastWin32Error()));
            }

            // Convert all binary SIDs into SDDL-format
            int groupCount = Marshal.ReadInt32(tokenGroups);
            for (int i = 0; i < groupCount; i++)
            {
                // jump over the first i TOKEN_GROUP structures (4+4 bytes) plus the counter at the beginning
                IntPtr sidPtr = Marshal.ReadIntPtr(new IntPtr(tokenGroups.ToInt32() + 4 + (i * (4 + 4))));

                // convert current SID into SDDL-format
                rc = ConvertSidToStringSid(sidPtr, out IntPtr pSIDString);
                if (rc == 0)
                {
                    throw new Exception("Error converting binary SID into SDDL-format", GetLastError(Marshal.GetLastWin32Error()));
                }
                try
                {
                    groupSIDs.Add(Marshal.PtrToStringAuto(pSIDString));
                }
                finally
                {
                    LocalFree(pSIDString);
                }
            }

            // free our allocated buffer
            Marshal.FreeHGlobal(tokenGroups);

            return groupSIDs;
        }

        /// <summary>
        /// Converts a SID into SDDL-format
        /// </summary>
        /// <param name="Sid">Array of bytes containing the SID</param>
        /// <returns>The SID in SDDL-format</returns>
        public static string ConvertSidToSDDL(byte[] Sid)
        {
            string stringSid = null;

            var rc = ConvertSidToStringSid(Marshal.UnsafeAddrOfPinnedArrayElement(Sid, 0), out IntPtr pSIDString);
            if (rc == 0)
            {
                throw new Exception("Error converting binary SID into SDDL-format", GetLastError(Marshal.GetLastWin32Error()));
            }
            try
            {
                stringSid = Marshal.PtrToStringAuto(pSIDString);
            }
            finally
            {
                LocalFree(pSIDString);
            }

            return stringSid;
        }

        /// <summary>
        /// Class wrapper for the value type Guid
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        internal class GuidAsClass
        {
            private Guid _guid;
            private GuidAsClass(Guid guid)
            {
                _guid = guid;
            }

            public Guid Guid
            {
                get
                {
                    return _guid;
                }
            }
        }

        #endregion
    }
}
