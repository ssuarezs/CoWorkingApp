using System;
using System.Collections.Generic;
using System.Text;

namespace App.Enumerations
{
    enum UserRole
    {
        Admin = 1,
        User = 2
    }
    enum MenuAdmin
    {
        ManageDesk = 1,
        ManageUsers = 2
    }
    enum AdminDesk
    {
        Create = 1,
        Edit = 2,
        Delete = 3,
        Block = 4
    }
    enum AdminUser
    {
        Create = 1,
        Edit = 2,
        Delete = 3,
        ChangePassword = 4
    }
    enum MenuUser
    {
        Reserve = 1,
        Cancel = 2,
        History = 3,
        ChangePassword = 4
    }
}
