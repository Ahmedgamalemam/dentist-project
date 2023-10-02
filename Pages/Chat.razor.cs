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
using System.Security.Cryptography;
using dentist_api.Model;
using static Azure.Core.HttpHeader;
using Microsoft.AspNetCore.SignalR.Client;

namespace dentist_project.Pages
{
    public partial class Chat
    {
        /// <summary>
        /// /////// class property /////////////////
        ///
        public string Sidparent { get; set; } = "";
        public string msg_emojie_container_sid { get; set; } = "";
        public string MyProperty1 { get; set; }
        public string sidmessage { get; set; } = "";
        public string left { get; set; } = "";
        /// </summary>


        [Inject]
        public IChatTraditional chattraditional { get; set; }
        [Inject]
        public IJSRuntime Runtime { get; set; }
        public HubConnection hubConnection { get; set; }
        public string searchTerm { get; set; } = "";
        public bool toggle { get; set; } = false;
        public string inputvalue { get; set; }
        public dentist_model.Chat delete_message { get; set; }
        public dentist_model.Chat update_message { get; set; }
        public string new_Message { get; set; }
        public int MyProperty { get; set; } = 0;
        public bool chat { get; set; } = false;
        public List<UserDtos> users { get; set; } = new List<UserDtos>();
        public List<UserDtos> filteruser { get; set; }
        public UserDtos friend { get; set; }
        public List<dentist_model.Chat> friend_Chat { get; set; } = new List<dentist_model.Chat>();
        public int messagesUnRead { get; set; } = 0;
        public int unreadmessage { get; set; } = 0;
        public string sid { get; set; } = "";
        public string rid { get; set; } = "";
        public string typing { get; set; }
        public DateTime fullDate { get; set; } = new DateTime();
        public string[] weekDays { get; set; } = new string[7] { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" };



        protected override async Task OnInitializedAsync()
        {
            sid = await localStorage.GetItemAsync<string>("id");

            hubConnection = new HubConnectionBuilder()
                .WithUrl("https://localhost:7008/chat")
                .Build();

            await hubConnection.StartAsync();

            hubConnection.On<dentist_model.Chat, string>("ReceiveMessage", (messageFromServer, sid) =>
            {
                recieveMessage(messageFromServer, sid);
                sortarray();
                if (rid == messageFromServer.Sid)
                {
                    this.sendMessageRecieved(this.sid, "read", messageFromServer.Sid);
                    friend_Chat.Add(messageFromServer);
                }
                else
                {
                    this.sendMessageRecieved(this.sid, "recevied", messageFromServer.Sid);
                }
                StateHasChanged();
            });

            hubConnection.On<string, string>("Recieved", (messagestatus, sid) =>
            {
                messageRecieved(messagestatus, sid);
                StateHasChanged();
            });

            //hubConnection.On<string, string>("typing", (messagestatus, sid) =>
            //{
            //    foreach (var user in users)
            //    {
            //        if (user.Id == sid)
            //        {
            //            user.Typing = messagestatus;
            //        }
            //    }
            //    StateHasChanged();
            //});

            foreach (var user in users)
            {
                sendMessageRecieved(sid, "recevied", user.Id);
            }

            hubConnection.On<dentist_model.Chat>("SendMessagetoChat", (messageFromServer) =>
            {
                sendMessagetoChat(messageFromServer);
                StateHasChanged();
            });

            //hubConnection.On<string, string>("Seen", (seenMessage, sid) =>
            //{
            //    lastSeen(seenMessage, sid);
            //    StateHasChanged();
            //}); hubConnection.On<string, string>("LastSeen", (seenMessage, sid) =>
            //{
            //    lastSeen(seenMessage, sid);
            //    StateHasChanged();
            //});

            hubConnection.On<string, string>("Deleted", (Message, sid) =>
            {
                foreach (var user in users)
                {
                    if (user.Id == sid)
                    {
                        messageRecievedUpdate(user, Message);
                    }
                }
                StateHasChanged();
            });

            await hubConnection.SendAsync("SendId", sid);
        }



        protected override async void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                await Runtime.InvokeVoidAsync("hidenave");


                var users = await chattraditional.GetallMessage(sid);

                if (users != null)
                {
                    foreach (var user in users)
                    {
                        if (user.Chat != null)
                        {
                            foreach (var message in user.Chat)
                            {
                                if (message.Read != 2 && message.Rid == sid)
                                {
                                    user.CountUnReadMs++;
                                    unreadmessage = user.CountUnReadMs;
                                }
                            }
                        }

                        this.users.Add(user);
                    }
                }
                //if (this.users.Count > 1)
                //{
                //    sortarray();
                //}

                StateHasChanged();
            }
           
