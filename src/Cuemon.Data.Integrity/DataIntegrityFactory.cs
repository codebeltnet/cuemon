using System;
using System.IO;

namespace Cuemon.Data.Integrity;
/// <summary>
/// Provides access to factory methods for creating and configuring implementations of the <see cref="IDataIntegrity"/> interface.
/// </summary>
public static class DataIntegrityFactory
{

    /// <summary>
    /// Creates and returns an object implementing the <see cref="IDataIntegrity"/> interface from the specified <paramref name="file"/>.
    /// </summary>
    /// <param name="file">The <see cref="FileInfo"/> to convert.</param>
    /// <param name="setup">The <see cref="FileIntegrityOptions"/> which need to be configured.</param>
    /// <returns>An object implementing the <see cref="IDataIntegrity"/> interface that represents the integrity of <paramref name="file"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="file"/> cannot be null.
    /// </exception>
    public static IDataIntegrity CreateIntegrity(FileInfo file, Action<FileIntegrityOptions> setup)
    {
        Validator.ThrowIfNull(file);
        var options = Patterns.Configure(setup);
        if (options.BytesToRead > 0)
        {
            long buffer = options.BytesToRead;
            if (file.Length < buffer) { buffer = file.Length; }

            var checksumBytes = new byte[buffer];
            using (var openFile = file.OpenRead())
            {
                openFile.Read(checksumBytes, 0, (int)buffer);
            }
            return options.IntegrityConverter(file, checksumBytes);
        }
        return options.IntegrityConverter(file, Array.Empty<byte>());
    }
}
