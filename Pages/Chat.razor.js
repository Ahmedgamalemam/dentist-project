//message_text = new Array();
//hubConnection: any;
//  async ngOnInit()
//{
//    this.nav.hide();
//    this.nav.hide_footer();
//    this.hubConnection = new signalR.HubConnectionBuilder()
//        .configureLogging(signalR.LogLevel.Debug)
//        .withUrl('https://localhost:7248/Chat', {
//            skipNegotiation: true,
//            transport: signalR.HttpTransportType.WebSockets,
//        })
//        .build();

//    await this.hubConnection
//        .start()
//        .then(() => console.log('connection started'))
//        .catch((err: any) =>
//            console.log('error while establishing signalr connection: ' + err)
//        );

//    await this.hubConnection.on(
//        'RecieveMessage',
//        (messageFromServer: any, sid: any) => {
//            this.recieveMessage(messageFromServer, sid);
//            this.sortarray();
//            if (this.rid == messageFromServer.sid) {
//                this.sendMessageRecieved(this.sid, 'read', messageFromServer.sid);
//                this.friend_Chat.push(messageFromServer);
//            }
//            else {
//                this.sendMessageRecieved(this.sid, 'recevied', messageFromServer.sid);
//            }
//        }
//    );

//    await this.hubConnection.on(
//        'Recieved',
//        (messagestatus: string, sid: any) => {
//            this.messageRecieved(messagestatus, sid);
//        }
//    );

//    await this.hubConnection.on(
//        'RecieveMessageReact',
//        (Msg: any, react: any, sid: any) => {
//            this.users.forEach((element: any) => {
//                if (element.id == sid) {
//                    element.countUnReadMs = 0;
//                    this.messageRecievedUpdate(element, Msg);
//                }
//            });
//        }
//    );

//    await this.hubConnection.on('typing', (messagestatus: string, sid: any) => {
//        this.users.forEach((element: any) => {
//            if (element.id == sid) {
//                element.typing = messagestatus;
//            }
//        });
//    });

//    await this.users.forEach((element: any) => {
//        this.sendMessageRecieved(this.sid, 'recevied', element.id);
//    });

//    await this.hubConnection.on(
//        'SendMessagetoChat',
//        (messageFromServer: any) => {
//            this.sendMessagetoChat(messageFromServer);
//        }
//    );

//    await this.hubConnection.on('Seen', (seenMessage: any, sid: any) => {
//        this.lastSeen(seenMessage, sid);
//    });

//    await this.hubConnection.on('LastSeen', (seenMessage: any, sid: any) => {
//        this.lastSeen(seenMessage, sid);
//    });

//    await this.hubConnection.on('Deleted', (Message: any, sid: any) => {
//        this.users.forEach((element: any) => {
//            if (element.id == sid) {
//                this.messageRecievedUpdate(element, Message);
//            }
//        });
//    });

//    await this.hubConnection
//        .invoke('SendId', this.sid)
//        .then((r: any) => { })
//        .catch((err: any) =>
//            console.log('error while establishing signalr connection: ' + err)
//        );
//}































//handelTime() {
//    const d = new Date();
//    let Hours = d.getHours();
//    let minutes = d.getMinutes().toString();
//    if (parseInt(minutes) <= 9) {
//        minutes = `0${minutes}`;
//    }
//    let time = '';
//    if (Hours == 0o0) {
//        Hours = 12;
//        time = `${Hours}:${minutes} am`;
//    } else if (Hours > 12) {
//        Hours -= 12;
//        time = `${Hours}:${minutes} pm`;
//    } else {
//        time = `${Hours}:${minutes} pm`;
//    }
//    return time;
//}

//show_delete_message(message: any) {
//    this.delete_message = message;
//    let active_delete = document.querySelectorAll('.active_delete');
//    active_delete.forEach((element: any) => {
//        element.classList.remove('active_delete');
//    });
//}


//date(date: any) {
//    let oldDate = new Date(date);
//    let newDate = new Date(this.fullDate);

//    if (
//        newDate.getFullYear() != oldDate.getFullYear() &&
//        newDate.getMonth() == 0 &&
//        oldDate.getMonth() == 11
//    ) {
//        if (oldDate.getDate() - newDate.getDate() <= 23) {
//            return 1;
//        }
//    } else if (newDate.getFullYear() == oldDate.getFullYear()) {
//        if (
//            newDate.getMonth() == oldDate.getMonth() &&
//            newDate.getDate() - oldDate.getDate() >= 7
//        ) {
//            return 1;
//        } else if (newDate.getMonth() != oldDate.getMonth()) {
//            if (
//                (newDate.getMonth() - oldDate.getMonth() == 1 &&
//                    oldDate.getDate() - newDate.getDate() <= 23) ||
//                newDate.getMonth() - oldDate.getMonth() >= 2
//            ) {
//                return 1;
//            }
//        }
//    }
//    return 2;
//}

//getday(date: any) {
//    let newDate = new Date(date);
//    return newDate;
//}


//hideDelete_Update(e: any) {
//    let divToHide = document.getElementById('update_and_delete');
//    if (divToHide == e.target) {
//        this.update_message = null;
//        this.delete_message = null;
//    }
//}