using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Message;

public class MessageDto
{
    public long Id { get; set; }
    public string Content { get; set; }
    public DateTime SentAt { get; set; }
    public long ChatId { get; set; }
    public long SenderId { get; set; }
    public long? ReplyMessageId { get; set; }
}