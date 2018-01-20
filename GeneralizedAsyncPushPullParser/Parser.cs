using System;
using System.Runtime.CompilerServices;
using System.Text;

namespace GeneralizedAsyncPushPullParser
{
    class Parser
    {
        Action<char?> pushTarget;

        public event Action<string> Token;

        public Parser()
        {
            Parse();
        }

        Chunk GetChunk()
        {
            return new Chunk(this);
        }

        async void Parse()
        {
            var c = await GetChunk();
            while (true)
            {
                if (c == null) return;

                if (!char.IsWhiteSpace(c.Value))
                {
                    // Found beginning of a token, read the token.
                    var buf = new StringBuilder();
                    buf.Append(c.Value);
                    while (true)
                    {
                        c = await GetChunk();
                        if (c == null) break;
                        if (char.IsWhiteSpace(c.Value)) break;
                        buf.Append(c.Value);
                    }
                    Token?.Invoke(buf.ToString());
                    if (c == null) return;
                }
                else
                {
                    c = await GetChunk();
                }
            }
        }

        public void Push(
            string s)
        {
            if (s == null)
            {
                pushTarget(null);
                return;
            }
            foreach (var c in s)
            {
                pushTarget(c);
            }
        }

        class Chunk
        {
            readonly ChunkAwaiter awaiter;
            public ChunkAwaiter GetAwaiter() => awaiter;
            public Chunk(Parser parser)
            {
                awaiter = new ChunkAwaiter(parser);
            }
            public class ChunkAwaiter
                : INotifyCompletion
            {
                Action continuation;
                char? value;

                public bool IsCompleted { get; private set; }

                public ChunkAwaiter(Parser parser)
                {
                    parser.pushTarget = SetValue;
                }
                public char? GetResult()
                {
                    return value;
                }
                public void OnCompleted(Action continuation)
                {
                    if (this.continuation != null) throw new InvalidOperationException("Each Chunk may only be awaited once.");

                    this.continuation = continuation;
                }
                void SetValue(char? s)
                {
                    value = s;
                    IsCompleted = true;
                    continuation?.Invoke();
                }
            }
        }
    }
}
