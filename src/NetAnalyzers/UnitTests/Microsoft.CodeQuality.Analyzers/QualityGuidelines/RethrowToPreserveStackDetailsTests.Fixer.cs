﻿using System.Threading.Tasks;
using Xunit;
using VerifyCS = Test.Utilities.CSharpCodeFixVerifier<
    Microsoft.CodeQuality.CSharp.Analyzers.QualityGuidelines.CSharpRethrowToPreserveStackDetailsAnalyzer,
    Microsoft.CodeQuality.Analyzers.QualityGuidelines.RethrowToPreserveStackDetailsFixer>;
using VerifyVB = Test.Utilities.VisualBasicCodeFixVerifier<
    Microsoft.CodeQuality.VisualBasic.Analyzers.QualityGuidelines.BasicRethrowToPreserveStackDetailsAnalyzer,
    Microsoft.CodeQuality.Analyzers.QualityGuidelines.RethrowToPreserveStackDetailsFixer>;

namespace Microsoft.CodeQuality.Analyzers.UnitTests.QualityGuidelines
{
    public class RethrowToPreserveStackDetailsTests
    {
        [Fact]
        public async Task TestCSharp_RethrowExplicitlyToThrowImplicitly()
        {
            await VerifyCS.VerifyCodeFixAsync(@"
using System;

class Program
{
    void CatchAndRethrowExplicitly()
    {
        try
        {
            ThrowException();
        }
        catch (ArithmeticException e)
        {
            throw e; //Some comments
        }
    }

    void ThrowException()
    {
        throw new ArithmeticException();
    }
}", VerifyCS.Diagnostic().WithLocation(14, 13),
@"
using System;

class Program
{
    void CatchAndRethrowExplicitly()
    {
        try
        {
            ThrowException();
        }
        catch (ArithmeticException e)
        {
            throw; //Some comments
        }
    }

    void ThrowException()
    {
        throw new ArithmeticException();
    }
}");
        }
        [Fact]
        public async Task TestBasic_RethrowExplicitlyToThrowImplicitly()
        {
            await VerifyVB.VerifyCodeFixAsync(@"
Imports System
Class Program
    Sub CatchAndRethrowExplicitly()
        Try
            Throw New ArithmeticException()
        Catch e As ArithmeticException
            Throw e 'Some comment
        End Try
    End Sub
End Class
", VerifyVB.Diagnostic().WithLocation(8, 13),
    @"
Imports System
Class Program
    Sub CatchAndRethrowExplicitly()
        Try
            Throw New ArithmeticException()
        Catch e As ArithmeticException
            Throw 'Some comment
        End Try
    End Sub
End Class
"
    );
        }
    }
}


