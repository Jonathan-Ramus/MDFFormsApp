using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDFFormsApp
{
    internal class LinearParser
    {
        private string? json;
        private int index = 0;
        private string? nextMessage;
        private bool hasNext = false;

        public void Load(string json)
        {
            this.json = json;
            index = 0;
            PGetNextMessage();
        }

        private void PGetNextMessage()
        {
            while(index < json?.Length && json[index++] != 'c') { }
            if (index >= json?.Length) {
                nextMessage = null;
                hasNext = false;
                return;
            }
            
            index -= 3;
            int end_index = index;
            while(end_index < json?.Length && json[end_index++] != '}') { }

            nextMessage = json?.Substring(index, end_index-index-2);
            hasNext = true;
            index = end_index;
        }

        public bool HasNextMessage()
        {
            return hasNext;
        }
        public string? GetNextMessage()
        {
            if (!hasNext) { return null; }

            string? message = nextMessage?.Replace("\t", "");
            PGetNextMessage();
            return message;
        }
    }
}
