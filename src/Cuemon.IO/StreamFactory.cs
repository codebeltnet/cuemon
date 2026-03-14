using System;
#if NETSTANDARD2_1_OR_GREATER || NET10_0_OR_GREATER
using System.Buffers;
#endif
using System.Collections.Generic;
using System.IO;
using Cuemon.Text;

namespace Cuemon.IO
{
    /// <summary>
    /// Provides access to factory methods for creating <see cref="Stream"/> instances.
    /// </summary>
    public static class StreamFactory
    {
        /// <summary>
        /// Creates and returns a <see cref="Stream"/> by the specified delegate <paramref name="writer"/>.
        /// </summary>
        /// <param name="writer">The delegate that will create an in-memory <see cref="Stream"/>.</param>
        /// <param name="setup">The <see cref="StreamWriterOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Stream"/> holding the content created by the delegate <paramref name="writer"/>.</returns>
        public static Stream Create(Action<StreamWriter> writer, Action<StreamWriterOptions> setup = null)
        {
            var factory = new ActionFactory<MutableTuple<StreamWriter>>(tuple => writer?.Invoke(tuple.Arg1), new MutableTuple<StreamWriter>(null), writer);
            return CreateStreamCore(factory, setup);
        }

        /// <summary>
        /// Creates and returns a <see cref="Stream"/> by the specified delegate <paramref name="writer"/>.
        /// </summary>
        /// <typeparam name="T">The type of the parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <param name="writer">The delegate that will create an in-memory <see cref="Stream"/>.</param>
        /// <param name="arg">The parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="setup">The <see cref="StreamWriterOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Stream"/> holding the content created by the delegate <paramref name="writer"/>.</returns>
        public static Stream Create<T>(Action<StreamWriter, T> writer, T arg, Action<StreamWriterOptions> setup = null)
        {
            var factory = new ActionFactory<MutableTuple<StreamWriter, T>>(tuple => writer?.Invoke(tuple.Arg1, tuple.Arg2), new MutableTuple<StreamWriter, T>(null, arg), writer);
            return CreateStreamCore(factory, setup);
        }

        /// <summary>
        /// Creates and returns a <see cref="Stream"/> by the specified delegate <paramref name="writer"/>.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <param name="writer">The delegate that will create an in-memory <see cref="Stream"/>.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="setup">The <see cref="StreamWriterOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Stream"/> holding the content created by the delegate <paramref name="writer"/>.</returns>
        public static Stream Create<T1, T2>(Action<StreamWriter, T1, T2> writer, T1 arg1, T2 arg2, Action<StreamWriterOptions> setup = null)
        {
            var factory = new ActionFactory<MutableTuple<StreamWriter, T1, T2>>(tuple => writer?.Invoke(tuple.Arg1, tuple.Arg2, tuple.Arg3), new MutableTuple<StreamWriter, T1, T2>(null, arg1, arg2), writer);
            return CreateStreamCore(factory, setup);
        }

        /// <summary>
        /// Creates and returns a <see cref="Stream"/> by the specified delegate <paramref name="writer"/>.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <param name="writer">The delegate that will create an in-memory <see cref="Stream"/>.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="setup">The <see cref="StreamWriterOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Stream"/> holding the content created by the delegate <paramref name="writer"/>.</returns>
        public static Stream Create<T1, T2, T3>(Action<StreamWriter, T1, T2, T3> writer, T1 arg1, T2 arg2, T3 arg3, Action<StreamWriterOptions> setup = null)
        {
            var factory = new ActionFactory<MutableTuple<StreamWriter, T1, T2, T3>>(tuple => writer?.Invoke(tuple.Arg1, tuple.Arg2, tuple.Arg3, tuple.Arg4), new MutableTuple<StreamWriter, T1, T2, T3>(null, arg1, arg2, arg3), writer);
            return CreateStreamCore(factory, setup);
        }

        /// <summary>
        /// Creates and returns a <see cref="Stream"/> by the specified delegate <paramref name="writer"/>.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <param name="writer">The delegate that will create an in-memory <see cref="Stream"/>.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="setup">The <see cref="StreamWriterOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Stream"/> holding the content created by the delegate <paramref name="writer"/>.</returns>
        public static Stream Create<T1, T2, T3, T4>(Action<StreamWriter, T1, T2, T3, T4> writer, T1 arg1, T2 arg2, T3 arg3, T4 arg4, Action<StreamWriterOptions> setup = null)
        {
            var factory = new ActionFactory<MutableTuple<StreamWriter, T1, T2, T3, T4>>(tuple => writer?.Invoke(tuple.Arg1, tuple.Arg2, tuple.Arg3, tuple.Arg4, tuple.Arg5), new MutableTuple<StreamWriter, T1, T2, T3, T4>(null, arg1, arg2, arg3, arg4), writer);
            return CreateStreamCore(factory, setup);
        }

