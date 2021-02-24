using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using ChatOnline.Models;

namespace ChatOnline.Hubs
{
    public class MyHub : Hub
    {
        //At first userlist is empty
        //any online client added to this list
        static List<UserInfo> UsersList = new List<UserInfo>();
        static List<Message_> MessageList = new List<Message_>();

        //-->>>>> ***** Receive Request From Client [  Connect  ] *****
        public void Connect(string TokenID)
        {
           
            var id = Context.ConnectionId;

            //Manage Hub Class
            //if freeflag==0 ==> Busy
            //if freeflag==1 ==> Free

            //if tpflag==0 ==> User
            //if tpflag==1 ==> Admin

            ChatOnlineEntities2 db = new ChatOnlineEntities2();

            var stat = (from x in db.User_Access where (x.TokenID==TokenID) select x).FirstOrDefault();
            var info = (from x in db.User_Info where (x.TokenID == TokenID) select x).FirstOrDefault();


            try
            {



                if (stat.Access == false) // Its a user
                {
                   
                    //now we encounter ordinary user which needs userGroup and at this step, 
                    //system assigns the first of free Admin among UsersList
                    var strg = (from s in UsersList
                                where (s.tpflag == "1") && (s.freeflag == "1")
                                select s).First();
                    //Here finds a free admin

                   
                       strg.freeflag = "0";
/*
                    foreach (UserInfo s in UsersList)
                    {
                        if (s.AdminID == strg.AdminID)
                        {
                            s.freeflag = "0";
             
                        }
                    }*/
                    
                    //now add USER to UsersList
                    UsersList.Add(new UserInfo
                    {
                        ConnectionId = id,
                        UserID = info.U_ID,
                        UserName = info.username_,
                        UserGroup =strg.UserGroup ,
                        freeflag = "0",//not free
                        tpflag = "0",// a user
                        AdminID = strg.AdminID
                }

                    
                );


                   
                    //whether it is Admin or User now both of them has userGroup and I Join this user or admin to specific group 
                    Groups.Add(Context.ConnectionId, strg.UserGroup);
                    Clients.Caller.onConnected(id, info.username_, info.U_ID, strg.UserGroup);
                }
                else
                {
                    Random r = new Random();
                    string UsGrp;
                    UsGrp = r.Next().ToString();
                    //If user has admin code so admin code is same userGroup
                    //now add ADMIN to UsersList
                    UsersList.Add(new UserInfo
                    {
                        AdminID = info.U_ID,
                        ConnectionId = id,
                        UserName = info.username_,
                        UserGroup = UsGrp,
                        freeflag = "1",
                        tpflag = "1"
                    });
                    //whether it is Admin or User now both of them has userGroup and I Join this user or admin to specific group 
                    Groups.Add(Context.ConnectionId, UsGrp);
                    Clients.Caller.onConnected(id, info.username_, info.U_ID, UsGrp);
                }
            }

            catch
            {
                string msg = "All Administrators are busy, please be patient and try again";
                //***** Return to Client *****
                Clients.Caller.NoExistAdmin();
            }
        }
        // <<<<<-- ***** Return to Client [  NoExist  ] *****

        //--group ***** Receive Request From Client [  SendMessageToGroup  ] *****
        public void SendMessageToGroup(string TokenID, string message,string receiver)
        {
            
            ChatOnlineEntities2 db = new ChatOnlineEntities2();
            var sender_info = (from x in db.User_Info where (x.TokenID == TokenID) select x).FirstOrDefault();
            string SenderToken;
            string strgroup;
            if (UsersList.Count != 0)
            {
                UserInfo Sender_User = (from s in UsersList where (s.UserID == sender_info.U_ID) || (s.AdminID == sender_info.U_ID)  select s).First();
                if (receiver=="0")
                {
                    SenderToken = TokenID;

                    var tem = (from x in db.User_Info where (x.U_ID == Sender_User.AdminID) select x).FirstOrDefault();
                    
                    receiver =tem.TokenID;
                    var adminFinder = (from x in UsersList where (x.AdminID == tem.U_ID) select x).FirstOrDefault();
                    strgroup = adminFinder.UserGroup.ToString() ;
                   

                    MessageList.Add(new Message_
                    { UserID = sender_info.U_ID, AdminID =tem.U_ID, MSG = message, Time_ =DateTime.Now.Hour.ToString()+':'+DateTime.Now.Minute.ToString() , Date_ =DateTime.Now.Year.ToString() +"/"+ DateTime.Now.Month.ToString() + "/"+ DateTime.Now.Day.ToString(), Type_ =0 });
                }
                else
                {
                    var recieverFinder = (from x in db.User_Info where (x.TokenID== receiver) select x).FirstOrDefault();

                    strgroup = Sender_User.UserGroup;
                    SenderToken = "0";
                    MessageList.Add(new Message_
                    { UserID = recieverFinder.U_ID, AdminID = sender_info.U_ID, MSG = message, Time_ = DateTime.Now.Hour.ToString() + ':' + DateTime.Now.Minute.ToString(), Date_ = DateTime.Now.Year.ToString() + "/" + DateTime.Now.Month.ToString()  +"/" +DateTime.Now.Day.ToString(), Type_ = 1 });

                }




                
               
                Clients.Group(strgroup).getMessages(receiver, message, SenderToken);
            }
        }
      
        public override System.Threading.Tasks.Task OnDisconnected(bool stopCalled)
        {
            ChatOnlineEntities2 db = new ChatOnlineEntities2();

            var item = UsersList.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);
            if (item != null)
            {
                foreach(Message_ m in MessageList)
                {
                    if(m.AdminID==item.AdminID || m.UserID==item.UserID)
                    {
                        Message mes = new Message();
                        mes.UserID = m.UserID;
                        mes.AdminID = m.AdminID;
                        mes.MSG = m.MSG;
                        mes.Time_ = m.Time_;
                        mes.Date_ = m.Date_;
                        mes.Type_ = m.Type_;
                        db.Messages.Add(mes);
                        MessageList.Remove(m);
                        db.SaveChanges();
                    }

                }
               

                UsersList.Remove(item);

                var id = Context.ConnectionId;

                //here we must free the Admin
                if (item.tpflag == "0")

                {
                    //user logged off == user
                    try
                    {
                        var stradmin = (from s in UsersList
                                        where
                         (s.UserGroup == item.UserGroup) && (s.tpflag == "1")
                                        select s).First();
                        //become free
                        UsersList.Remove(stradmin);
                        stradmin.freeflag = "1";
                        Random r = new Random();
                        string UsGrp;
                        UsGrp = r.Next().ToString();
                        stradmin.UserGroup = UsGrp;
                        UsersList.Add(stradmin);

                    }

                    catch
                    {
                        //***** Return to Client *****
                        Clients.Caller.NoExistAdmin();
                    }
                }

              
            }
           
            return base.OnDisconnected( stopCalled);
        }
    }
}