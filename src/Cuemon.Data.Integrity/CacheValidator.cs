using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Cuemon.Collections.Generic;
using Cuemon.Security;

namespace Cuemon.Data.Integrity;
/// <summary>
/// Provides a way to represent cacheable data-centric content that can be validated by cache-aware applications.
/// </summary>
public class CacheValidator : ChecksumBuilder, IEntityInfo
{
    private const long NullOrZeroLengthChecksum = 23719;
    private static readonly CacheValidator DefaultCacheValidatorValue = new(new EntityInfo(DateTime.MinValue, DateTime.MinValue), () => Security.HashFactory.CreateFnv128());
    private static CacheValidator _referencePointCacheValidator;
    private static Assembly _assemblyValue;
    private static readonly Lazy<Assembly> LazyAssembly = new(() => Assembly.GetEntryAssembly() ?? typeof(ChecksumBuilder).GetTypeInfo().Assembly);

    /// <summary>
    /// Gets the most significant <see cref="CacheValidator"/> object from the most significant (largest) value of either <see cref="Created"/> or <see cref="Modified"/> in the specified <paramref name="sequence"/>.
    /// </summary>
    /// <param name="sequence">A sequence of  <see cref="CacheValidator"/> objects to parse for the most significant (largest) value of either <see cref="Created"/> or <see cref="Modified"/>.</param>
    /// <returns>The most significant <see cref="CacheValidator"/> object from the specified <paramref name="sequence"/>.</returns>
    public static CacheValidator GetMostSignificant(params CacheValidator[] sequence)
    {
        Validator.ThrowIfNull(sequence);
        var mostSignificant = Default;
        foreach (var candidate in sequence)
        {
            if (candidate.GetMostSignificant().Ticks > mostSignificant.GetMostSignificant().Ticks) { mostSignificant = candidate; }
        }
        return mostSignificant;
    }

    /// <summary>
    /// Gets or sets the <see cref="Assembly"/> that will serve as the ideal candidate for a <see cref="CacheValidator"/> reference point. Default is <see cref="Assembly.GetEntryAssembly"/> with a fallback to <c>Cuemon.Core.dll</c>.
    /// </summary>
    /// <value>The assembly to use as a <see cref="CacheValidator"/> reference point.</value>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="value"/> is null.
    /// </exception>
    public static Assembly AssemblyReference
    {
        get => _assemblyValue ??= LazyAssembly.Value;
        set
        {
            Validator.ThrowIfNull(value);
            _assemblyValue = value;
            _referencePointCacheValidator = null;
        }
    }

    /// <summary>
    /// Gets a <see cref="CacheValidator"/> object that is initialized to a default representation that should be considered invalid for usage beyond this check.
    /// </summary>
    /// <value>A <see cref="CacheValidator"/> object that is initialized to a default representation.</value>
    public static CacheValidator Default => DefaultCacheValidatorValue.Clone();

    /// <summary>
    /// Gets a <see cref="CacheValidator"/> object that represents an <see cref="Assembly"/> reference point.
    /// </summary>
    /// <value>A <see cref="CacheValidator"/> object that represents an <see cref="Assembly"/> reference point.</value>
    public static CacheValidator ReferencePoint
    {
        get
        {
            _referencePointCacheValidator ??= CacheValidatorFactory.CreateValidator(AssemblyReference);
            return _referencePointCacheValidator.Clone();
        }
    }

