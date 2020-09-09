﻿Public Class servinterface
    Friend Shared clinecodestruc As xmlunpkd.linecodestruc
    Friend Shared Sub ldc_i_checker(ByRef codes As ArrayList, value As Object, Optional convtoi8 As Boolean = False, Optional datatype As String = "int32")
        rem_float(value)
        Select Case datatype
            Case "int32"
                If CDec(value) > Int32.MaxValue Or CDec(value) < Int32.MinValue Then
                    cil.push_int64_onto_stack(codes, CDec(value))
                Else
                    cil.push_int32_onto_stack(codes, CDec(value))
                    If convtoi8 Then
                        cil.conv_to_int64(codes)
                    End If
                End If
            Case "int16"
                If CDec(value) > Int16.MaxValue Or CDec(value) < Int16.MinValue Then
                    dserr.args.Add(value)
                    dserr.new_error(conserr.errortype.INTEGRALOVERFLOW, clinecodestruc.line, ilbodybulider.path, authfunc.get_line_error(ilbodybulider.path, servinterface.get_target_info(clinecodestruc), clinecodestruc.value))
                Else
                    cil.push_int32_onto_stack(codes, CDec(value))
                End If
            Case "int8"
                If CDec(value) > SByte.MaxValue Or CDec(value) < SByte.MinValue Then
                    dserr.args.Add(value)
                    dserr.new_error(conserr.errortype.INTEGRALOVERFLOW, clinecodestruc.line, ilbodybulider.path, authfunc.get_line_error(ilbodybulider.path, servinterface.get_target_info(clinecodestruc), clinecodestruc.value))
                Else
                    cil.push_int32_onto_stack(codes, CDec(value))
                End If
            Case "bool"
                If CDec(value) <> 1 AndAlso CDec(value) <> 0 Then
                    dserr.args.Add(value)
                    dserr.args.Add("bool")
                    dserr.new_error(conserr.errortype.ERRORINCONVERT, clinecodestruc.line, ilbodybulider.path, authfunc.get_line_error(ilbodybulider.path, servinterface.get_target_info(clinecodestruc), clinecodestruc.value))
                Else
                    cil.push_int32_onto_stack(codes, CDec(value))
                End If
        End Select

    End Sub

    Friend Shared Sub ldc_r_checker(ByRef codes As ArrayList, value As Object, Optional convtor8 As Boolean = False)
        If convtor8 Or value > Single.MaxValue Or value < Single.MinValue Then
            cil.push_float64_onto_stack(codes, CDbl(value))
        Else
            cil.push_float32_onto_stack(codes, CSng(value))
        End If
    End Sub

    Friend Shared Function is_i8(datatype As String) As Boolean
        For index = 0 To compdt.i8cmtypes.Length - 1
            If datatype = compdt.i8cmtypes(index) Then
                Return True
            End If
        Next
        Return False
    End Function

    Friend Shared Sub rem_float(ByRef value As Object)
        If value.ToString.Contains(conrex.DOT) Then
            Dim expr As String = value
            value = CObj(expr.Remove(expr.IndexOf(conrex.DOT)))
            Return
        End If
    End Sub

    Friend Shared Function get_target_info(clinecodestruc As xmlunpkd.linecodestruc) As lexer.targetinf
        Dim linecinf As New lexer.targetinf
        linecinf.lstart = clinecodestruc.ist
        linecinf.line = clinecodestruc.line
        linecinf.length = clinecodestruc.ile
        linecinf.lend = clinecodestruc.ien
        Return linecinf
    End Function
End Class
