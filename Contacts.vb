Public Class Contacts


    Private _item As List(Of polestar.cloud.pim.Contact) = New List(Of polestar.cloud.pim.Contact)

    Public ReadOnly Property Item As List(Of polestar.cloud.pim.Contact)
        Get
            Return Me._item
        End Get
    End Property

    Public Function equal(contacts As polestar.cloud.pim.Contacts) As Boolean

        Return contacts.equal(Me)

    End Function


    Public Function exist(contactDevice As Long, contactType As Long, contactValue As String) As Long

        Dim result As Boolean = False
        Dim conInfo As Dictionary(Of String, String) = (New PIM).getConnectionString
        Dim _pimDB As New polestar.cloud.db.SQLServer(conInfo.Keys(0), conInfo(conInfo.Keys(0)))
        Dim sql As String = String.Format("select CONTACT_ID from CONTACT where CONTACT_DEVICE={0} and INVALID={1} and CONTACT_TYPE = {2} and CONTACT_VALUE='{3}'", contactDevice, 0, contactType, contactValue)

        _pimDB.Open()
        Dim dt As System.Data.DataTable = _pimDB.DataReader2DataTable(_pimDB.ExecuteQuery(sql))
        If dt.Rows.Count > 0 Then result = dt.Rows(0).Item(0)

        Return result

    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="email"></param>
    ''' <param name="pimDB"></param>
    ''' <returns>Identify Contact ID</returns>
    ''' <remarks></remarks>
    Public Function existEmail(email As String, Optional pimDB As polestar.cloud.db.SQLServer = Nothing) As Long

        Dim result As Long = 0

        Dim conf As Dictionary(Of String, String) = (New PIM).getConnectionString
        Dim _pimDB As polestar.cloud.db.SQLServer

        If IsNothing(pimDB) Then
            _pimDB = New polestar.cloud.db.SQLServer(conf.Keys(0), conf(conf.Keys(0)))
            _pimDB.Open()
        Else
            _pimDB = pimDB
        End If

        Dim sql As String = String.Format("select CONTACT_ID from CONTACT where INVALID={0} and CONTACT_VALUE='{1}'", 0, email)


        Dim dt As System.Data.DataTable = _pimDB.DataReader2DataTable(_pimDB.ExecuteQuery(sql))
        If dt.Rows.Count > 0 Then result = dt.Rows(0).Item(0)

        Return result

    End Function



















    Public Function add(contactDevice As Long, contactType As Long, contactValue As String, Optional pimDB As polestar.cloud.db.SQLServer = Nothing) As Boolean

        Dim conf As Dictionary(Of String, String) = (New PIM).getConnectionString
        Dim _pimDB As polestar.cloud.db.SQLServer
        Dim sql As String

        If IsNothing(pimDB) Then
            _pimDB = New polestar.cloud.db.SQLServer(conf.Keys(0), conf(conf.Keys(0)))
        Else
            _pimDB = pimDB
        End If
        sql = String.Format("insert into CONTACT(CONTACT_TYPE, CONTACT_VALUE, CONTACT_DEVICE, CREATE_DATE, MODIFY_DATE) values({0},'{1}',{2}, getDate(), getDate())", contactType, contactValue, contactDevice)

        Return _pimDB.ExecuteQueryNoResult(sql)

    End Function

    Public Function addFromHttp(contactDevice As Long, contactType As Long, contactValue As String) As System.Data.DataSet

        If Me.exist(contactDevice, contactType, contactValue) Then Throw New Exception("Exist Contact Value. ")
        Dim sql As String = String.Format("insert into CONTACT(CONTACT_TYPE, CONTACT_VALUE, CONTACT_DEVICE) values({0},'{1}',{2})", contactType, contactValue, contactDevice)
        Return (New polestar.cloud.pim.PIM).getHttpResponseExecuteNoResult(sql)

    End Function

    Public Function modify(IdentifyID As Long, contactType As Long, contactValue As String) As Boolean
        Return (New polestar.cloud.pim.pim).ExecuteQueryNoResult( _
            String.Format("update CONTACT set  CONTACT_TYPE = {1}, CONTACT_VALUE='{2}' , UPDATE_DATE='{3}' where CONTACT_ID = {0} and INVALID=0", IdentifyID, contactType, contactValue, (New polestar.cloud.pim.pim).getUniversalDateTime))
    End Function

    Public Function remove(IdentifyID As Long) As Boolean
        Return (New polestar.cloud.pim.pim).ExecuteQueryNoResult(String.Format("update CONTACT set INVALID =1, UPDATE_DATE='{1}' where CONTACT_ID = {0}", IdentifyID, (New polestar.cloud.pim.pim).getUniversalDateTime))
    End Function

    Public Function getDataTable(Optional validData As Boolean = True) As System.Data.DataTable

        If validData Then
            Return ((New polestar.cloud.pim.pim).getDataTable(String.Format("select * from CONTACT where INVALID=0")))
        Else
            Return ((New polestar.cloud.pim.pim).getDataTable(String.Format("select * from CONTACT")))
        End If

    End Function

    Public Function getDataTable(IdentifyID As Long, Optional validData As Boolean = True) As System.Data.DataTable

        If validData Then
            Return ((New polestar.cloud.pim.pim).getDataTable(String.Format("select * from CONTACT where INVALID=0 and CONTACT_ID={0}", IdentifyID)))
        Else
            Return ((New polestar.cloud.pim.pim).getDataTable(String.Format("select * from CONTACT where CONTACT_ID={0}", IdentifyID)))
        End If

    End Function

    'Public Function getPersonContact(IdentifyID As Long, Optional validData As Boolean = True) As System.Data.DataTable

    '    If validData Then
    '        Return ((New polestar.cloud.pim.pimData).getDataTable(String.Format("select * from CONTACT where INVALID=0 and PERSON_ID={0}", IdentifyID)))
    '    Else
    '        Return ((New polestar.cloud.pim.pimData).getDataTable(String.Format("select * from CONTACT where PERSON_ID={0}", IdentifyID)))
    '    End If

    'End Function

    Public Function DataTable2Class(src As System.Data.DataTable) As List(Of Contact)

        Dim result As New List(Of Contact)

        For i As Long = 0 To src.Rows.Count - 1
            Dim dr As System.Data.DataRow = src.Rows(i)
            Dim c As New polestar.cloud.pim.Contact

            c.ContactID = dr.Item(0)
            c.Invalid = dr.Item(1)
            c.ContactType = dr.Item(2)
            c.ContactDevice = dr.Item(3)
            c.ContactValue = dr.Item(4)
            c.CreateDate = dr.Item(5)
            c.UpdateDate = dr.Item(6)

            result.Add(c)

        Next

        Return result

    End Function



End Class
