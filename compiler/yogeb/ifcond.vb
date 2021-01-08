﻿Imports YOCA

Public Class ifcond
    Friend Shared leavebrachlabel As New ArrayList
    Private _ilmethod As ilformat._ilmethodcollection
    Public Sub New(ilmethod As ilformat._ilmethodcollection)
        Me._ilmethod = ilmethod
    End Sub

    Public Function set_if_statement(clinecodestruc() As xmlunpkd.linecodestruc, ByRef _illocalinit() As ilformat._illocalinit, ByRef localinit As localinitdata) As ilformat._ilmethodcollection
        Dim condlinecodestruc() As xmlunpkd.linecodestruc = condproc.get_condition(clinecodestruc, 1)
        Dim ifenblock As xmlunpkd.linecodestruc = condproc.get_block_body(clinecodestruc)
        Dim nbranch As New condproc.branchtargetinfo
        nbranch.truebranch = lngen.get_line_prop("st_if")
        nbranch.falsebranch = lngen.get_line_prop("en_if")
        Dim cdproc As New condproc(nbranch)
        cdproc.set_condition(_ilmethod, condlinecodestruc, False)
        set_if_body(ifenblock, nbranch, _illocalinit, localinit)
        Return _ilmethod
    End Function

    Private Sub set_if_body(ifenblock As xmlunpkd.linecodestruc, nbranch As condproc.branchtargetinfo, ByRef illocalinit() As ilformat._illocalinit, ByRef localinit As localinitdata)
        '  stjmper.set_new_jmper(tokenhared.token.if, nbranch.falsebranch, Nothing )
        lngen.set_direct_label(nbranch.truebranch, _ilmethod.codes)
        Dim iltrans As New iltranscore(ilbodybulider.path, ifenblock.value, illocalinit, localinit)
        iltrans.gen_transpile_code(_ilmethod, False)
        illocalinit = _ilmethod.locallinit
        '   stjmper.reset_jmper(tokenhared.token.IF )
        lngen.set_direct_label(nbranch.falsebranch, _ilmethod.codes)
    End Sub

    Public Function set_else_statement(clinecodestruc() As xmlunpkd.linecodestruc, ByRef illocalinit() As ilformat._illocalinit, localinit As localinitdata) As ilformat._ilmethodcollection
        syntaxchecker.check_statement(clinecodestruc, syntaxloader.statements.ELSE)
        Dim iltrans As New iltranscore(ilbodybulider.path, clinecodestruc(1).value, illocalinit, localinit)
        iltrans.gen_transpile_code(_ilmethod, False)
        illocalinit = _ilmethod.locallinit
        Return _ilmethod
    End Function
End Class
