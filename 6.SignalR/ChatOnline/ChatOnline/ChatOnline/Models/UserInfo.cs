using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class UserInfo
{
    public string UserName { get; set; }
    public string ConnectionId { get; set; }
    public string UserGroup { get; set; }

    //if freeflag==0 ==> Busy
    //if freeflag==1 ==> Free
    public string freeflag { get; set; }  // Is busy or not 
      
    //if tpflag==0 ==> User Member
    //if tpflag==1 ==> Admin
    public string tpflag { get; set; } //Type Flag

    public int UserID { get; set; }
    public int AdminID { get; set; }
}