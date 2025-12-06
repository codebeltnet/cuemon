using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Cuemon.Assets;
using Cuemon.Extensions.Collections.Generic;
#if NET48_OR_GREATER
using Cuemon.Extensions;
#endif
using Codebelt.Extensions.Xunit;
using Cuemon.Extensions;
using Xunit;

namespace Cuemon
{
    public class ValidatorTest : Test
    {
        public ValidatorTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void ThrowIfObjectDisposed_ByTypeShouldThrowObjectDisposedException()
        {
            var sut = Assert.Throws<ObjectDisposedException>(() =>
            {
                Validator.ThrowIfDisposed(true, GetType());
            });

            Assert.Equal("""
                         Cannot access a disposed object.
                         Object name: 'Cuemon.ValidatorTest'.
                         """.ReplaceLineEndings(), sut.Message);

            sut = Assert.Throws<ObjectDisposedException>(() =>
            {
                Validator.ThrowIfDisposed(true, null);
            });

            Assert.Equal("""
                         Cannot access a disposed object.
                         """, sut.Message);

            var dic = new Dictionary<string, object>();

            sut = Assert.Throws<ObjectDisposedException>(() =>
            {
                Validator.ThrowIfDisposed(true, dic.GetType());
            });

            Assert.Equal("""
                         Cannot access a disposed object.
                         Object name: 'System.Collections.Generic.Dictionary<System.String,System.Object>'.
                         """.ReplaceLineEndings(), sut.Message);

            Validator.ThrowIfDisposed(false, GetType());
        }

        [Fact]
        public void ThrowIfObjectDisposed_ByObjectShouldThrowObjectDisposedException()
        {
            var sut = Assert.Throws<ObjectDisposedException>(() =>
            {
                Validator.ThrowIfDisposed(true, this);
            });

            Assert.Equal("""
                         Cannot access a disposed object.
                         Object name: 'Cuemon.ValidatorTest'.
                         """.ReplaceLineEndings(), sut.Message);

            sut = Assert.Throws<ObjectDisposedException>(() =>
            {
                Validator.ThrowIfDisposed(true, null);
            });

            Assert.Equal("""
                         Cannot access a disposed object.
                         """, sut.Message);

            var dic = new Dictionary<string, object>();

            sut = Assert.Throws<ObjectDisposedException>(() =>
            {
                Validator.ThrowIfDisposed(true, dic);
            });

            Assert.Equal("""
                         Cannot access a disposed object.
                         Object name: 'System.Collections.Generic.Dictionary<System.String,System.Object>'.
                         """.ReplaceLineEndings(), sut.Message);

            Validator.ThrowIfDisposed(false, this);
        }

        [Fact]
        public void ThrowIfObjectInDistress_ShouldThrowInvalidOperationException()
        {
            var sut = Assert.Throws<InvalidOperationException>(() =>
            {
                Validator.ThrowIfInvalidState(1 == 1);
            });

            Assert.Equal("Operation is not valid due to the current state of the object. (Expression '1 == 1')", sut.Message);
        }

        [Fact]
        public void ThrowIfNull_Decorator_ShouldInitFakeOptionsToDefault()
        {
            var sut = Decorator.Enclose(new ValidatableOptions());

            Validator.ThrowIfNull(sut, out var options);
            Assert.Equal(sut.Inner, options);
        }

        [Theory]
        [InlineData(null)]
        public void ThrowIfNull_Decorator_ShouldThrowArgumentNullException(Decorator<ValidatableOptions> sut)
        {
            var result = Assert.Throws<ArgumentNullException>(() =>
            {
                Validator.ThrowIfNull(sut, out var options, paramName: "paramName");
                Assert.Null(options);
            });

            Assert.StartsWith("Value cannot be null", result.Message);
            Assert.Contains("paramName", result.Message);
            Assert.DoesNotContain("sut", result.Message);

            sut = Decorator.Enclose<ValidatableOptions>(null, false);

            result = Assert.Throws<ArgumentNullException>(() =>
            {
                Validator.ThrowIfNull(sut, out var options);
                Assert.Null(options);
            });

            Assert.StartsWith("Value cannot be null.", result.Message);
            Assert.Contains("sut", result.Message);
            Assert.DoesNotContain("paramName", result.Message);
        }

