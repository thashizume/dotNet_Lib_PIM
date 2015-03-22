Public Class Person


    Private _personID As Long
    Private _invalud As Long
    Private _firstName As String
    Private _lastName As String
    Private _createDate As Date
    Private _updateDate As Date
    Private _contacts As List(Of polestar.cloud.pim.Contact)
    Private _organization As List(Of polestar.cloud.pim.Organization)


    Public Property PersonID As Long
        Get
            Return Me._personID
        End Get
        Set(value As Long)
            Me._personID = value
        End Set
    End Property

    Public Property Invalid As Long
        Get
            Return Me._invalud
        End Get
        Set(value As Long)
            Me._invalud = value
        End Set
    End Property

    Public Property FirstName As String
        Get
            Return Me._firstName
        End Get
        Set(value As String)
            Me._firstName = value

        End Set
    End Property

    Public Property LastName As String
        Get
            Return Me._lastName
        End Get
        Set(value As String)
            Me._lastName = value
        End Set
    End Property

    Public Property CreateDate As Date
        Get
            Return Me._createDate
        End Get
        Set(value As Date)
            Me._createDate = value
        End Set
    End Property

    Public Property UpdateDate As Date
        Get
            Return Me._updateDate

        End Get
        Set(value As Date)
            Me._updateDate = value
        End Set
    End Property

    Public Property Contacts As List(Of polestar.cloud.pim.Contact)
        Get
            Return Me._contacts
        End Get
        Set(value As List(Of polestar.cloud.pim.Contact))
            Me._contacts = value
        End Set
    End Property

    Public Property Organizations As List(Of polestar.cloud.pim.Organization)
        Get
            Return Me._organization
        End Get
        Set(value As List(Of polestar.cloud.pim.Organization))
            Me._organization = value
        End Set
    End Property


End Class
