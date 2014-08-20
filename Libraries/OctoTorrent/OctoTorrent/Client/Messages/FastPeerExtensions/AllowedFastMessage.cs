//
// AllowedFastMessage.cs
//
// Authors:
//   Alan McGovern alan.mcgovern@gmail.com
//
// Copyright (C) 2006 Alan McGovern
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

namespace OctoTorrent.Client.Messages.FastPeer
{
    using System.Text;

    public class AllowedFastMessage : PeerMessage, IFastPeerMessage
    {
        internal const byte MessageId = 0x11;
        private const int MessageLength = 5;

        #region Member Variables

        public int PieceIndex { get; private set; }

        #endregion

        #region Constructors

        internal AllowedFastMessage()
        {
        }

        internal AllowedFastMessage(int pieceIndex)
        {
            PieceIndex = pieceIndex;
        }

        #endregion

        #region Methods

        public override int ByteLength
        {
            get { return MessageLength + 4; }
        }

        public override int Encode(byte[] buffer, int offset)
        {
            if (!ClientEngine.SupportsFastPeer)
                throw new ProtocolException("Message encoding not supported");

            var written = offset;

            written += Write(buffer, written, MessageLength);
            written += Write(buffer, written, MessageId);
            written += Write(buffer, written, PieceIndex);

            return CheckWritten(written - offset);
        }

        public override void Decode(byte[] buffer, int offset, int length)
        {
            if (!ClientEngine.SupportsFastPeer)
                throw new ProtocolException("Message decoding not supported");

            PieceIndex = ReadInt(buffer, offset);
        }

        #endregion

        #region Overidden Methods

        public override bool Equals(object obj)
        {
            var msg = obj as AllowedFastMessage;
            if (msg == null)
                return false;

            return PieceIndex == msg.PieceIndex;
        }

        public override int GetHashCode()
        {
            return PieceIndex.GetHashCode();
        }

        public override string ToString()
        {
            var sb = new StringBuilder(24);
            sb.Append("AllowedFast");
            sb.Append(" Index: ");
            sb.Append(PieceIndex);
            return sb.ToString();
        }

        #endregion
    }
}