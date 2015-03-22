Public Class Contact

    Private _contactID As Integer
    Private _invalid As Integer
    Private _contactType As EnumContactType
    Private _contactDevice As EnumCotactDevice
    Private _contactValue As String
    Private _createDate As Date
    Private _updateDate As Date


    Public Property ContactID As Integer
        Get
            Return Me._contactID
        End Get
        Set(value As Integer)
            Me._contactID = value
        End Set
    End Property

    Public Property Invalid As Integer
        Get
            Return Me._invalid
        End Get
        Set(value As Integer)
            Me._invalid = value
        End Set
    End Property

    Public Property ContactType As EnumContactType
        Get
            Return Me._contactType
        End Get
        Set(value As EnumContactType)
            Me._contactType = value
        End Set
    End Property

    Public Property ContactDevice As EnumCotactDevice
        Get
            Return Me._contactDevice
        End Get
        Set(value As EnumCotactDevice)
            Me._contactDevice = value
        End Set
    End Property


    Public Property ContactValue As String
        Get
            Return Me._contactValue
        End Get
        Set(value As String)
            Me._contactValue = value
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

End Class

Public Enum EnumContactType As Integer
    UNDEFINE = 0
    OFFICE = 1
    PERSONAL = 2

End Enum

Public Enum EnumCotactDevice As Integer
    UNDEFINE = 0
    EMAIL = 1
    MOBILE_PHONE = 2
    PHONE = 3
    FAX = 4

End Enum
