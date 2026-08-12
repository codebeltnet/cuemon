using System;
using System.Linq;
using Cuemon.Reflection;

namespace Cuemon.Resilience;
/// <summary>
/// Provides evidence about a faulted operation.
/// </summary>
public class TransientFaultEvidence : IEquatable<TransientFaultEvidence>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TransientFaultEvidence"/> class.
    /// </summary>
    /// <param name="attempts">The number of attempts the <paramref name="descriptor"/> was invoked.</param>
    /// <param name="recoveryWaitTime">The last wait time attempting recovery of <paramref name="descriptor"/>.</param>
    /// <param name="totalRecoveryWaitTime">The total wait time attempting recovery of <paramref name="descriptor"/>.</param>
    /// <param name="latency">The latency experienced with <paramref name="descriptor"/>.</param>
    /// <param name="descriptor">The information about the method being protected from a transient fault.</param>
    public TransientFaultEvidence(int attempts, TimeSpan recoveryWaitTime, TimeSpan totalRecoveryWaitTime, TimeSpan latency, MethodDescriptor descriptor)
    {
        Attempts = attempts;
        RecoveryWaitTime = recoveryWaitTime;
        TotalRecoveryWaitTime = totalRecoveryWaitTime;
        Latency = latency;
        Descriptor = new MethodSignature(Decorator.Enclose(descriptor.Caller).ToFriendlyName(o => o.FullName = true), descriptor.MethodName, descriptor.Parameters?.Select(ps => ps.ParameterName).ToArray(), descriptor.RuntimeArguments?.Select(kvp => kvp.Value).ToArray());
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TransientFaultEvidence"/> class.
    /// </summary>
    /// <param name="attempts">The number of attempts the <paramref name="descriptor"/> was invoked.</param>
    /// <param name="recoveryWaitTime">The last wait time attempting recovery of <paramref name="descriptor"/>.</param>
    /// <param name="totalRecoveryWaitTime">The total wait time attempting recovery of <paramref name="descriptor"/>.</param>
    /// <param name="latency">The latency experienced with <paramref name="descriptor"/>.</param>
    /// <param name="descriptor">The information about the method being protected from a transient fault.</param>
    public TransientFaultEvidence(int attempts, TimeSpan recoveryWaitTime, TimeSpan totalRecoveryWaitTime, TimeSpan latency, MethodSignature descriptor)
    {
        Attempts = attempts;
        RecoveryWaitTime = recoveryWaitTime;
        TotalRecoveryWaitTime = totalRecoveryWaitTime;
        Latency = latency;
        Descriptor = descriptor;
    }

    /// <summary>
    /// Gets the number of attempts the <see cref="Descriptor"/> was invoked.
    /// </summary>
    /// <value>The number of attempts the <see cref="Descriptor"/> was invoked.</value>
    public int Attempts { get; }

    /// <summary>
    /// Gets the last wait time attempting recovery of <see cref="Descriptor"/>.
    /// </summary>
    /// <value>The last wait time attempting recovery of <see cref="Descriptor"/>.</value>
    public TimeSpan RecoveryWaitTime { get; }

    /// <summary>
    /// Gets the total wait time attempting recovery of <see cref="Descriptor"/>.
    /// </summary>
    /// <value>The total wait time attempting recovery of <see cref="Descriptor"/>.</value>
    public TimeSpan TotalRecoveryWaitTime { get; }

    /// <summary>
    /// Gets the latency experienced with <see cref="Descriptor"/>.
    /// </summary>
    /// <value>The latency experienced with <see cref="Descriptor"/>.</value>
    public TimeSpan Latency { get; }

    /// <summary>
    /// Gets the information about the method being protected from a transient fault.
    /// </summary>
    /// <value>The information about the method being protected from a transient fault.</value>
    public MethodSignature Descriptor { get; }

    /// <summary>
    /// Returns a <see cref="string" /> that represents this instance.
    /// </summary>
    /// <returns>A <see cref="string" /> that represents this instance.</returns>
    public override string ToString()
    {
        var descriptor = Descriptor.ToString();
        return FormattableString.Invariant($"{descriptor} was invoked {Attempts} time(s) over a period of {Latency.Add(TotalRecoveryWaitTime)}. Last recovery wait time was {RecoveryWaitTime}, giving a total recovery wait time of {TotalRecoveryWaitTime}. Latency was {Latency}.");
    }

    /// <summary>
    /// Indicates whether the current object is equal to another object of the same type.
    /// </summary>
    /// <param name="other">An object to compare with this object.</param>
    /// <returns>true if the current object is equal to the <paramref name="other">other</paramref> parameter; otherwise, false.</returns>
    public virtual bool Equals(TransientFaultEvidence other)
    {
        if (other is null) { return false; }
        if (ReferenceEquals(this, other)) { return true; }
        return Attempts == other.Attempts &&
               RecoveryWaitTime.Equals(other.RecoveryWaitTime) &&
               TotalRecoveryWaitTime.Equals(other.TotalRecoveryWaitTime) &&
               Latency.Equals(other.Latency) && Descriptor.ToString().Equals(other.Descriptor.ToString(), StringComparison.Ordinal);
    }

    /// <summary>
    /// Determines whether the specified <see cref="object" /> is equal to this instance.
    /// </summary>
    /// <param name="obj">The object to compare with the current object.</param>
    /// <returns><c>true</c> if the specified <see cref="object" /> is equal to this instance; otherwise, <c>false</c>.</returns>
    public override bool Equals(object obj)
    {
        if (obj is null) { return false; }
        if (ReferenceEquals(this, obj)) { return true; }
        if (obj.GetType() != this.GetType()) { return false; }
        return Equals((TransientFaultEvidence)obj);
    }

    /// <summary>
    /// Returns a hash code for this instance.
    /// </summary>
    /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
    public override int GetHashCode()
    {
        // ReSharper disable once NonReadonlyMemberInGetHashCode
        var descriptor = Descriptor.ToString();
        return Generate.HashCode32(RecoveryWaitTime.Ticks, TotalRecoveryWaitTime.Ticks, Latency.Ticks, descriptor);
    }
}
