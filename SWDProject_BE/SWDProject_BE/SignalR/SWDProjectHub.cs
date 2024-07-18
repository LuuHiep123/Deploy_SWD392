
using BusinessLayer.RequestModels;
using BusinessLayer.ResponseModels;
using BusinessLayer.Services;
using DataLayer.Model;
using DataLayer.Repository;
using Microsoft.AspNetCore.SignalR;


namespace SWDProject_BE.SignalR
{
	public class SWDProjectHub: Hub
	{
		private readonly IGroupService _groupService;
		private readonly IMessageService _messageService;

		public SWDProjectHub(IMessageService messageService, IGroupService groupService)
		{
			_messageService = messageService;
			_groupService = groupService;
		}

		public override Task OnConnectedAsync()
		{
			return base.OnConnectedAsync();
		}

		public override Task OnDisconnectedAsync(Exception? exception)
		{
			return base.OnDisconnectedAsync(exception);
		}

		//tao nhom theo bai post
		public async Task CreateGroup(Group group)
		{
			
			//Save group to db
			await _groupService.AddGroupAsync(group);

            var newGroupDto = new GroupResponseModel
            {
                Id = group.Id,
                PostId = group.PostId,
                UserExchangeId = group.UserExchangeId,
            };

            await Clients.All.SendAsync("ReceiveNewGroup", newGroupDto);				
		}

		//Join vao nhom
		public async Task JoinGroup(int postId)
		{
			//Dang ki nhan tin nhan tu Group voi groupId
			await Groups.AddToGroupAsync(Context.ConnectionId, postId.ToString());
			//
		}

		//Roi khoi nhom
		public async Task LeaveGroup(int postId)
		{
			//roi khoi nhom, khong nhan tin nhan nua
			await Groups.RemoveFromGroupAsync(Context.ConnectionId, postId.ToString());
		}

		//Join vao tat ca cac nhom, thuc hien khi bat dau ket noi(dang nhap)
		public async Task JoinAllGroup(int userId)
		{
			//Lay tat ca cac nhom ma user da tham gia
			var groups = await _groupService.FindAllByUserId(userId);
			foreach (var group in groups)
			{
				await Groups.AddToGroupAsync(Context.ConnectionId, group.PostId.ToString());
			}
		}

        public async Task GetAllGroupChat(int userId)
        {
            // Retrieve all groups that the user has joined
            var groups = await _groupService.FindAllByUserId(userId);

            // Convert the group entities to a response model
            var groupDtos = groups.Select(group => new GroupResponseModel
            {
                Id = group.Id,
                PostId = group.PostId,
                UserExchangeId = group.UserExchangeId,
            }).ToList();

            // Send the list of groups to the client
            await Clients.All.SendAsync("ReceiveAllGroupChats", groupDtos);
        }

        //Gui tin nhan
        public async Task SendMessage(Message message)
		{
            //Luu tin nhan vao db
            var newMessage = await _messageService.AddMessage(message);

            var messageDto = new MessageResponseModel
			{
				Id = newMessage.Id,
				GroupId = newMessage.GroupId,
				SenderId = newMessage.SenderId,
				Content = newMessage.Content,
				CreatedDate = DateTime.UtcNow,
			};
			
			//Gui tin nhan theo group co PostId
			await Clients.Group(newMessage.GroupId.ToString()).SendAsync("ReceiveMessage", messageDto);
		}

		//tai toan bo tin nhan tu nhom co ten la postId, thuc hien khi user mo form chat
		public async Task LoadMessageByGroupId(int groupId)
		{
			//lay tat ca tin nhan co postId
			//tai mot phan tin nhan, sau khi lan chuot se tiep tuc load
			var messages = await _messageService.FindByGroupId(groupId);
			await Clients.Clients(Context.ConnectionId).SendAsync("ReceiveMessages", messages);
		}

		//front-end task:
		//khoi tao HubConnection
		//dang ki function, cac function nay se duoc server goi lai 
		//ket noi Hub signalR BE qua duong link
		//front-end can xu ly reconnect( xu ly ket noi lai khi gap su co mang)
	}
}