        /// <summary>
        /// Creates and returns a <see cref="Stream"/> by the specified delegate <paramref name="writer"/>.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <param name="writer">The delegate that will create an in-memory <see cref="Stream"/>.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg5">The fifth parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="setup">The <see cref="StreamWriterOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Stream"/> holding the content created by the delegate <paramref name="writer"/>.</returns>
        public static Stream Create<T1, T2, T3, T4, T5>(Action<StreamWriter, T1, T2, T3, T4, T5> writer, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, Action<StreamWriterOptions> setup = null)
        {
            var factory = new ActionFactory<MutableTuple<StreamWriter, T1, T2, T3, T4, T5>>(tuple => writer?.Invoke(tuple.Arg1, tuple.Arg2, tuple.Arg3, tuple.Arg4, tuple.Arg5, tuple.Arg6), new MutableTuple<StreamWriter, T1, T2, T3, T4, T5>(null, arg1, arg2, arg3, arg4, arg5), writer);
            return CreateStreamCore(factory, setup);
        }

#if NETSTANDARD2_1_OR_GREATER || NET10_0_OR_GREATER

        /// <summary>
        /// Creates and returns a <see cref="Stream"/> by the specified delegate <paramref name="writer"/>.
        /// </summary>
        /// <param name="writer">The delegate that will create an in-memory <see cref="Stream"/>.</param>
        /// <param name="setup">The <see cref="BufferWriterOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Stream"/> holding the content created by the delegate <paramref name="writer"/>.</returns>
        public static Stream Create(Action<IBufferWriter<byte>> writer, Action<BufferWriterOptions> setup = null)
        {
            var factory = new ActionFactory<MutableTuple<IBufferWriter<byte>>>(tuple => writer?.Invoke(tuple.Arg1), new MutableTuple<IBufferWriter<byte>>(null), writer);
            return CreateBufferStreamCore(factory, setup);
        }

        /// <summary>
        /// Creates and returns a <see cref="Stream"/> by the specified delegate <paramref name="writer"/>.
        /// </summary>
        /// <typeparam name="T">The type of the parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <param name="writer">The delegate that will create an in-memory <see cref="Stream"/>.</param>
        /// <param name="arg">The parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="setup">The <see cref="BufferWriterOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Stream"/> holding the content created by the delegate <paramref name="writer"/>.</returns>
        public static Stream Create<T>(Action<IBufferWriter<byte>, T> writer, T arg, Action<BufferWriterOptions> setup = null)
        {
            var factory = new ActionFactory<MutableTuple<IBufferWriter<byte>, T>>(tuple => writer?.Invoke(tuple.Arg1, tuple.Arg2), new MutableTuple<IBufferWriter<byte>, T>(null, arg), writer);
            return CreateBufferStreamCore(factory, setup);
        }

        /// <summary>
        /// Creates and returns a <see cref="Stream"/> by the specified delegate <paramref name="writer"/>.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <param name="writer">The delegate that will create an in-memory <see cref="Stream"/>.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="setup">The <see cref="BufferWriterOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Stream"/> holding the content created by the delegate <paramref name="writer"/>.</returns>
        public static Stream Create<T1, T2>(Action<IBufferWriter<byte>, T1, T2> writer, T1 arg1, T2 arg2, Action<BufferWriterOptions> setup = null)
        {
            var factory = new ActionFactory<MutableTuple<IBufferWriter<byte>, T1, T2>>(tuple => writer?.Invoke(tuple.Arg1, tuple.Arg2, tuple.Arg3), new MutableTuple<IBufferWriter<byte>, T1, T2>(null, arg1, arg2), writer);
            return CreateBufferStreamCore(factory, setup);
        }

        /// <summary>
        /// Creates and returns a <see cref="Stream"/> by the specified delegate <paramref name="writer"/>.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <param name="writer">The delegate that will create an in-memory <see cref="Stream"/>.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="setup">The <see cref="BufferWriterOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Stream"/> holding the content created by the delegate <paramref name="writer"/>.</returns>
        public static Stream Create<T1, T2, T3>(Action<IBufferWriter<byte>, T1, T2, T3> writer, T1 arg1, T2 arg2, T3 arg3, Action<BufferWriterOptions> setup = null)
        {
            var factory = new ActionFactory<MutableTuple<IBufferWriter<byte>, T1, T2, T3>>(tuple => writer?.Invoke(tuple.Arg1, tuple.Arg2, tuple.Arg3, tuple.Arg4), new MutableTuple<IBufferWriter<byte>, T1, T2, T3>(null, arg1, arg2, arg3), writer);
            return CreateBufferStreamCore(factory, setup);
        }

