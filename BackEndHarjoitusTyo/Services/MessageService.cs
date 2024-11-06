using BackEndHarjoitusTyo.Models;
using BackEndHarjoitusTyo.Repositories;
using Humanizer;

namespace BackEndHarjoitusTyo.Services
{
    public class MessageService : iMessageService
    {
        private readonly iMessageRepository _repository;
        private readonly iUserRepository _userRepository;
        public MessageService(iMessageRepository repository, iUserRepository userRepository)
        {
            _repository = repository;
            _userRepository = userRepository;
        }

        public async Task<bool> DeleteMessageAsync(long id)
        {
            Message? message = await _repository.GetMessageAsync(id);
            if (message! == null) 
            {
                await _repository.DeleteMessageAsync(message);
                return true;
            }
            return false;
        }

        public async Task<MessageDTO?> GetMessageAsync(long id)
        {
            return MessageToDTO(await _repository.GetMessageAsync(id));
        }

        public async Task<IEnumerable<MessageDTO>> GetMessagesAsync()
        {
            IEnumerable<Message> messages = await _repository.GetMessagesAsync();
            List<MessageDTO> result = new List<MessageDTO>();
            foreach (Message message in messages)
            {
                result.Add(MessageToDTO(message));
            }
            return result;
        }

        public async Task<MessageDTO?> NewMessageAsync(MessageDTO message)
        {
            return MessageToDTO(await _repository.NewMessageAsync(await DTOtoMessageAsync(message)));
        }

        public async Task<bool> UpdateMessageAsync(MessageDTO message)
        {
           Message? dbMessage = await _repository.GetMessageAsync(message.Id);
            if (dbMessage != null)
            {
                dbMessage.Title = message.Title;
                dbMessage.Body = message.Body;
                return await _repository.UpdateMessageAsync(dbMessage);
            }
            return false;
        }

        private MessageDTO MessageToDTO(Message message)
        {
            MessageDTO dTO = new MessageDTO();

            dTO.Id = message.Id;
            dTO.Title = message.Title;
            dTO.Body = message.Body;
            dTO.Sender = message.Sender.UserName;
            if (message.Recipient != null)
            {
                dTO.Recipient = message.Recipient.UserName;
            }
            if (message.PrevMessage != null)
            {
                dTO.PrevMessageId = message.PrevMessage.Id;
            }

            return dTO;
        }

        private async Task<Message> DTOtoMessageAsync(MessageDTO dTO)
        {
            Message newMessage = new Message();
            newMessage.Id = dTO.Id;
            newMessage.Title = dTO.Title;
            newMessage.Body = dTO.Body;

            //TODO! hae käyttäjä ja vastaannottaja käyttäjänimen perusteella
            User? sender = await _userRepository.GetUserAsync(dTO.Sender);
            if (sender != null)
            {
                newMessage.Sender = sender;
            }
            if (dTO.Recipient != null)
            {
                User? recipient = await _userRepository.GetUserAsync(dTO.Recipient);
                if (recipient != null)
                {
                    newMessage.Recipient = recipient;
                }
            }

            if (dTO.PrevMessageId != null)
            {
               Message prevMessage = await _repository.GetMessageAsync((long)dTO.PrevMessageId);
                newMessage.PrevMessage = prevMessage;
            }
            return newMessage;
        }

    }
}

