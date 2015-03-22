Public Class Persons

    Private _item As New List(Of polestar.cloud.pim.Person)
    Private _useTransaction As Boolean = False

    'Public ReadOnly Property Item(IdentifyID As Long, Optional validData As Boolean = True) As List(Of polestar.cloud.person)
    '    Get

    '        Dim dt As System.Data.DataTable = Me.getPersons(IdentifyID, validData)

    '        Me._item.Clear()

    '        For i As Long = 0 To dt.Rows.Count - 1
    '            Dim dr As System.Data.DataRow = dt.Rows(i)
    '            Dim p As New polestar.cloud.person

    '            p.PersonID = dr.Item("PERSON_ID")
    '            p.Invalid = dr.Item("INVALID")
    '            p.FirstName = dr.Item("FIRST_NAME")
    '            p.LastName = dr.Item("LAST_NAME")
    '            p.CreateDate = dr.Item("CREATE_DATE")
    '            p.UpdateDate = dr.Item("UPDATE_DATE")
    '            p.Contacts = (New polestar.pim.Contacts).Item(IdentifyID, validData)
    '            p.Organizations = (New polestar.pim.Organizations).Item(IdentifyID, validData)


    '            _item.Add(p)

    '        Next

    '        Return Me._item

    '    End Get
    'End Property

    Public Sub New()
    End Sub


    Public Sub New(Optional UseTransaction As Boolean = False)
        Me._useTransaction = UseTransaction
    End Sub

    Public Function add(firstName As String) As Boolean
        Dim sql As String = String.Format("insert into PERSON(FIRST_NAME) values('{0}')", firstName)
        Return (New polestar.cloud.pim.pim).ExecuteQueryNoResult(sql)
    End Function

    Public Function add(firstName As String, lastName As String) As Boolean
        Dim sql As String = String.Format("insert into PERSON(FIRST_NAME, LAST_NAME) values('{0}','{1}')", firstName, lastName)
        Return (New polestar.cloud.pim.pim).ExecuteQueryNoResult(sql)
    End Function

    Public Function addFromHttp(firstName As String) As System.Data.DataSet
        Dim sql As String = String.Format("insert into PERSON(FIRST_NAME) values('{0}')", firstName)
        Return (New polestar.cloud.pim.pim(Me._useTransaction)).getHttpResponseExecuteNoResult(sql)
    End Function

    Public Function addFromHttp(firstName As String, lastName As String) As System.Data.DataSet
        Dim sql As String = String.Format("insert into PERSON(FIRST_NAME, LAST_NAME) values('{0}','{1}')", firstName, lastName)
        Return (New polestar.cloud.pim.pim).getHttpResponseExecuteNoResult(sql)
    End Function


    Public Function exist(IdentifyID As Long) As Boolean
        If ((New polestar.cloud.pim.pim).getDataTable(String.Format("select * from PERSON where person_id={0} and INVALID=0", IdentifyID))).Rows.Count > 0 Then Return True
        Return False

    End Function





    Public Function modify(IdentifyID As Long, firstName As String, lastName As String) As Boolean
        Dim sql As String = String.Format("update PERSON set FIRST_NAME='{2}', LAST_NAME='{3}', UPDATE_DATE='{1}' where PERSON_ID = {0} and INVALID=0", IdentifyID, (New polestar.cloud.pim.pim).getUniversalDateTime, firstName, lastName)
        Return (New polestar.cloud.pim.pim).ExecuteQueryNoResult(sql)
    End Function


    Public Function chhangeNameFromHttp(IdentifyID As Long, firstName As String, lastName As String) As System.Data.DataSet
        Dim sql As String = String.Format("update PERSON set FIRST_NAME='{2}', LAST_NAME='{3}', UPDATE_DATE='{1}' where PERSON_ID = {0} and INVALID=0", IdentifyID, (New polestar.cloud.pim.pim).getUniversalDateTime, firstName, lastName)
        Return (New polestar.cloud.pim.pim).getHttpResponseExecuteNoResult(sql)

    End Function


    Public Function remove(IdentifyID As Long) As Boolean
        Dim sql As String = String.Format("update PERSON set INVALID =1, UPDATE_DATE='{1}' where PERSON_ID = {0}", IdentifyID, (New polestar.cloud.pim.pim).getUniversalDateTime)
        Return (New polestar.cloud.pim.pim).ExecuteQueryNoResult(sql)
    End Function
    Public Function removeFromHttp(IdentifyID As Long) As System.Data.DataSet
        Dim sql As String = String.Format("update PERSON set INVALID =1, UPDATE_DATE='{1}' where PERSON_ID = {0}", IdentifyID, (New polestar.cloud.pim.pim).getUniversalDateTime)
        Return (New polestar.cloud.pim.pim(Me._useTransaction)).getHttpResponseExecuteNoResult(sql)
    End Function
    Public Function setBirthday(IdentifyID As Long, birthday As Date) As Boolean

        Dim sql As String = String.Format("update PERSON set birthday='{1}' , UPDATE_DATE='{2}' where PERSON_ID = {0} ", IdentifyID, birthday, (New polestar.cloud.pim.pim).getUniversalDateTime)
        Return (New polestar.cloud.pim.pim).ExecuteQueryNoResult(sql)

    End Function
    Public Function setGender(IdentifyID As Long, gender As Long) As Boolean
        Return True
    End Function
    Public Function getPersons(Optional validData As Boolean = True) As System.Data.DataTable
        If validData Then
            Return ((New polestar.cloud.pim.pim).getDataTable(String.Format("select * from PERSON where INVALID=0")))
        Else
            Return ((New polestar.cloud.pim.pim).getDataTable(String.Format("select * from PERSON")))
        End If
    End Function
    Public Function getPersonsFromHttp(Optional validData As Boolean = True) As System.Data.DataSet

        If validData Then
            Return ((New polestar.cloud.pim.pim).getHTTPResponseExecuteQuery(String.Format("select * from PERSON where INVALID=0")))
        Else
            Return ((New polestar.cloud.pim.pim).getHTTPResponseExecuteQuery(String.Format("select * from PERSON")))
        End If

    End Function
    Public Function getPersonsFromHttp(IdentifyID As Long, Optional validData As Boolean = True) As System.Data.DataSet
        If validData Then
            Return ((New polestar.cloud.pim.pim).getHTTPResponseExecuteQuery(String.Format("select * from PERSON where INVALID=0 and PERSON_ID={0}", IdentifyID)))
        Else
            Return ((New polestar.cloud.pim.pim).getHTTPResponseExecuteQuery(String.Format("select * from PERSON where PERSON_ID={0}", IdentifyID)))
        End If
    End Function
    Public Function addContact(IdentifyID As Long, ContactID As Long) As Boolean
        Dim d As New polestar.cloud.pim.pim(True)
        Dim result As Boolean = False

        Try



        Catch ex As Exception
            d.rollBack()
            result = False
        End Try

        Return True

    End Function
    Public Function removeContact(IdentifyID As Long, ContactID As Long) As Boolean
        Return True
    End Function
    Public Function addOrganization(IdentifyID As Long, OrganizationID As Long) As Boolean
        Return True

    End Function
    Public Function removeOrganization(IdentifyID As Long, OrganizationID As Long) As Boolean
        Return True
    End Function
    Public Function DataTable2Class(response As System.Data.DataTable) As List(Of polestar.cloud.pim.Person)

        Dim result As New List(Of polestar.cloud.pim.Person)

        For Each dr As System.Data.DataRow In response.Rows

            Dim p As New polestar.cloud.pim.Person
            p.PersonID = dr.Item("PERSON_ID")

            p.Invalid = dr.Item("INVALID")

            p.FirstName = dr.Item("FIRST_NAME")

            p.LastName = dr.Item("LAST_NAME")

            p.CreateDate = dr.Item("CREATE_DATE")

            p.UpdateDate = dr.Item("UPDATE_DATE")

            p.CreateDate = p.CreateDate.AddHours(9)
            p.UpdateDate = p.UpdateDate.AddHours(9)

            result.Add(p)

        Next

        Return result

    End Function


End Class
