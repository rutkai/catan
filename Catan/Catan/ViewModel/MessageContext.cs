using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Catan.ViewModel.Commons;

namespace Catan.ViewModel
{
    public enum MessageType
    {
        Information, Error, Warning
    }

    public class MessageContext : ViewModelBase
    {
        private string _Title;
        private string _Message;

        public GameTableContext Context { get; set; }

        public string Message
        {
            get { return _Message; }
            set
            {
                _Message = value;
                OnPropertyChanged(() => Message);
            }
        }

        public string Title
        {
            get { return _Title; }
            set
            {
                _Title = value;
                OnPropertyChanged(() => Title);
            }
        }

        public MessageType MessageType { get; set; }

        public MessageContext(GameTableContext context, string message)
            : this(context, message, string.Empty, MessageType.Information)
        {
        }

        public MessageContext(GameTableContext context, string message, string title, MessageType messageType)
        {
            if (context == null) throw new ArgumentNullException("context");
            if (string.IsNullOrWhiteSpace(message)) throw new ArgumentNullException("message");
            if (title == null) throw new ArgumentNullException("title");
            Context = context;
            Message = message;
            Title = title;
            MessageType = messageType;
        }
    }
}
