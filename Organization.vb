Public Class Organization

    Private _organizationID As Long
    Private _invalid As Long
    Private _organizationName As String
    Private _createDate As Date
    Private _updateDate As Date

    Public Property OrganiztionID As Long
        Get
            Return Me._organizationID
        End Get
        Set(value As Long)
            Me._organizationID = value
        End Set
    End Property

    Public Property INVALID As Long
        Get
            Return Me._invalid
        End Get
        Set(value As Long)
            Me._invalid = value

        End Set
    End Property

    Public Property OrganiztionName As String
        Get
            Return Me._organizationName
        End Get
        Set(value As String)
            Me._organizationName = value
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
