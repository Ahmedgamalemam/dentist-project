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