        [Fact]
        public void ThrowIfInvalidConfigurator_ShouldThrowArgumentException_WithInnerNotImplementedException()
        {
            ValidatableOptions options = null;
            var result = Assert.Throws<ArgumentException>(() =>
            {
                Action<ValidatableOptions> setup = null;
                Validator.ThrowIfInvalidConfigurator(setup, out options);
            });
            Assert.Equivalent(new ValidatableOptions(), options, true);

            Assert.StartsWith("Delegate must configure the public read-write properties to be in a valid state.", result.Message);
            Assert.Contains("setup", result.Message);
            Assert.IsType<NotImplementedException>(result.InnerException);
        }

        [Fact]
        public void ThrowIfInvalidConfigurator_ShouldNotThrow_SinceNonValidatable()
        {
            Action<EssentialOptions> setup = null;
            Validator.ThrowIfInvalidConfigurator(setup, out var options);
            Assert.Equivalent(new EssentialOptions(), options, true);
        }

        [Fact]
        public void ThrowIfInvalidOptions_ShouldNotThrow_SinceNonValidatable()
        {
            Validator.ThrowIfInvalidOptions(new EssentialOptions());
        }

        [Fact]
        public void ThrowIfInvalidOptions_ShouldThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                Validator.ThrowIfInvalidOptions((ValidatableOptions)null);
            });

            Assert.Throws<ArgumentNullException>(() =>
            {
                Validator.ThrowIfInvalidOptions((EssentialOptions)null);
            });
        }

        [Fact]
        public void ThrowIfInvalidOptions_ShouldPassSincePostConfiguredAndValid()
        {
            var options = new PostConfigurableOptions();
            Validator.ThrowIfInvalidOptions(options);
        }

        [Fact]
        public void ThrowIfInvalidOptions_ShouldFailSincePostConfiguredAndInvalid()
        {
            var options = new FailPostConfigurableOptions();
            Assert.Throws<ArgumentException>(() =>
            {
                Validator.ThrowIfInvalidOptions(options);
            });
        }

        [Theory]
        [MemberData(nameof(GetValidatableOptions))]
        public void ThrowIfInvalidOptions_ShouldThrowArgumentException_WithInnerNotImplementedException(ValidatableOptions paramName)
        {
            var result = Assert.Throws<ArgumentException>(() =>
            {
                Validator.ThrowIfInvalidOptions(paramName);
            });

            Assert.StartsWith($"{nameof(ValidatableOptions)} are not in a valid state.", result.Message);
            Assert.Contains(nameof(paramName), result.Message);
            Assert.IsType<NotImplementedException>(result.InnerException);
        }

        public static IEnumerable<object[]> GetValidatableOptions()
        {
            yield return [new ValidatableOptions()];
        }

        [Theory]
        [InlineData(null)]
        [InlineData("cuemon")]
        public void CheckParameter_ShouldThrowArgumentNullExceptionOrReturnString(string value)
        {
            if (value == null)
            {
                var ex = Assert.Throws<ArgumentNullException>(() =>
                {
                    Validator.CheckParameter(value, () =>
                    {
                        Validator.ThrowIfNull(value);
                    });
                });
                Assert.Equal(ex.ParamName, nameof(value));
            }
            else
            {
                Assert.Equal("cuemon", Validator.CheckParameter(value, () =>
                {
                    Validator.ThrowIfNull(value);
                }));
            }
        }

        [Fact]
        public void ThrowIfContainsType_ShouldThrowArgumentOutOfRangeException()
        {
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                Validator.ThrowIfContainsType(typeof(ArgumentNullException), typeof(ArgumentException).Yield().ToArray());
            });

            Assert.Equal(ex.ParamName, "typeof(ArgumentNullException)");

            ex = Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                Validator.ThrowIfContainsType(new ArgumentNullException(), typeof(ArgumentException).Yield().ToArray());
            });

            Assert.Equal(ex.ParamName, "new ArgumentNullException()");
        }

        [Fact]
        public void ThrowIfContainsType_ShouldThrowTypeArgumentOutOfRangeException()
        {
            Assert.Throws<TypeArgumentOutOfRangeException>(() =>
            {
                Validator.ThrowIfContainsType<ArgumentNullException>("typeParamName", typeof(ArgumentException));
            });
        }

        [Fact]
        public void ThrowIfNotContainsType_ShouldThrowArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                Validator.ThrowIfNotContainsType(typeof(ArgumentNullException), typeof(OutOfMemoryException).Yield().ToArray());
            });

            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                Validator.ThrowIfNotContainsType(new ArgumentNullException(), typeof(OutOfMemoryException).Yield().ToArray());
            });
        }

        [Fact]
        public void ThrowIfNotContainsInterface_ShouldThrowArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                Validator.ThrowIfNotContainsInterface(typeof(Stream), typeof(IConvertible).Yield().ToArray());
            });
        }

        [Fact]
        public void ThrowIfNotContainsInterface_ShouldThrowTypeArgumentOutOfRangeException()
        {
            Assert.Throws<TypeArgumentOutOfRangeException>(() =>
            {
                Validator.ThrowIfNotContainsInterface<Stream>("paramName", typeof(IConvertible));
            });
        }

        [Fact]
        public void ThrowIfContainsInterface_ShouldThrowArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                Validator.ThrowIfContainsInterface(typeof(string), typeof(IConvertible).Yield().ToArray());
            });
        }

        [Theory]
        [InlineData(true)]
        public void ThrowIfContainsInterface_ShouldThrowTypeArgumentOutOfRangeException<TBool>(TBool value) where TBool : struct, IConvertible
        {
            var ex = Assert.Throws<TypeArgumentOutOfRangeException>(() =>
            {
                Validator.ThrowIfContainsInterface<TBool>(nameof(TBool), typeof(IConvertible));
            });
            Assert.Equal(nameof(TBool), ex.ParamName);
        }

        [Fact]
        public void ThrowIfNotContainsType_ShouldThrowTypeArgumentOutOfRangeException()
        {
            var ex = Assert.Throws<TypeArgumentOutOfRangeException>(() =>
            {
                Validator.ThrowIfNotContainsType<ArgumentNullException>("typeParamName", typeof(OutOfMemoryException));
            });
            Assert.Equal("typeParamName", ex.ParamName);
        }

        [Theory]
        [InlineData("michael@wbpa.dk")]
        public void ThrowIfEmailAddress_ShouldThrowArgumentException(string emailAddress)
        {
            var ex = Assert.Throws<ArgumentException>(() =>
            {
                Validator.ThrowIfEmailAddress(emailAddress);
            });
            Assert.Equal(nameof(emailAddress), ex.ParamName);
        }

        [Fact]
        public void ThrowIfNotEmailAddress_ShouldThrowArgumentException()
        {
            var ex = Assert.Throws<ArgumentException>(() =>
            {
                Validator.ThrowIfNotEmailAddress("michael", paramName: "paramName");
            });
            Assert.Equal("paramName", ex.ParamName);
        }

        [Theory]
        [InlineData("")]
        public void ThrowIfEmpty_ShouldThrowArgumentException(string paramName)
        {
            var ex = Assert.Throws<ArgumentException>(() =>
            {
                Validator.ThrowIfEmpty(paramName);
            });
            Assert.Equal(nameof(paramName), ex.ParamName);
        }

        [Theory]
        [MemberData(nameof(GetEmptySequence))]
        public void ThrowIfSequenceEmpty_ShouldThrowArgumentException(IEnumerable<string> sequence)
        {
            var ex = Assert.Throws<ArgumentException>(() =>
            {
                Validator.ThrowIfSequenceEmpty(sequence);
            });
            Assert.Equal(nameof(sequence), ex.ParamName);
        }

        public static IEnumerable<object[]> GetEmptySequence()
        {
            yield return [Enumerable.Empty<string>()];
        }

        [Theory]
        [InlineData(null)]
        public void ThrowIfSequenceNullOrEmpty_ShouldThrowArgumentNullException(IEnumerable<string> nullSequence)
        {
            var ex = Assert.Throws<ArgumentNullException>(() =>
            {
                Validator.ThrowIfSequenceNullOrEmpty(nullSequence);
            });
            Assert.Equal(nameof(nullSequence), ex.ParamName);
        }

        [Fact]
        public void ThrowIfSequenceNullOrEmpty_ShouldThrowArgumentException()
        {
            var ex = Assert.Throws<ArgumentException>(() =>
            {
                var list = new List<string>();
                Validator.ThrowIfSequenceNullOrEmpty(list, paramName: "paramName");
            });
            Assert.Equal("paramName", ex.ParamName);
        }

        [Theory]
        [InlineData("Up")]
        public void ThrowIfEnum_ShouldThrowArgumentException(string enumAsString)
        {
            var ex = Assert.Throws<ArgumentException>(() =>
            {
                Validator.ThrowIfEnum<VerticalDirection>(enumAsString);
            });
            Assert.Equal(nameof(enumAsString), ex.ParamName);
        }

        [Fact]
        public void ThrowIfEnumType_ShouldThrowArgumentException()
        {
            var ex = Assert.Throws<ArgumentException>(() =>
            {
                Validator.ThrowIfEnumType(typeof(VerticalDirection));
            });
            Assert.Equal("typeof(VerticalDirection)", ex.ParamName);
        }

        [Fact]
        public void ThrowIfNotEnumType_ShouldThrowArgumentException()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                Validator.ThrowIfNotEnumType(typeof(Stream));
            });
        }

        [Theory]
        [InlineData("Ups")]
        public void ThrowIfNotEnum_ShouldThrowArgumentException(string paramName)
        {
            var ex = Assert.Throws<ArgumentException>(() =>
            {
                Validator.ThrowIfNotEnum<VerticalDirection>(paramName);
            });
            Assert.Equal(nameof(paramName), ex.ParamName);
        }

        [Fact]
        public void ThrowIfEnumType_ShouldThrowTypeArgumentException()
        {
            Assert.Throws<TypeArgumentException>(() =>
            {
                Validator.ThrowIfEnumType<VerticalDirection>("typeParamName");
            });
        }

        [Fact]
        public void ThrowIfNotEnumType_ShouldThrowTypeArgumentException()
        {
            Assert.Throws<TypeArgumentException>(() =>
            {
                Validator.ThrowIfNotEnumType<DateTime>("typeParamName");
            });
        }

        [Fact]
        public void ThrowIfEqual_ShouldThrowArgumentOutOfRangeException()
        {
            var sut = Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                Validator.ThrowIfEqual(1, 1, "paramName");
            });

            Assert.StartsWith("Specified arguments x and y are equal to one another.", sut.Message);
            Assert.Contains("paramName", sut.Message);
            Assert.EndsWith("Actual value was 1 == 1.", sut.Message);
        }

        [Fact]
        public void ThrowIfEqual_ShouldThrowArgumentOutOfRangeException_Message()
        {
            var sut = Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                Validator.ThrowIfEqual(1, 1, "marapName", message: "customMessage");
            });

            Assert.StartsWith("customMessage", sut.Message);
            Assert.Contains("marapName", sut.Message);
            Assert.EndsWith("Actual value was 1 == 1.", sut.Message);
        }

        [Fact]
        public void ThrowIfNotEqual_ShouldThrowArgumentOutOfRangeException()
        {
            var sut = Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                Validator.ThrowIfNotEqual(1, 2, "paramName");
            });

            Assert.StartsWith("Specified arguments x and y are not equal to one another.", sut.Message);
            Assert.Contains("paramName", sut.Message);
            Assert.EndsWith("Actual value was 1 != 2.", sut.Message);
        }

        [Fact]
        public void ThrowIfNotEqual_ShouldThrowArgumentOutOfRangeException_Message()
        {
            var sut = Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                Validator.ThrowIfNotEqual(1, 2, "marapName", message: "customMessage");
            });

            Assert.StartsWith("customMessage", sut.Message);
            Assert.Contains("marapName", sut.Message);
            Assert.EndsWith("Actual value was 1 != 2.", sut.Message);
        }

        [Fact]
        public void ThrowIfFalse_ShouldThrowArgumentException()
        {
            var sut = Assert.Throws<ArgumentException>(() =>
            {
                Validator.ThrowIfFalse(false, "paramName");
            });
#if NET48_OR_GREATER
            Assert.Equal("Value is not in a valid state. (Expression 'false')\r\nParameter name: paramName".ReplaceLineEndings(), sut.Message);
#else
            Assert.Equal("Value is not in a valid state. (Expression 'false') (Parameter 'paramName')", sut.Message);
#endif
        }

        [Fact]
        public void ThrowIfFalse_PredicateShouldThrowArgumentException()
        {
            var sut = Assert.Throws<ArgumentException>(() =>
            {
                Validator.ThrowIfFalse(() => false, "paramName", "Value is false ;-)");
            });
#if NET48_OR_GREATER
            Assert.Equal("Value is false ;-) (Expression '() => false')\r\nParameter name: paramName".ReplaceLineEndings(), sut.Message);
#else
            Assert.Equal("Value is false ;-) (Expression '() => false') (Parameter 'paramName')", sut.Message);
#endif
        }

        [Fact]
        public void ThrowIfTrue_ShouldThrowArgumentException()
        {
            var sut = Assert.Throws<ArgumentException>(() =>
            {
                Validator.ThrowIfTrue(true, "paramName");
            });
            TestOutput.WriteLine(sut.ToString());
#if NET48_OR_GREATER
            Assert.Equal("Value is not in a valid state. (Expression 'true')\r\nParameter name: paramName".ReplaceLineEndings(), sut.Message);
#else
            Assert.Equal("Value is not in a valid state. (Expression 'true') (Parameter 'paramName')", sut.Message);
#endif
        }

        [Fact]
        public void ThrowIfTrue_PredicateShouldThrowArgumentException()
        {
            var sut = Assert.Throws<ArgumentException>(() =>
            {
                Validator.ThrowIfTrue(() => true, "paramName", "Value is true ;-)");
            });
#if NET48_OR_GREATER
            Assert.Equal("Value is true ;-) (Expression '() => true')\r\nParameter name: paramName".ReplaceLineEndings(), sut.Message);
#else
            Assert.Equal("Value is true ;-) (Expression '() => true') (Parameter 'paramName')", sut.Message);
#endif

        }

        [Fact]
        public void ThrowIfGreaterThan_ShouldThrowArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                Validator.ThrowIfGreaterThan(2, 1, "paramName");
            });
        }

        [Fact]
        public void ThrowIfGreaterThanOrEqual_ShouldThrowArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                Validator.ThrowIfGreaterThanOrEqual(2, 2, "paramName");
            });
        }

        [Theory]
        [InlineData("eccdb302-f94b-4df8-8ef4-f46794e1dcbd")]
        public void ThrowIfGuid_ShouldThrowArgumentException(string guidString)
        {
            var ex = Assert.Throws<ArgumentException>(() =>
            {
                Validator.ThrowIfGuid(guidString);
            });
            Assert.Equal(nameof(guidString), ex.ParamName);
        }

        [Theory]
        [InlineData("not-a-guid")]
        public void ThrowIfNotGuid_ShouldThrowArgumentException(string value)
        {
            var ex = Assert.Throws<ArgumentException>(() =>
            {
                Validator.ThrowIfNotGuid(value);
            });
            Assert.Equal(nameof(value), ex.ParamName);
        }

        [Fact]
        public void ThrowIfHex_ShouldThrowArgumentException()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                Validator.ThrowIfHex("AAB0F3C1");
            });
        }

        [Fact]
        public void ThrowIfNotHex_ShouldThrowArgumentException()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                Validator.ThrowIfNotHex("olkiujhy");
            });
        }

        [Fact]
        public void ThrowIfLowerThan_ShouldThrowArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                Validator.ThrowIfLowerThan(1, 2, "paramName");
            });
        }

        [Fact]
        public void ThrowIfLowerThanOrEqual_ShouldThrowArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                Validator.ThrowIfLowerThanOrEqual(2, 2, "paramName");
            });
        }

        [Theory]
        [InlineData("22")]
        public void ThrowIfNumber_ShouldThrowArgumentException(string number)
        {
            var ex = Assert.Throws<ArgumentException>(() =>
            {
                Validator.ThrowIfNumber(number);
            });
            Assert.Equal(nameof(number), ex.ParamName);
        }

        [Fact]
        public void ThrowIfNotNumber_ShouldThrowArgumentException()
        {
            var sut = Assert.Throws<ArgumentException>(() =>
            {
                Validator.ThrowIfNotNumber("22cads", paramName: "paramName");
            });

            Assert.StartsWith("Value must be a number.", sut.Message);
            Assert.Contains("paramName", sut.Message);
        }

        [Fact]
        public void ThrowIfNotNumber_ShouldThrowArgumentException_Message()
        {
            var sut = Assert.Throws<ArgumentException>(() =>
            {
                Validator.ThrowIfNotNumber("22cads", message: "customMessage", paramName: "marapName");
            });

            Assert.StartsWith("customMessage", sut.Message);
            Assert.Contains("marapName", sut.Message);
        }

        [Theory]
        [InlineData(null)]
        public void ThrowIfNull_ShouldThrowArgumentNullException(string value)
        {
            var sut = Assert.Throws<ArgumentNullException>(() =>
            {
                Validator.ThrowIfNull(value);
            });

            Assert.StartsWith("Value cannot be null.", sut.Message);
            Assert.Contains("value", sut.Message);

            TestOutput.WriteLine(sut.ToString());

            sut = Assert.Throws<ArgumentNullException>(() =>
            {
                Validator.ThrowIfNull(value, paramName: "paramName");
            });

            Assert.StartsWith("Value cannot be null.", sut.Message);
            Assert.Contains("paramName", sut.Message);
        }

        [Theory]
        [InlineData(null)]
        public void ThrowIfNullOrEmpty_ShouldThrowArgumentNullException(string value)
        {
            var sut = Assert.Throws<ArgumentNullException>(() =>
            {
                Validator.ThrowIfNullOrEmpty(value);
            });

            Assert.StartsWith("Value cannot be null.", sut.Message);
            Assert.Contains("value", sut.Message);

            sut = Assert.Throws<ArgumentNullException>(() =>
            {
                Validator.ThrowIfNullOrEmpty(value);
            });

            Assert.StartsWith("Value cannot be null.", sut.Message);
            Assert.Contains("value", sut.Message);

            sut = Assert.Throws<ArgumentNullException>(() =>
            {
                Validator.ThrowIfNullOrEmpty(value, "message");
            });

            Assert.StartsWith("message", sut.Message);
            Assert.Contains("value", sut.Message);
        }

        [Theory]
        [InlineData("")]
        public void ThrowIfNullOrEmpty_ShouldThrowArgumentException(string value)
        {
            var sut = Assert.Throws<ArgumentException>(() =>
            {
                Validator.ThrowIfNullOrEmpty(value);
            });

            Assert.StartsWith("Value cannot be empty.", sut.Message);
            Assert.Contains("value", sut.Message);

            sut = Assert.Throws<ArgumentException>(() =>
            {
                Validator.ThrowIfNullOrEmpty("", paramName: nameof(value));
            });


            Assert.StartsWith("Value cannot be empty.", sut.Message);
            Assert.Contains("value", sut.Message);

            sut = Assert.Throws<ArgumentException>(() =>
            {
                Validator.ThrowIfNullOrEmpty(value, "message");
            });

            Assert.StartsWith("message", sut.Message);
            Assert.Contains("value", sut.Message);
        }

        [Theory]
        [InlineData(null)]
        public void ThrowIfNullOrWhitespace_ShouldThrowArgumentNullException(string value)
        {
            var sut = Assert.Throws<ArgumentNullException>(() =>
            {
                Validator.ThrowIfNullOrWhitespace(value);
            });

            Assert.StartsWith("Value cannot be null.", sut.Message);
            Assert.Contains("value", sut.Message);

            var sut2 = Assert.Throws<ArgumentException>(() =>
            {
                Validator.ThrowIfNullOrWhitespace(" ", paramName: nameof(value));
            });

            Assert.StartsWith("Value cannot consist only of white-space characters", sut2.Message);
            Assert.Contains("value", sut2.Message);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public void ThrowIfNullOrWhitespace_ShouldThrowArgumentException(string value)
        {
            var sut = Assert.Throws<ArgumentException>(() =>
            {
                Validator.ThrowIfNullOrWhitespace(value);
            });

            Assert.StartsWith(Condition.TernaryIf(value.Length == 0, () => "Value cannot be empty.", () => "Value cannot consist only of white-space characters."), sut.Message);
            Assert.Contains("value", sut.Message);

            sut = Assert.Throws<ArgumentException>(() =>
            {
                Validator.ThrowIfNullOrWhitespace(value);
            });

            Assert.StartsWith(Condition.TernaryIf(value.Length == 0, () => "Value cannot be empty.", () => "Value cannot consist only of white-space characters."), sut.Message);
            Assert.Contains("value", sut.Message);

            sut = Assert.Throws<ArgumentException>(() =>
            {
                Validator.ThrowIfNullOrWhitespace(value, "message");
            });

            Assert.StartsWith("message", sut.Message);
            Assert.Contains("value", sut.Message);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void ThrowIfNullOrWhitespace_ShouldThrowArgumentException_WithCustomMessage(string value)
        {
            var expected = "Value cannot be null, empty or consist only of white-space characters.";

            if (value == null)
            {
                var sut = Assert.Throws<ArgumentNullException>(() =>
                {
                    Validator.ThrowIfNullOrWhitespace(value, message: expected);
                });
                Assert.StartsWith(expected, sut.Message);
            }
            else
            {
                var sut = Assert.Throws<ArgumentException>(() =>
                {
                    Validator.ThrowIfNullOrWhitespace(value, message: expected);
                });
                Assert.StartsWith(expected, sut.Message);
            }
        }

        [Fact]
        public void ThrowIfSame_ShouldThrowArgumentOutOfRangeException()
        {
            var t1 = TimeZoneInfo.Utc;
            var t2 = t1;
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                Validator.ThrowIfSame(t1, t2, "paramName");
            });
        }

        [Fact]
        public void ThrowIfNotSame_ShouldThrowArgumentOutOfRangeException()
        {
            var t1 = TimeZoneInfo.Utc;
            var t2 = TimeZoneInfo.Local;
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                Validator.ThrowIfNotSame(t1, t2, "paramName");
            });
        }

        [Fact]
        public void ThrowIfUri_ShouldThrowArgumentException()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                Validator.ThrowIfUri("https://www.cuemon.net/");
            });

            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                Validator.ThrowIfUri("https://www.cuemon.net/", UriKind.RelativeOrAbsolute);
            });

            Assert.Throws<ArgumentException>(() =>
            {
                Validator.ThrowIfUri("/blog", UriKind.Relative);
            });

            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                Validator.ThrowIfUri("/blog", UriKind.RelativeOrAbsolute);
            });
        }

        [Theory]
        [InlineData("www.cuemon.net")]
        public void ThrowIfNotUri_ShouldThrowArgumentException(string cuemonUrl)
        {
            var ex = Assert.Throws<ArgumentException>(() =>
            {
                Validator.ThrowIfNotUri(cuemonUrl);
            });

            Assert.Equal(nameof(cuemonUrl), ex.ParamName);

            var ae = Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                Validator.ThrowIfNotUri("www.cuemon.net", UriKind.RelativeOrAbsolute, paramName: "paramName");
            });

            TestOutput.WriteLine(ae.ToString());

            Assert.Equal("paramName", ae.ParamName);

            Assert.Throws<ArgumentException>(() =>
            {
                Validator.ThrowIfNotUri("blog:that", UriKind.Relative);
            });

            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                Validator.ThrowIfNotUri("blog:that", UriKind.RelativeOrAbsolute);
            });
        }

        [Fact]
        public void ThrowIfWhiteSpace_ShouldThrowArgumentException()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                Validator.ThrowIfWhiteSpace(" ");
            });
        }

        [Fact]
        public void ThrowIfNotBinaryDigits_ShouldThrowArgumentOutOfRangeException()
        {
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                Validator.ThrowIfNotBinaryDigits("12345678");
            });
            Assert.StartsWith("Value must consist only of binary digits", ex.Message);
        }

        [Fact]
        public void ThrowIfNotBase64String_ShouldThrowArgumentOutOfRangeException()
        {
            var sut = Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                Validator.ThrowIfNotBase64String("DJ BOBO", paramName: "paramName");
            });

            Assert.Equal("paramName", sut.ParamName);
            Assert.Equal("DJ BOBO", sut.ActualValue);
            Assert.StartsWith("Value must consist only of base-64 digits.", sut.Message);

            sut = Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                Validator.ThrowIfNotBase64String(sut.ParamName);
            });

            Assert.Equal("sut.ParamName", sut.ParamName);
            Assert.Equal("paramName", sut.ActualValue);
            Assert.StartsWith("Value must consist only of base-64 digits.", sut.Message);
        }

        [Fact]
        public void ThrowIfContainsReservedKeyword_ShouldThrowReservedKeywordException()
        {
            var sut = Assert.Throws<ArgumentReservedKeywordException>(() =>
            {
                var resKw = "dj bobo";
                var resKwList = new string[]
                {
                    "rene",
                    "baumann",
                    "dj bobo"
                };
                Validator.ThrowIfContainsReservedKeyword(resKw, resKwList);
            });

            Assert.Equal("resKw", sut.ParamName);
            Assert.Equal("dj bobo", sut.ActualValue);
            Assert.StartsWith("Specified argument is a reserved keyword.", sut.Message);
        }

        [Fact]
        public void ThrowIfContainsReservedKeyword_WithEqualityComparer_ShouldThrowReservedKeywordException()
        {
            var resKw = "DJ BOBO";
            var resKwList = new string[]
            {
                "rene",
                "baumann",
                "dj bobo"
            };

            Validator.ThrowIfContainsReservedKeyword(resKw, resKwList); // should not throw as we are using EqualityComparer<string>.Default

            var sut = Assert.Throws<ArgumentReservedKeywordException>(() =>
            {
                Validator.ThrowIfContainsReservedKeyword(resKw, resKwList, StringComparer.OrdinalIgnoreCase);
            });

            Assert.Equal("resKw", sut.ParamName);
            Assert.Equal("DJ BOBO", sut.ActualValue);
            Assert.StartsWith("Specified argument is a reserved keyword.", sut.Message);
        }

        [Fact]
        public void ThrowIfNotDifferent_ShouldThrowArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                Validator.ThrowIfNotDifferent("aaabbbccc", "cccbbbbaaaa", "paramName");
            });
        }

        [Fact]
        public void ThrowIfDifferent_ShouldThrowArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                Validator.ThrowIfDifferent("aaabbbccc", "dddeeefff", "paramName");
            });
        }

        [Fact]
        public void ThrowIfContainsAny_ShouldThrowArgumentOutOfRangeException()
        {
            var sut = Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                var argument = "dj bobo\tis the best 90ies artist!";
                var characters = new[] { ' ', Alphanumeric.TabChar };
                Validator.ThrowIfContainsAny(argument, characters);
            });

            Assert.Equal("argument", sut.ParamName);
            Assert.Equal("' ','\t'", sut.ActualValue);
            Assert.StartsWith("One or more character matches were found.", sut.Message);
        }

        [Fact]
        public void ThrowIfNotContainsAny_ShouldThrowArgumentOutOfRangeException()
        {
            var sut = Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                var argument = "dj bobo\tis the best 90ies artist!";
                var characters = new[] { Alphanumeric.LinefeedChar, Alphanumeric.CaretChar };
                Validator.ThrowIfNotContainsAny(argument, characters);
            });

            Assert.Equal("argument", sut.ParamName);
            Assert.Equal("'\n','^'", sut.ActualValue);
            Assert.StartsWith("No matching characters were found.", sut.Message);
        }

        [Fact]
        public void ThrowIfContainsAny_ShouldNotThrowAnyException()
        {
            var argument = "dj bobo\tis the best 90ies artist!";
            var characters = new[] { Alphanumeric.LinefeedChar, Alphanumeric.CaretChar };
            Validator.ThrowIfContainsAny(argument, characters);
        }

        [Fact]
        public void ThrowIfNotContainsAny_ShouldNotThrowAnyException()
        {
            var argument = "dj bobo\tis the best 90ies artist!";
            var characters = new[] { Alphanumeric.TabChar, ' ' };
            Validator.ThrowIfNotContainsAny(argument, characters);
        }
    }
}