    private CacheValidator(Func<Hash> hashFactory) : base(hashFactory)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CacheValidator" /> class.
    /// </summary>
    /// <param name="entity">An <see cref="EntityInfo"/> object that representing the meta-data of an entity.</param>
    /// <param name="hashFactory">The function delegate that is invoked to produce the <see cref="HashResult" />.</param>
    /// <param name="method">A <see cref="EntityDataIntegrityMethod"/> enumeration value that indicates how a checksum is manipulated. Default is <see cref="EntityDataIntegrityMethod.Unaltered"/>.</param>
    /// <exception cref="InvalidEnumArgumentException">method</exception>
    public CacheValidator(EntityInfo entity, Func<Hash> hashFactory, EntityDataIntegrityMethod method = EntityDataIntegrityMethod.Unaltered) : base(entity?.Checksum.GetBytes(), hashFactory)
    {
        Validator.ThrowIfNull(entity);

        Created = entity.Created;
        Modified = entity.Modified;
        Validation = entity.Validation;
        Method = method;

        switch (method)
        {
            case EntityDataIntegrityMethod.Unaltered:
                break;
            case EntityDataIntegrityMethod.Timestamp:
                Bytes = new List<byte>(Convertible.GetBytes(Created.Ticks ^ Modified?.Ticks ?? DateTime.MinValue.Ticks));
                break;
            case EntityDataIntegrityMethod.Combined:
                var checksumValue = entity.Checksum.HasValue ? Generate.HashCode64(Bytes.Cast<IConvertible>()) : NullOrZeroLengthChecksum;
                Bytes = new List<byte>(Convertible.GetBytes(Created.Ticks ^ Modified?.Ticks ?? DateTime.MinValue.Ticks ^ checksumValue));
                break;
            default:
                throw new InvalidEnumArgumentException(nameof(method), (int)method, typeof(EntityDataIntegrityMethod));
        }

    }

    /// <summary>
    /// Gets a <see cref="DateTime"/> value from when data this instance represents was first created, expressed as the Coordinated Universal Time (UTC).
    /// </summary>
    /// <value>A <see cref="DateTime"/> value from when data this instance represents was first created, expressed as the Coordinated Universal Time (UTC).</value>
    public DateTime Created { get; private set; }

    /// <summary>
    /// Gets a <see cref="DateTime"/> value from when data this instance represents was last modified, expressed as the Coordinated Universal Time (UTC).
    /// </summary>
    /// <value>A <see cref="DateTime"/> value from when data this instance represents was last modified, expressed as the Coordinated Universal Time (UTC).</value>
    public DateTime? Modified { get; private set; }

    /// <summary>
    /// Gets an enumeration value of <see cref="EntityDataIntegrityMethod"/> indicating the usage method of this instance.
    /// </summary>
    /// <value>One of the enumeration values of <see cref="EntityDataIntegrityMethod"/> that indicates the usage method of this instance.</value>
    public EntityDataIntegrityMethod Method { get; private set; }

    /// <summary>
    /// Gets an enumeration value of <see cref="EntityDataIntegrityValidation"/> indicating the strength of this instance.
    /// </summary>
    /// <value>One of the enumeration values of <see cref="EntityDataIntegrityValidation"/> that specifies the strength of this instance.</value>
    public EntityDataIntegrityValidation Validation { get; private set; }

    /// <summary>
    /// Combines the <paramref name="additionalChecksum" /> to the representation of this instance.
    /// </summary>
    /// <param name="additionalChecksum">A <see cref="byte"/> array containing a checksum of the additional data this instance must represent.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    public override ChecksumBuilder CombineWith(byte[] additionalChecksum)
    {
        var isChecksumNullOrZeroLength = (additionalChecksum == null || additionalChecksum.Length == 0);
        if (isChecksumNullOrZeroLength) { Validation = EntityDataIntegrityValidation.Strong; }
        return base.CombineWith(additionalChecksum);
    }

    /// <summary>
    /// Creates a shallow copy of the current <see cref="CacheValidator"/> object.
    /// </summary>
    /// <returns>A new <see cref="CacheValidator"/> that is a copy of this instance.</returns>
    public virtual CacheValidator Clone()
    {
        return new CacheValidator(HashFactory)
        {
            Method = Method,
            Modified = Modified,
            Created = Created,
            Validation = Validation,
            Bytes = Bytes.ToList(),
            ComputedHash = ComputedHash
        };
    }

    /// <summary>
    /// Gets the most significant (largest) value of either <see cref="Created"/> or <see cref="Modified"/>.
    /// </summary>
    /// <returns>The most significant (largest) value of either <see cref="Created"/> or <see cref="Modified"/>.</returns>
    public DateTime GetMostSignificant()
    {
        return Arguments.ToEnumerableOf(Created, Modified ?? DateTime.MinValue).Max();
    }
}
