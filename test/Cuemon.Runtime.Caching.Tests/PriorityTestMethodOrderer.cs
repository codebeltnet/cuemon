using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Xunit.Sdk;
using Xunit.v3;

namespace Cuemon.Runtime.Caching;

/// <summary>
/// Orders test methods by their <c>Priority</c> trait, with unprioritized methods last.
/// </summary>
public sealed class PriorityTestMethodOrderer : ITestMethodOrderer
{
    /// <summary>
    /// Orders test methods by numeric priority and then by method name.
    /// </summary>
    /// <typeparam name="TTestMethod">The type of test method.</typeparam>
    /// <param name="testMethods">The test methods to order.</param>
    /// <returns>The test methods in execution order.</returns>
    public IReadOnlyCollection<TTestMethod> OrderTestMethods<TTestMethod>(IReadOnlyCollection<TTestMethod> testMethods)
        where TTestMethod : notnull, ITestMethod
    {
        return testMethods
            .OrderBy(testMethod => GetPriority(testMethod))
            .ThenBy(testMethod => testMethod.MethodName, StringComparer.Ordinal)
            .ToArray();
    }

    private static int GetPriority(ITestMethod testMethod)
    {
        if (testMethod.Traits.TryGetValue("Priority", out var values) &&
            int.TryParse(values.FirstOrDefault(), NumberStyles.Integer, CultureInfo.InvariantCulture, out var priority))
        {
            return priority;
        }

        return int.MaxValue;
    }
}
