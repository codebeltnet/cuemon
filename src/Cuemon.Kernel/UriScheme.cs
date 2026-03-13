using System;

namespace Cuemon
{
    /// <summary>
    /// Defines the schemes available for a <see cref="Uri"/> class.
    /// </summary>
    public enum UriScheme
    {
        /// <summary>
        /// Specifies an undefined scheme.
        /// </summary>
        Undefined,
        /// <summary>
        /// Specifies that the URI is a pointer to a file.
        /// </summary>
        File,
        /// <summary>
        /// Specifies that the URI is accessed through the File Transfer Protocol (FTP).
        /// </summary>
        Ftp,
        /// <summary>
        /// Specifies that the URI is accessed through the Gopher protocol.
        /// </summary>
        Gopher,
        /// <summary>
        /// Specifies that the URI is accessed through the Hypertext Transfer Protocol (HTTP).
        /// </summary>
        Http,
        /// <summary>
        /// Specifies that the URI is accessed through the Secure Hypertext Transfer Protocol (HTTPS).
        /// </summary>
        Https,
        /// <summary>
        /// Specifies that the URI is an e-mail address and is accessed through the Simple Mail Transport Protocol (SMTP).
        /// </summary>
        Mailto,
        /// <summary>
        /// Specifies that the URI is accessed through the NetPipe scheme of the "Indigo" system.
        /// </summary>
        NetPipe,
        /// <summary>
        /// Specifies that the URI is accessed through the NetTcp scheme of the "Indigo" system.
        /// </summary>
        NetTcp,
        /// <summary>
        /// Specifies that the URI is an Internet news group and is accessed through the Network News Transport Protocol (NNTP).
        /// </summary>
        News,
        /// <summary>
        /// Specifies that the URI is an Internet news group and is accessed through the Network News Transport Protocol (NNTP).
        /// </summary>
        Nntp,
        /// <summary>
        /// Specifies that the URI is accessed through the Secure File Transfer Protocol (SFTP).
        /// </summary>
        Sftp
    }
}
