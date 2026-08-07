using System;
using System.IO;
using System.Reflection;
using Cuemon.Security;

namespace Cuemon.Data.Integrity;
/// <summary>
/// Provides access to factory methods for creating and configuring <see cref="CacheValidator"/> instances.
/// </summary>
public static class CacheValidatorFactory
{
    /// <summary>
    /// Creates and returns an instance of <see cref="CacheValidator"/> from the specified <paramref name="file"/>.
    /// </summary>
    /// <param name="file">The <see cref="FileInfo"/> to convert.</param>
    /// <param name="hashFactory">The function delegate that is invoked to produce the <see cref="HashResult" />. Default is <see cref="HashFactory.CreateFnv128"/>.</param>
    /// <param name="setup">The <see cref="FileChecksumOptions"/> which may be configured.</param>
    /// <returns>A <see cref="CacheValidator"/> that represents the <paramref name="file"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="file"/> cannot be null.
    /// </exception>
    public static CacheValidator CreateValidator(FileInfo file, Func<Hash> hashFactory = null, Action<FileChecksumOptions> setup = null)
    {
        Validator.ThrowIfNull(file);
        var options = Patterns.Configure(setup);
        hashFactory ??= DefaultFactoryProvider(options.BytesToRead);
        return DataIntegrityFactory.CreateIntegrity(file, fio =>
        {
            fio.BytesToRead = options.BytesToRead;
            fio.IntegrityConverter = (fi, checksumBytes) =>
            {
                if (checksumBytes.Length > 0)
                {
                    return new CacheValidator(new EntityInfo(fi.CreationTimeUtc, fi.LastWriteTimeUtc, checksumBytes, EntityDataIntegrityValidation.Strong), hashFactory);
                }
                var fileNameHashCode64 = Generate.HashCode64(file.FullName);
                return new CacheValidator(new EntityInfo(fi.CreationTimeUtc, fi.LastWriteTimeUtc, Convertible.GetBytes(fileNameHashCode64)), hashFactory, options.Method);
            };
        }) as CacheValidator;
    }

    /// <summary>
    /// Creates and returns an instance of <see cref="CacheValidator"/> from the specified <paramref name="assembly"/>.
    /// </summary>
    /// <param name="assembly">The <see cref="Assembly"/> to convert.</param>
    /// <param name="hashFactory">The function delegate that is invoked to produce the <see cref="HashResult" />. Default is <see cref="HashFactory.CreateFnv128"/>.</param>
    /// <param name="setup">The <see cref="FileChecksumOptions"/> which may be configured.</param>
    /// <returns>A <see cref="CacheValidator"/> that represents the <paramref name="assembly"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="assembly"/> cannot be null.
    /// </exception>
    public static CacheValidator CreateValidator(Assembly assembly, Func<Hash> hashFactory = null, Action<FileChecksumOptions> setup = null)
    {
        Validator.ThrowIfNull(assembly);
        var options = Patterns.Configure(setup);
        hashFactory ??= DefaultFactoryProvider(options.BytesToRead);
        var assemblyHashCode64 = Generate.HashCode64(assembly.FullName);
        var assemblyLocation = assembly.Location;
        return assembly.IsDynamic
            ? new CacheValidator(new EntityInfo(DateTime.MinValue, DateTime.MaxValue, Convertible.GetBytes(assemblyHashCode64)), hashFactory, options.Method)
            : CreateValidator(new FileInfo(assemblyLocation), hashFactory, setup);
    }

    private static Func<Hash> DefaultFactoryProvider(int bytesToRead)
    {
        return () => Condition.TernaryIf(bytesToRead < 256, () => HashFactory.CreateFnv64(), HashFactory.CreateCrc64);
    }
}
