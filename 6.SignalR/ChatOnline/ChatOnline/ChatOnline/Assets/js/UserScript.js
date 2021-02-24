function openChatWindow() {
    document.getElementById("ChatWindow").style.display = "block";
}

function closeChatWindow() {
    document.getElementById("ChatWindow").style.display = "none";
}

var MyTokenID = $("#Token").val();



$(function () { //This section will run whenever we call Chat.cshtml page



    var objHub = $.connection.myHub;

    loadClientMethods(objHub);

    $.connection.hub.start().done(function () {

        loadEvents(objHub);

    });
});

function loadEvents(objHub) {

    $("#Chatbtn").click(function () {

        var TokenID = $("#Token").val();

        objHub.server.connect(TokenID);

    });


    $('#btnSendMessage').click(function () {
        var currentdate = new Date(); 
        var msg = $("#txtMessage").val();
        var TokenID = $("#Token").val();
        console.log(currentdate);
        $('#txtMessage').empty();
        $('#txtMessage').val('');
        var AppendHTML = ' <div class="User-chat"><div class=" d-flex flex-row p-3" id="User-chat">' +
            '<div class="DateTime">' +

            + currentdate.getHours() + ":" + currentdate.getMinutes() + "," + currentdate.getFullYear() + "/" + currentdate.getMonth() + "/" + currentdate.getDay() +
             ' </div >' +
            '  <div class="bg-white mr-2 p-3">'
            +
            msg +
            ' </div> <img src="https://img.icons8.com/color/48/000000/circled-user-male-skin-type-7.png" width="30" height="30">' +
            ' </div>' + ' </div> <br/>';
        $('#main').append(AppendHTML);

        if (msg.length > 0) {
                                 objHub.server.sendMessageToGroup(TokenID, msg,0);

        }
    });



    $("#txtMessage").keypress(function (e) {
        if (e.which == 13) {
            $('#btnSendMessage').click();
        }
    });
}

function loadClientMethods(objHub) {

    objHub.client.NoExistAdmin = function () {
        var divNoExist = $('<div class="NoAdmin"><p>متاسفانه در حال حاضر اپراتور آنلاین نیست.</P></div>');
       

        $('#main').prepend(divNoExist);
        $('txtMessage').hide();
        $(divNoExist).fadeIn(900).delay(13000).fadeOut(900);
    }

    objHub.client.getMessages = function (TokenID, message, SenderToken)
    {
        console.log('Some message:');
        console.log(message);
        console.log('message Token:');
        console.log(TokenID);
        console.log('MyTokenID');
        console.log(MyTokenID);
        var stat = (TokenID === MyTokenID);
        console.log('stat:');
        console.log(stat);

        if (stat == true)
        {
            var currentdate = new Date(); 
            var AppendHTML = ' <div class="Admin-chat">' +
                ' <div class="d-flex flex-row p-3">' +
                '<img src="https://img.icons8.com/color/48/000000/circled-user-female-skin-type-7.png" width="30" height="30">' +
                '<div class="chat ml-2 p-3">' +
                message +
                '</div>' +
                ' <div class="DateTime">' +
                + currentdate.getHours() + ":" + currentdate.getMinutes() + "," + currentdate.getFullYear() + "/" + currentdate.getMonth() + "/" + currentdate.getDay() +                '</div>' +
                ' </div>' +
                ' <br />';
            $('#main').append(AppendHTML);
        }
        
       

    }

    objHub.client.onConnected = function (id, userName, UserID, userGroup) {
        console.log('connected on connected');
        console.log('UserGroup:');
        console.log(userGroup);
        $('txtMessage').show();
    }
}