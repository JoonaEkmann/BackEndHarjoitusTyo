﻿using BackEndHarjoitusTyo.Models;

namespace BackEndHarjoitusTyo.Repositories
{
    public interface iMessageRepository
    {
        Task<IEnumerable<Message>> GetMessagesAsync();
        Task<IEnumerable<Message>> GetMySentMessagesAsync(User user);
        Task<IEnumerable<Message>> GetMyReceivedMessagesAsync(User user);

        Task<Message?> GetMessageAsync(long id);
        Task<Message> NewMessageAsync(Message message);
        Task<bool> UpdateMessageAsync(Message message);
        Task<bool> DeleteMessageAsync(Message message);



    }
}
