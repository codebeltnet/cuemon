namespace Cuemon.Reflection;
/// <summary>
/// Represent the signature of a method in a lightweight format.
/// </summary>
public class MethodSignature
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MethodSignature"/> class.
    /// </summary>
    /// <param name="caller">The class on which the method portrayed by <paramref name="methodName"/> resides.</param>
    /// <param name="methodName">The name of the method to portray.</param>
    /// <param name="parameters">The optional parameters of the method portrayed by <paramref name="methodName"/>.</param>
    /// <param name="arguments">The optional runtime arguments passed to the method portrayed by <paramref name="methodName"/>.</param>
    public MethodSignature(string caller, string methodName, string[] parameters, object[] arguments)
    {
        Caller = caller;
        MethodName = methodName;
        Parameters = parameters;
        Arguments = arguments;
    }

    /// <summary>
    /// Gets the caller of the class where the method portrayed by <see cref="MethodName"/> is located.
    /// </summary>
    /// <value>The caller of the class where the method portrayed by <see cref="MethodName"/> is located.</value>
    public string Caller { get; }

    /// <summary>
    /// Gets the name of the portrayed method.
    /// </summary>
    /// <value>The name of the portrayed method.</value>
    public string MethodName { get; }

    /// <summary>
    /// Gets the parameters (if any) of the portrayed method.
    /// </summary>
    /// <value>An array of string values that matches the signature of the portrayed method.</value>
    public string[] Parameters { get; }

    /// <summary>
    /// Gets the runtime arguments (if any) that was passed to the portrayed method.
    /// </summary>
    /// <value>An array of objects that was passed to the portrayed method.</value>
    public object[] Arguments { get; }

    /// <summary>
    /// Returns a <see cref="string" /> that represents this instance.
    /// </summary>
    /// <returns>A <see cref="string" /> that represents this instance.</returns>
    public override string ToString()
    {
        return string.Concat(Caller, ".", MethodName);
    }
}
