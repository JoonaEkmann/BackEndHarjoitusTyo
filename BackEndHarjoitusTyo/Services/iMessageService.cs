using BackEndHarjoitusTyo.Models;

namespace BackEndHarjoitusTyo.Services
{
    public interface iMessageService
    {
        Task<IEnumerable<MessageDTO>> GetMessagesAsync();
        Task<MessageDTO?>GetMessageAsync(long id);
        Task<MessageDTO?> NewMessageAsync(MessageDTO message);
        Task<bool> UpdateMessageAsync(MessageDTO message);
        Task<bool> DeleteMessageAsync(long id);

    }
}