        /// <summary>
        /// Creates and returns a <see cref="Stream"/> by the specified delegate <paramref name="writer"/>.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <param name="writer">The delegate that will create an in-memory <see cref="Stream"/>.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="setup">The <see cref="BufferWriterOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Stream"/> holding the content created by the delegate <paramref name="writer"/>.</returns>
        public static Stream Create<T1, T2, T3, T4>(Action<IBufferWriter<byte>, T1, T2, T3, T4> writer, T1 arg1, T2 arg2, T3 arg3, T4 arg4, Action<BufferWriterOptions> setup = null)
        {
            var factory = new ActionFactory<MutableTuple<IBufferWriter<byte>, T1, T2, T3, T4>>(tuple => writer?.Invoke(tuple.Arg1, tuple.Arg2, tuple.Arg3, tuple.Arg4, tuple.Arg5), new MutableTuple<IBufferWriter<byte>, T1, T2, T3, T4>(null, arg1, arg2, arg3, arg4), writer);
            return CreateBufferStreamCore(factory, setup);
        }

        /// <summary>
        /// Creates and returns a <see cref="Stream"/> by the specified delegate <paramref name="writer"/>.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <param name="writer">The delegate that will create an in-memory <see cref="Stream"/>.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg5">The fifth parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="setup">The <see cref="BufferWriterOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Stream"/> holding the content created by the delegate <paramref name="writer"/>.</returns>
        public static Stream Create<T1, T2, T3, T4, T5>(Action<IBufferWriter<byte>, T1, T2, T3, T4, T5> writer, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, Action<BufferWriterOptions> setup = null)
        {
            var factory = new ActionFactory<MutableTuple<IBufferWriter<byte>, T1, T2, T3, T4, T5>>(tuple => writer?.Invoke(tuple.Arg1, tuple.Arg2, tuple.Arg3, tuple.Arg4, tuple.Arg5, tuple.Arg6), new MutableTuple<IBufferWriter<byte>, T1, T2, T3, T4, T5>(null, arg1, arg2, arg3, arg4, arg5), writer);
            return CreateBufferStreamCore(factory, setup);
        }

        private static Stream CreateBufferStreamCore<TTuple>(ActionFactory<TTuple> factory, Action<BufferWriterOptions> setup = null) where TTuple : MutableTuple<IBufferWriter<byte>>
        {
            var options = Patterns.Configure(setup);
            return CreateStreamCore<TTuple, IBufferWriter<byte>>(factory, options, options.BufferSize, (f, ms) =>
            {
                var writer = new ArrayBufferWriter<byte>(options.BufferSize);
                f.GenericArguments.Arg1 = writer;
                f.ExecuteMethod();
                ms.Write(writer.WrittenSpan);
            });
        }

#endif

        private static Stream CreateStreamCore<TTuple>(ActionFactory<TTuple> factory, Action<StreamWriterOptions> setup = null) where TTuple : MutableTuple<StreamWriter>
        {
            var options = Patterns.Configure(setup);
            return CreateStreamCore<TTuple, StreamWriter>(factory, options, options.BufferSize, (f, ms) =>
            {
                var writer = new InternalStreamWriter(ms, options);
                f.GenericArguments.Arg1 = writer;
                f.ExecuteMethod();
                writer.Flush();
            });
        }

        private static Stream CreateStreamCore<TTuple, TWriter>(ActionFactory<TTuple> factory, StreamEncodingOptions options, int bufferSize, Action<ActionFactory<TTuple>, MemoryStream> writerFactory) where TTuple : MutableTuple<TWriter>
        {
            return Patterns.SafeInvoke(() => new MemoryStream(bufferSize), (ms, f) =>
            {
                writerFactory(f, ms);

                ms.Flush();
                ms.Position = 0;
                if (options.Preamble == PreambleSequence.Remove)
                {
                    var preamble = options.Encoding.GetPreamble();
                    if (preamble.Length > 0)
                    {
                        return ByteOrderMark.Remove(ms, options.Encoding) as MemoryStream;
                    }
                }
                return ms;
            }, factory, (ex, f) =>
            {
                var parameters = new List<object>();
                parameters.AddRange(f.GenericArguments.ToArray());
                parameters.Add(options);
                throw ExceptionInsights.Embed(new InvalidOperationException("There is an error in the Stream being written.", ex), f.DelegateInfo, parameters.ToArray());
            });
        }
    }
}
