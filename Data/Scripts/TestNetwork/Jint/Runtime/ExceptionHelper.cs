﻿using System;
using Jint.Runtime.CallStack;

namespace Jint.Runtime
{
    internal static class ExceptionHelper
    {
        public static void ThrowSyntaxError(Engine engine, string message = null)
        {
            throw new JavaScriptException(engine.SyntaxError, message);
        }

        public static T ThrowArgumentException<T>(string message = null)
        {
            ThrowArgumentException(message);
            return default(T);
        }

        public static void ThrowArgumentException(string message = null)
        {
            ThrowArgumentException(message, null);
        }

        public static void ThrowArgumentException(string message, string paramName)
        {
            throw new ArgumentException(message, paramName);
        }

        public static void ThrowReferenceError(Engine engine, string message = null)
        {
            throw new JavaScriptException(engine.ReferenceError, message);
        }

        public static T ThrowTypeError<T>(Engine engine, string message = null, Exception exception = null)
        {
            ThrowTypeError(engine, message, exception);
            return default(T);
        }

        public static void ThrowTypeError(Engine engine, string message = null, Exception exception = null)
        {
            throw new JavaScriptException(engine.TypeError, message, exception);
        }

        public static void ThrowRangeError(Engine engine, string message = null)
        {
            throw new JavaScriptException(engine.RangeError, message);
        }

        public static void ThrowUriError(Engine engine)
        {
            throw new JavaScriptException(engine.UriError);
        }

        public static void ThrowNotImplementedException()
        {
            throw new Exception();
        }

        public static void ThrowArgumentOutOfRangeException(string paramName, string message)
        {
            throw new Exception(message);
        }

        public static void ThrowTimeoutException()
        {
            throw new Exception();
        }

        public static void ThrowStatementsCountOverflowException()
        {
            throw new StatementsCountOverflowException();
        }

        public static void ThrowArgumentOutOfRangeException()
        {
            throw new Exception();
        }

        public static void ThrowNotSupportedException(string message = null)
        {
            throw new NotSupportedException(message);
        }

        public static void ThrowInvalidOperationException(string message = null)
        {
            throw new InvalidOperationException(message);
        }

        public static void ThrowJavaScriptException(string message)
        {
            throw new JavaScriptException("TypeError");
        }

        public static void ThrowRecursionDepthOverflowException(JintCallStack currentStack, string currentExpressionReference)
        {
            throw new RecursionDepthOverflowException(currentStack, currentExpressionReference);
        }

        public static void ThrowArgumentNullException(string paramName)
        {
            throw new ArgumentNullException(paramName);
        }

        public static T ThrowArgumentNullException<T>(string paramName)
        {
            throw new ArgumentNullException(paramName);
        }

        public static void ThrowError(Engine engine, string message)
        {
            throw new JavaScriptException(engine.Error, message);
        }

        public static void ThrowPlatformNotSupportedException(string message)
        {
            throw new Exception();
        }

        public static void ThrowMemoryLimitExceededException(string message)
        {
            throw new MemoryLimitExceededException(message);
        }
    }
}