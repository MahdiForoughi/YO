﻿Public Class parampt

    Friend Shared Function check_param_types(_ilmethod As ilformat._ilmethodcollection, paramtypes As ArrayList, cargcodestruc As xmlunpkd.linecodestruc()) As Boolean
        Dim getdatatype As String = conrex.NULL
        For index = 0 To cargcodestruc.Length - 1
            getdatatype = paramtypes(index).ToString()
            If getdatatype.EndsWith("&") Then
                getdatatype = getdatatype.Remove(getdatatype.Length - 1)
            End If
            Select Case getdatatype
                Case "string"
                    If chstr(_ilmethod, cargcodestruc(index)) = False Then Return False
                Case illdloc.check_integer_type(getdatatype)
                    If chint(_ilmethod, cargcodestruc(index), getdatatype) = False Then Return False
                Case "float32"
                    If chflt(_ilmethod, cargcodestruc(index), getdatatype) = False Then Return False
                Case "float64"
                    If chflt(_ilmethod, cargcodestruc(index), getdatatype) = False Then Return False
                Case "char"
                    If chchr(_ilmethod, cargcodestruc(index), getdatatype) = False Then Return False
                Case "bool"
                    If chbool(_ilmethod, cargcodestruc(index), getdatatype) = False Then Return False
                Case Else
                    'Other Types
                    Return False
            End Select
        Next

        Return True
    End Function

    Friend Shared Function chstr(_ilmethod As ilformat._ilmethodcollection, cargcodestruc As xmlunpkd.linecodestruc) As Boolean
        Select Case cargcodestruc.tokenid
            Case tokenhared.token.TYPE_DU_STR
                Return True
            Case tokenhared.token.TYPE_CO_STR
                Return True
            Case tokenhared.token.IDENTIFIER
                Dim getdatatype As String = String.Empty
                If servinterface.is_variable(_ilmethod, cargcodestruc.value, getdatatype) AndAlso getdatatype = "string" Then
                    Return True
                End If
                Return False
            Case tokenhared.token.NULL
                Return True
            Case Else
                Return False
        End Select
    End Function

    Friend Shared Function chint(_ilmethod As ilformat._ilmethodcollection, cargcodestruc As xmlunpkd.linecodestruc, datatype As String) As Boolean
        Dim convtoi8 As Boolean = servinterface.is_i8(datatype)
        servinterface.clinecodestruc = cargcodestruc
        Select Case cargcodestruc.tokenid
            Case tokenhared.token.TYPE_INT
                Return True
            Case tokenhared.token.IDENTIFIER
                Dim getdatatype As String = String.Empty
                If servinterface.is_variable(_ilmethod, cargcodestruc.value, getdatatype) AndAlso getdatatype = datatype Then
                    Return True
                End If
                Return False
            Case tokenhared.token.NULL
                Return True
            Case tokenhared.token.EXPRESSION
                Return True
            Case Else
                Return False
        End Select
    End Function

    Friend Shared Function chflt(_ilmethod As ilformat._ilmethodcollection, cargcodestruc As xmlunpkd.linecodestruc, datatype As String) As Boolean
        Dim convtor8 As Boolean = False
        If datatype = "float64" Then convtor8 = True
        Select Case cargcodestruc.tokenid
            Case tokenhared.token.TYPE_FLOAT
                Return True
            Case tokenhared.token.IDENTIFIER
                Dim getdatatype As String = String.Empty
                If servinterface.is_variable(_ilmethod, cargcodestruc.value, getdatatype) AndAlso getdatatype = datatype Then
                    Return True
                End If
                Return False
            Case tokenhared.token.NULL
                Return True
            Case tokenhared.token.EXPRESSION
                Return True
            Case Else
                Return False
        End Select
    End Function

    Friend Shared Function chchr(_ilmethod As ilformat._ilmethodcollection, cargcodestruc As xmlunpkd.linecodestruc, datatype As String) As Boolean
        Dim convtoi8 As Boolean = servinterface.is_i8(datatype)
        servinterface.clinecodestruc = cargcodestruc
        Select Case cargcodestruc.tokenid
            Case tokenhared.token.TYPE_DU_STR
                If cargcodestruc.value.Length = 3 Then
                    Return True
                Else
                    Return False
                End If
            Case tokenhared.token.TYPE_CO_STR
                If cargcodestruc.value.Length = 3 Then
                    Return True
                Else
                    Return False
                End If
            Case tokenhared.token.IDENTIFIER
                Dim getdatatype As String = String.Empty
                If servinterface.is_variable(_ilmethod, cargcodestruc.value, getdatatype) AndAlso getdatatype = datatype Then
                    Return True
                End If
                Return False
            Case tokenhared.token.NULL
                Return True
            Case Else
                Return False
        End Select
    End Function

    Friend Shared Function chbool(_ilmethod As ilformat._ilmethodcollection, cargcodestruc As xmlunpkd.linecodestruc, datatype As String) As Boolean
        Dim convtoi8 As Boolean = servinterface.is_i8(datatype)
        servinterface.clinecodestruc = cargcodestruc
        Select Case cargcodestruc.tokenid
            Case tokenhared.token.TRUE
                Return True
            Case tokenhared.token.FALSE
                Return True
            Case tokenhared.token.IDENTIFIER
                Dim getdatatype As String = String.Empty
                If servinterface.is_variable(_ilmethod, cargcodestruc.value, getdatatype) AndAlso getdatatype = datatype Then
                    Return True
                End If
                Return False
            Case tokenhared.token.NULL
                Return True
            Case Else
                Return False
        End Select

    End Function
End Class
