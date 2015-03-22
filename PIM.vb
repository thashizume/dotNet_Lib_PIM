Public Class PIM
    Implements IDisposable

    'Private Shadows Const connectionString = "Server=tcp:f7t78cz869.database.windows.net,1433;Database=PIM;User ID=dsnyAdmin@f7t78cz869;Password=1Qaz2Wsx,;Trusted_Connection=False;Encrypt=True;Connection Timeout=30;"
    Private Shadows Const connectionString = "Server=tcp:buj6x99b0o.database.windows.net,1433;Database=pimsvc;User ID=polestarpim@buj6x99b0o;Password=2wsX3edC,;Trusted_Connection=False;Encrypt=True;Connection Timeout=30;"
    Private _con As System.Data.SqlClient.SqlConnection
    Private _useTransaction As Boolean = False
    Private _transaction As System.Data.SqlClient.SqlTransaction = Nothing
    Private _state As New Dictionary(Of EnumPIMResponseState, String)


    Public Function getConnectionString() As Dictionary(Of String, String)

        Dim result As New Dictionary(Of String, String)
        Dim fp As String = (New polestar.Security.Cryptography).getFingerPrint
        result.Add(fp, (New polestar.Security.Cryptography).Encrypt(connectionString, fp))
        Return result

    End Function































    Public Sub New()
        Me._useTransaction = False

        _state.Add(EnumPIMResponseState.SUCCESS, "success")
        _state.Add(EnumPIMResponseState.EXCEPTION, "exception")

    End Sub

    Public Sub New(useTransaction As Boolean)
        Me._useTransaction = useTransaction


        _state.Add(EnumPIMResponseState.SUCCESS, "success")
        _state.Add(EnumPIMResponseState.EXCEPTION, "exception")

    End Sub

    Public Sub Dispose() Implements IDisposable.Dispose
        Try
            If Me._useTransaction Then
                If IsNothing(Me._transaction) Then
                    Me._transaction.Rollback()
                End If
            End If

        Catch ex As Exception

        Finally

            Me._con.Close()
            Me._con.Dispose()

        End Try

    End Sub

    Private ReadOnly Property Connection As System.Data.SqlClient.SqlConnection
        Get

            If IsNothing(Me._con) Then

                Me._con = New System.Data.SqlClient.SqlConnection(PIM.connectionString)
                Me._con.Open()

                If Me._useTransaction = True Then
                    _transaction = Me._con.BeginTransaction
                End If

            End If

            Return Me._con
        End Get
    End Property

    Public Function ExecuteQueryNoResult(sql As String) As Long

        Dim cmd As New System.Data.SqlClient.SqlCommand(sql, Me.Connection)
        Return cmd.ExecuteNonQuery()

    End Function

    Public Function ExecuteQuery(sql As String) As System.Data.SqlClient.SqlDataReader

        Dim cmd As New System.Data.SqlClient.SqlCommand(sql, Me.Connection)
        Return cmd.ExecuteReader

    End Function

    Public Sub commit()
        If Me._useTransaction Then
            If IsNothing(Me._transaction) Then
                Me._transaction.Commit()

            End If
        End If
    End Sub

    Public Sub rollBack()
        If Me._useTransaction Then
            If IsNothing(Me._transaction) Then
                Me._transaction.Rollback()
            End If
        End If
    End Sub

    Public Function getDataTable(sql As String) As System.Data.DataTable
        Dim result As New System.Data.DataTable("rows")
        Dim c As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sql, Me.Connection)
        Dim reader As System.Data.SqlClient.SqlDataReader = c.ExecuteReader()
        result.Load(reader)

        'Dim dt As System.Data.DataTable = reader.GetSchemaTable
        'If reader.HasRows = False Then Return dt

        'Do While reader.Read

        '    Dim dr As System.Data.DataRow = dt.NewRow

        '    For Each _c As System.Data.DataColumn In dt.Columns

        '    Next
        'Loop

        Return result

    End Function

    Public Function decryptDataTable(fingerPrint As String, src As System.Data.DataTable) As System.Data.DataTable

        Dim result As New System.Data.DataTable("rows")

        '
        '   すべてのカラムを、文字型にして、Finger Printで複合化する
        '

        '   カラム名を定義
        For Each _column As System.Data.DataColumn In src.Columns
            result.Columns.Add(_column.ColumnName, GetType(String))
        Next

        '   row を暗号化
        For Each _row As System.Data.DataRow In src.Rows
            Dim row As System.Data.DataRow = result.NewRow
            For i As Integer = 0 To src.Columns.Count - 1
                If _row(i).Equals(DBNull.Value) Then
                Else
                    row(i) = (New Polestar.Security.Cryptography(fingerPrint)).Decrypt(_row(i))
                End If
            Next
            result.Rows.Add(row)
        Next

        Return result

    End Function

    Public Function cryptDataTable(fingerPrint As String, sql As String) As System.Data.DataTable
        Dim tmpResult As New System.Data.DataTable("rows")
        Dim result As New System.Data.DataTable("rows")
        Dim c As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sql, Me.Connection)
        Dim reader As System.Data.SqlClient.SqlDataReader = c.ExecuteReader()
        tmpResult.Load(reader)

        '
        '   すべてのカラムを、文字型にして、Finger Printで暗号化する
        '

        '   カラム名を定義
        For Each _column As System.Data.DataColumn In tmpResult.Columns
            result.Columns.Add(_column.ColumnName, GetType(String))
        Next

        '   row を暗号化
        For Each _row As System.Data.DataRow In tmpResult.Rows
            Dim row As System.Data.DataRow = result.NewRow
            For i As Integer = 0 To tmpResult.Columns.Count - 1
                If _row(i).Equals(DBNull.Value) Then

                Else
                    row(i) = (New polestar.Security.Cryptography(fingerPrint)).Encrypt(_row(i))
                End If

            Next
            result.Rows.Add(row)
        Next

        Return result

    End Function

    Public Function getHttpResponseExecuteNoResult(sql As String) As System.Data.DataSet

        Dim result As System.Data.DataSet
        Dim rows As Long = 0
        Dim fingerPrint As String = (New Polestar.Security.Cryptography).getFingerPrint

        Try

            rows = Me.ExecuteQueryNoResult(sql)
            'If rows > 0 Then
            '    If Me._useTransaction Then Me._transaction.Commit()

            'End If


            result = createResponseSummary(fingerPrint, EnumPIMResponseState.SUCCESS, , , rows)

        Catch ex As Exception
            result = createResponseSummary(EnumPIMResponseState.EXCEPTION, ex.Message)
        End Try
        Return result


    End Function


    Public Function getHTTPResponseExecuteQuery(sql As String) As System.Data.DataSet
        Dim dt As System.Data.DataTable
        Dim result As System.Data.DataSet
        Dim fingerPrint As String = (New Polestar.Security.Cryptography).getFingerPrint

        Try
            dt = Me.cryptDataTable(fingerPrint, sql)
            result = createResponseSummary(fingerPrint, EnumPIMResponseState.SUCCESS, , dt)

        Catch ex As Exception
            result = createResponseSummary(EnumPIMResponseState.EXCEPTION, ex.Message)
        End Try
        Return result

    End Function


    Private Function createResponseSummary( _
                    fingerPrint As String, _
                    state_id As EnumPIMResponseState, _
                    Optional exceptionMessage As String = Nothing, _
                    Optional rows As System.Data.DataTable = Nothing, _
                    Optional rowCount As Long = 0) As System.Data.DataSet

        Dim ds As New System.Data.DataSet
        Dim dt As New System.Data.DataTable("summary")

        dt.Columns.Add("state_id", GetType(Int32))
        dt.Columns.Add("state", GetType(String))
        dt.Columns.Add("finger_print", GetType(String))
        dt.Columns.Add("exception_message", GetType(String))
        dt.Columns.Add("row_count", GetType(Int32))

        Dim dr As System.Data.DataRow = dt.NewRow
        dr("state_id") = state_id
        dr("state") = Me._state(state_id)
        dr("finger_print") = (New Polestar.Security.Cryptography).getFingerPrint
        dr("exception_message") = (New Polestar.Security.Cryptography(dr("finger_print"))).Encrypt(exceptionMessage)
        If IsNothing(rows) Then
            dr("row_count") = rowCount
        Else
            dr("row_count") = rows.Rows.Count
        End If

        dt.Rows.Add(dr)
        ds.Tables.Add(dt)

        If Not IsNothing(rows) Then
            ds.Tables.Add(rows)

        End If

        Return ds

    End Function


    Public Function getUniversalDateTime() As String
        Dim result As String

        result = String.Format(
            "{0}-{1}-{2} {3}:{4}:{5}" _
            , (Now.ToUniversalTime).Year _
            , (Now.ToUniversalTime).Month _
            , (Now.ToUniversalTime).Day _
            , (Now.ToUniversalTime).Hour _
            , (Now.ToUniversalTime).Minute _
            , (Now.ToUniversalTime).Second _
            )
        Return result
    End Function


    Public Function parseHttpResponse(response As System.Data.DataSet) As System.Data.DataTable


        Dim sum As System.Data.DataTable = response.Tables("summary")
        If sum.Rows(0).Item("state_id") = EnumPIMResponseState.EXCEPTION Then
            Throw New Exception(sum.Rows(0).Item("exception_message"))
            Return Nothing
        End If

        Return response.Tables("rows")

    End Function

End Class



Public Enum EnumPIMResponseState As Long
    SUCCESS = 1
    EXCEPTION = 2

End Enum