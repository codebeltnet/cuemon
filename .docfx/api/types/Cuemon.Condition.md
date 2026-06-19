---
uid: Cuemon.Condition
example:
- *content
---

The following example demonstrates how to use the `Condition` class to perform common validation checks, equality comparisons, conditional branching, and range assertions.

```csharp
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Cuemon;

namespace MyApp.Validation
{
    public class ConditionExamples
    {
        public void DemonstrateConditions()
        {
            // AreEqual / AreNotEqual with default and custom comparers
            if (Condition.AreEqual("hello", "hello"))
            {
                Console.WriteLine("Strings are equal using default ordinal comparison.");

            if (Condition.AreEqual("hello", "HELLO", StringComparer.OrdinalIgnoreCase))
            {
                Console.WriteLine("Strings are equal ignoring case.");

            if (Condition.AreNotEqual("hello", "world"))
            {
                Console.WriteLine("Strings are different.");

            // Reference equality checks
            var same = new object();
            var different = new object();
            Console.WriteLine(Condition.AreSame(same, same));   // True
            Console.WriteLine(Condition.AreSame(same, different)); // False

            // FlipFlop - executes one of two actions based on condition
            var log = new List<string>();
            Condition.FlipFlop(true,
                () => log.Add("condition was true"),
                () => log.Add("condition was false"));

            Condition.FlipFlop(false,
                (int x) => log.Add($"got {x}"),
                (int x) => log.Add($"skipped {x}"),
                42);

            Console.WriteLine(string.Join(", ", log)); // "condition was true, skipped 42"

            // TernaryIf - functional ternary
            var result = Condition.TernaryIf(true,
                () => "first branch",
                () => "second branch");
            Console.WriteLine(result); // "first branch"

            var guided = Condition.TernaryIf(42 > 10,
                (int x) => x * 2,
                (int x) => x / 2,
                42);
            Console.WriteLine(guided); // 84

            // IsTrue / IsFalse as conditional invocations
            Condition.IsTrue(Condition.IsEmailAddress("user@example.com"), () =>
            {
                Console.WriteLine("Valid email address.");
            });

            Condition.IsFalse(string.IsNullOrEmpty("hello"), () =>
            {
                Console.WriteLine("String is not null or empty.");
            });

            // Validation checks
            Console.WriteLine(Condition.IsGuid("550e8400-e29b-41d4-a716-446655440000")); // True
            Console.WriteLine(Condition.IsGuid("not-a-guid")); // False
            Console.WriteLine(Condition.IsUri("https://example.com")); // True
            Console.WriteLine(Condition.IsNumeric("3.14", NumberStyles.Float, CultureInfo.InvariantCulture)); // True
            Console.WriteLine(Condition.IsEmailAddress("test@test.com")); // True
            Console.WriteLine(Condition.IsEven(42)); // True
            Console.WriteLine(Condition.IsOdd(41)); // True
            Console.WriteLine(Condition.IsPrime(17)); // True
            Console.WriteLine(Condition.IsDefault(0)); // True
            Console.WriteLine(Condition.IsNotDefault(42)); // True
            Console.WriteLine(Condition.IsHex("FF00A1")); // True
            Console.WriteLine(Condition.IsBase64("SGVsbG8=")); // True

            // Range checks
            Console.WriteLine(Condition.IsWithinRange(5, 1, 10)); // True
            Console.WriteLine(Condition.IsNotWithinRange(15, 1, 10)); // True
            Console.WriteLine(Condition.IsGreaterThan(100, 50)); // True
            Console.WriteLine(Condition.IsLowerThan(3, 10)); // True

            // Consecutive characters
            Console.WriteLine(Condition.HasConsecutiveCharacters("bookkeeper", 'o')); // True
            Console.WriteLine(Condition.HasConsecutiveCharacters("abc", new[] { 'x', 'y' })); // False

            // Countable sequences
            Console.WriteLine(Condition.IsCountableSequence(new[] { 1, 3, 5, 7 })); // True
            Console.WriteLine(Condition.IsCountableSequence("abc")); // True

            // Async flip-flop
            var asyncLog = new List<string>();
            var task = Condition.FlipFlopAsync(true,
                async () => { asyncLog.Add("async:true"); await Task.CompletedTask; },
                async () => { asyncLog.Add("async:false"); await Task.CompletedTask; });
            task.Wait();
            Console.WriteLine(string.Join(", ", asyncLog)); // "async:true"

}}}}}
}

```
