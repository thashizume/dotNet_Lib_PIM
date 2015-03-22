Public Class Organizations

    Public _item As New List(Of polestar.cloud.pim.Organization)


    Public ReadOnly Property Item(IdentifyID As Long, Optional validData As Boolean = True) As List(Of polestar.cloud.pim.Organization)
        Get

            Dim dt As System.Data.DataTable = Me.getDatatable(IdentifyID, validData)

            Me._item.Clear()

            For i As Long = 0 To dt.Rows.Count - 1
                Dim dr As System.Data.DataRow = dt.Rows(i)
                Dim o As New Polestar.cloud.pim.Organization

                o.OrganiztionID = dr.Item("ORGANIZATION_ID")
                o.INVALID = dr.Item("INVALID")
                o.OrganiztionName = dr.Item("ORGANIZATION_NAME")
                o.CreateDate = dr.Item("CREATE_DATE")
                o.UpdateDate = dr.Item("UPDATE_DATE")

                _item.Add(o)

            Next

            Return Me._item

        End Get
    End Property

    Public Function add(organizationName As String) As Boolean
        Dim sql As String = String.Format("insert into ORGANIZATION(ORGANIZATION_NAME) values('{0}')", organizationName)
        Return (New polestar.cloud.pim.pim).ExecuteQueryNoResult(sql)
    End Function

    Public Function exist() As Boolean
        Return False
    End Function

    Public Function remove(IdentifyID As Long) As Boolean
        Dim sql As String = String.Format("update ORGANIZATION set INVALID =1 , UPDATE_DATE='{1}' where ORGANIZATION_ID = {0}", IdentifyID, (New polestar.cloud.pim.pim).getUniversalDateTime)
        Return (New polestar.cloud.pim.pim).ExecuteQueryNoResult(sql)
    End Function

    Public Function modify(IdentifyID As Long, organizationName As String) As Boolean
        Dim sql As String = String.Format("update ORGANIZATION set ORGANIZATION_NAME ='{1}'  , UPDATE_DATE='{2}' where ORGANIZATION_ID = {0} and INVALID = 0", IdentifyID, organizationName, (New polestar.cloud.pim.pim).getUniversalDateTime)
        Return (New polestar.cloud.pim.pim).ExecuteQueryNoResult(sql)
    End Function

    Public Function getDatatable(Optional validData As Boolean = True) As System.Data.DataTable
        If validData Then
            Return ((New polestar.cloud.pim.pim).getDataTable(String.Format("select * from ORGANIZATION where INVALID=0")))
        Else
            Return ((New polestar.cloud.pim.pim).getDataTable(String.Format("select * from ORGANIZATION")))
        End If
    End Function


    Public Function getDatatable(IdentifyID As Long, Optional validData As Boolean = True) As System.Data.DataTable
        If validData Then
            Return ((New polestar.cloud.pim.pim).getDataTable(String.Format("select * from ORGANIZATION where INVALID=0 and ORGANIZATION_ID={0}", IdentifyID)))
        Else
            Return ((New polestar.cloud.pim.pim).getDataTable(String.Format("select * from ORGANIZATION where PERSON_ID={0}", IdentifyID)))
        End If
    End Function


End Class