            base.OnAfterRender(firstRender);

        }

        public async void scroll()
        {
            if (friend_Chat.Count == friend.Chat.Count)
            {
                await Runtime.InvokeAsync<string>("scroll", (this.friend_Chat.Count - 1).ToString());
            }
        }


        private List<UserDtos> filteredProducts => users
            .Where(user => user.Fname.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                   user.Lname.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
            .ToList();

        private string HighlightSearchTerm(string productName, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return productName;
            }

            var words = productName.Split(' ');

            var highlightedWords = new List<string>();

            foreach (var word in words)
            {
                if (word.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                {
                    var highlightedWord = word.Replace(searchTerm, $"<span style='background-color: purple;color:white;'>{searchTerm}</span>");
                    highlightedWords.Add(highlightedWord);
                }
                else
                {
                    highlightedWords.Add(word);
                }
            }

            return string.Join(" ", highlightedWords);
        }


        public async void buttonfilter()
        {
            users = filteruser;
            List<UserDtos> filteredUsers = new List<UserDtos>();
            foreach (var user in users)
            {
                if (user.CountUnReadMs > 0)
                {
                    filteredUsers.Add(user);
                }
            }
            users = filteredUsers;
            if (toggle == false)
            {
                await Runtime.InvokeAsync<string>("filteredbutton", "buttonfilter");
            }
        }



        public void lastSeen(string seenMessage, string sid)
        {
            foreach (var user in users)
            {
                if (user.Id == sid)
                {
                    user.Seen = seenMessage;
                }
            }
        }


        public async void sendMessageRecieved(string sid, string messagestatus, string rid)
        {
            await hubConnection.SendAsync("MessageRecieved", messagestatus, sid);
        }


        public void sendMessagetoChat(dentist_model.Chat message)
        {
            foreach (var user in users)
            {
                if (user.Id == message.Rid)
                {
                    user.Chat.Add(message);
                    friend_Chat.Add(message);
                }
            }
            //if (this.users.Count > 1)
            //{
            //    sortarray();
            //}
        }


        public void messageRecieved(string messagestatus, string sid)
        {
            foreach (var user in users)
            {
                if (user.Id == sid)
                {
                    messageRecievedUpdate(user, messagestatus);

                }
            }
        }
        public void messageRecievedUpdate(UserDtos element, string messagestatus)
        {
            foreach (var message in element.Chat)
            {
                if (messagestatus == "recevied" && message.Sid == sid && message.Read == 0)
                {
                    message.Read = 1;
                    chattraditional.UpdateUser(message);
                }
                else if (messagestatus == "read" && message.Sid == sid)
                {
                    message.Read = 2;
                    chattraditional.UpdateUser(message);
                }
                ////////////////////Erorr/////////////////
                ///
                //else if (message.Id == messagestatus.id)
                //{
                //    //message.SidDelete = messagestatus.sidDelete;
                //    //message.RidDelete = messagestatus.ridDelete;
                //    //message.Txt = messagestatus.txt;
                //}
            }
        }



        public void recieveMessage(dentist_model.Chat message, string sid)
        {
            foreach (var user in users)
            {
                if (user.Id == sid)
                {
                    user.Chat.Add(message);
                    if (message.Sid != rid)
                    {
                        user.CountUnReadMs++;
                        unreadmessage = user.CountUnReadMs;
                    }
                    else
                    {
                        message.Read = 2;
                    }
                }
            }
        }

        public void foreach_on_message(UserDtos friend)
        {
            foreach (var message in friend.Chat)
            {
                if ((message.Rid == this.sid && message.RidDelete < 2 && message.SidDelete < 2) || message.SidDelete < 2)
                {
                    this.friend_Chat.Add(message);
                }
            }
        }
        public void filter_UnRead_message(UserDtos friend)
        {
            foreach (var user in users)
            {
                if (user.Id == friend.Id && user.CountUnReadMs != 0 && user.Chat[user.Chat.Count - 1].Rid == sid)
                {
                    user.CountUnReadMs = 0;
                }
            }
        }
        public void message_friend(UserDtos friend)
        {
            this.friend = friend;
            this.rid = friend.Id;
            this.chat = true;

            if (rid != "" && friend.Id != rid)
            {
                unreadmessage = 0;
            }

            foreach_on_message(friend);
            filter_UnRead_message(friend);

            sendMessageRecieved(sid, "read", rid);
        }


        /// <summary>
        /// //////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        public void delete_from_all()
        {
            delete_message.SidDelete = 1;
            delete_message.RidDelete = 1;
            updated_database(delete_message);
            foreach (var m in friend.Chat)
            {
                if (m.Sid == sid && delete_message.Id.ToString() == m.SidMsgEmojie)
                {
                    m.SidDelete = 5;
                    updated_database(m);
                }
            }

            delete_message = null;

        }


        public async void updated_database(dentist_model.Chat message)
        {
            chattraditional.UpdateUser(message);

            await hubConnection.SendAsync("DeleteMessage", message, sid, rid);
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
            foreach (var m in friend.Chat)
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


        public void showEmojie_In_updateMsg() { }


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


        public void show_delete_message(dentist_model.Chat message)
        {
            delete_message = message;
            Runtime.InvokeVoidAsync("queryselectall");
        }
        public async void updatemessages()
        {
            var input_text = await Runtime.InvokeAsync<string>("getbyidvalue", "input");
            update_message.Txt = input_text;
            updated_database(update_message);
            sendmessage($"updated{update_message.Txt}", update_message);

            new_Message = null;
            update_message = null;
        }


        public async void sendIsTyping(char message)
        {
            if (message == 't')
            {
                await hubConnection.SendAsync("MessageRecieved", sid, "is typing...", rid);
            }
            else
            {
                await hubConnection.SendAsync("MessageRecieved", sid, "", rid);
            }
        }


        public async void sendmessage(string message = "", dentist_model.Chat obj = null)
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
                var response = chattraditional.AddMessage(chat);
                await hubConnection.SendAsync("SendMessage", response, sid, rid);

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
                    Txt = message,
                };
                var response = chattraditional.AddMessage(chat);
                await hubConnection.SendAsync("SendMessage", response, sid, rid);
            }
        }



        public void searchFilter(EventArgs e)
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


        public async void active_delete(EventArgs e)
        {
            await Runtime.InvokeVoidAsync("active_delete", e);
        }

        public async void hideDelete_Update(EventArgs e)
        {
            var response = await Runtime.InvokeAsync<bool>("hideDelete_Update", e);
            if (response == true)
            {
                update_message = null;
                delete_message = null;
            }
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
            date = date.Replace(" ", "");
            var oldDate = DateTime.ParseExact(date, "MM/dd/yyyy", null);
            var newDate = fullDate;

            if (
                newDate.Year != oldDate.Year &&
                newDate.Month == 1 &&
                oldDate.Month == 12
            )
            {
                if (oldDate.Day - newDate.Day <= 23)
                {
                    return 1;
                }
            }
            else if (newDate.Year == oldDate.Year)
            {
                if (
                    newDate.Month == oldDate.Month &&
                    newDate.Day - oldDate.Day >= 7
                )
                {
                    return 1;
                }
                else if (newDate.Month != oldDate.Month)
                {
                    if (
                        (newDate.Month - oldDate.Month == 1 &&
                            oldDate.Day - newDate.Day <= 23) ||
                        newDate.Month - oldDate.Month >= 2
                    )
                    {
                        return 1;
                    }
                }
            }
            return 2;
        }
        public string getday(string date)
        {
            date = date.Replace(" ", "");
            var newDate = DateTime.ParseExact(date, "MM/dd/yyyy", null);
             var newDatestring = newDate.ToString("dddd");
            return newDatestring;
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
        public string handeltime()
        {
            var time = DateTime.Now.ToString("hh:mm tt");
            return time;
        }
        public void sortarray()
        {
            users.Sort((a, b) =>
            {
                var a_date = DateTime.ParseExact(a.Chat[a.Chat.Count - 1].Date, "MM/dd/yyyy", null);
                var b_date = DateTime.ParseExact(b.Chat[b.Chat.Count - 1].Date, "MM/dd/yyyy", null);
                var a_time = DateTime.ParseExact(a.Chat[a.Chat.Count - 1].Time, "MM/dd/yyyy", null);
                var b_time = DateTime.ParseExact(b.Chat[b.Chat.Count - 1].Time, "MM/dd/yyyy", null);

                int result = b_date.Month.CompareTo(a_date.Month);
                if (result == 0)
                {
                    result = b_date.Month.CompareTo(a_date.Month);
                    if (result == 0)
                    {
                        result = b_date.Day.CompareTo(a_date.Day);
                        if (result == 0)
                        {
                            result = b_time.Hour.CompareTo(a_time.Hour);
                            if (result == 0)
                            {
                                result = b_time.Minute.CompareTo(a_time.Minute);
                                if (result == 0)
                                {
                                    result = b_time.Second.CompareTo(a_time.Second);
                                }
                            }
                        }
                    }
                }
                return result;
            });
        }
    }
}