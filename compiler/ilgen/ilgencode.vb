﻿Public Class ilgencode

    Dim classdt() As tknformat._class
    Dim ilcollection As ilformat.ildata
    Dim ilasm As ilasmgen
    Public Sub New(tknfmtclass() As tknformat._class)
        classdt = tknfmtclass
        ilcollection = New ilformat.ildata
    End Sub


    'TODO : compile multi code generator.
    Public Sub codegenerator()
        For index = 0 To classdt.Length - 1
            'Class Generate ...
            'Single File ;
            ilasm = New ilasmgen(ilcollection)
            Dim resultdata As ilformat.resultildata = ilasm.gen(classdt(index))
            If resultdata.result Then
                Dim bodybuilder As New ilbodybulider(resultdata.ilfmtdata)
                bodybuilder.conv_to_msil()
                coutputdata.write_file_data("msil_source.il", bodybuilder.source)
            Else
                'Somethings error ...
            End If
        Next
    End Sub
End Class