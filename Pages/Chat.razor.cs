using Azure;
using dentist_model;
using dentist_project.Serivce;
using Microsoft.AspNetCore.Components;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Reflection.Metadata;
using System.Xml.Linq;
using System;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components.Forms;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace dentist_project.Pages
{
    public partial class Chat
    {
        [Inject]
        public IChatTraditional chattraditional { get; set; }
        [Inject]
        public IJSRuntime Runtime { get; set; }
        public string inputvalue { get; set; }
        public dentist_model.Chat delete_message { get; set; }
        public dentist_model.Chat update_message { get; set; }
        public string new_Message { get; set; }
        public int MyProperty { get; set; } = 0;
        public bool chat { get; set; }
        public bool toggle { get; set; }
        public List<UserDtos> users { get; set; } = new List<UserDtos>();
        public UserDtos friend { get; set; }
        public List<dentist_model.Chat> friend_Chat { get; set; }
        public int messagesUnRead { get; set; } = 0;
        public int unreadmessage { get; set; } = 0;
        public string sid { get; set; } = "";
        public string rid { get; set; } = "";
        public string typing { get; set; }
        public DateTime fullDate { get; set; } = new DateTime();
        public string[] weekDays { get; set; } = new string[7] { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" };

        protected override void OnInitialized()
        {
            base.OnInitialized();
        }
        protected override async void OnAfterRender(bool firstRender)
        {
            var users = await chattraditional.GetallMessage(sid);
            sid = await localStorage.GetItemAsStringAsync("id");
            foreach (var user in users)
            {
                foreach (var message in user.Chat)
                {
                    if (message.Read != 2 && message.Rid == sid)
                    {
                        user.CountUnReadMs++;
                        unreadmessage = user.CountUnReadMs;
                    }
                }

                users.Add(user);
            }
            sortarray();

            StateHasChanged();
            base.OnAfterRender(firstRender);

        }


        //        sortarray()
        //        {
        //            this.users.sort(function(u1, u2) {
        //                let user1_date = u1.chat[u1.chat.length - 1].date.replaceAll('-', '');
        //                let user2_date = u2.chat[u2.chat.length - 1].date.replaceAll('-', '');
        //                // let user1_time = u1.chat[u1.chat.length - 1].time.replaceAll(':', '');
        //                // let user2_time = u2.chat[u2.chat.length - 1].time.replaceAll(':', '');
        //                let u1_hrs;
        //                let u2_hrs;
        //                let u1_mins;
        //                let u2_mins;
        //                var user1_time = u1.chat[u1.chat.length - 1].time
        //                    .split(' ')[1]
        //                    .split(':')[0];
        //                u1_hrs = u1.chat[u1.chat.length - 1].time.split(':')[0];
        //                if (user1_time == 'pm')
        //                {
        //                    if (u1_hrs != '12')
        //                    {
        //                        u1_hrs = parseInt(u1_hrs) + 12;
        //                    }
        //                }
        //                else
        //                {
        //                    u1_hrs = parseInt(u1_hrs);
        //                }
        //                u1_mins = u1.chat[u1.chat.length - 1].time.split(':')[1].split(' ')[0];
        //                u1_mins = parseInt(u1_mins);
        //                console.log(u1_mins);

        //                var user2_time = u2.chat[u2.chat.length - 1].time
        //                    .split(' ')[1]
        //                    .split(':')[0];
        //                u2_hrs = u2.chat[u2.chat.length - 1].time.split(':')[0];
        //                if (user2_time == 'pm')
        //                {
        //                    if (u2_hrs != '12')
        //                    {
        //                        u2_hrs = parseInt(u2_hrs) + 12;
        //                    }
        //                }
        //                else
        //                {
        //                    u2_hrs = parseInt(u2_hrs);
        //                }
        //                u2_mins = u2.chat[u2.chat.length - 1].time.split(':')[1].split(' ')[0];
        //                u2_mins = parseInt(u2_mins);
        //                console.log(u2_mins);

        //                user1_date = parseInt(user1_date);
        //                user2_date = parseInt(user2_date);
        //                user1_time = parseInt(user1_time);
        //                user2_time = parseInt(user2_time);
        //                if (user1_date > user2_date)
        //                {
        //                    return -1;
        //                }
        //                if (user1_date < user2_date)
        //                {
        //                    return 1;
        //                }
        //                if (user1_date == user2_date)
        //                {
        //                    if (u1_hrs > u2_hrs)
        //                    {
        //                        console.log(1);

        //                        return -1;
        //                    }
        //                    if (u1_hrs < u2_hrs)
        //                    {
        //                        console.log(2);

        //                        return 1;
        //                    }
        //                    if (u1_hrs == u2_hrs)
        //                    {
        //                        if (u1_mins > u2_mins)
        //                        {
        //                            console.log(3);

        //                            return -1;
        //                        }
        //                        if (u1_mins < u2_mins)
        //                        {
        //                            console.log(4);

        //                            return 1;
        //                        }
        //                        if (u1_mins == u2_mins)
        //                        {
        //                            console.log(5);

        //                            return -1;
        //                        }
        //                    }
        //                }
        //                return 0;
        //            });
        //        }
        //        lastSeen(seenMessage: any, sid: any)
        //        {
        //            this.users.forEach((element: any) => {
        //                if (element.id == sid)
        //                {
        //                    if (seenMessage != 'Active Now')
        //                    {
        //                        element.seen = seenMessage;
        //                    }
        //                    else
        //                    {
        //                        element.seen = seenMessage;
        //                    }
        //                }
        //            });
        //        }
        //        recieveMessage(message: any, sid: any)
        //        {
        //            this.users.forEach((element: any) => {
        //                if (element.id == sid)
        //                {
        //                    element.chat.push(message);
        //                    if (message.sid != this.rid)
        //                    {
        //                        element.countUnReadMs++;
        //                        this.unreadmessage = element.countUnReadMs;
        //                    }
        //                    else
        //                    {
        //                        message.read = 2;
        //                    }
        //                }
        //            });
        //        }
        //        sendMessageRecieved(sid: any, messagestatus: string, rid: any)
        //        {
        //            this.hubConnection
        //                .invoke('MessageRecieved', sid, messagestatus, rid)
        //                .then((r: any) => {
        //                this.scroll();
        //            })
        //        .catch((err: any) => { });
        //        }
        //        messageRecieved(messagestatus: string, sid: any)
        //        {
        //            this.users.forEach((element: any) => {
        //                if (element.id == sid)
        //                {
        //                    this.messageRecievedUpdate(element, messagestatus);
        //                }
        //            });
        //        }
        //        messageRecievedUpdate(element: any, messagestatus: any)
        //        {
        //            element.chat.forEach((ch: any) => {
        //                if (messagestatus == 'recevied' && ch.sid == this.sid && ch.read == 0)
        //                {
        //                    ch.read = 1;
        //                    this.chatTrad.updateMessage(ch).subscribe((response: any) => { });
        //                }
        //                else if (messagestatus == 'read' && ch.sid == this.sid)
        //                {
        //                    ch.read = 2;
        //                    this.chatTrad.updateMessage(ch).subscribe((response: any) => { });
        //                }
        //                else if (ch.id == messagestatus.id)
        //                {
        //                    ch.sidMsgEmojie = messagestatus.sidMsgEmojie;
        //                    ch.ridMsgEmojie = messagestatus.ridMsgEmojie;
        //                    ch.sidDelete = messagestatus.sidDelete;
        //                    ch.ridDelete = messagestatus.ridDelete;
        //                    ch.txt = messagestatus.txt;
        //                }
        //            });
        //        }
        //        async sendMessagetoChat(message: any)
        //        {
        //            await this.users.forEach((element: any) => {
        //                if (element.id == message.rid)
        //                {
        //                    element.chat.push(message);
        //                    this.friend_Chat.push(message);
        //                }
        //            });
        //            await this.sortarray();
        //        }
        //        filteruser: any = this.users;
        //searchFilter(event: Event) {
        //            this.users = this.filteruser;
        //            let inputsearch = (event.target as HTMLInputElement).value;
        //        console.log(inputsearch);
        //    let spanOf_lname_Or_fname = document.querySelectorAll('span');

        //        spanOf_lname_Or_fname.forEach((element: any) => {
        //        if (element.innerText.toLowerCase().includes(inputsearch.toLowerCase())) {
        //            element.classList.remove('back_purple_dark');
        //            element.classList.remove('back_green_dark');
        //            element.classList.remove('back_yellow_dark');
        //        }
        //});
        //let filter = this.users.filter(
        //    (u: any) =>

        //        u.fname.toLowerCase().includes(inputsearch.toLowerCase()) ||
        //        u.lname.toLowerCase().includes(inputsearch.toLowerCase())
        //);
        //this.users = filter;
        //spanOf_lname_Or_fname.forEach((element: any) => {
        //    if (
        //        element.innerText.toLowerCase().includes(inputsearch.toLowerCase()) &&
        //        inputsearch != '' &&
        //        inputsearch != null
        //    )
        //    {
        //        console.log('color');
        //        if (this.theme_color == 'purple')
        //        {
        //            element.classList.add('back_purple_dark');
        //        }
        //        else if (this.theme_color == 'green')
        //        {
        //            element.classList.add('back_green_dark');
        //        }
        //        else
        //        {
        //            element.classList.add('back_yellow_dark');
        //        }
        //    }
        //});
        //}
        //toggle: boolean = false;
        //buttonfilter() {
        //    this.users = this.filteruser;
        //    let buttonfilter = document.getElementById('buttonfilter');
        //    if (this.toggle == false)
        //    {
        //        let filter = this.users.filter((u) => u.countUnReadMs > 0);
        //        this.users = filter;
        //        if (this.theme_color == 'purple')
        //        {
        //            buttonfilter?.classList.add('back_purple_dark');
        //        }
        //        else if (this.theme_color == 'green')
        //        {
        //            buttonfilter?.classList.add('back_green_dark');
        //        }
        //        else
        //        {
        //            buttonfilter?.classList.add('back_yellow_dark');
        //        }
        //        this.toggle = true;
        //    }
        //    else
        //    {
        //        let filter = this.users.filter((u) => u.countUnReadMs >= 0);
        //        buttonfilter?.classList.remove('back_purple_dark');
        //        buttonfilter?.classList.remove('back_green_dark');
        //        buttonfilter?.classList.remove('back_yellow_dark');
        //        this.users = filter;
        //        this.toggle = false;
        //    }
        //}
        //foreach_on_message(friend: any) {
        //    let filter_Friend = friend.chat.filter(
        //        (u: any) =>
        //            (u.rid == this.sid && u.ridDelete < 2 && u.sidDelete < 2) ||
        //            u.sidDelete < 2
        //    );
        //    this.friend_Chat = filter_Friend;
        //    this.friend = friend;
        //    this.rid = friend.id;
        //}
        //async message_friend(friend: any) {
        //    this.chat = true;

        //    if (this.rid != '' && friend.id != this.rid)
        //    {
        //        this.unreadmessage = 0;
        //    }

        //    await this.foreach_on_message(friend);

        //    let user = this.users.filter((u) => u.id == friend.id);
        //    if (user[0].countUnReadMs != 0)
        //    {
        //        if (user[0].chat[user[0].chat.length - 1].rid == this.sid)
        //        {
        //            user[0].countUnReadMs = 0;
        //        }
        //    }
        //    await this.sendMessageRecieved(this.sid, 'read', this.rid);
        //}
        //scroll() {
        //    if (this.friend_Chat.length == this.friend.chat.length)
        //    {
        //        console.log(1235466);
        //        document
        //            .getElementById((this.friend_Chat.length - 1).toString())!
        //            .scrollIntoView();
        //    }
        //}
        
        public void delete_from_all()
        {
            delete_message.SidDelete = 1;
            delete_message.RidDelete = 1;
            updated_database(delete_message);
            foreach (var m in friend.Chat)
            {
                if(m.Sid == sid && delete_message.Id.ToString() == m.SidMsgEmojie)
                {
                    m.SidDelete = 5;
                    updated_database(m);
                }
            }
           
            delete_message = null;

        }


        public void updated_database(dentist_model.Chat message)
        {
            chattraditional.UpdateUser(message);
          //send to hub
        }


        public void delete_from_me()
        {
            if (delete_message.Sid == sid)
            {
                delete_message.SidDelete = 1;
            }
            else
            {
                delete_message.RidDelete = 1;
            }
            foreach(var m in friend.Chat)
            {
                if (m.Sid == sid && delete_message.Id.ToString() == m.SidMsgEmojie)
                {
                    m.SidDelete = 5;
                    updated_database(m);
                }
            }
            
            delete_message = null;
        }


        public void delete()
        {
            if (delete_message.Sid == sid)
            {
                delete_message.SidDelete = 2;
            }
            else
            {
                delete_message.RidDelete = 2;
            }

            updated_database(delete_message);
            delete_message = null;
        }


        public void cancel()
        {
            delete_message = null;
            update_message = null;
        }


        public void showEmojie_In_updateMsg(){}


        public async void newmessage()
        {
            var input_text = await Runtime.InvokeAsync<string>("getbyidvalue", "input");
            new_Message = input_text;
        }


        public void show_update_message(dentist_model.Chat message)
        {
            update_message = message;
            Runtime.InvokeVoidAsync("queryselectall");
        }


        public async void updatemessages()
        {
            var input_text = await Runtime.InvokeAsync<string>("getbyidvalue", "input");
            update_message.Txt = input_text;
            updated_database(update_message);
            sendmessage($"updated{update_message.Txt}", update_message);

            this.new_Message = null;
            this.update_message = null;
        }


        public void sendIsTyping()
        {
           // this.hubConnection
           //.invoke('MessageRecieved', this.sid, message, this.rid)
           //.then((r: any) => { })
           // .catch((err: any) =>
           // console.log('error while establishing signalr connection: ' + err));
        }


        public async void sendmessage(string message = "",dentist_model.Chat obj = null)
        {
            var fullDate = DateTime.Now.ToString("MM / dd / yyyy");
            var input_text = await Runtime.InvokeAsync<string>("getbyidvalue", "input");
            if (message == "")
            {
                //var input = await jSRuntime.InvokeAsync<IJSObjectReference>("getbyid", "input");
                dentist_model.Chat chat = new dentist_model.Chat()
                {
                    Date = fullDate,
                    Time = handeltime(),
                    Rid = rid,
                    Sid = sid,
                    Read = 0,
                    SidDelete = 0,
                    RidDelete = 0,
                    Txt = input_text,
                };

                //send to hub
                //input?.replaceChildren();
                inputvalue = "";
            }
            else
            {
                dentist_model.Chat chat = new dentist_model.Chat()
                {
                    Date = fullDate,
                    Time = handeltime(),
                    Rid = rid,
                    Sid = sid,
                    Read = 0,
                    SidDelete = 6,
                    RidDelete = 0,
                    Txt = input_text,
                };

                //send to hub
            }
        }


        public void message_friend(UserDtos user)
        {

        }
        public void searchFilter(EventArgs e)
        {

        }
        public void buttonfilter()
        {

        }


        public async void openside()
        {
            await Runtime.InvokeVoidAsync("openside");
        }


        public async void closeside()
        {
            await Runtime.InvokeVoidAsync("closeside");
        }


        public void active_delete(EventArgs e)
        {

        }


        public async void hideDelete_Update(EventArgs e)
        {
            await Runtime.InvokeVoidAsync("hideDelete_Update",e);
        }
        public bool showDate(int i)
        {
            if (i != 0)
            {
                if (friend_Chat[i].Date == friend_Chat[i - 1].Date)
                {
                    return false;
                }
            }
            return true;
        }
        public int date(string date)
        {
            return 1;
        }
        public int getday(string date)
        {
            return 0;
        }
        public void getDay()
        {

        }
        public bool showUnreadMessageElement(int i)
        {
            if (i != 0)
            {
                if (friend_Chat[i].Read != 2 && friend_Chat[i - 1].Read == 2)
                {
                    return true;
                }
            }

            return false;
        }
        public void sortarray()
        {

        }
        public string handeltime()
        {
            return " ";
        }
    }
}
