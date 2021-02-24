function openChatWindow() {
    document.getElementById("ChatWindow").style.display = "block";
};

function closeChatWindow() {
    document.getElementById("ChatWindow").style.display = "none";
};
var objHub = $.connection.myHub;
var currentUser = 0;
var MyTokenID = $("#MyToken").val();
var bell = '<i class="fa fa-bell"> </i>';
var pastUser;
var connectUser = "0";
var LostHTML = ' ';
$(function () {

       
    loadClientMethods(objHub);
    console.log('connected');
    $.connection.hub.start().done(function ()
    {

        loadEvents(objHub);

    });
});



$('#btnSendMessage').click(function () {

    var currentdate = new Date(); 
 
    var msg = $("#txtMessage").val();

    if (msg.length > 0) {
        var AppendHTML = '<div class="outgoing_msg">' +
            '<div class="sent_msg">' +
            '<p>' +
            msg +
            ' </p>' +
            '<span class="time_date"> ' + currentdate.getHours() + ":" + currentdate.getMinutes() + "," + currentdate.getFullYear() + "/" + currentdate.getMonth() + "/" + currentdate.getDay() + '</span>' +
            ' </div >' +
            ' </div >';
        $('#msg_').append(AppendHTML);
        $("#txtMessage").val('');
        console.log('currentUser:')
        console.log(currentUser);

        // <<<<<-- ***** Return to Server [  SendMessageToGroup  ] *****
        objHub.server.sendMessageToGroup(MyTokenID, msg, currentUser);

    }
});

$("#txtMessage").keypress(function (e) {
    console.log('press');

    if (e.which == 13) {
        $('#btnSendMessage').click();
    }
});


function ondf(TokenID) {
    currentUser = TokenID;
    var idRecognizer = "#N" + TokenID;
    console.log('idRecognizer:');
    console.log(idRecognizer);
    var name1 = $(idRecognizer).val();
    var showName = '<div class="nameUser">' + name1 + '</div>  <hr>';
    
    console.log('name:');
    console.log(name1);
    var temp = '#' + currentUser;
   /* $(temp).removeProp(bell);*/
    $('active_chat').css('background-color', 'green');
    localStorage.setItem('isCliked', true);
    $('#msg_').empty();
    $('#msg_').append(showName);
    var Token = TokenID;
    $(document).ready(function () {
        $.post('/HomePanel/GetMessages', { TokenID: Token },
            function (data, status) {
                if (status == "success") {
                    if (data != "") {
                        var MessageList = JSON.parse(data);
                      
                        $.each(MessageList, function (index, value) {

                            var group = value['Type_'];

                            if (group == 0) {

                                var AppendHTML = '<div class="incoming_msg">' +
                                    '<div class="incoming_msg_img">' + '<img src="https://ptetutorials.com/images/user-profile.png" > </div>' +
                                    '  <div class="received_msg">' +
                                    '   <div class="received_withd_msg">' +
                                    ' <p>' +
                                    value['MSG'] +
                                    '</p>' +
                                    '<span class="time_date">' + value['Time_'] + ',' + value['Date_'] + '</span>' +
                                    ' </div>' +
                                    ' </div>' +
                                    ' </div >';

                                $('#msg_').append(AppendHTML);

                            }
                            else {
                                var AppendHTML = '<div class="outgoing_msg">' +
                                    '<div class="sent_msg">' +
                                    '<p>' +
                                    value['MSG'] +
                                    ' </p>' +
                                    '<span class="time_date"> ' + value['Time_'] + ',' + value['Date_'] + '</span>' +
                                    ' </div >' +
                                    ' </div >';


                                $('#msg_').append(AppendHTML);
                            }
                            
                        });
                        console.log("LostHTML:");
                        console.log(LostHTML);
                        $('#msg_').append(LostHTML);
                        LostHTML = "";
                       
                    }
                   
                }
          
            }

        );
       
    });
    pastUser = currentUser;
};


function loadEvents(objHub) {

    $("#Chatbtn").click(function () {
        var MyTokenID = $("#MyToken").val();

        objHub.server.connect(MyTokenID);




    });
}


function loadClientMethods(objHub) {
    console.log('happend');

    

    objHub.client.getMessages = function (TokenID, message, SenderToken) {
        var currentdate = new Date();
        console.log('some answer');
        console.log(message);
        if (TokenID === MyTokenID) {
            console.log('MessageTokenID');
            console.log(TokenID);
            if (currentUser != pastUser)
            {
                var temp = '#' + pastUser;
                $(temp).css("background-color", "#ebebeb");

            }
            if (SenderToken === currentUser) {

                console.log('same Token');
                var AppendHTML = '<div class="incoming_msg">' +
                    '<div class="incoming_msg_img">' + '<img src="https://ptetutorials.com/images/user-profile.png" > </div>' +
                    '  <div class="received_msg">' +
                    '   <div class="received_withd_msg">' +
                    ' <p>' +
                    message +
                    '</p>' +
                    '<span class="time_date"> ' + currentdate.getHours() + ":" + currentdate.getMinutes() + "," + currentdate.getFullYear() + "/" + currentdate.getMonth() + "/" + currentdate.getDay() + '</span>' +

                    ' </div>' +
                    ' </div>' +
                    ' </div >';
                $('#msg_').append(AppendHTML);
            }

            else
            {
                console.log('not same');
                var FinderID = '#' + SenderToken;
                $(FinderID).css("background-color", "#ffe3de");
                $(FinderID).click(function ()
                {
                   
                    console.log('clicked');
                    
                    LostHTML = '<div class="incoming_msg">' +
                        '<div class="incoming_msg_img">' + '<img src="https://ptetutorials.com/images/user-profile.png" > </div>' +
                        '  <div class="received_msg">' +
                        '   <div class="received_withd_msg">' +
                        ' <p>' +
                        message +
                        '</p>' +
                        '<span class="time_date"> ' + currentdate.getHours() + ":" + currentdate.getMinutes() + "," + currentdate.getFullYear() + "/" + currentdate.getMonth() + "/" + currentdate.getDay() + '</span>' +

                        ' </div>' +
                        ' </div>' +
                        ' </div >';
                   
                    currentUser = SenderToken;


                });

            }
        }
    }

    objHub.client.onConnected = function (id, userName, UserID, userGroup) {

        console.log('connected on connected');
        console.log('UserGroup:');
        console.log(userGroup);
    }
}





