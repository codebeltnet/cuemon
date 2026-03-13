namespace Cuemon
{
    /// <summary>
    /// Encapsulates a method and returns a value that indicates success of the type specified by the <typeparamref name="TSuccess" /> parameter and returns a out result value of the type specified by the <typeparamref name="TResult" /> parameter.
    /// </summary>
    /// <typeparam name="TResult">The type of the out result value of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="TSuccess">The type of the return value that indicates success of the method that this function delegate encapsulates.</typeparam>
    /// <param name="result">The result of the method that this function delegate encapsulates.</param>
    /// <returns>The return value that indicates success of the method that this function delegate encapsulates.</returns>
    public delegate TSuccess TesterFunc<TResult, out TSuccess>(out TResult result);

    /// <summary>
    /// Encapsulates a method that has one parameter and returns a value that indicates success of the type specified by the <typeparamref name="TSuccess" /> parameter and returns a out result value of the type specified by the <typeparamref name="TResult" /> parameter.
    /// </summary>
    /// <typeparam name="T">The type of the parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="TResult">The type of the out result value of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="TSuccess">The type of the return value that indicates success of the method that this function delegate encapsulates.</typeparam>
    /// <param name="arg">The parameter of the method that this function delegate encapsulates.</param>
    /// <param name="result">The result of the method that this function delegate encapsulates.</param>
    /// <returns>The return value that indicates success of the method that this function delegate encapsulates.</returns>
    public delegate TSuccess TesterFunc<in T, TResult, out TSuccess>(T arg, out TResult result);

    /// <summary>
    /// Encapsulates a method that has two parameters and returns a value that indicates success of the type specified by the <typeparamref name="TSuccess" /> parameter and returns a out result value of the type specified by the <typeparamref name="TResult" /> parameter.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="TResult">The type of the out result value of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="TSuccess">The type of the return value that indicates success of the method that this function delegate encapsulates.</typeparam>
    /// <param name="arg1">The first parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg2">The second parameter of the method that this function delegate encapsulates.</param>
    /// <param name="result">The result of the method that this function delegate encapsulates.</param>
    /// <returns>The return value that indicates success of the method that this function delegate encapsulates.</returns>
    public delegate TSuccess TesterFunc<in T1, in T2, TResult, out TSuccess>(T1 arg1, T2 arg2, out TResult result);

    /// <summary>
    /// Encapsulates a method that has three parameters and returns a value that indicates success of the type specified by the <typeparamref name="TSuccess" /> parameter and returns a out result value of the type specified by the <typeparamref name="TResult" /> parameter.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="TResult">The type of the out result value of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="TSuccess">The type of the return value that indicates success of the method that this function delegate encapsulates.</typeparam>
    /// <param name="arg1">The first parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg2">The second parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg3">The third parameter of the method that this function delegate encapsulates.</param>
    /// <param name="result">The result of the method that this function delegate encapsulates.</param>
    /// <returns>The return value that indicates success of the method that this function delegate encapsulates.</returns>
    public delegate TSuccess TesterFunc<in T1, in T2, in T3, TResult, out TSuccess>(T1 arg1, T2 arg2, T3 arg3, out TResult result);

    /// <summary>
    /// Encapsulates a method that has four parameters and returns a value that indicates success of the type specified by the <typeparamref name="TSuccess" /> parameter and returns a out result value of the type specified by the <typeparamref name="TResult" /> parameter.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="TResult">The type of the out result value of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="TSuccess">The type of the return value that indicates success of the method that this function delegate encapsulates.</typeparam>
    /// <param name="arg1">The first parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg2">The second parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg3">The third parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg4">The fourth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="result">The result of the method that this function delegate encapsulates.</param>
    /// <returns>The return value that indicates success of the method that this function delegate encapsulates.</returns>
    public delegate TSuccess TesterFunc<in T1, in T2, in T3, in T4, TResult, out TSuccess>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, out TResult result);

    /// <summary>
    /// Encapsulates a method that has five parameters and returns a value that indicates success of the type specified by the <typeparamref name="TSuccess" /> parameter and returns a out result value of the type specified by the <typeparamref name="TResult" /> parameter.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T5">The type of the fourth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="TResult">The type of the out result value of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="TSuccess">The type of the return value that indicates success of the method that this function delegate encapsulates.</typeparam>
    /// <param name="arg1">The first parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg2">The second parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg3">The third parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg4">The fourth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg5">The fifth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="result">The result of the method that this function delegate encapsulates.</param>
    /// <returns>The return value that indicates success of the method that this function delegate encapsulates.</returns>
    public delegate TSuccess TesterFunc<in T1, in T2, in T3, in T4, in T5, TResult, out TSuccess>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, out TResult result);

    /// <summary>
    /// Encapsulates a method that has five parameters and returns a value that indicates success of the type specified by the <typeparamref name="TSuccess" /> parameter and returns a out result value of the type specified by the <typeparamref name="TResult" /> parameter.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T5">The type of the fourth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="TResult">The type of the out result value of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="TSuccess">The type of the return value that indicates success of the method that this function delegate encapsulates.</typeparam>
    /// <param name="arg1">The first parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg2">The second parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg3">The third parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg4">The fourth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg5">The fifth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg6">The sixth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="result">The result of the method that this function delegate encapsulates.</param>
    /// <returns>The return value that indicates success of the method that this function delegate encapsulates.</returns>
    public delegate TSuccess TesterFunc<in T1, in T2, in T3, in T4, in T5, in T6, TResult, out TSuccess>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, out TResult result);

    /// <summary>
    /// Encapsulates a method that has five parameters and returns a value that indicates success of the type specified by the <typeparamref name="TSuccess" /> parameter and returns a out result value of the type specified by the <typeparamref name="TResult" /> parameter.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T5">The type of the fourth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T7">The type of the seventh parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="TResult">The type of the out result value of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="TSuccess">The type of the return value that indicates success of the method that this function delegate encapsulates.</typeparam>
    /// <param name="arg1">The first parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg2">The second parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg3">The third parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg4">The fourth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg5">The fifth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg6">The sixth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg7">The seventh parameter of the method that this function delegate encapsulates.</param>
    /// <param name="result">The result of the method that this function delegate encapsulates.</param>
    /// <returns>The return value that indicates success of the method that this function delegate encapsulates.</returns>
    public delegate TSuccess TesterFunc<in T1, in T2, in T3, in T4, in T5, in T6, in T7, TResult, out TSuccess>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, out TResult result);

    /// <summary>
    /// Encapsulates a method that has five parameters and returns a value that indicates success of the type specified by the <typeparamref name="TSuccess" /> parameter and returns a out result value of the type specified by the <typeparamref name="TResult" /> parameter.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T5">The type of the fourth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T7">The type of the seventh parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T8">The type of the eighth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="TResult">The type of the out result value of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="TSuccess">The type of the return value that indicates success of the method that this function delegate encapsulates.</typeparam>
    /// <param name="arg1">The first parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg2">The second parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg3">The third parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg4">The fourth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg5">The fifth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg6">The sixth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg7">The seventh parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg8">The eighth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="result">The result of the method that this function delegate encapsulates.</param>
    /// <returns>The return value that indicates success of the method that this function delegate encapsulates.</returns>
    public delegate TSuccess TesterFunc<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, TResult, out TSuccess>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, out TResult result);

    /// <summary>
    /// Encapsulates a method that has five parameters and returns a value that indicates success of the type specified by the <typeparamref name="TSuccess" /> parameter and returns a out result value of the type specified by the <typeparamref name="TResult" /> parameter.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T5">The type of the fourth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T7">The type of the seventh parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T8">The type of the eighth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T9">The type of the ninth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="TResult">The type of the out result value of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="TSuccess">The type of the return value that indicates success of the method that this function delegate encapsulates.</typeparam>
    /// <param name="arg1">The first parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg2">The second parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg3">The third parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg4">The fourth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg5">The fifth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg6">The sixth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg7">The seventh parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg8">The eighth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg9">The ninth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="result">The result of the method that this function delegate encapsulates.</param>
    /// <returns>The return value that indicates success of the method that this function delegate encapsulates.</returns>
    public delegate TSuccess TesterFunc<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, TResult, out TSuccess>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, out TResult result);

    /// <summary>
    /// Encapsulates a method that has five parameters and returns a value that indicates success of the type specified by the <typeparamref name="TSuccess" /> parameter and returns a out result value of the type specified by the <typeparamref name="TResult" /> parameter.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T5">The type of the fourth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T7">The type of the seventh parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T8">The type of the eighth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T9">The type of the ninth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T10">The type of the tenth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="TResult">The type of the out result value of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="TSuccess">The type of the return value that indicates success of the method that this function delegate encapsulates.</typeparam>
    /// <param name="arg1">The first parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg2">The second parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg3">The third parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg4">The fourth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg5">The fifth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg6">The sixth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg7">The seventh parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg8">The eighth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg9">The ninth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg10">The tenth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="result">The result of the method that this function delegate encapsulates.</param>
    /// <returns>The return value that indicates success of the method that this function delegate encapsulates.</returns>
    public delegate TSuccess TesterFunc<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, in T10, TResult, out TSuccess>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, out TResult result);

    /// <summary>
    /// Encapsulates a method that has five parameters and returns a value that indicates success of the type specified by the <typeparamref name="TSuccess" /> parameter and returns a out result value of the type specified by the <typeparamref name="TResult" /> parameter.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T5">The type of the fourth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T7">The type of the seventh parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T8">The type of the eighth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T9">The type of the ninth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T10">The type of the tenth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T11">The type of the eleventh parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="TResult">The type of the out result value of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="TSuccess">The type of the return value that indicates success of the method that this function delegate encapsulates.</typeparam>
    /// <param name="arg1">The first parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg2">The second parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg3">The third parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg4">The fourth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg5">The fifth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg6">The sixth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg7">The seventh parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg8">The eighth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg9">The ninth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg10">The tenth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg11">The eleventh parameter of the method that this function delegate encapsulates.</param>
    /// <param name="result">The result of the method that this function delegate encapsulates.</param>
    /// <returns>The return value that indicates success of the method that this function delegate encapsulates.</returns>
    public delegate TSuccess TesterFunc<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, in T10, in T11, TResult, out TSuccess>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, out TResult result);

    /// <summary>
    /// Encapsulates a method that has five parameters and returns a value that indicates success of the type specified by the <typeparamref name="TSuccess" /> parameter and returns a out result value of the type specified by the <typeparamref name="TResult" /> parameter.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T5">The type of the fourth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T7">The type of the seventh parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T8">The type of the eighth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T9">The type of the ninth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T10">The type of the tenth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T11">The type of the eleventh parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T12">The type of the twelfth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="TResult">The type of the out result value of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="TSuccess">The type of the return value that indicates success of the method that this function delegate encapsulates.</typeparam>
    /// <param name="arg1">The first parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg2">The second parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg3">The third parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg4">The fourth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg5">The fifth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg6">The sixth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg7">The seventh parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg8">The eighth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg9">The ninth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg10">The tenth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg11">The eleventh parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg12">The twelfth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="result">The result of the method that this function delegate encapsulates.</param>
    /// <returns>The return value that indicates success of the method that this function delegate encapsulates.</returns>
    public delegate TSuccess TesterFunc<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, in T10, in T11, in T12, TResult, out TSuccess>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, out TResult result);

    /// <summary>
    /// Encapsulates a method that has five parameters and returns a value that indicates success of the type specified by the <typeparamref name="TSuccess" /> parameter and returns a out result value of the type specified by the <typeparamref name="TResult" /> parameter.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T5">The type of the fourth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T7">The type of the seventh parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T8">The type of the eighth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T9">The type of the ninth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T10">The type of the tenth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T11">The type of the eleventh parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T12">The type of the twelfth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T13">The type of the thirteenth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="TResult">The type of the out result value of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="TSuccess">The type of the return value that indicates success of the method that this function delegate encapsulates.</typeparam>
    /// <param name="arg1">The first parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg2">The second parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg3">The third parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg4">The fourth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg5">The fifth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg6">The sixth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg7">The seventh parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg8">The eighth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg9">The ninth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg10">The tenth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg11">The eleventh parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg12">The twelfth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg13">The thirteenth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="result">The result of the method that this function delegate encapsulates.</param>
    /// <returns>The return value that indicates success of the method that this function delegate encapsulates.</returns>
    public delegate TSuccess TesterFunc<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, in T10, in T11, in T12, in T13, TResult, out TSuccess>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, out TResult result);

    /// <summary>
    /// Encapsulates a method that has five parameters and returns a value that indicates success of the type specified by the <typeparamref name="TSuccess" /> parameter and returns a out result value of the type specified by the <typeparamref name="TResult" /> parameter.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T5">The type of the fourth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T7">The type of the seventh parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T8">The type of the eighth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T9">The type of the ninth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T10">The type of the tenth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T11">The type of the eleventh parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T12">The type of the twelfth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T13">The type of the thirteenth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T14">The type of the fourteenth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="TResult">The type of the out result value of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="TSuccess">The type of the return value that indicates success of the method that this function delegate encapsulates.</typeparam>
    /// <param name="arg1">The first parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg2">The second parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg3">The third parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg4">The fourth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg5">The fifth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg6">The sixth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg7">The seventh parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg8">The eighth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg9">The ninth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg10">The tenth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg11">The eleventh parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg12">The twelfth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg13">The thirteenth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg14">The fourteenth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="result">The result of the method that this function delegate encapsulates.</param>
    /// <returns>The return value that indicates success of the method that this function delegate encapsulates.</returns>
    public delegate TSuccess TesterFunc<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, in T10, in T11, in T12, in T13, in T14, TResult, out TSuccess>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, out TResult result);

    /// <summary>
    /// Encapsulates a method that has five parameters and returns a value that indicates success of the type specified by the <typeparamref name="TSuccess" /> parameter and returns a out result value of the type specified by the <typeparamref name="TResult" /> parameter.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T5">The type of the fourth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T7">The type of the seventh parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T8">The type of the eighth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T9">The type of the ninth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T10">The type of the tenth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T11">The type of the eleventh parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T12">The type of the twelfth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T13">The type of the thirteenth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T14">The type of the fourteenth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T15">The type of the fifteenth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="TResult">The type of the out result value of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="TSuccess">The type of the return value that indicates success of the method that this function delegate encapsulates.</typeparam>
    /// <param name="arg1">The first parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg2">The second parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg3">The third parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg4">The fourth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg5">The fifth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg6">The sixth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg7">The seventh parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg8">The eighth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg9">The ninth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg10">The tenth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg11">The eleventh parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg12">The twelfth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg13">The thirteenth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg14">The fourteenth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg15">The fifteenth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="result">The result of the method that this function delegate encapsulates.</param>
    /// <returns>The return value that indicates success of the method that this function delegate encapsulates.</returns>
    public delegate TSuccess TesterFunc<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, in T10, in T11, in T12, in T13, in T14, in T15, TResult, out TSuccess>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, out TResult result);

    /// <summary>
    /// Encapsulates a method that has five parameters and returns a value that indicates success of the type specified by the <typeparamref name="TSuccess" /> parameter and returns a out result value of the type specified by the <typeparamref name="TResult" /> parameter.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T5">The type of the fourth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T7">The type of the seventh parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T8">The type of the eighth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T9">The type of the ninth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T10">The type of the tenth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T11">The type of the eleventh parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T12">The type of the twelfth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T13">The type of the thirteenth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T14">The type of the fourteenth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T15">The type of the fifteenth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T16">The type of the sixteenth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="TResult">The type of the out result value of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="TSuccess">The type of the return value that indicates success of the method that this function delegate encapsulates.</typeparam>
    /// <param name="arg1">The first parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg2">The second parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg3">The third parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg4">The fourth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg5">The fifth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg6">The sixth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg7">The seventh parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg8">The eighth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg9">The ninth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg10">The tenth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg11">The eleventh parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg12">The twelfth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg13">The thirteenth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg14">The fourteenth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg15">The fifteenth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg16">The sixteenth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="result">The result of the method that this function delegate encapsulates.</param>
    /// <returns>The return value that indicates success of the method that this function delegate encapsulates.</returns>
    public delegate TSuccess TesterFunc<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, in T10, in T11, in T12, in T13, in T14, in T15, in T16, TResult, out TSuccess>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16, out TResult result);
}