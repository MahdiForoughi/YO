﻿Public Class fmtshared
    Public tknfmt As tknformat
    Private sourceloc As String
    Dim bdyformatter As bodyformatter
    Dim state As statecursor
    Public xclass(0) As tknformat._class
    Public xmethods(0) As tknformat._method
    Dim funcstate As funcstatecursor
    Dim parastate As funcparastate
    Public externlist As ArrayList
    Public Sub New(file As String)
        tknfmt = New tknformat
        sourceloc = file
        state = statecursor.OUT
        funcstate = funcstatecursor.OUT
        parastate = funcparastate.WAITFORSTARTBRACKET
        xmethods(0) = New tknformat._method
        xclass(0) = New tknformat._class
        externlist = New ArrayList
    End Sub

    Enum statecursor
        INFUNC
        INIMPORTS
        OUT
    End Enum

    Enum funcstatecursor
        OUT
        ACCESSRESTR
        FUNCINTRO
        FUNCNAME
        FUNCPARA
        FUNCRETURNTYPE
        FUNCSTBLOCK
        FUNCBODY
        FUNCENBLOCK
    End Enum

    Enum funcparastate
        WAITFORSTARTBRACKET
        WAITFORIDENTIFIER
        WAITFORASSIGNMENTOPERATOR
        WAITFORPARAMETERTYPE
        WAITFORNEWPARAMETER
    End Enum
    Public Sub imp_token(value As String, rd_token As tokenhared.token, linecinf As lexer.targetinf)
        If state = statecursor.OUT Then
            Select Case rd_token
                Case tokenhared.token.FUNC
                    _rev_func(value, rd_token, linecinf)
                Case tokenhared.token.EXTERN
                    state = statecursor.INIMPORTS
            End Select

        ElseIf state = statecursor.INFUNC Then
            _rev_func(value, rd_token, linecinf)
        ElseIf state = statecursor.INIMPORTS Then
            _rev_extern(value, rd_token, linecinf)
        End If
    End Sub

    Private Sub _rev_extern(value As String, rd_token As tokenhared.token, linecinf As lexer.targetinf)
        If rd_token = tokenhared.token.IDENTIFIER Then
            externlist.Add(value)
        Else
            dserr.new_error(conserr.errortype.IDENTIFIEREXPECTED, linecinf.line, sourceloc, authfunc.get_line_error(sourceloc, linecinf, value), "extern mylib")
        End If
        state = statecursor.OUT
    End Sub
    Private Sub _rev_func(value As String, rd_token As tokenhared.token, linecinf As lexer.targetinf)
        Static Dim i As Integer = 0
        Select Case funcstate
            Case funcstatecursor.OUT
                If rd_token = tokenhared.token.FUNC Then
                    If i <> 0 Then
                        Array.Resize(xmethods, i + 1)
                    End If
                    funcstate = funcstatecursor.FUNCNAME
                    state = statecursor.INFUNC
                    Return
                End If
            Case funcstatecursor.FUNCNAME
                If rd_token = tokenhared.token.IDENTIFIER Then
                    xmethods(i).name = value
                    funcstate = funcstatecursor.FUNCPARA
                Else
                    dserr.new_error(conserr.errortype.IDENTIFIEREXPECTED, linecinf.line, sourceloc, authfunc.get_line_error(sourceloc, linecinf, value), "func main()
{...}")
                End If
            Case funcstatecursor.FUNCPARA
                Select Case parastate
                    Case funcparastate.WAITFORSTARTBRACKET
                        If rd_token = tokenhared.token.PRSTART Then
                            xmethods(i).nopara = True
                            parastate = funcparastate.WAITFORNEWPARAMETER
                            Return
                        Else
                            dserr.new_error(conserr.errortype.FUNCOPBRACKETEXPECTED, linecinf.line, sourceloc, authfunc.get_line_error(sourceloc, linecinf, value), "func main()
{...}")
                        End If
                    Case funcparastate.WAITFORNEWPARAMETER
                        If rd_token = tokenhared.token.PREND Then
                            funcstate = funcstatecursor.FUNCSTBLOCK
                            Return
                        ElseIf rd_token = tokenhared.token.IDENTIFIER Then
                            xmethods(i).nopara = False
                            'TODO
                            Return
                        Else
                            dserr.new_error(conserr.errortype.IDENTIFIEREXPECTED, linecinf.line, sourceloc, authfunc.get_line_error(sourceloc, linecinf, value), "func get_data(id : i16)
{...}")
                        End If
                End Select

            Case funcstatecursor.FUNCRETURNTYPE
                Dim cildatatype As String = String.Empty
                If rd_token <> tokenhared.token.IDENTIFIER AndAlso rd_token <> tokenhared.token.COMMONDATATYPE Then
                    dserr.new_error(conserr.errortype.IDENTIFIEREXPECTED, linecinf.line, sourceloc, authfunc.get_line_error(sourceloc, linecinf, value), "func get_data(id : i16) :: i32
{...}")
                End If
                If servinterface.is_common_data_type(value, cildatatype) Then
                    'Common Data Type
                    xmethods(i).returntype = cildatatype
                Else
                    'Else Types ...
                End If
                funcstate = funcstatecursor.FUNCSTBLOCK
            Case funcstatecursor.FUNCSTBLOCK
                If rd_token = tokenhared.token.BLOCKOPEN Then
                    funcstate = funcstatecursor.FUNCBODY
                    bdyformatter = New bodyformatter("Func", sourceloc)
                    bdyformatter.new_token_shared(value, rd_token, linecinf)
                ElseIf rd_token = tokenhared.token.DUTNQ AndAlso xmethods(i).returntype = String.Empty Then
                    funcstate = funcstatecursor.FUNCRETURNTYPE
                Else
                    dserr.new_error(conserr.errortype.BLOCKOPENEXPECTED, linecinf.line, sourceloc, authfunc.get_line_error(sourceloc, linecinf, value), "func get_data(id : i16)
{...}")
                End If

            Case funcstatecursor.FUNCBODY
                If bdyformatter.new_token_shared(value, rd_token, linecinf) = True Then
                    xmethods(i).bodyxmlfmt = bdyformatter.xmlresult
                    i += 1
                    _settingup()
                End If
        End Select

    End Sub

    Public Function _to_organize() As tknformat._class
        xclass(0).methods = xmethods
        xclass(0).name = conrex.NULL
        Return xclass(0)
    End Function
    Public Sub _settingup()
        state = statecursor.OUT
        funcstate = funcstatecursor.OUT
        parastate = funcparastate.WAITFORSTARTBRACKET
    End Sub

End Class
